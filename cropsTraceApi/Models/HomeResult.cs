namespace cropsTraceApi.Models
{
    #region 首页返回结构

    /// <summary>
    /// 首页返回的数据
    /// </summary>
    public class HomeResult
    {
        /// <summary>
        /// 泵房数据列表
        /// </summary>
        public List<HomePumpHouseResult> PumpHouse { get; set; }
    }

    /// <summary>
    /// 泵房数据返回
    /// </summary>
    public class HomePumpHouseResult 
    {
        /// <summary>
        /// 泵房数据
        /// </summary>
        public PumpHouseInfoParameter PumpHouseInfo { get; set; }


        /// <summary>
        /// 农作物列表
        /// </summary>
        public HomeSeedInfoResult seedInfo { get; set; }

    }

    /// <summary>
    /// 农作物返回数据
    /// </summary>
    public class HomeSeedInfoResult 
    {
        /// <summary>
        /// 农作物数据
        /// </summary>
        public SeedInfoParameter SeedInfo { get; set; }

        /// <summary>
        /// 地块名称
        /// </summary>
        public string landName { get; set; }

        /// <summary>
        /// 株高
        /// </summary>
        public string PlantHeight { get; set; }


        /// <summary>
        /// 胸径
        /// </summary>
        public string DBH { get; set; }


        /// <summary>
        /// 叶片数
        /// </summary>
        public string NumberOfBlades { get; set; }


        /// <summary>
        /// 出苗率
        /// </summary>
        public string EmergenceRate { get; set; }

        /// <summary>
        /// 农作物关联的图片列表
        /// </summary>
        public List<HomeFileInfoResult> seedInfoFiles { get; set; }
    }

    /// <summary>
    /// 图片返回数据
    /// </summary>
    public class HomeFileInfoResult 
    {
        /// <summary>
        /// 文件数据
        /// </summary>
        public FileInfoParameter FileInfo { get; set; }
    }
    #endregion
}
