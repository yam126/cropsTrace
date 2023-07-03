USE [cropsTrace]
GO
/****** Object:  Table [dbo].[FileInfo]    Script Date: 2022/8/2/周二 17:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileInfo](
	[FileId] [bigint] NOT NULL,
	[CropsId] [bigint] NULL,
	[GrowthRecordId] [bigint] NOT NULL,
	[FileName] [nvarchar](150) NULL,
	[FileUrl] [nvarchar](150) NULL,
	[FileLength] [bigint] NULL,
	[CreatedDateTime] [datetime] NULL,
	[ShowParamJson] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GrowthInfo]    Script Date: 2022/8/2/周二 17:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrowthInfo](
	[RecordId] [bigint] NOT NULL,
	[PumpId] [bigint] NOT NULL,
	[CropsId] [bigint] NOT NULL,
	[GrowthName] [nvarchar](50) NULL,
	[LandName] [nvarchar](200) NULL,
	[PlantHeight] [decimal](18, 2) NULL,
	[DBH] [decimal](18, 2) NULL,
	[NumberOfBlades] [int] NULL,
	[EmergenceRate] [decimal](18, 2) NULL,
	[CreatedDateTime] [datetime] NULL,
	[ModifiedDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC,
	[PumpId] ASC,
	[CropsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PumpHouseInfo]    Script Date: 2022/8/2/周二 17:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PumpHouseInfo](
	[PumpId] [bigint] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[PumpHouseName] [nvarchar](150) NULL,
	[PumpHouseClass] [nvarchar](50) NULL,
	[PersonIinCharge] [nvarchar](50) NULL,
	[CreatedDateTime] [datetime] NULL,
	[ModifiedDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[PumpId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SeedInfo]    Script Date: 2022/8/2/周二 17:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SeedInfo](
	[CropsId] [bigint] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[SeedName] [nvarchar](200) NULL,
	[SeedVariety] [nvarchar](50) NULL,
	[SeedClass] [nvarchar](50) NULL,
	[PlantArea] [numeric](18, 2) NULL,
	[CreatedDateTime] [datetime] NULL,
	[ModifiedDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[CropsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShowFields]    Script Date: 2022/8/2/周二 17:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShowFields](
	[RecordId] [bigint] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Device] [nvarchar](64) NULL,
	[PointId] [bigint] NOT NULL,
	[FieldName] [nvarchar](150) NULL,
	[ShowFieldName] [nvarchar](150) NULL,
	[Unit] [nvarchar](16) NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[RecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StorageInfo]    Script Date: 2022/8/2/周二 17:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageInfo](
	[CropsId] [bigint] NOT NULL,
	[BatchNumber] [bigint] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[InQuantity] [decimal](18, 2) NULL,
	[OutQuantity] [decimal](18, 2) NULL,
	[InDateTime] [datetime] NULL,
	[OutDateTime] [datetime] NULL,
	[WarehouseNumber] [nvarchar](100) NULL,
	[WarehouseTemperature] [decimal](18, 2) NULL,
	[WarehouseHumidity] [decimal](18, 2) NULL,
	[CreatedDateTime] [datetime] NULL,
	[validityDateTime] [datetime] NULL,
	[State] [int] NULL,
 CONSTRAINT [PK__StorageI__C04DE75A7F729183] PRIMARY KEY CLUSTERED 
(
	[CropsId] ASC,
	[BatchNumber] ASC,
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GrowthInfo] ADD  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
ALTER TABLE [dbo].[SeedInfo] ADD  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件编号[自增列因为目前只是本表查询]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FileInfo', @level2type=N'COLUMN',@level2name=N'FileId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FileInfo', @level2type=N'COLUMN',@level2name=N'CropsId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'周期记录编号[外键对应生长周期表GrowthInfo字段RecordId关系多对1]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FileInfo', @level2type=N'COLUMN',@level2name=N'GrowthRecordId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FileInfo', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件URL路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FileInfo', @level2type=N'COLUMN',@level2name=N'FileUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FileInfo', @level2type=N'COLUMN',@level2name=N'FileLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FileInfo', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示参数JSON字符串' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FileInfo', @level2type=N'COLUMN',@level2name=N'ShowParamJson'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FileInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录编号[用雪花ID生成]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'RecordId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'泵房编号[外键值对应PumpHouseInfo表字段PumpId多对1]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'PumpId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'CropsId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'周期名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'GrowthName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'位置名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'LandName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'株高' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'PlantHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'胸径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'DBH'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'叶片数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'NumberOfBlades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出苗率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'EmergenceRate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo', @level2type=N'COLUMN',@level2name=N'ModifiedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生长信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GrowthInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'泵房编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PumpHouseInfo', @level2type=N'COLUMN',@level2name=N'PumpId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PumpHouseInfo', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'泵房名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PumpHouseInfo', @level2type=N'COLUMN',@level2name=N'PumpHouseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'泵房种类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PumpHouseInfo', @level2type=N'COLUMN',@level2name=N'PumpHouseClass'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'负责人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PumpHouseInfo', @level2type=N'COLUMN',@level2name=N'PersonIinCharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PumpHouseInfo', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PumpHouseInfo', @level2type=N'COLUMN',@level2name=N'ModifiedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'泵房信息[此表数据通过外部接口获取而来]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PumpHouseInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'农作物编号[雪花ID生成]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeedInfo', @level2type=N'COLUMN',@level2name=N'CropsId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号[对应登录者编号]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeedInfo', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'种子名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeedInfo', @level2type=N'COLUMN',@level2name=N'SeedName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'种子品种' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeedInfo', @level2type=N'COLUMN',@level2name=N'SeedVariety'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'种子类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeedInfo', @level2type=N'COLUMN',@level2name=N'SeedClass'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'种植面积' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeedInfo', @level2type=N'COLUMN',@level2name=N'PlantArea'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeedInfo', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeedInfo', @level2type=N'COLUMN',@level2name=N'ModifiedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'种子信息[农作物信息]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SeedInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShowFields', @level2type=N'COLUMN',@level2name=N'RecordId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShowFields', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设备' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShowFields', @level2type=N'COLUMN',@level2name=N'Device'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'测点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShowFields', @level2type=N'COLUMN',@level2name=N'PointId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字段名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShowFields', @level2type=N'COLUMN',@level2name=N'FieldName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'界面显示名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShowFields', @level2type=N'COLUMN',@level2name=N'ShowFieldName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShowFields', @level2type=N'COLUMN',@level2name=N'Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示字段表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShowFields'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'CropsId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入库批次号[雪花ID生成]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'BatchNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入库数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'InQuantity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出库数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'OutQuantity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入库时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'InDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出库时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'OutDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仓库编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'WarehouseNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仓库温度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'WarehouseTemperature'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仓库湿度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'WarehouseHumidity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'有效期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'validityDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态[0-入库、1-出库]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'库房信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StorageInfo'
GO
