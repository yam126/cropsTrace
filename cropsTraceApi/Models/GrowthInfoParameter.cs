namespace cropsTraceApi.Models
{
    /// <summary>
    /// 生长信息参数
    /// </summary>
    public class GrowthInfoParameter
    {
        /// <summary>
        /// 生长信息编号
        /// </summary>
        public string RecordId { get; set; }


        /// <summary>
        /// 泵房编号
        /// </summary>
        public string PumpId { get; set; }


        /// <summary>
        /// 农作物编号
        /// </summary>
        public string CropsId { get; set; }


        /// <summary>
        /// 生长信息名称
        /// </summary>
        public string GrowthName { get; set; }


        /// <summary>
        /// 位置名称
        /// </summary>
        public string LandName { get; set; }


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
        /// 创建时间
        /// </summary>
        public string CreatedDateTime { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedDateTime { get; set; }


        /// <summary>
        /// 泵房名称
        /// </summary>
        public string PumpHouseName { get; set; }


        /// <summary>
        /// 农作物名称
        /// </summary>
        public string SeedName { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyId { get; set; }
    }
}
