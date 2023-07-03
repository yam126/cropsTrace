namespace cropsTraceApi.Models
{
    /// <summary>
    /// 泵房信息参数
    /// </summary>
    public class PumpHouseInfoParameter
    {
        /// <summary>
        ///泵房编号
        /// </summary>
        public System.String PumpId { get; set; }

        /// <summary>
        ///公司编号
        /// </summary>
        public System.String CompanyId { get; set; }

        /// <summary>
        ///泵房名称
        /// </summary>
        public System.String PumpHouseName { get; set; }

        /// <summary>
        ///泵房种类
        /// </summary>
        public System.String PumpHouseClass { get; set; }

        /// <summary>
        ///负责人
        /// </summary>
        public System.String PersonIinCharge { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public System.String CreatedDateTime { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        public System.String ModifiedDateTime { get; set; }

        /// <summary>
        /// 监控地址
        /// </summary>
        public System.String MonitoringAddress { get; set; }
    }
}
