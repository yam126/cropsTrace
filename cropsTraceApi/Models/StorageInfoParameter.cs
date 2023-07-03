namespace cropsTraceApi.Models
{
    /// <summary>
    /// 库房信息参数
    /// </summary>
    public class StorageInfoParameter
    {
        /// <summary>
        ///农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
        /// </summary>
        public System.String CropsId { get; set; }

        /// <summary>
        ///入库批次号[雪花ID生成]
        /// </summary>
        public System.String BatchNumber { get; set; }

        /// <summary>
        ///公司编号
        /// </summary>
        public System.String CompanyId { get; set; }

        /// <summary>
        ///入库数量
        /// </summary>
        public System.String InQuantity { get; set; }

        /// <summary>
        ///出库数量
        /// </summary>
        public System.String OutQuantity { get; set; }

        /// <summary>
        ///入库时间
        /// </summary>
        public System.String InDateTime { get; set; }

        /// <summary>
        ///出库时间
        /// </summary>
        public System.String OutDateTime { get; set; }

        /// <summary>
        ///仓库编号
        /// </summary>
        public System.String WarehouseNumber { get; set; }

        /// <summary>
        ///仓库温度
        /// </summary>
        public System.String WarehouseTemperature { get; set; }

        /// <summary>
        ///仓库湿度
        /// </summary>
        public System.String WarehouseHumidity { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public System.String CreatedDateTime { get; set; }

        /// <summary>
        ///有效期
        /// </summary>
        public System.String validityDateTime { get; set; }

        /// <summary>
        ///状态[0-入库、1-出库]
        /// </summary>
        public System.String State { get; set; }
    }
}
