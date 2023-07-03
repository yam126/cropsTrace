CREATE TABLE [dbo].[CompanyInfo]
(
	[companyId] BIGINT NOT NULL PRIMARY KEY, 
    [companyName] NVARCHAR(300) NULL, 
    [barcodeUrl] NVARCHAR(800) NULL, 
    [Backup01] NVARCHAR(300) NULL,
    [Backup02] NVARCHAR(300) NULL,
    [Backup03] NVARCHAR(300) NULL,
    [Backup04] NVARCHAR(300) NULL,
    [Backup05] NVARCHAR(300) NULL,
    [Backup06] NVARCHAR(300) NULL,
    [created] NVARCHAR(50) NULL, 
    [CreatedTime] DATETIME NULL, 
    [Modifier] NVARCHAR(50) NULL, 
    [ModifiedTime] DATETIME NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'公司编号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'companyId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'公司名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'companyName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'二维码URL',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'barcodeUrl'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'备用字段01',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'Backup01'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'备用字段02',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'Backup02'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'备用字段03',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'Backup03'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'备用字段04',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'Backup04'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'备用字段05',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'Backup05'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'备用字段06',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'Backup06'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'创建人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'created'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'创建时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'CreatedTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'修改人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'Modifier'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'修改时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = N'COLUMN',
    @level2name = N'ModifiedTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'公司信息表',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CompanyInfo',
    @level2type = NULL,
    @level2name = NULL