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
    [EnableCors("Cors")]
    [Route("api/mobile")]
    [ApiController]
    public class MobileController : Controller
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
        public MobileController(IMOIRepository repository, IMapper _mapper)
        {
            m_repository = repository;
            m_mapper = _mapper == null ? throw new ArgumentNullException(nameof(_mapper)) : _mapper;
        }
        #endregion

        #region Get

        /// <summary>
        /// 获取扫码页数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>扫码页返回数据</returns>
        [EnableCors("Cors")]
        [HttpGet("mobilePageData")]
        public EntityResult<MobileResult> CropsGrowthArchives(
            string companyId = "",
            string year = "",
            string cropsId = "",
            string pumpHouseID = "")
        {
            #region 声明变量


            MobileResultParameters parameter = new MobileResultParameters();

            //查询条件
            string SqlWhere = string.Empty;

            //错误消息
            string message = string.Empty;

            //返回数据
            MobileResult ResultData = new MobileResult();

            //生长信息
            List<vw_GrowthInfo_Plus> growthInfos = null;

            //首页返回的数据
            var result = new EntityResult<MobileResult>();

            //生长周期
            List<string> grwthNames = new List<string>() { "播种","苗期","穗期","花粒期","成熟期","采收","存储" };
            #endregion

            parameter = new MobileResultParameters() {
              CompanyId = companyId,
              Year = year,
              CropsId = cropsId,
              pumpHouseID = pumpHouseID
            };

            #region 读取生长数据
            SqlWhere = $" PumpId='{parameter.pumpHouseID}'";
            SqlWhere += $" and Year(CreatedDateTime)='{parameter.Year}'";
            SqlWhere += $" and CropsId='{parameter.CropsId}' ";
            growthInfos = m_repository.QueryViewGrowthInfoPlus(SqlWhere, out message);
            if (growthInfos == null || growthInfos.Count <= 0)
            {
                if (!string.IsNullOrEmpty(message))
                    message = $"读取生长数据出错，原因[{message}]";
                else
                    message = $"读取生长数据出错，原因[没有读取到生长数据]";
                result = new EntityResult<MobileResult>()
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
                    g.GrowthName
                }).Select(newgroup => new {
                    GrowthName=newgroup.Key.GrowthName,
                    SeedName =newgroup.First().SeedName,
                    PlantYear=parameter.Year,
                    PlantArea=newgroup.First().PlantArea.ToString("0.00"),
                    landName=newgroup.First().LandName,
                    SoilType=newgroup.First().SoilType,
                    PumpHouseName=newgroup.First().PumpHouseName,
                    traceNo= parameter.Year+newgroup.First().CropsId+newgroup.First().PumpId,
                    FileInfos=newgroup.ToList()
                });
            #endregion

            #region 循环赋值返回数据
            ResultData.SeedName=growthGroup.First().SeedName;
            ResultData.PlantYear=growthGroup.First().PlantYear;
            ResultData.PlantArea=growthGroup.First().PlantArea;
            ResultData.landName = growthGroup.First().landName;
            ResultData.SoilType=growthGroup.First().SoilType;
            ResultData.PumpHouseName=growthGroup.First().PumpHouseName;
            ResultData.traceNo=ShortHelper.Encode(Convert.ToDouble(growthGroup.First().traceNo));
            ResultData.growthInfoResults = new List<MobileGrowthInfoResult>();
            foreach (string growthName in grwthNames)
            {
                
                if(growthGroup.Any(item=>item.GrowthName.IndexOf(growthName)!=-1))
                {
                    var growthInfoResult = growthGroup.First(query=>query.GrowthName.IndexOf(growthName)!=-1);
                    var FileInfos=growthInfoResult.FileInfos;
                    MobileGrowthInfoResult mobileGrowthInfo = new MobileGrowthInfoResult();
                    mobileGrowthInfo.GrowthName=growthName;
                    mobileGrowthInfo.fileInfoResults = new List<MobileFileInfoResult>();
                    foreach (var itemResultFile in FileInfos)
                    {
                        mobileGrowthInfo.fileInfoResults.Add(new MobileFileInfoResult() {
                          FileName=itemResultFile.FileName,
                          FileLength=itemResultFile.FileLength.ToString("0.00"),
                          FileUrl=itemResultFile.FileUrl.ToString(),
                          CreatedDateTime=itemResultFile.CreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                          ShowParamJson=itemResultFile.ShowParamJson.ToString()
                        });
                    }
                    ResultData.growthInfoResults.Add(mobileGrowthInfo);
                }
            }
            #endregion

            result = new EntityResult<MobileResult>()
            {
                Status = 0,
                Msg = string.Empty,
                Result = ResultData
            };
            return result;
        }
        #endregion
    }
}
