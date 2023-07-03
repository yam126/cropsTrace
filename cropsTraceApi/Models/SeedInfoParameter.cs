namespace cropsTraceApi.Models
{
   /// <summary>
   /// 农作物信息参数
   /// </summary>
    public class SeedInfoParameter
    {
        /// <summary>
        ///农作物编号[雪花ID生成]
        /// </summary>
        public System.String CropsId { get; set; }

        /// <summary>
        ///公司编号[对应登录者编号]
        /// </summary>
        public System.String CompanyId { get; set; }

        /// <summary>
        ///种子名称
        /// </summary>
        public System.String SeedName { get; set; }

        /// <summary>
        ///种子品种
        /// </summary>
        public System.String SeedVariety { get; set; }

        /// <summary>
        ///种子类型
        /// </summary>
        public System.String SeedClass { get; set; }

        /// <summary>
        ///种植面积
        /// </summary>
        public System.String PlantArea { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.String CreatedDateTime { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        public System.String ModifiedDateTime { get; set; }

        /// <summary>
        ///农作物介绍
        /// </summary>
        public System.String Introduce { get; set; }

        /// <summary>
        ///土壤类型
        /// </summary>
        public System.String SoilType { get; set; }
    }
}
