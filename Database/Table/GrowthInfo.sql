CREATE TABLE [dbo].[GrowthInfo]
(
	[RecordId] BIGINT NOT NULL , 
    [PumpId] BIGINT NOT NULL, 
    [CropsId] BIGINT NOT NULL, 
    [GrowthName] NVARCHAR(50) NULL,
    [LandName] NVARCHAR(200) NULL, 
    [PlantHeight] DECIMAL(18, 2) NULL, 
    [DBH] DECIMAL(18, 2) NULL, 
    [NumberOfBlades] INT NULL, 
    [EmergenceRate] DECIMAL(18, 2) NULL, 
    [CreatedDateTime] DATETIME NULL DEFAULT getDate(), 
    [ModifiedDateTime] DATETIME NULL, 
    PRIMARY KEY ([RecordId], [PumpId], [CropsId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'记录编号[用雪花ID生成]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'RecordId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'泵房编号[外键值对应PumpHouseInfo表字段PumpId多对1]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'PumpId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'CropsId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'周期名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'GrowthName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'位置名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'LandName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'株高',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'PlantHeight'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'胸径',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'DBH'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'叶片数',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'NumberOfBlades'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出苗率',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'EmergenceRate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'创建时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'修改时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = N'COLUMN',
    @level2name = N'ModifiedDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'生长信息',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GrowthInfo',
    @level2type = NULL,
    @level2name = NULL