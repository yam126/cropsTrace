namespace cropsTraceApi.Models
{
    /// <summary>
    /// 手机界面接口查询
    /// </summary>
    public class MobileResultParameters
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
        /// 泵房编号
        /// </summary>
        public string pumpHouseID { get; set; }
    }
}
