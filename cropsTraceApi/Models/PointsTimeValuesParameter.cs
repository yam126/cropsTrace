namespace cropsTraceApi.Models
{
    /// <summary>
    /// 测点值参数
    /// </summary>
    public class PointsTimeValuesParameter
    {
        /// <summary>
        /// 测点ID集合
        /// </summary>
        public List<int> ids { get; set; }

        /// <summary>
        /// 时间字符串
        /// </summary>
        public string time { get; set; }
    }
}
