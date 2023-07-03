namespace cropsTraceApi.Models.PointsTimeValuesResult
{
    /// <summary>
    /// 测点时间返回项目
    /// </summary>
    public class ResultItem
    {
        /// <summary>
        /// 测点值
        /// </summary>
        public string value { get; set; }
        
        /// <summary>
        /// 测点时间
        /// </summary>
        public string time { get; set; }
    }

    /// <summary>
    /// 测点时间返回JSON
    /// </summary>
    public class Root
    {
        /// <summary>
        /// 接口状态
        /// </summary>
        public int status { get; set; }
        
        /// <summary>
        /// 错误消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 测点值集合
        /// </summary>
        public List<ResultItem> result { get; set; }
    }
}
