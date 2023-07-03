namespace cropsTraceApi.Models
{
    /// <summary>
    /// 生长信息加强参数
    /// </summary>
    public class GrowthInfoViewPlusParameter
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

        /// <summary>
        /// 种子品种
        /// </summary>
        public string SeedVariety { get; set; }


        /// <summary>
        /// 种植面积
        /// </summary>
        public string PlantArea { get; set; }


        /// <summary>
        /// 农作物介绍
        /// </summary>
        public string Introduce { get; set; }


        /// <summary>
        /// 土壤类型
        /// </summary>
        public string SoilType { get; set; }

        /// <summary>
        /// 文件URL地址
        /// </summary>
        public string FileUrl { get; set; }


        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        /// 文件长度
        /// </summary>
        public string FileLength { get; set; }


        /// <summary>
        /// 显示参数
        /// </summary>
        public string ShowParamJson { get; set; }
    }
}
