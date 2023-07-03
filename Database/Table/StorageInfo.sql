CREATE TABLE [dbo].[StorageInfo]
(
	[CropsId] BIGINT NOT NULL , 
    [BatchNumber] BIGINT NOT NULL,
    [CompanyId] INT NOT NULL,
    [InQuantity] DECIMAL(18, 2) NULL, 
    [OutQuantity] DECIMAL(18, 2) NULL, 
    [InDateTime] DATETIME NULL, 
    [OutDateTime] DATETIME NULL, 
    [WarehouseNumber] NVARCHAR(100) NULL, 
    [WarehouseTemperature] DECIMAL(18, 2) NULL, 
    [WarehouseHumidity] DECIMAL(18, 2) NULL, 
    [CreatedDateTime] DATETIME NULL, 
    [validityDateTime] DATETIME NULL, 
    [State] INT NULL, 
    PRIMARY KEY ([BatchNumber],[CropsId],[CompanyId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'库房信息',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'CropsId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'入库批次号[雪花ID生成]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'BatchNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'入库数量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'InQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出库数量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'OutQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'入库时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'InDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出库时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'OutDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'仓库编号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'WarehouseNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'仓库温度',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'WarehouseTemperature'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'仓库湿度',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'WarehouseHumidity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'创建时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'有效期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'validityDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'状态[0-入库、1-出库]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'State'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'公司编号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'StorageInfo',
    @level2type = N'COLUMN',
    @level2name = N'CompanyId'