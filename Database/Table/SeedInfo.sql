CREATE TABLE [dbo].[SeedInfo]
(
	[CropsId] BIGINT NOT NULL , 
    [CompanyId] INT NOT NULL, 
    [SeedName] NVARCHAR(200) NULL, 
    [SeedVariety] NVARCHAR(50) NULL, 
    [SeedClass] NVARCHAR(50) NULL, 
    [PlantArea] NUMERIC(18, 2) NULL, 
    [CreatedDateTime] DATETIME NULL DEFAULT getDate(), 
    [ModifiedDateTime] DATETIME NULL, 
    PRIMARY KEY ([CompanyId], [CropsId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'种子信息[农作物信息]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SeedInfo',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'农作物编号[雪花ID生成]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SeedInfo',
    @level2type = N'COLUMN',
    @level2name = N'CropsId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'公司编号[对应登录者编号]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SeedInfo',
    @level2type = N'COLUMN',
    @level2name = N'CompanyId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'种子名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SeedInfo',
    @level2type = N'COLUMN',
    @level2name = N'SeedName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'种子品种',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SeedInfo',
    @level2type = N'COLUMN',
    @level2name = N'SeedVariety'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'种子类型',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SeedInfo',
    @level2type = N'COLUMN',
    @level2name = N'SeedClass'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'种植面积',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SeedInfo',
    @level2type = N'COLUMN',
    @level2name = N'PlantArea'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'创建时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SeedInfo',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'修改时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SeedInfo',
    @level2type = N'COLUMN',
    @level2name = N'ModifiedDateTime'