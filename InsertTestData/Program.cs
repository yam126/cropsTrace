using cropsTraceDataAccess.Model;
using cropsTraceDataAccess.Data;
using Snowflake.Net;
using ePioneer.Data.Kernel;
using Microsoft.Extensions.Configuration;
using InsertTestData.Model;

/// <summary>
/// 添加显示字段测试数据
/// </summary>
static async void InsertShowFieldsTestData(string MOIConnStr)
{
    #region 声明和初始化

    //测试数据数据量
    int testDataCount = 20;

    //测试数据
    List<ShowFields> testData = new List<ShowFields>();

    //返回消息
    Message dbResult = new Message();

    //初始化数据库帮助类
    MOIRepository m_dbHelper = new MOIRepository(MOIConnStr);
    #endregion

    #region 循环添加测试数据
    for (var i = 0; i <= testDataCount; i++)
    {
        testData.Add(new ShowFields()
        {
            RecordId = new IdWorker(1, i).NextId(),
            CompanyId = 7,
            Device = $"设备{i}",
            FieldName = $"Parameter{i}",
            ShowFieldName = $"参数{i}",
            PointId = new IdWorker(1, i).NextId(),
            Unit = ""
        });
    }
    #endregion

    #region 保存数据
    dbResult = await m_dbHelper.InsertShowFields(testData);
    if (dbResult.Successful)
        Console.WriteLine("保存成功");
    else
        Console.WriteLine($"保存失败,原因[{dbResult.Content}]");
    #endregion
}

/// <summary>
/// 添加泵房数据
/// </summary>
static async void InsertPumpHouseInfo(string MOIConnStr)
{
    #region 声明和初始化

    //测试数据数据量
    int testDataCount = 20;

    //测试数据
    List<PumpHouseInfo> testData = new List<PumpHouseInfo>();

    //返回消息
    Message dbResult = new Message();

    //初始化数据库帮助类
    MOIRepository m_dbHelper = new MOIRepository(MOIConnStr);
    #endregion

    #region 循环添加测试数据
    for (var i = 0; i <= testDataCount; i++)
    {
        testData.Add(new PumpHouseInfo()
        {
            PumpId = new IdWorker(1, i).NextId(),
            CompanyId = 7,
            CreatedDateTime = DateTime.Now,
            ModifiedDateTime = DateTime.Now,
            PumpHouseName = $"测试泵房名称{i}",
            PumpHouseClass = $"测试泵房种类",
            PersonIinCharge = "nmdkxm"
        });
    }
    #endregion

    #region 保存数据
    dbResult = await m_dbHelper.InsertPumpHouseInfo(testData);
    if (dbResult.Successful)
        Console.WriteLine("保存成功");
    else
        Console.WriteLine($"保存失败,原因[{dbResult.Content}]");
    #endregion

    Console.Read();
}

/// <summary>
/// 添加生长信息测试数据
/// </summary>
static async void InsertGrowthInfoTestData(string MOIConnStr)
{
    #region 声明和初始化

    //测试数据数据量
    int testDataCount = 10;

    //临时索引
    int index = 0;

    //查询条件
    string SqlWhere = string.Empty;

    //错误消息
    string message = string.Empty;

    //农作物主数据
    List<SeedInfo> seedInfos = new List<SeedInfo>();

    //泵房主数据
    List<PumpHouseInfo> pumpHouses = new List<PumpHouseInfo>();

    //生长周期
    List<string> GrowthCycle = new List<string>() { "播种", "苗期", "穗期", "花粒期", "成熟期", "采收", "存储" };

    //生长信息测试数据
    List<GrowthInfo> testData = new List<GrowthInfo>();

    //返回消息
    Message dbResult = new Message();

    //雪花ID
    var snowId = new IdWorker(1, 1);

    //初始化数据库帮助类
    MOIRepository m_dbHelper = new MOIRepository(MOIConnStr);
    #endregion

    #region 读取农作物主数据
    Console.WriteLine("开始读取农作物主数据");
    seedInfos = m_dbHelper.QuerySeedInfo(SqlWhere, out message);
    if (seedInfos == null || seedInfos.Count == 0)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Console.WriteLine($"读取农作物主数据出错，原因[{message}]");
            return;
        }
        else
        {
            Console.WriteLine("读取农作物主数据出错，原因[没有配置农作物主数据，请先配置农作物主数据]");
            return;
        }
    }
    Console.WriteLine("读取农作物主数据结束");
    #endregion

    #region 读取泵房数据
    Console.WriteLine("开始读取泵房主数据");
    pumpHouses = m_dbHelper.QueryPumpHouseInfo(SqlWhere, out message);
    if (pumpHouses == null || pumpHouses.Count == 0)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Console.WriteLine($"读取泵房主数据出错，原因[{message}]");
            return;
        }
        else
        {
            Console.WriteLine("读取泵房主数据出错，原因[没有配置泵房主数据，请先配置泵房主数据]");
            return;
        }
    }
    Console.WriteLine("读取泵房主数据结束");
    #endregion

    #region 循环添加生长信息
    Console.WriteLine("开始生成生长数据");
    foreach (PumpHouseInfo pumpHouse in pumpHouses)
    {
        for (var i = 0; i < seedInfos.Count; i++)
        {
            SeedInfo seedInfo = seedInfos[i];
            foreach (string cycle in GrowthCycle)
            {
                for (var j = 0; j < testDataCount; j++)
                {
                    GrowthInfo growthInfo = new GrowthInfo()
                    {
                        RecordId = snowId.NextId(),
                        PumpId = pumpHouse.PumpId,
                        CropsId = seedInfo.CropsId,
                        LandName = $"寿阳县景尚乡第{i+1}号{j+1}",
                        GrowthName = cycle,
                        PlantHeight = new Random().Next(1, 20),
                        NumberOfBlades = new Random().Next(1, 20),
                        DBH = new Random().Next(1, 20),
                        EmergenceRate = new Random().Next(1, 20),
                        CreatedDateTime = DateTime.Now.AddYears(-j).AddSeconds(i + j),
                        ModifiedDateTime = DateTime.Now.AddYears(-j).AddSeconds(i + j)
                    };
                    Console.WriteLine($"已经生成了{index}");
                    index += 1;
                    testData.Add(growthInfo);
                }
            }
        }
    }
    Console.WriteLine("生成生长数据结束");
    #endregion

    #region 保存数据
    dbResult = await m_dbHelper.InsertGrowthInfo(testData);
    if (dbResult.Successful)
        Console.WriteLine("保存成功");
    else
        Console.WriteLine($"保存失败,原因[{dbResult.Content}]");
    #endregion

    Console.Read();
}

/// <summary>
/// 添加文件测试数据
/// </summary>
static async void InsertFileInfoTestData(string MOIConnStr)
{
    #region 声明变量

    //错误消息
    string message = string.Empty;

    //查询条件
    string SqlWhere = string.Empty;

    //测试图片文件
    List<System.IO.FileInfo> files = new List<System.IO.FileInfo>();

    //测试数据数据量
    int testDataCount = 9;

    //农作物图片
    string cropsImagePath = @"E:\workproject\cropsTrace\cropsTraceApi\wwwroot\uploadFiles";

    //测试文件数据
    List<tempFileInfo> growFileUrls = new List<tempFileInfo>() {
        new tempFileInfo(){
            FileName="e7e0cd74-41d0-41e1-a6f8-d309885e6e5f.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220829/e7e0cd74-41d0-41e1-a6f8-d309885e6e5f.jpeg",
            Length=134756
        },
        new tempFileInfo(){
            FileName="b2d6e1c3-4727-4ee0-b93e-249e4a3c4c7d.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220829/b2d6e1c3-4727-4ee0-b93e-249e4a3c4c7d.jpeg",
            Length=122908
        },
        new tempFileInfo(){
            FileName="6cced92f-f800-473f-8e20-f784a68e5e67.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220829/6cced92f-f800-473f-8e20-f784a68e5e67.jpeg",
            Length=73170
        },
        new tempFileInfo(){
            FileName="fbbeea2d-3a59-42d1-9424-72b419ab6b68.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220829/fbbeea2d-3a59-42d1-9424-72b419ab6b68.jpeg",
            Length=64183,
        },
        new tempFileInfo() {
            FileName="4cd98c08-259d-4ca2-a9c1-d3771070f714.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220829/4cd98c08-259d-4ca2-a9c1-d3771070f714.jpeg",
            Length=78159,
        },
        new tempFileInfo() {
            FileName="14ad9f2b-fabe-4f88-ae8b-b39c9ba45dcd.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220829/14ad9f2b-fabe-4f88-ae8b-b39c9ba45dcd.jpeg",
            Length=86106
        },
        new tempFileInfo() {
            FileName="9f33fe27-8b8e-40c6-9780-0e1f4f72eb62.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220829/9f33fe27-8b8e-40c6-9780-0e1f4f72eb62.jpeg",
            Length=86106
        },
        new tempFileInfo(){
            FileName="29710711-4a35-44df-8429-8383998219dc.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220829/29710711-4a35-44df-8429-8383998219dc.jpeg",
            Length=24406
        },
        new tempFileInfo(){
            FileName="b6414a24-f86b-4723-8e8c-18eaa15b88dc.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220829/b6414a24-f86b-4723-8e8c-18eaa15b88dc.jpeg",
            Length=92894
        },
        new tempFileInfo(){
            FileName="875afe10-1067-48fb-9d89-80eca42feb0e.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220826/875afe10-1067-48fb-9d89-80eca42feb0e.jpeg",
            Length=78159
        },
        new tempFileInfo(){
            FileName="3c773766-943f-4423-9e9c-50ab30a48260.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220826/3c773766-943f-4423-9e9c-50ab30a48260.jpeg",
            Length = 73170
         },
         new tempFileInfo(){
             FileName="ace53c65-b811-4d70-a1ec-b753d9540540.jpeg",
             Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220826/ace53c65-b811-4d70-a1ec-b753d9540540.jpeg",
             Length=86106
         },
         new tempFileInfo(){
            FileName="7fa81ec5-74a4-4b19-8f8a-e9af19fb4ef6.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220826/7fa81ec5-74a4-4b19-8f8a-e9af19fb4ef6.jpeg",
            Length=24406,
         },
         new tempFileInfo(){
            FileName="b3eb605a-37ef-442f-a356-1accc3b5a3c2.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220826/b3eb605a-37ef-442f-a356-1accc3b5a3c2.jpeg",
            Length=86106
         },
         new tempFileInfo(){
            FileName="887aa3d1-a62b-4f66-bfb3-a4b3d12ec9c2.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220826/887aa3d1-a62b-4f66-bfb3-a4b3d12ec9c2.jpeg",
            Length=78159
         },
         new tempFileInfo(){
            FileName="2c352760-76cd-4929-868c-4eb90e27da92.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220826/2c352760-76cd-4929-868c-4eb90e27da92.jpeg",
            Length=86106
         },
         new tempFileInfo(){
            FileName="cd2ecd57-7f62-4caa-a961-addef521a6d7.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220826/cd2ecd57-7f62-4caa-a961-addef521a6d7.jpeg",
            Length=86106
         },
         new tempFileInfo(){
            FileName="cf427186-aab8-4f25-b0e7-b322448df352.jpeg",
            Url="http://8.142.16.236:6060/images/cropsTraceFiles/20220826/cf427186-aab8-4f25-b0e7-b322448df352.jpeg",
            Length=78159
         }
     };

    //生长数据
    List<GrowthInfo> growthInfos = new List<GrowthInfo>();

    //农作物图片
    List<System.IO.FileInfo> cropsFiles = new List<System.IO.FileInfo>();

    //初始化数据库帮助类
    MOIRepository m_dbHelper = new MOIRepository(MOIConnStr);

    //返回消息
    Message dbResult = new Message();

    var snowId = new IdWorker(1, 1);

    //测试数据
    List<cropsTraceDataAccess.Model.FileInfo> testData = new List<cropsTraceDataAccess.Model.FileInfo>();
    #endregion

    #region 读取文件
    Console.WriteLine("读取农作物图片开始");
    string[] allFilePath = Directory.GetFiles(cropsImagePath);
    foreach (string file in allFilePath)
    {
        cropsFiles.Add(new System.IO.FileInfo(file));
    }
    Console.WriteLine("读取农作物图片结束");
    #endregion

    #region 读取生长数据
    Console.WriteLine("开始读取生长数据");
    growthInfos = m_dbHelper.QueryGrowthInfo(SqlWhere, out message);
    if (growthInfos == null || growthInfos.Count == 0)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Console.WriteLine($"读取生长数据出错，原因[{message}]");
            return;
        }
        else
        {
            Console.WriteLine("读取生长数据出错，原因[没有配置生长数据，请先配置生长数据]");
            return;
        }
    }
    Console.WriteLine("读取生长数据结束");
    #endregion

    #region 生成文件数据
    Console.WriteLine("开始生成文件数据");
    for (int i = 0; i < growthInfos.Count; i++)
    {
        GrowthInfo growthInfo = growthInfos[i];
        for (var j = 0; j < testDataCount; j++)
        {
            int index = new Random().Next(0, cropsFiles.Count);
            int fileIndex = new Random().Next(0, growFileUrls.Count);
            System.IO.FileInfo file = cropsFiles[index];
            tempFileInfo tempFile = growFileUrls[fileIndex];
            testData.Add(new cropsTraceDataAccess.Model.FileInfo()
            {
                FileId = snowId.NextId(),
                FileName = tempFile.FileName,
                FileLength = tempFile.Length,
                FileUrl = tempFile.Url,
                CropsId = growthInfo.CropsId,
                GrowthRecordId = growthInfo.RecordId,
                CreatedDateTime = DateTime.Now.AddSeconds(i),
                ShowParamJson = "{\"空气温度\":\"" + (15+j) + "\",\"空气湿度\":\"" + (35+j) + "\",\"风向\":\"西北风\",\"风力\":\"2到3级\",\"土壤温度\":\""+(32+j)+"\",\"土壤湿度\":\""+(30+j)+"%\"}"
            });
        }
    }
    Console.WriteLine("生成文件数据结束");
    #endregion

    #region 保存数据
    dbResult = await m_dbHelper.InsertFileInfo(testData);
    if (dbResult.Successful)
        Console.WriteLine("保存成功");
    else
        Console.WriteLine($"保存失败,原因[{dbResult.Content}]");
    #endregion

    Console.Read();
}


static void Main()
{
    IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
    //string MOIConnStr = configuration["ConnectionStrings:MOIConnStr"];
    //string MOIConnStr = "Data Source=DESKTOP-55GUG4F\\SQLSERVER2017;Initial Catalog=cropsTrace;User ID=sa;Password=yamsql126;Encrypt=True;TrustServerCertificate=True";
    string MOIConnStr = "Data Source=DESKTOP-NH96BIF\\MSSQL2017;Initial Catalog=cropsTrace1;User ID=sa;Password=abc123456;Encrypt=True;TrustServerCertificate=True";
    //InsertShowFieldsTestData(MOIConnStr);
    //InsertPumpHouseInfo(MOIConnStr);
    Console.WriteLine(MOIConnStr);
    //InsertGrowthInfoTestData(MOIConnStr);
    InsertFileInfoTestData(MOIConnStr);
    Console.ReadLine();
}

Main();