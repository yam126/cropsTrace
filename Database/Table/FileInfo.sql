CREATE TABLE [dbo].[FileInfo]
(
	[FileId] BIGINT NOT NULL PRIMARY KEY, 
    [CropsId] BIGINT NULL, 
    [GrowthRecordId] BIGINT NOT NULL, 
    [FileName] NVARCHAR(150) NULL, 
    [FileUrl] NVARCHAR(150) NULL, 
    [FileLength] BIGINT NULL, 
    [CreatedDateTime] DATETIME NULL, 
    [ShowParamJson] NVARCHAR(MAX) NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'文件信息',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FileInfo',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'文件编号[自增列因为目前只是本表查询]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FileInfo',
    @level2type = N'COLUMN',
    @level2name = N'FileId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FileInfo',
    @level2type = N'COLUMN',
    @level2name = N'CropsId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'周期记录编号[外键对应生长周期表GrowthInfo字段RecordId关系多对1]',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FileInfo',
    @level2type = N'COLUMN',
    @level2name = N'GrowthRecordId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'文件名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FileInfo',
    @level2type = N'COLUMN',
    @level2name = N'FileName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'文件URL路径',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FileInfo',
    @level2type = N'COLUMN',
    @level2name = N'FileUrl'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'文件大小',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FileInfo',
    @level2type = N'COLUMN',
    @level2name = N'FileLength'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'创建时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FileInfo',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'显示参数JSON字符串',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FileInfo',
    @level2type = N'COLUMN',
    @level2name = N'ShowParamJson'