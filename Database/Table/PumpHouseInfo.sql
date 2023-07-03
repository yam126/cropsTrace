CREATE TABLE [dbo].[PumpHouseInfo]
(
	[PumpId] BIGINT NOT NULL,
    [CompanyId] INT NOT  NULL,
    [PumpHouseName] NVARCHAR(150) NULL, 
    [PumpHouseClass] NVARCHAR(50) NULL, 
    [PersonIinCharge] NVARCHAR(50) NULL, 
    [CreatedDateTime] DATETIME NULL, 
    [ModifiedDateTime] DATETIME NULL, 
    PRIMARY KEY ([CompanyId], [PumpId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'泵房信息[此表数据通过外部接口获取而来]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PumpHouseInfo',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'泵房编号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PumpHouseInfo',
    @level2type = N'COLUMN',
    @level2name = N'PumpId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'泵房名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PumpHouseInfo',
    @level2type = N'COLUMN',
    @level2name = N'PumpHouseName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'泵房种类',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PumpHouseInfo',
    @level2type = N'COLUMN',
    @level2name = N'PumpHouseClass'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'负责人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PumpHouseInfo',
    @level2type = N'COLUMN',
    @level2name = N'PersonIinCharge'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'创建时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PumpHouseInfo',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'修改时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PumpHouseInfo',
    @level2type = N'COLUMN',
    @level2name = N'ModifiedDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'公司编号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PumpHouseInfo',
    @level2type = N'COLUMN',
    @level2name = N'CompanyId'