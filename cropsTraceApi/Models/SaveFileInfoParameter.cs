namespace cropsTraceApi.Models
{
    /// <summary>
    /// 保存文件信息参数
    /// </summary>
    public class BatchSaveFileInfoParameter
    {
        /// <summary>
        /// 农作物编号
        /// </summary>
        public string CropsId { get; set; }

        /// <summary>
        /// 生长信息编号
        /// </summary>
        public string GrowthInfoId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// token数据
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 要保存的文件数据
        /// </summary>
        public List<cropsTraceApi.Models.FileInfoParameter> saveFilesData { get; set; }
    }
}
