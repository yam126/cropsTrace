CREATE TABLE [dbo].[ShowFields]
(
	[RecordId] BIGINT NOT NULL,
    [CompanyId] int NOT NULL,    
    [Device] nvarchar(64) NULL,
    [PointId] bigint NOT NULL,
    [FieldName] NVARCHAR(150) NULL, 
    [ShowFieldName] NVARCHAR(150) NULL, 
    [Unit] nvarchar(16) NULL,
    [IsShow] INT NULL DEFAULT 0, 
    PRIMARY KEY ([CompanyId], [RecordId])
) 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'显示字段表',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShowFields',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'记录编号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShowFields',
    @level2type = N'COLUMN',
    @level2name = N'RecordId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'字段名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShowFields',
    @level2type = N'COLUMN',
    @level2name = N'FieldName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'界面显示名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShowFields',
    @level2type = N'COLUMN',
    @level2name = N'ShowFieldName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'设备',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShowFields',
    @level2type = N'COLUMN',
    @level2name = N'Device'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'测点',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShowFields',
    @level2type = N'COLUMN',
    @level2name = N'PointId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'单位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShowFields',
    @level2type = N'COLUMN',
    @level2name = N'Unit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'公司编号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShowFields',
    @level2type = N'COLUMN',
    @level2name = N'CompanyId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否显示_0不显示_1显示',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShowFields',
    @level2type = N'COLUMN',
    @level2name = N'IsShow'