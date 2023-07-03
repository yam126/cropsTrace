using AutoMapper;
using cropsTraceApi.Models;
using cropsTraceApi.Models.Result;
using cropsTraceDataAccess.Data;
using ePioneer.Data.Kernel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Snowflake.Net;
using Newtonsoft.Json;
using Common;

namespace cropsTraceApi.Controllers
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [EnableCors("Cors")]
    [Route("api/file-info")]
    [ApiController]
    public class FileInfoController : Controller
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

        /// <summary>
        /// 获取网站路径
        /// </summary>
        private readonly IWebHostEnvironment m_webHostEnvironment;
        #endregion

        #region Constructors

        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="repository">数据库操作类</param>
        public FileInfoController(IMOIRepository repository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            m_repository = repository;
            m_webHostEnvironment = webHostEnvironment;
            m_mapper = mapper == null ? throw new ArgumentNullException(nameof(mapper)) : mapper;
        }
        #endregion

        #region Get

        /// <summary>
        /// 根据生长编号读取文件信息
        /// </summary>
        /// <param name="GrowthId">生长编号</param>
        /// <returns></returns>
        [HttpGet("by-growthinfo/{GrowthId}")]
        public async Task<ActionResult<ListResult<FileInfoParameter>>> QueryFileInfoByGrowthId(long? GrowthId) 
        {
            #region 声明变量

            //查询条件
            string SqlWhere = string.Empty;

            //报错消息
            string message = string.Empty;

            //文件信息
            List<cropsTraceDataAccess.Model.FileInfo> fileInfos = null;

            //返回条件
            List<FileInfoParameter> resultFiles = new List<FileInfoParameter>();

            //文件信息
            ListResult<FileInfoParameter> result = new ListResult<FileInfoParameter>();
            #endregion

            #region 参数非空验证
            if (GrowthId == null || GrowthId <= 0) 
            {
                result.Status = -1;
                result.Msg = "生长信息编号不能为空";
                return result;
            }
            #endregion

            #region 读取文件信息
            SqlWhere = $" GrowthRecordId='{GrowthId}' ";
            fileInfos=m_repository.QueryFileInfo(SqlWhere,out message);
            if (fileInfos == null || fileInfos.Count <= 0) 
            {
                if (!string.IsNullOrEmpty(message)) 
                {
                    result = new ListResult<FileInfoParameter>()
                    {
                        Status = -1,
                        Msg = $"读取生长周期编号为[{GrowthId}]的文件数据出错，原因[{message}]"
                    };
                }
                else
                {
                    result = new ListResult<FileInfoParameter>()
                    {
                        Status = 0,
                        Msg = string.Empty
                    };
                }
                return result;
            }
            #endregion

            #region 赋值返回值
            fileInfos.ForEach(item =>
            {
                FileInfoParameter itemParam = m_mapper.Map<cropsTraceDataAccess.Model.FileInfo, FileInfoParameter>(item);
                itemParam.CreatedDateTime = item.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                resultFiles.Add(itemParam);
            });
            result = new ListResult<FileInfoParameter>() 
            { 
                Status=0,
                Msg= string.Empty,
                Result= resultFiles
            };
            #endregion

            return result;
        }
        #endregion

        #region Post

        /// <summary>
        /// 通过上传的方式添加文件
        /// </summary>
        /// <param name="formData">提交的数据集合</param>
        /// <returns>文件结果</returns>
        [HttpPost]
        public async Task<EntityResult<FileInfoParameter>> uploadFile([FromForm]IFormCollection formData) 
        {
            #region 声明变量

            //公司编号
            string CompanyId = string.Empty;

            //上传的文件
            IFormFile formFile=null;

            //文件信息参数
            FileInfoParameter fileInfoParm=null;

            //上传文件保存的文件夹
            string uploadFilePath = "uploadFiles";

            //上传文件保存的物理路径
            string realUploadFilePath = string.Empty;

            //网站根目录
            string wwwrootPath = string.Empty;

            //错误消息
            string message = string.Empty;

            //文件扩展名
            string fileExtension = "";

            //保存的文件名
            string SaveFileName = "";

            //非空验证
            string checkEmptyMessage = string.Empty;

            //数据库文件数据
            cropsTraceDataAccess.Model.FileInfo dbFileInfo = new cropsTraceDataAccess.Model.FileInfo();

            //返回值
            Message dbResult = new Message(true, string.Empty);

            //显示参数
            Dictionary<string, string> ShowFieldParms = null;

            //返回值
            EntityResult<FileInfoParameter> result = new EntityResult<FileInfoParameter>() { Status = 0, Msg = string.Empty };
            #endregion

            #region 参数赋值
            formFile=(formData==null||formData.Files==null||formData.Files.Count==0)?null:formData.Files[0];
            CompanyId = formData["CompanyId"];
            #endregion

            #region 参数验证
            if (formFile == null)
                checkEmptyMessage += "没有上传文件、";
            if (string.IsNullOrEmpty(CompanyId))
                checkEmptyMessage += "公司编号不能为空、";
            if (!string.IsNullOrEmpty(checkEmptyMessage)) 
            {
                result = new EntityResult<FileInfoParameter>() 
                { 
                    Status = -1, 
                    Msg = $"参数验证出错，原因[{checkEmptyMessage}]" 
                };
                return result;
            }
            #endregion

            #region 验证文件类型
            fileExtension = Path.GetExtension(formFile.FileName);
            if (checkUploadFileExtension(fileExtension,out message)==false)
            {
                result = new EntityResult<FileInfoParameter>()
                {
                    Status = -1,
                    Msg = $"文件类型验证出错，原因[{checkEmptyMessage}]"
                };
                return result;
            }
            #endregion

            #region 保存文件
            try
            {
                wwwrootPath = m_webHostEnvironment.WebRootPath;
                realUploadFilePath = @$"{wwwrootPath}\{uploadFilePath}\";
                if (!Directory.Exists(realUploadFilePath))
                    Directory.CreateDirectory(realUploadFilePath);
                SaveFileName = $"{new IdWorker(1, 1).NextId()}{fileExtension}";
                using (var fileStream = new FileStream($@"{realUploadFilePath}\{SaveFileName}", FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex) 
            {
                result = new EntityResult<FileInfoParameter>()
                {
                    Status = -1,
                    Msg = $"文件保存出错，原因[{ex.Message}]"
                };
                return result;
            }
            #endregion

            #region 读取显示参数
            ShowFieldParms = queryShowFields(Utils.StrToInt(CompanyId, 0),"",out message);
            if (ShowFieldParms == null) 
            {
                result = new EntityResult<FileInfoParameter>()
                {
                    Status = -1,
                    Msg = $"读取显示参数出错，原因[{message}]"
                };
                return result;
            }
            #endregion

            #region 生成文件数据
            fileInfoParm = new FileInfoParameter()
            {
                FileId = Convert.ToString(new IdWorker(1, 1).NextId()),
                FileLength = Convert.ToString(formFile.Length),
                FileName = SaveFileName,
                FileUrl = $@"{uploadFilePath}/{SaveFileName}",
                CreatedDateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                CropsId = string.Empty,
                GrowthRecordId = string.Empty,
                //ShowParamJson = JsonConvert.SerializeObject(ShowFieldParms)
                ShowParamJson = DictionaryToJson(ShowFieldParms)
            };
            #endregion

            result = new EntityResult<FileInfoParameter>() 
            {
                Status=0,
                Msg=string.Empty,
                Result=fileInfoParm
            };
            return result;
        }

        /// <summary>
        /// 上传二维码图片
        /// </summary>
        /// <param name="formData">提交的数据集合</param>
        /// <returns></returns>
        [HttpPost("qr-code/file")]
        public async Task<EntityResult<string>> uploadQrCodeFile([FromForm] IFormCollection formData)
        {
            #region 声明变量

            //公司编号
            string CompanyId = string.Empty;

            //上传的文件
            IFormFile formFile = null;

            //文件信息参数
            FileInfoParameter fileInfoParm = null;

            //上传文件保存的文件夹
            string uploadFilePath = "uploadFiles";

            //上传文件保存的物理路径
            string realUploadFilePath = string.Empty;

            //网站根目录
            string wwwrootPath = string.Empty;

            //错误消息
            string message = string.Empty;

            //文件扩展名
            string fileExtension = "";

            //保存的文件名
            string SaveFileName = "";

            //非空验证
            string checkEmptyMessage = string.Empty;

            //数据库文件数据
            cropsTraceDataAccess.Model.FileInfo dbFileInfo = new cropsTraceDataAccess.Model.FileInfo();

            //返回值
            Message dbResult = new Message(true, string.Empty);

            //显示参数
            Dictionary<string, string> ShowFieldParms = null;

            //返回值
            EntityResult<string> result = new EntityResult<string>() { Status = 0, Msg = string.Empty };
            #endregion

            #region 参数赋值
            formFile = (formData == null || formData.Files == null || formData.Files.Count == 0) ? null : formData.Files[0];
            #endregion

            #region 参数验证
            if (formFile == null)
                checkEmptyMessage += "没有上传文件、";
            if (!string.IsNullOrEmpty(checkEmptyMessage))
            {
                result = new EntityResult<string>()
                {
                    Status = -1,
                    Msg = $"参数验证出错，原因[{checkEmptyMessage}]"
                };
                return result;
            }
            #endregion

            #region 验证文件类型
            fileExtension = Path.GetExtension(formFile.FileName);
            if (checkUploadFileExtension(fileExtension, out message) == false)
            {
                result = new EntityResult<string>()
                {
                    Status = -1,
                    Msg = $"文件类型验证出错，原因[{checkEmptyMessage}]"
                };
                return result;
            }
            #endregion

            #region 保存文件
            try
            {
                wwwrootPath = m_webHostEnvironment.WebRootPath;
                realUploadFilePath = @$"{wwwrootPath}\{uploadFilePath}\";
                if (!Directory.Exists(realUploadFilePath))
                    Directory.CreateDirectory(realUploadFilePath);
                SaveFileName = $"{new IdWorker(1, 1).NextId()}{fileExtension}";
                using (var fileStream = new FileStream($@"{realUploadFilePath}\{SaveFileName}", FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                result = new EntityResult<string>()
                {
                    Status = -1,
                    Msg = $"文件保存出错，原因[{ex.Message}]"
                };
                return result;
            }
            #endregion

            result = new EntityResult<string>()
            {
                Status = 0,
                Msg = string.Empty,
                Result = $@"{uploadFilePath}/{SaveFileName}"
            };
            return result;
        }

        /// <summary>
        /// 批量保存文件信息
        /// </summary>
        /// <param name="parameter">保存参数</param>
        /// <returns>返回结果</returns>
        [HttpPost("batch-save-file")]
        public async Task<Result> BatchSaveFileInfo([FromForm] IFormCollection formData) 
        {
            #region 声明变量

            BatchSaveFileInfoParameter parameter = null;

            string jsonStr = string.Empty;

            //查询条件
            string SqlWhere = string.Empty;

            //非空验证
            string checkEmptyMessage = string.Empty;

            //非空验证(循环验证)
            string checkItemEmptyMessage = string.Empty;

            //公司编号
            string CompanyId = string.Empty;

            //数据库消息
            Message dbMessage=new Message(true,string.Empty);

            //显示参数
            Dictionary<string, string> ShowFieldParms = null;

            //要保存的数据
            List<cropsTraceDataAccess.Model.FileInfo> saveData=new List<cropsTraceDataAccess.Model.FileInfo>();

            //返回值
            Result result = new Result() { Status=0,Msg=string.Empty };
            #endregion

            jsonStr = formData["data"];
            if (!string.IsNullOrEmpty(jsonStr)) 
            {
                try
                {
                    parameter = JsonConvert.DeserializeObject<BatchSaveFileInfoParameter>(jsonStr);
                }
                catch(Exception ex)
                {
                    result = new Result() { Status = -1, Msg = ex.Message };
                    return result;
                }
            }

            #region 参数非空验证
            if (parameter == null) 
            {
                result = new Result() 
                {
                    Status=-1,
                    Msg=$"参数不能为空"
                };
                return result;
            }
            if (string.IsNullOrEmpty(parameter.CompanyId))
                checkEmptyMessage += "公司编号、";
            if (string.IsNullOrEmpty(parameter.GrowthInfoId))
                checkEmptyMessage += "生长信息编号、";
            if (string.IsNullOrEmpty(parameter.Token))
                checkEmptyMessage += "token、";
            if (parameter.saveFilesData == null || parameter.saveFilesData.Count <= 0)
                checkEmptyMessage += "要保存的数据、";
            else 
            {
                checkItemEmptyMessage = string.Empty;
                for (int i = 0; i < parameter.saveFilesData.Count; i++) 
                {
                    FileInfoParameter item=parameter.saveFilesData[i];
                    parameter.saveFilesData[i].CropsId= parameter.CropsId;
                    parameter.saveFilesData[i].GrowthRecordId = parameter.GrowthInfoId;
                    //if (string.IsNullOrEmpty(item.CropsId) && string.IsNullOrEmpty(item.GrowthRecordId))
                    //    checkItemEmptyMessage += $"第{i+1}条数据，农作物编号或生长编号不能都为空、";
                    if (string.IsNullOrEmpty(item.FileName))
                        checkItemEmptyMessage += $"第{i + 1}条数据，文件名不能为空、";
                    if(string.IsNullOrEmpty(item.FileUrl))
                        checkItemEmptyMessage += $"第{i + 1}条数据，文件路径不能为空、";
                }
                checkEmptyMessage += checkItemEmptyMessage;
            }
            if (!string.IsNullOrEmpty(checkEmptyMessage)) 
            {
                result = new Result() 
                {
                    Status=-1,
                    Msg=$"文件信息非空验证出错，原因[{checkEmptyMessage}]"
                };
                return result;
            }
            #endregion

            #region 删除原有数据
            SqlWhere = $" GrowthRecordId='{parameter.GrowthInfoId}' ";
            dbMessage = await m_repository.DeleteFileInfo(SqlWhere);
            if (!dbMessage.Successful) 
            {
                result = new Result()
                {
                    Status = -1,
                    Msg = $"删除原有文件信息出错，原因[{dbMessage.Content}]"
                };
                return result;
            }
            #endregion

            #region 保存数据
            int snowIndex = 0;
            parameter.saveFilesData.ForEach(item => {
                cropsTraceDataAccess.Model.FileInfo fileInfo = null;
                string message = string.Empty;
                ShowFieldParms = queryShowFields(int.Parse(parameter.CompanyId),parameter.Token,out message);
                fileInfo = m_mapper.Map<cropsTraceDataAccess.Model.FileInfo>(item);
                fileInfo.FileId = new IdWorker(snowIndex + 1, snowIndex+1).NextId();
                fileInfo.ShowParamJson = "{"+DictionaryToJson(ShowFieldParms)+"}";
                snowIndex += 1;
                saveData.Add(fileInfo);
            });
            dbMessage = await m_repository.InsertFileInfo(saveData);
            if (!dbMessage.Successful)
                result = new Result() { Status=-1, Msg=$"保存文件数据出错，原因[{dbMessage.Content}]" };
            else
                result = new Result() { Status = 0, Msg = string.Empty };
            #endregion

            return result;
        }
        #endregion

        #region Delete

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">数据编号</param>
        /// <returns>是否成功消息</returns>
        [EnableCors("Cors")]
        [HttpDelete("{FileId}")]
        public async Task<Result> DeleteData(string FileId) 
        {
            #region 声明变量

            //错误消息
            string message = string.Empty;

            //数据库返回消息
            Message dbMessage = null;

            //返回值
            Result result = null;
            #endregion

            if (string.IsNullOrEmpty(FileId))
            {
                result = new Result()
                {
                    Msg = "参数为空不能删除",
                    Status = -1
                };
            }

            try
            {
                dbMessage = await m_repository.DeleteFileInfo($" FileId='{FileId}' ");
            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    Msg = $"删除文件信息报错，原因[{ex.Message}]",
                    Status = -1
                };
                return result;
            }

            result = new Result()
            {
                Msg = string.Empty,
                Status = 0
            };
            return result;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ServerFileName">服务器文件名</param>
        /// <returns></returns>
        [EnableCors("Cors")]
        [HttpDelete("delete-file/{ServerFileName}")]
        public async Task<Result> DeleteFile(string ServerFileName) 
        {
            #region 声明变量

            //上传文件保存的文件夹
            string uploadFilePath = "uploadFiles";

            //上传文件保存的物理路径
            string realUploadFileFullPath = string.Empty;

            //网站根目录
            string wwwrootPath = string.Empty;

            //错误消息
            string message = string.Empty;

            //返回值
            Result result = new Result() { Status=0,Msg=string.Empty };
            #endregion

            wwwrootPath = m_webHostEnvironment.WebRootPath;
            realUploadFileFullPath = @$"{wwwrootPath}\{uploadFilePath}\{ServerFileName}";
            try
            {
                if (System.IO.File.Exists(realUploadFileFullPath))
                    System.IO.File.Delete(realUploadFileFullPath);
            }
            catch (Exception ex) 
            {
                result = new Result() 
                {
                    Status=-1,
                    Msg=$"删除文件出错，原因[{ex.Message}]"
                };
            }

            return result;
        }
        #endregion

        #region Private

        /// <summary>
        /// 验证文件扩展名
        /// </summary>
        /// <param name="fileExtension">文件扩展名</param>
        /// <returns>验证结果</returns>
        private bool checkUploadFileExtension(string fileExtension,out string message) 
        {
            #region 声明和初始化

            //错误消息
            message = string.Empty;

            //返回值
            bool result = true;

            //配置文件扩展名配置
            string[] configFileExtension = null;

            //网站配置文件
            IConfiguration configuration;
            #endregion

            #region 读取网站配置文件
            try
            {
                configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();
                if(string.IsNullOrEmpty(configuration["UploadFile:limitExtension"]))
                {
                    message = "没有配置上传文件限制";
                    return false;
                }
                configFileExtension = configuration["UploadFile:limitExtension"].Split(',');
            }
            catch (Exception ex)
            {
                message=$"读取网站配置文件出错，原因[{ex.Message}]";
                return false;
            }
            #endregion

            if(configFileExtension.ToList().Any(item=>item.ToUpper()== fileExtension.ToUpper())==false)
            {
                message = $"不是允许上传的文件类型，不能上传";
                return false;
            }

            return result;
        }

        /// <summary>
        /// 将字段转换为json
        /// </summary>
        /// <param name="dict">字典</param>
        /// <returns>转换后的json</returns>
        private string DictionaryToJson(Dictionary<string,string> dict) 
        {
            string result = string.Empty;
            List<string> resultJsonList = new List<string>();
            if(dict != null && dict.Values.Count > 0) 
            {
                for(int i = 0; i < dict.Values.Count; i++)
                {
                    string key = Convert.ToString(dict.Keys.ToArray()[i]).Split('|')[0];
                    string value = Convert.ToString(dict.Values.ToArray()[i]);
                    resultJsonList.Add($"\"{key}\":\"{value}\"");
                }
            }
            result = $"{string.Join(",",resultJsonList.ToArray())}";
            return result;
        }

        /// <summary>
        /// 读取显示字段
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="token">token</param>
        /// <returns>字段返回值</returns>
        private Dictionary<string, string> queryShowFields(int CompanyId,string token,out string message) 
        {
            #region 声明变量

            int temp = 1;

            //错误消息
            message = string.Empty;

            //返回值
            Dictionary<string, string> result = null;

            Dictionary<string, string> ApiResult = null;

            //测点值参数
            PointsTimeValuesParameter parameter = new PointsTimeValuesParameter();

            //显示字段
            List<cropsTraceDataAccess.Model.ShowFields> showFields = new List<cropsTraceDataAccess.Model.ShowFields>();
            #endregion

            #region 读取显示字段
            showFields = m_repository.QueryShowFields($" CompanyId='{CompanyId}' and IsShow='1' ", out message);
            if(showFields==null||showFields.Count==0)
            {
                if(!string.IsNullOrEmpty(message))
                    message = $"读取显示字段失败，原因[{message}]";
                else
                    message = $"读取显示字段失败，原因[没有配置显示字段，请先配置字段]";
                return result;
            }
            else
            {
                parameter.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                parameter.ids = new List<int>();
                foreach (var field in showFields) 
                    parameter.ids.Add(Convert.ToInt32(field.PointId));
            }
            #endregion

            #region 调用接口获取测点值
            ApiResult = CallApiMethodReturn(parameter, token, out message);
            if (!string.IsNullOrEmpty(message))
            {
                message = $"调用接口失败，原因[{message}]";
                return result;
            }
            #endregion

            result = new Dictionary<string, string>();
            for (int i=0;i< showFields.Count;i++) 
            {
                string pointId = ApiResult.Keys.ToArray()[i];
                cropsTraceDataAccess.Model.ShowFields showField = showFields.First(query=>query.PointId==Utils.StrToLong(pointId));
                string value = ApiResult[pointId]+ showField.Unit;
                string key = showField.FieldName+"|"+showField.PointId;
                result.Add(key, value);
                temp += 1;
            }

            return result;
        }

        /// <summary>
        /// 调用API返回测点结果
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        private Dictionary<string, string> CallApiMethodReturn(PointsTimeValuesParameter parameter, string Token,out string message) 
        {
            #region 声明变量

            //错误消息
            message = string.Empty;

            //api请求host
            string urlBase = string.Empty;

            //api路径
            string apiPath = "points/time-values";

            //返回值
            Dictionary<string, string> result=new Dictionary<string, string>();

            //配置文件
            IConfiguration configuration = null;

            //json字符串
            string jsonStr = string.Empty;

            //调用成功返回值
            string _successResult = string.Empty;

            //调用失败返回值
            string _failResult = string.Empty;

            //测点返回数据
            cropsTraceApi.Models.PointsTimeValuesResult.Root apiResult = new Models.PointsTimeValuesResult.Root();
            #endregion

            #region 读取urlBase
            try
            {
                configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();
                urlBase = configuration["urlBaseLiot:url"];
                if (urlBase == string.Empty) 
                {
                    message = "读取参数接口配置出错，原因[没有配置api接口host[配置节点urlBaseLiot:url]请检查网站配置文件appsettings.json]";
                    return null;
                }
            }
            catch(Exception ex)
            {
                message = $"读取参数接口配置出错，原因[{ex.Message}]";
                return null;
            }
            #endregion

            #region 转换json
            try
            {
                jsonStr = JsonConvert.SerializeObject(parameter);
            }
            catch(Exception ex) 
            {
                message = $"接口参数转换JSON出错，原因[{ex.Message}]";
                return null;
            }
            #endregion

            #region 调用接口
            HttpRequestManager.HttpRespons(
                $"{urlBase}/{apiPath}",
                HttpType.POST,
                "application/json",
                out _successResult,
                out _failResult,
                jsonStr,
                Token
                );
            if (!string.IsNullOrEmpty(_failResult)) 
            {
                message = $"调用接口出错，原因[{_failResult}]";
                return null;
            }
            #endregion

            #region 赋值返回值
            try
            {
                apiResult=JsonConvert.DeserializeObject<Models.PointsTimeValuesResult.Root>(_successResult);
                if(apiResult!=null&&apiResult.status==0)
                {
                    if(apiResult.result!=null&& apiResult.result.Count > 0) 
                    {
                        if(apiResult.result.Count!=parameter.ids.Count)
                        {
                            message = $"接口调用失败，原因[接口返回的测点值和参数传递的测点值不一致]";
                            return null;
                        }
                        for(int i=0;i<apiResult.result.Count;i++)
                        {
                            string key = parameter.ids[i].ToString();
                            string value=apiResult.result[i].value;
                            result.Add(key, value);
                        }
                    }
                }
                else 
                {
                    message = $"接口调用失败，原因[{apiResult.msg}]";
                    return null;
                }
            }
            catch (Exception ex) 
            {
                message = $"转换接口返回值出错，原因[{ex.Message}]";
                return null;
            }
            #endregion

            return result;
        }
        #endregion
    }
}
