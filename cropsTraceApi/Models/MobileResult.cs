namespace cropsTraceApi.Models
{
    #region 手机返回结构

    /// <summary>
    /// 泵房数据返回
    /// </summary>
    public class MobileResult
    {
        /// <summary>
        /// 作物种类
        /// </summary>
        public System.String SeedName { get; set; }

        /// <summary>
        /// 种植年份
        /// </summary>
        public System.String PlantYear { get; set; }

        /// <summary>
        ///种植面积
        /// </summary>
        public System.String PlantArea { get; set; }

        /// <summary>
        /// 地块名称
        /// </summary>
        public System.String landName { get; set; }

        /// <summary>
        ///土壤类型
        /// </summary>
        public System.String SoilType { get; set; }

        /// <summary>
        ///泵房名称
        /// </summary>
        public System.String PumpHouseName { get; set; }

        /// <summary>
        /// 溯源编号
        /// </summary>
        public System.String traceNo { get; set; }

        /// <summary>
        /// 生长周期
        /// </summary>
        public List<MobileGrowthInfoResult> growthInfoResults { get; set; }
    }

    /// <summary>
    /// 生长周期返回值
    /// </summary>
    public class MobileGrowthInfoResult 
    {
        /// <summary>
        /// 周期名称
        /// </summary>
        public System.String GrowthName { get; set; }

        /// <summary>
        /// 文件返回值
        /// </summary>
        public List<MobileFileInfoResult> fileInfoResults { get; set; }
    }

    /// <summary>
    /// 文件返回值
    /// </summary>
    public class MobileFileInfoResult 
    {
        /// <summary>
        ///文件名
        /// </summary>
        public System.String FileName { get; set; }

        /// <summary>
        ///文件URL路径
        /// </summary>
        public System.String FileUrl { get; set; }

        /// <summary>
        ///文件大小
        /// </summary>
        public System.String FileLength { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public System.String CreatedDateTime { get; set; }

        /// <summary>
        ///显示参数JSON字符串
        /// </summary>
        public System.String ShowParamJson { get; set; }
    }
    #endregion
}
