using AutoMapper;
using Common;
using cropsTraceApi.Models;
using cropsTraceApi.Models.Result;
using cropsTraceDataAccess.Data;
using cropsTraceDataAccess.Model;
using ePioneer.Data.Kernel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Snowflake.Net;

namespace cropsTraceApi.Controllers
{
    /// <summary>
    /// 首页数据查询控制器
    /// </summary>
    [EnableCors("Cors")]
    [Route("api/home")]
    [ApiController]
    public class HomeController : Controller
    {
        #region Fields

        /// <summary>
        /// 数据库操作类
        /// </summary>
        private IMOIRepository m_repository;

        /// <summary>
        /// AutoMapper参数映射类
        /// </summary>
        private IMapper m_mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="repository">数据库操作类</param>
        public HomeController(IMOIRepository repository, IMapper _mapper)
        {
            m_repository = repository;
            m_mapper = _mapper == null ? throw new ArgumentNullException(nameof(_mapper)) : _mapper;
        }
        #endregion

        #region Post

        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <param name="parameter">查询参数</param>
        /// <returns></returns>
        [EnableCors("Cors")]
        [HttpPost]
        public EntityResult<HomeResult> GetHomeResult(HomeResultParameters parameter) 
        {
            #region 声明变量

            //查询条件
            string SqlWhere = string.Empty;

            //错误消息
            string message = string.Empty;

            //返回数据
            HomeResult ResultData = new HomeResult();

            //泵房数据
            List<PumpHouseInfo> db_pumpHouseInfo = null;

            //泵房数据
            List<HomePumpHouseResult> ResultPumpHouse = null;

            //农作物主数据
            List<SeedInfo> seedInfos = null;

            List<string> pumpHouseID = new List<string>();

            //农作物主数据
            List<HomeSeedInfoResult> homeSeedInfos = new List<HomeSeedInfoResult>();

            //生长信息
            List<vw_GrowthInfo_Plus> growthInfos = null;

            //首页返回的数据
            var result = new EntityResult<HomeResult>();
            #endregion

            #region 读取泵房数据
            SqlWhere = $" CompanyId='11' ";
            if (!string.IsNullOrEmpty(parameter.pumpHouseID))
                SqlWhere += $" and PumpId='{parameter.pumpHouseID}' ";
            db_pumpHouseInfo = m_repository.QueryPumpHouseInfo(SqlWhere, out message);
            if (db_pumpHouseInfo == null || db_pumpHouseInfo.Count <= 0)
            {
                if (!string.IsNullOrEmpty(message))
                    message = $"读取泵房数据出错，原因[{message}]";
                else
                    message = $"读取泵房数据出错，原因[没有读取到泵房数据]";
                result = new EntityResult<HomeResult>()
                {
                    Status = -1,
                    Msg = message
                };
                return result;
            }
            else
            {
                ResultData.PumpHouse = new List<HomePumpHouseResult>();
                db_pumpHouseInfo.ForEach(itemPump =>
                {
                    pumpHouseID.Add(itemPump.PumpId.GetValueOrDefault().ToString());
                });
            }
            #endregion

            #region 读取生长数据
            SqlWhere = $" PumpId in ('{string.Join("','", pumpHouseID)}') and Year(CreatedDateTime)='{parameter.Year}'";
            SqlWhere += $" and CropsId='{parameter.CropsId}' ";
            if(!string.IsNullOrEmpty(parameter.growthName))
                SqlWhere += $" and GrowthName like '%{parameter.growthName}%' ";
            if (!string.IsNullOrEmpty(parameter.where)) 
            {
                string whereStr = string.Empty;
                whereStr += $" cast(RecordId as nvarchar(50)) like '%{parameter.where}%' ";
                whereStr += $" Or cast(PlantArea as nvarchar(50)) like '%{parameter.where}%' ";
                whereStr += $" Or SoilType like '%{parameter.where}%' ";
                whereStr += $" Or SeedVariety like '%{parameter.where}%' ";
                whereStr += $" Or LandName like '%{parameter.where}%' ";
                SqlWhere += $" and ({whereStr}) ";
            }          
            growthInfos = m_repository.QueryViewGrowthInfoPlus(SqlWhere, out message);
            if (growthInfos == null || growthInfos.Count <= 0) 
            {
                if (!string.IsNullOrEmpty(message))
                    message = $"读取生长数据出错，原因[{message}]";
                else
                    message = $"读取生长数据出错，原因[没有读取到生长数据]";
                result = new EntityResult<HomeResult>()
                {
                    Status = -1,
                    Msg = message
                };
                return result;
            }
            #endregion

            #region 分组数据
            var growthGroup = growthInfos
                .GroupBy(g => new { 
                    g.PumpId
                }).Select(newgroup =>new {
                    PumpId=newgroup.Key.PumpId,
                    PumpName=newgroup.First().PumpHouseName,
                    CropsId=newgroup.First().CropsId,
                    SeedName= newgroup.First().SeedName,
                    SeedVariety =newgroup.First().SeedVariety,
                    //PlantArea=newgroup.Sum(sum=>sum.PlantArea),
                    PlantArea=newgroup.First().PlantArea,
                    LandName=newgroup.First().LandName,
                    SoilType=newgroup.First().SoilType,
                    Introduce=newgroup.First().Introduce,
                    PlantHeight=newgroup.First().PlantHeight,
                    DBH = newgroup.First().DBH,
                    NumberOfBlades=newgroup.First().NumberOfBlades,
                    EmergenceRate=newgroup.First().EmergenceRate,
                    FileList =newgroup.ToList()
                });
            #endregion

            #region 循环赋值返回数据
            foreach (var growth in growthGroup) 
            {
                HomePumpHouseResult pumpResult = new HomePumpHouseResult();
                pumpResult.PumpHouseInfo = new PumpHouseInfoParameter() 
                {
                    PumpId=Convert.ToString(growth.PumpId),
                    PumpHouseName=growth.PumpName
                };
                pumpResult.seedInfo = new HomeSeedInfoResult();
                pumpResult.seedInfo.SeedInfo = new SeedInfoParameter();
                pumpResult.seedInfo.SeedInfo.SeedName = growth.SeedName;
                pumpResult.seedInfo.SeedInfo.PlantArea = growth.PlantArea.ToString("0.00");
                pumpResult.seedInfo.landName = growth.LandName;
                pumpResult.seedInfo.PlantHeight = growth.PlantHeight.ToString("0.00");
                pumpResult.seedInfo.DBH = growth.DBH.ToString("0.00");
                pumpResult.seedInfo.NumberOfBlades = growth.NumberOfBlades.ToString();
                pumpResult.seedInfo.EmergenceRate = growth.EmergenceRate.ToString("0.00");
                pumpResult.seedInfo.SeedInfo.Introduce= growth.Introduce;
                pumpResult.seedInfo.SeedInfo.SoilType = growth.SoilType;
                pumpResult.seedInfo.seedInfoFiles = new List<HomeFileInfoResult>();
                foreach (var fileInfo in growth.FileList)
                {
                    HomeFileInfoResult resultFileInfo = new HomeFileInfoResult();
                    resultFileInfo.FileInfo=new FileInfoParameter();
                    resultFileInfo.FileInfo.FileName= fileInfo.FileName;
                    resultFileInfo.FileInfo.FileUrl = fileInfo.FileUrl;
                    resultFileInfo.FileInfo.FileLength = fileInfo.FileLength.ToString("00.00");
                    resultFileInfo.FileInfo.ShowParamJson = fileInfo.ShowParamJson;
                    pumpResult.seedInfo.seedInfoFiles.Add(resultFileInfo);
                }
                ResultData.PumpHouse.Add(pumpResult);
            }
            #endregion

            result = new EntityResult<HomeResult>()
            {
                Status = 0,
                Msg = string.Empty,
                Result=ResultData
            };
            return result;
        }
        #endregion
    }
}
