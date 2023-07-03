namespace cropsTraceApi.Models
{
    public class FileInfoParameter
    {
        /// <summary>
        ///文件编号[自增列因为目前只是本表查询]
        /// </summary>
        public System.String FileId { get; set; }

        /// <summary>
        ///农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
        /// </summary>
        public System.String CropsId { get; set; }

        /// <summary>
        ///周期记录编号[外键对应生长周期表GrowthInfo字段RecordId关系多对1]
        /// </summary>
        public System.String GrowthRecordId { get; set; }

        /// <summary>
        ///文件名
        /// </summary>
        public System.String FileName { get; set; }

        /// <summary>
        ///文件URL路径
        /// </summary>
        public System.String FileUrl { get; set; }

        /// <summary>
        ///文件大小
        /// </summary>
        public System.String FileLength { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public System.String CreatedDateTime { get; set; }

        /// <summary>
        ///显示参数JSON字符串
        /// </summary>
        public System.String ShowParamJson { get; set; }
    }
}
