using AutoMapper;
using Common;
using cropsTraceApi.Models;
using cropsTraceDataAccess.Model;

namespace cropsTraceApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region 农作物实体映射
            CreateMap<SeedInfoParameter, SeedInfo>()
                .ForMember(target => target.CropsId, map => map.MapFrom(source => string.IsNullOrEmpty(source.CropsId) ? 0 : Convert.ToInt64(source.CropsId)))
                .ForMember(target => target.CompanyId, map => map.MapFrom(source => string.IsNullOrEmpty(source.CompanyId) ? 0 : Convert.ToInt32(source.CompanyId)))
                .ForMember(target => target.SeedName, map => map.MapFrom(source => string.IsNullOrEmpty(source.SeedName) ? string.Empty : source.SeedName))
                .ForMember(target => target.SeedVariety, map => map.MapFrom(source => string.IsNullOrEmpty(source.SeedVariety) ? string.Empty : source.SeedVariety))
                .ForMember(target => target.SeedClass, map => map.MapFrom(source => string.IsNullOrEmpty(source.SeedClass) ? string.Empty : source.SeedClass))
                .ForMember(target => target.PlantArea, map => map.MapFrom(source => string.IsNullOrEmpty(source.PlantArea) ? 0 : Convert.ToDecimal(source.PlantArea)))
                .ForMember(target => target.CreatedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.CreatedDateTime).ToString("yyyy-MM-dd")))
                .ForMember(target => target.ModifiedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.ModifiedDateTime).ToString("yyyy-MM-dd")))
                .ForMember(target => target.Introduce, map => map.MapFrom(source => string.IsNullOrEmpty(source.Introduce) ? string.Empty : source.Introduce))
                .ForMember(target => target.SoilType, map => map.MapFrom(source => string.IsNullOrEmpty(source.SoilType) ? string.Empty : source.SoilType))
                .ReverseMap();
            #endregion

            #region 显示字段实体映射
            CreateMap<ShowFieldsParameter, ShowFields>()
                .ForMember(target => target.RecordId, map => map.MapFrom(source => Utils.StrToLong(source.RecordId)))
                .ForMember(target => target.CompanyId, map => map.MapFrom(source => Utils.StrToInt(source.CompanyId, 0)))
                .ForMember(target => target.Device, map => map.MapFrom(source => source.Device))
                .ForMember(target => target.PointId, map => map.MapFrom(source => source.PointId))
                .ForMember(target => target.FieldName, map => map.MapFrom(source => source.FieldName))
                .ForMember(target => target.ShowFieldName, map => map.MapFrom(source => source.ShowFieldName))
                .ForMember(target => target.Unit, map => map.MapFrom(source => source.Unit))
                .ForMember(target => target.IsShow, map => map.MapFrom(source => Utils.StrToInt(source.IsShow, 0)))
                .ForMember(target => target.id, map => map.MapFrom(source => Utils.StrToInt(source.id, 0)))
                .ForMember(target => target.value, map => map.MapFrom(source =>source.value))
                .ForMember(target => target.updateTime, map => map.MapFrom(source => source.updateTime))
                .ForMember(target => target.deviceCode, map => map.MapFrom(source => source.deviceCode))
                .ForMember(target => target.deviceName, map => map.MapFrom(source => source.deviceName))
                .ForMember(target => target.name, map => map.MapFrom(source => source.name))
                .ReverseMap();
            #endregion

            #region 生长信息字段实体映射

            #region 生长信息视图
            CreateMap<GrowthInfoParameter, vw_GrowthInfo>()
                .ForMember(target => target.RecordId, map => map.MapFrom(source => Utils.StrToLong(source.RecordId)))
                .ForMember(target => target.PumpId, map => map.MapFrom(source => Utils.StrToLong(source.PumpId)))
                .ForMember(target => target.CropsId, map => map.MapFrom(source => Utils.StrToLong(source.CropsId)))
                .ForMember(target => target.GrowthName, map => map.MapFrom(source => source.GrowthName))
                .ForMember(target => target.LandName, map => map.MapFrom(source => source.LandName))
                .ForMember(target => target.PlantHeight, map => map.MapFrom(source => Utils.StrToDecimal(source.PlantHeight)))
                .ForMember(target => target.DBH, map => map.MapFrom(source => Utils.StrToDecimal(source.DBH)))
                .ForMember(target => target.NumberOfBlades, map => map.MapFrom(source => Utils.StrToInt(source.NumberOfBlades, 0)))
                .ForMember(target => target.EmergenceRate, map => map.MapFrom(source => Utils.StrToDecimal(source.EmergenceRate)))
                .ForMember(target => target.CreatedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.CreatedDateTime)))
                .ForMember(target => target.ModifiedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.ModifiedDateTime)))
                .ForMember(target => target.PumpHouseName, map => map.MapFrom(source => source.PumpHouseName))
                .ForMember(target => target.SeedName, map => map.MapFrom(source => source.SeedName))
                .ForMember(target => target.CompanyId, map => map.MapFrom(source => Utils.StrToInt(source.CompanyId, 0)))
                .ReverseMap();
            #endregion

            #region 生长信息视图加强
            CreateMap<GrowthInfoViewPlusParameter, vw_GrowthInfo_Plus>()
                .ForMember(target => target.RecordId, map => map.MapFrom(source => Utils.StrToLong(source.RecordId)))
                .ForMember(target => target.PumpId, map => map.MapFrom(source => Utils.StrToLong(source.PumpId)))
                .ForMember(target => target.CropsId, map => map.MapFrom(source => Utils.StrToLong(source.CropsId)))
                .ForMember(target => target.GrowthName, map => map.MapFrom(source => source.GrowthName))
                .ForMember(target => target.LandName, map => map.MapFrom(source => source.LandName))
                .ForMember(target => target.PlantHeight, map => map.MapFrom(source => Utils.StrToDecimal(source.PlantHeight)))
                .ForMember(target => target.DBH, map => map.MapFrom(source => Utils.StrToDecimal(source.DBH)))
                .ForMember(target => target.NumberOfBlades, map => map.MapFrom(source => Utils.StrToInt(source.NumberOfBlades, 0)))
                .ForMember(target => target.EmergenceRate, map => map.MapFrom(source => Utils.StrToDecimal(source.EmergenceRate)))
                .ForMember(target => target.CreatedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.CreatedDateTime)))
                .ForMember(target => target.ModifiedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.ModifiedDateTime)))
                .ForMember(target => target.PumpHouseName, map => map.MapFrom(source => source.PumpHouseName))
                .ForMember(target => target.SeedName, map => map.MapFrom(source => source.SeedName))
                .ForMember(target => target.CompanyId, map => map.MapFrom(source => Utils.StrToInt(source.CompanyId, 0)))
                .ForMember(target => target.SeedVariety, map => map.MapFrom(source => source.SeedVariety))
                .ForMember(target => target.PlantArea, map => map.MapFrom(source => Utils.StrToDecimal(source.PlantArea)))
                .ForMember(target => target.Introduce, map => map.MapFrom(source => source.Introduce))
                .ForMember(target => target.SoilType, map => map.MapFrom(source => source.SoilType))
                .ForMember(target => target.FileUrl, map => map.MapFrom(source => source.FileUrl))
                .ForMember(target => target.FileName, map => map.MapFrom(source => source.FileName))
                .ForMember(target => target.FileLength, map => map.MapFrom(source => Utils.StrToLong(source.FileLength)))
                .ForMember(target => target.ShowParamJson, map => map.MapFrom(source => source.ShowParamJson))
                .ReverseMap();
            #endregion

            #region 生长信息实体
            CreateMap<GrowthInfoParameter, GrowthInfo>()
                .ForMember(target => target.RecordId, map => map.MapFrom(source => Utils.StrToLong(source.RecordId)))
                .ForMember(target => target.PumpId, map => map.MapFrom(source => Utils.StrToLong(source.PumpId)))
                .ForMember(target => target.CropsId, map => map.MapFrom(source => Utils.StrToLong(source.CropsId)))
                .ForMember(target => target.GrowthName, map => map.MapFrom(source => source.GrowthName))
                .ForMember(target => target.LandName, map => map.MapFrom(source => source.LandName))
                .ForMember(target => target.PlantHeight, map => map.MapFrom(source => Utils.StrToDecimal(source.PlantHeight)))
                .ForMember(target => target.DBH, map => map.MapFrom(source => Utils.StrToDecimal(source.DBH)))
                .ForMember(target => target.NumberOfBlades, map => map.MapFrom(source => Utils.StrToInt(source.NumberOfBlades, 0)))
                .ForMember(target => target.EmergenceRate, map => map.MapFrom(source => Utils.StrToDecimal(source.EmergenceRate)))
                .ForMember(target => target.CreatedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.CreatedDateTime)))
                .ForMember(target => target.ModifiedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.ModifiedDateTime)))
                .ReverseMap();
            #endregion

            #endregion

            #region 泵房信息
            CreateMap<PumpHouseInfoParameter, PumpHouseInfo>()
                .ForMember(target => target.PumpId, map => map.MapFrom(source => Utils.StrToLong(source.PumpId)))
                .ForMember(target => target.CompanyId, map => map.MapFrom(source => Utils.StrToInt(source.CompanyId, 0)))
                .ForMember(target => target.PumpHouseName, map => map.MapFrom(source => source.PumpHouseName))
                .ForMember(target => target.PumpHouseClass, map => map.MapFrom(source => source.PumpHouseClass))
                .ForMember(target => target.PersonIinCharge, map => map.MapFrom(source => source.PersonIinCharge))
                .ForMember(target => target.CreatedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.CreatedDateTime)))
                .ForMember(target => target.ModifiedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.ModifiedDateTime)))
                .ForMember(target => target.MonitoringAddress, map => map.MapFrom(source => source.MonitoringAddress))
                .ReverseMap();
            #endregion

            #region 文件信息
            CreateMap<FileInfoParameter, cropsTraceDataAccess.Model.FileInfo>()
                .ForMember(target => target.FileId, map => map.MapFrom(source => Utils.StrToLong(source.FileId)))
                .ForMember(target => target.CropsId, map => map.MapFrom(source => Utils.StrToLong(source.CropsId)))
                .ForMember(target => target.GrowthRecordId, map => map.MapFrom(source => Utils.StrToLong(source.GrowthRecordId)))
                .ForMember(target => target.FileName, map => map.MapFrom(source => source.FileName))
                .ForMember(target => target.FileUrl, map => map.MapFrom(source => source.FileUrl))
                .ForMember(target => target.FileLength, map => map.MapFrom(source => Utils.StrToLong(source.FileLength)))
                .ForMember(target => target.CreatedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.CreatedDateTime)))
                .ForMember(target => target.ShowParamJson, map => map.MapFrom(source => source.ShowParamJson))
                .ReverseMap();
            #endregion

            #region 库存信息
            CreateMap<StorageInfoParameter, cropsTraceDataAccess.Model.StorageInfo>()
                .ForMember(target => target.CropsId, map => map.MapFrom(source => Utils.StrToLong(source.CropsId)))
                .ForMember(target => target.BatchNumber, map => map.MapFrom(source => Utils.StrToLong(source.BatchNumber)))
                .ForMember(target => target.CompanyId, map => map.MapFrom(source => Utils.StrToInt(source.CompanyId, 0)))
                .ForMember(target => target.InQuantity, map => map.MapFrom(source => Utils.StrToDecimal(source.InQuantity)))
                .ForMember(target => target.OutQuantity, map => map.MapFrom(source => Utils.StrToDecimal(source.OutQuantity)))
                .ForMember(target => target.InDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.InDateTime)))
                .ForMember(target => target.OutDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.OutDateTime)))
                .ForMember(target => target.WarehouseNumber, map => map.MapFrom(source => source.WarehouseNumber))
                .ForMember(target => target.WarehouseTemperature, map => map.MapFrom(source => Utils.StrToDecimal(source.WarehouseTemperature)))
                .ForMember(target => target.WarehouseHumidity, map => map.MapFrom(source => Utils.StrToDecimal(source.WarehouseHumidity)))
                .ForMember(target => target.CreatedDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.CreatedDateTime)))
                .ForMember(target => target.validityDateTime, map => map.MapFrom(source => Utils.StrToDateTime(source.validityDateTime)))
                .ForMember(target => target.State, map => map.MapFrom(source => Utils.StrToInt(source.State, 0)))
                .ReverseMap();
            #endregion

            #region 公司信息表参数转换
            CreateMap<CompanyInfoParameter, CompanyInfo>()
              .ForMember(target => target.companyId, map => map.MapFrom(source => string.IsNullOrEmpty(source.companyId) ? 0 : Convert.ToInt64(source.companyId)))
              .ForMember(target => target.companyName, map => map.MapFrom(source => string.IsNullOrEmpty(source.companyName) ? string.Empty : Convert.ToString(source.companyName).Trim()))
              .ForMember(target => target.barcodeUrl, map => map.MapFrom(source => string.IsNullOrEmpty(source.barcodeUrl) ? string.Empty : Convert.ToString(source.barcodeUrl).Trim()))
              .ForMember(target => target.Backup01, map => map.MapFrom(source => string.IsNullOrEmpty(source.Backup01) ? string.Empty : Convert.ToString(source.Backup01).Trim()))
              .ForMember(target => target.Backup02, map => map.MapFrom(source => string.IsNullOrEmpty(source.Backup02) ? string.Empty : Convert.ToString(source.Backup02).Trim()))
              .ForMember(target => target.Backup03, map => map.MapFrom(source => string.IsNullOrEmpty(source.Backup03) ? string.Empty : Convert.ToString(source.Backup03).Trim()))
              .ForMember(target => target.Backup04, map => map.MapFrom(source => string.IsNullOrEmpty(source.Backup04) ? string.Empty : Convert.ToString(source.Backup04).Trim()))
              .ForMember(target => target.Backup05, map => map.MapFrom(source => string.IsNullOrEmpty(source.Backup05) ? string.Empty : Convert.ToString(source.Backup05).Trim()))
              .ForMember(target => target.Backup06, map => map.MapFrom(source => string.IsNullOrEmpty(source.Backup06) ? string.Empty : Convert.ToString(source.Backup06).Trim()))
              .ForMember(target => target.created, map => map.MapFrom(source => string.IsNullOrEmpty(source.created) ? string.Empty : Convert.ToString(source.created).Trim()))
              .ForMember(target => target.CreatedTime, map => map.MapFrom(source => Utils.StrToDateTime(source.CreatedTime)))
              .ForMember(target => target.Modifier, map => map.MapFrom(source => string.IsNullOrEmpty(source.Modifier) ? string.Empty : Convert.ToString(source.Modifier).Trim()))
              .ForMember(target => target.ModifiedTime, map => map.MapFrom(source => Utils.StrToDateTime(source.ModifiedTime)))
              .ReverseMap();
            #endregion
        }
    }
}
