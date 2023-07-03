namespace cropsTraceApi.Models
{
    /// <summary>
    /// 公司信息表
    /// </summary>
    public class CompanyInfoParameter
    {
        /// <summary>
        ///公司编号
        /// </summary>
        public System.String? companyId { get; set; }

        /// <summary>
        ///公司名称
        /// </summary>
        public System.String? companyName { get; set; }

        /// <summary>
        ///二维码URL
        /// </summary>
        public System.String? barcodeUrl { get; set; }

        /// <summary>
        ///备用字段01
        /// </summary>
        public System.String? Backup01 { get; set; }

        /// <summary>
        ///备用字段02
        /// </summary>
        public System.String? Backup02 { get; set; }

        /// <summary>
        ///备用字段03
        /// </summary>
        public System.String? Backup03 { get; set; }

        /// <summary>
        ///备用字段04
        /// </summary>
        public System.String? Backup04 { get; set; }

        /// <summary>
        ///备用字段05
        /// </summary>
        public System.String? Backup05 { get; set; }

        /// <summary>
        ///备用字段06
        /// </summary>
        public System.String? Backup06 { get; set; }

        /// <summary>
        ///创建人
        /// </summary>
        public System.String? created { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public System.String? CreatedTime { get; set; }

        /// <summary>
        ///修改人
        /// </summary>
        public System.String? Modifier { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        public System.String? ModifiedTime { get; set; }
    }
}
