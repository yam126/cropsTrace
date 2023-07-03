namespace cropsTraceApi.Models
{
    /// <summary>
    /// 首页接口查询参数
    /// </summary>
    public class HomeResultParameters
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 农作物编号
        /// </summary>
        public string CropsId { get; set; }

        /// <summary>
        /// 生长周期名称
        /// </summary>
        public string growthName { get; set; }

        /// <summary>
        /// 泵房编号
        /// </summary>
        public string pumpHouseID { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string where { get; set; }
    }
}
