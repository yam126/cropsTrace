
create VIEW [dbo].[vw_GrowthInfo_Plus] AS SELECT
	GrowthInfo.RecordId, 
	GrowthInfo.PumpId, 
	GrowthInfo.CropsId, 
	GrowthInfo.GrowthName, 
	GrowthInfo.LandName, 
	GrowthInfo.PlantHeight, 
	GrowthInfo.DBH, 
	GrowthInfo.NumberOfBlades, 
	GrowthInfo.EmergenceRate, 
	GrowthInfo.CreatedDateTime, 
	GrowthInfo.ModifiedDateTime, 
	PumpHouseInfo.PumpHouseName, 
	SeedInfo.SeedName,
	SeedInfo.CompanyId,
	SeedInfo.SeedVariety,
	SeedInfo.PlantArea,
	SeedInfo.Introduce,
	SeedInfo.SoilType,
	FileInfo.FileUrl,
	FileInfo.FileName,
	FileInfo.FileLength,
	FileInfo.ShowParamJson
FROM
	FileInfo
	LEFT JOIN GrowthInfo on
		 FileInfo.GrowthRecordId=GrowthInfo.RecordId 
	LEFT JOIN
	SeedInfo
	ON 
		GrowthInfo.CropsId = SeedInfo.CropsId
	left join
	PumpHouseInfo
	on 
	   GrowthInfo.PumpId=PumpHouseInfo.PumpId

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生长信息视图扩展' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生长信息编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'RecordId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'泵房编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'PumpId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'农作物编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'CropsId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生长信息名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'GrowthName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'位置名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'LandName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'株高' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'PlantHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'胸径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'DBH'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'叶片数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'NumberOfBlades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出苗率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'EmergenceRate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'ModifiedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'泵房名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'PumpHouseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'农作物名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'SeedName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'种子品种' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'SeedVariety'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'种植面积' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'PlantArea'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'农作物介绍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'Introduce'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'土壤类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'SoilType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件URL地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'FileUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件长度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'FileLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo_Plus', @level2type=N'COLUMN',@level2name=N'ShowParamJson'