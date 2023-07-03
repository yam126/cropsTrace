using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cropsTraceDataAccess.Model
{
    /// <summary>
    /// 文件信息 
    /// </summary>
    [Serializable]
    public partial class FileInfo
    {
        /// <summary>
        ///文件编号[自增列因为目前只是本表查询]
        /// </summary>
        public System.Int64? FileId { get; set; }
        /// <summary>
        ///农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
        /// </summary>
        public System.Int64? CropsId { get; set; }
        /// <summary>
        ///周期记录编号[外键对应生长周期表GrowthInfo字段RecordId关系多对1]
        /// </summary>
        public System.Int64? GrowthRecordId { get; set; }
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
        public System.Int64? FileLength { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public System.DateTime? CreatedDateTime { get; set; }
        /// <summary>
        ///显示参数JSON字符串
        /// </summary>
        public System.String ShowParamJson { get; set; }
    }

    /// <summary>
    /// 生长信息 
    /// </summary>
    [Serializable]
    public partial class GrowthInfo
    {
        /// <summary>
        ///记录编号[用雪花ID生成]
        /// </summary>
        public System.Int64? RecordId { get; set; }
        /// <summary>
        ///泵房编号[外键值对应PumpHouseInfo表字段PumpId多对1]
        /// </summary>
        public System.Int64? PumpId { get; set; }
        /// <summary>
        ///农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
        /// </summary>
        public System.Int64? CropsId { get; set; }
        /// <summary>
        ///周期名称
        /// </summary>
        public System.String GrowthName { get; set; }
        /// <summary>
        ///位置名称
        /// </summary>
        public System.String LandName { get; set; }
        /// <summary>
        ///株高
        /// </summary>
        public System.Decimal? PlantHeight { get; set; }
        /// <summary>
        ///胸径
        /// </summary>
        public System.Decimal? DBH { get; set; }
        /// <summary>
        ///叶片数
        /// </summary>
        public System.Int32? NumberOfBlades { get; set; }
        /// <summary>
        ///出苗率
        /// </summary>
        public System.Decimal? EmergenceRate { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public System.DateTime? CreatedDateTime { get; set; }
        /// <summary>
        ///修改时间
        /// </summary>
        public System.DateTime? ModifiedDateTime { get; set; }
    }

    /// <summary>
    /// 泵房信息[此表数据通过外部接口获取而来] 
    /// </summary>
    [Serializable]
    public partial class PumpHouseInfo
    {
        /// <summary>
        ///泵房编号
        /// </summary>
        public System.Int64? PumpId { get; set; }
        /// <summary>
        ///公司编号
        /// </summary>
        public System.Int32? CompanyId { get; set; }
        /// <summary>
        ///泵房名称
        /// </summary>
        public System.String PumpHouseName { get; set; }
        /// <summary>
        ///泵房种类
        /// </summary>
        public System.String PumpHouseClass { get; set; }
        /// <summary>
        ///负责人
        /// </summary>
        public System.String PersonIinCharge { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public System.DateTime? CreatedDateTime { get; set; }
        /// <summary>
        ///修改时间
        /// </summary>
        public System.DateTime? ModifiedDateTime { get; set; }
        /// <summary>
        ///监控地址
        /// </summary>
        public System.String MonitoringAddress { get; set; }

    }

    /// <summary>
    /// 种子信息[农作物信息] 
    /// </summary>
    [Serializable]
    public partial class SeedInfo
    {
        /// <summary>
        ///农作物编号[雪花ID生成]
        /// </summary>
        public System.Int64? CropsId { get; set; }
        /// <summary>
        ///公司编号[对应登录者编号]
        /// </summary>
        public System.Int32? CompanyId { get; set; }
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
        public System.Decimal? PlantArea { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public System.DateTime? CreatedDateTime { get; set; }
        /// <summary>
        ///修改时间
        /// </summary>
        public System.DateTime? ModifiedDateTime { get; set; }
        /// <summary>
        ///农作物介绍
        /// </summary>
        public System.String Introduce { get; set; }
        /// <summary>
        ///土壤类型
        /// </summary>
        public System.String SoilType { get; set; }
    }

    /// <summary>
    /// 显示字段表 
    /// </summary>
    [Serializable]
    public partial class ShowFields
    {
        /// <summary>
        ///记录编号
        /// </summary>
        public System.Int64? RecordId { get; set; }
        /// <summary>
        ///公司编号
        /// </summary>
        public System.Int32? CompanyId { get; set; }
        /// <summary>
        ///设备
        /// </summary>
        public System.String Device { get; set; }
        /// <summary>
        ///测点
        /// </summary>
        public System.Int64? PointId { get; set; }
        /// <summary>
        ///字段名
        /// </summary>
        public System.String FieldName { get; set; }
        /// <summary>
        ///界面显示名称
        /// </summary>
        public System.String ShowFieldName { get; set; }
        /// <summary>
        ///单位
        /// </summary>
        public System.String Unit { get; set; }
        /// <summary>
        ///是否显示_0不显示_1显示
        /// </summary>
        public System.Int32? IsShow { get; set; }
        /// <summary>
        ///设备编号
        /// </summary>
        public System.Int32? id { get; set; }
        /// <summary>
        ///设备值
        /// </summary>
        public System.String value { get; set; }
        /// <summary>
        ///更新时间
        /// </summary>
        public System.String updateTime { get; set; }
        /// <summary>
        ///设备编码
        /// </summary>
        public System.String deviceCode { get; set; }
        /// <summary>
        ///设备名称
        /// </summary>
        public System.String deviceName { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public System.String name { get; set; }
    }

    /// <summary>
    /// 库房信息 
    /// </summary>
    [Serializable]
    public partial class StorageInfo
    {
        /// <summary>
        ///农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
        /// </summary>
        public System.Int64? CropsId { get; set; }
        /// <summary>
        ///入库批次号[雪花ID生成]
        /// </summary>
        public System.Int64? BatchNumber { get; set; }
        /// <summary>
        ///公司编号
        /// </summary>
        public System.Int32? CompanyId { get; set; }
        /// <summary>
        ///入库数量
        /// </summary>
        public System.Decimal? InQuantity { get; set; }
        /// <summary>
        ///出库数量
        /// </summary>
        public System.Decimal? OutQuantity { get; set; }
        /// <summary>
        ///入库时间
        /// </summary>
        public System.DateTime? InDateTime { get; set; }
        /// <summary>
        ///出库时间
        /// </summary>
        public System.DateTime? OutDateTime { get; set; }
        /// <summary>
        ///仓库编号
        /// </summary>
        public System.String WarehouseNumber { get; set; }
        /// <summary>
        ///仓库温度
        /// </summary>
        public System.Decimal? WarehouseTemperature { get; set; }
        /// <summary>
        ///仓库湿度
        /// </summary>
        public System.Decimal? WarehouseHumidity { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public System.DateTime? CreatedDateTime { get; set; }
        /// <summary>
        ///有效期
        /// </summary>
        public System.DateTime? validityDateTime { get; set; }
        /// <summary>
        ///状态[0-入库、1-出库]
        /// </summary>
        public System.Int32? State { get; set; }
    }

    /// <summary>
    /// 生长信息视图
    /// </summary>
    [Serializable]
    public class vw_GrowthInfo
    {


        /// <summary>
        /// 生长信息编号
        /// </summary>
        public long RecordId { get; set; }


        /// <summary>
        /// 泵房编号
        /// </summary>
        public long PumpId { get; set; }


        /// <summary>
        /// 农作物编号
        /// </summary>
        public long CropsId { get; set; }


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
        public decimal PlantHeight { get; set; }


        /// <summary>
        /// 胸径
        /// </summary>
        public decimal DBH { get; set; }


        /// <summary>
        /// 叶片数
        /// </summary>
        public int NumberOfBlades { get; set; }


        /// <summary>
        /// 出苗率
        /// </summary>
        public decimal EmergenceRate { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDateTime { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedDateTime { get; set; }


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
        public int CompanyId { get; set; }

    }

    /// <summary>
    /// 生长信息视图扩展
    /// </summary>
    [Serializable]
    public class vw_GrowthInfo_Plus
    {


        /// <summary>
        /// 生长信息编号
        /// </summary>
        public long RecordId { get; set; }


        /// <summary>
        /// 泵房编号
        /// </summary>
        public long PumpId { get; set; }


        /// <summary>
        /// 农作物编号
        /// </summary>
        public long CropsId { get; set; }


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
        public decimal PlantHeight { get; set; }


        /// <summary>
        /// 胸径
        /// </summary>
        public decimal DBH { get; set; }


        /// <summary>
        /// 叶片数
        /// </summary>
        public int NumberOfBlades { get; set; }


        /// <summary>
        /// 出苗率
        /// </summary>
        public decimal EmergenceRate { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDateTime { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedDateTime { get; set; }


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
        public int CompanyId { get; set; }


        /// <summary>
        /// 种子品种
        /// </summary>
        public string SeedVariety { get; set; }


        /// <summary>
        /// 种植面积
        /// </summary>
        public decimal PlantArea { get; set; }


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
        public long FileLength { get; set; }


        /// <summary>
        /// 显示参数
        /// </summary>
        public string ShowParamJson { get; set; }

    }

    /// <summary>
    /// 公司信息表 
    /// </summary>
    [Serializable]
    public partial class CompanyInfo
    {

        /// <summary>
        ///公司编号
        /// </summary>
        public System.Int64? companyId { get; set; }

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
        public System.DateTime? CreatedTime { get; set; }

        /// <summary>
        ///修改人
        /// </summary>
        public System.String? Modifier { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        public System.DateTime? ModifiedTime { get; set; }
    }
}
