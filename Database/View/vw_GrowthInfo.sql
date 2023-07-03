Create VIEW [dbo].[vw_GrowthInfo] AS SELECT
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
	SeedInfo.CompanyId
FROM
	GrowthInfo
	LEFT JOIN
	PumpHouseInfo
	ON 
		GrowthInfo.PumpId = PumpHouseInfo.PumpId
	LEFT JOIN
	SeedInfo
	ON 
		GrowthInfo.CropsId = SeedInfo.CropsId
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生长信息视图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生长信息编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'RecordId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'泵房编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'PumpId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'农作物编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'CropsId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生长信息名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'GrowthName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'位置名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'LandName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'株高' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'PlantHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'胸径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'DBH'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'叶片数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'NumberOfBlades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出苗率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'EmergenceRate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'ModifiedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'泵房名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'PumpHouseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'农作物名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_GrowthInfo', @level2type=N'COLUMN',@level2name=N'SeedName'