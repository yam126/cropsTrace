namespace cropsTraceApi.Models
{
    /// <summary>
    /// 设置是否显示字段参数
    /// </summary>
    public class SetIsShowFieldsParameter
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 选中记录编号数组
        /// </summary>
        public string[] RecordIds { get; set; }

        /// <summary>
        /// 未选中记录编号数组
        /// </summary>
        public string[] unSelectedRecordIds { get; set; }

        /// <summary>
        /// 是否显示默认为0
        /// </summary>
        public string isShow { get; set; } = "0";
    }
}
