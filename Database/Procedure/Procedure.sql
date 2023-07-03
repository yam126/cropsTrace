
if exists(select 1 from sysobjects where id=object_id('dbo.Create_FileInfo') and xtype='P')
   drop PROCEDURE dbo.Create_FileInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Update_FileInfo') and xtype='P')
   drop PROCEDURE dbo.Update_FileInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_FileInfo') and xtype='P')
   drop PROCEDURE dbo.Query_FileInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_FileInfo_Page') and xtype='P')
   drop PROCEDURE dbo.Query_FileInfo_Page   
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Create_FileInfo
(
                  @FileId bigint,
                  @CropsId bigint,
                  @GrowthRecordId bigint,
                    @FileName nvarchar(150),
                    @FileUrl nvarchar(150),
                  @FileLength bigint,
                  @CreatedDateTime datetime,
                    @ShowParamJson nvarchar(max)
)
as
begin
     insert into FileInfo
     (
               [FileId],
               [CropsId],
               [GrowthRecordId],
               [FileName],
               [FileUrl],
               [FileLength],
               [CreatedDateTime],
               [ShowParamJson]
     )
     values
     (
               @FileId,
               @CropsId,
               @GrowthRecordId,
               @FileName,
               @FileUrl,
               @FileLength,
               @CreatedDateTime,
               @ShowParamJson
     )
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Update_FileInfo
(
                  @FileId bigint,
                  @CropsId bigint,
                  @GrowthRecordId bigint,
                    @FileName nvarchar(150),
                    @FileUrl nvarchar(150),
                  @FileLength bigint,
                  @CreatedDateTime datetime,
                    @ShowParamJson nvarchar(max),
         @SqlWhere NVARCHAR(max)
)
as
begin
     Declare @SqlStr nvarchar(max);
     Set @SqlStr='Update FileInfo Set ';
              Set @SqlStr=@SqlStr+'FileId='+rtrim(ltrim(cast(@FileId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CropsId='+rtrim(ltrim(cast(@CropsId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'GrowthRecordId='+rtrim(ltrim(cast(@GrowthRecordId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'FileName='''+@FileName+''',';
              Set @SqlStr=@SqlStr+'FileUrl='''+@FileUrl+''',';
              Set @SqlStr=@SqlStr+'FileLength='+rtrim(ltrim(cast(@FileLength as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CreatedDateTime='''+CONVERT(varchar,@CreatedDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'ShowParamJson='''+@ShowParamJson+'';
    if @SqlWhere Is Not Null And @SqlWhere<>''
       Set @SqlStr=@SqlStr+' where '+@SqlWhere;
    exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_FileInfo
(
    @SqlWhere Nvarchar(max)
)
as
begin
    Declare @SqlStr nvarchar(max);
	Set @SqlStr='select ';
                Set @SqlStr=@SqlStr+'[FileId],'
                Set @SqlStr=@SqlStr+'[CropsId],'
                Set @SqlStr=@SqlStr+'[GrowthRecordId],'
                Set @SqlStr=@SqlStr+'[FileName],'
                Set @SqlStr=@SqlStr+'[FileUrl],'
                Set @SqlStr=@SqlStr+'[FileLength],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[ShowParamJson]'
    Set @SqlStr=@SqlStr+' from FileInfo';
	if @SqlWhere is not Null Or @SqlWhere<>''
	begin
	   Set @SqlStr=@SqlStr+' where '+@SqlWhere; 
	end
	exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_FileInfo_Page
(
  @StartRow int, --开始位置
  @EndRow int, --结束位置
  @TotalNumber int out,--总数据量
  @SortField nvarchar(max),--排序字段
  @SortMethod nvarchar(10),--排序方法
  @SqlWhere nvarchar(max) --查询条件
)
as
BEGIN
	declare @SqlStr nvarchar(max),
    @totalsql nvarchar(max),
    @PageSql nvarchar(max),
    @TableName nvarchar(max);
    set @TableName='dbo.[FileInfo]';
    set @SqlStr='select 
                  Row_Number() over(order by '+@SortField+' '+@SortMethod+') as Row,';
                Set @SqlStr=@SqlStr+'[FileId],'
                Set @SqlStr=@SqlStr+'[CropsId],'
                Set @SqlStr=@SqlStr+'[GrowthRecordId],'
                Set @SqlStr=@SqlStr+'[FileName],'
                Set @SqlStr=@SqlStr+'[FileUrl],'
                Set @SqlStr=@SqlStr+'[FileLength],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[ShowParamJson]'
    Set @SqlStr=@SqlStr+' from '+@TableName;
    if @SqlWhere<>'' 
    Begin
       set @SqlStr=@SqlStr+' Where '+@SqlWhere;
    End
    set @totalsql='with Result as('+@SqlStr+')select @t=count(*) from Result';
    EXEC sp_executesql
        @totalsql, 
        N'@t AS INT OUTPUT',
        @t = @TotalNumber OUTPUT;
    set @PageSql='with Result as('+@SqlStr+')select * from Result where Row>='+cast(@StartRow as varchar)+' and Row<='+cast(@EndRow as varchar);
    print @PageSql;
    exec(@PageSql);	
END
if exists(select 1 from sysobjects where id=object_id('dbo.Create_GrowthInfo') and xtype='P')
   drop PROCEDURE dbo.Create_GrowthInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Update_GrowthInfo') and xtype='P')
   drop PROCEDURE dbo.Update_GrowthInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_GrowthInfo') and xtype='P')
   drop PROCEDURE dbo.Query_GrowthInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_GrowthInfo_Page') and xtype='P')
   drop PROCEDURE dbo.Query_GrowthInfo_Page   
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Create_GrowthInfo
(
                  @RecordId bigint,
                  @PumpId bigint,
                  @CropsId bigint,
                    @GrowthName nvarchar(50),
                    @LandName nvarchar(200),
                  @PlantHeight decimal,
                  @DBH decimal,
                  @NumberOfBlades int,
                  @EmergenceRate decimal,
                  @CreatedDateTime datetime,
                  @ModifiedDateTime datetime
)
as
begin
     insert into GrowthInfo
     (
               [RecordId],
               [PumpId],
               [CropsId],
               [GrowthName],
               [LandName],
               [PlantHeight],
               [DBH],
               [NumberOfBlades],
               [EmergenceRate],
               [CreatedDateTime],
               [ModifiedDateTime]
     )
     values
     (
               @RecordId,
               @PumpId,
               @CropsId,
               @GrowthName,
               @LandName,
               @PlantHeight,
               @DBH,
               @NumberOfBlades,
               @EmergenceRate,
               @CreatedDateTime,
               @ModifiedDateTime
     )
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Update_GrowthInfo
(
                  @RecordId bigint,
                  @PumpId bigint,
                  @CropsId bigint,
                    @GrowthName nvarchar(50),
                    @LandName nvarchar(200),
                  @PlantHeight decimal,
                  @DBH decimal,
                  @NumberOfBlades int,
                  @EmergenceRate decimal,
                  @CreatedDateTime datetime,
                  @ModifiedDateTime datetime,
         @SqlWhere NVARCHAR(max)
)
as
begin
     Declare @SqlStr nvarchar(max);
     Set @SqlStr='Update GrowthInfo Set ';
              Set @SqlStr=@SqlStr+'RecordId='+rtrim(ltrim(cast(@RecordId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'PumpId='+rtrim(ltrim(cast(@PumpId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CropsId='+rtrim(ltrim(cast(@CropsId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'GrowthName='''+@GrowthName+''',';
              Set @SqlStr=@SqlStr+'LandName='''+@LandName+''',';
              Set @SqlStr=@SqlStr+'PlantHeight='+rtrim(ltrim(cast(@PlantHeight as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'DBH='+rtrim(ltrim(cast(@DBH as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'NumberOfBlades='+rtrim(ltrim(cast(@NumberOfBlades as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'EmergenceRate='+rtrim(ltrim(cast(@EmergenceRate as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CreatedDateTime='''+CONVERT(varchar,@CreatedDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'ModifiedDateTime='''+CONVERT(varchar,@ModifiedDateTime,120)+'';              
    if @SqlWhere Is Not Null And @SqlWhere<>''
       Set @SqlStr=@SqlStr+' where '+@SqlWhere;
    exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_GrowthInfo
(
    @SqlWhere Nvarchar(max)
)
as
begin
    Declare @SqlStr nvarchar(max);
	Set @SqlStr='select ';
                Set @SqlStr=@SqlStr+'[RecordId],'
                Set @SqlStr=@SqlStr+'[PumpId],'
                Set @SqlStr=@SqlStr+'[CropsId],'
                Set @SqlStr=@SqlStr+'[GrowthName],'
                Set @SqlStr=@SqlStr+'[LandName],'
                Set @SqlStr=@SqlStr+'[PlantHeight],'
                Set @SqlStr=@SqlStr+'[DBH],'
                Set @SqlStr=@SqlStr+'[NumberOfBlades],'
                Set @SqlStr=@SqlStr+'[EmergenceRate],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[ModifiedDateTime]'
    Set @SqlStr=@SqlStr+' from GrowthInfo';
	if @SqlWhere is not Null Or @SqlWhere<>''
	begin
	   Set @SqlStr=@SqlStr+' where '+@SqlWhere; 
	end
	exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_GrowthInfo_Page
(
  @StartRow int, --开始位置
  @EndRow int, --结束位置
  @TotalNumber int out,--总数据量
  @SortField nvarchar(max),--排序字段
  @SortMethod nvarchar(10),--排序方法
  @SqlWhere nvarchar(max) --查询条件
)
as
BEGIN
	declare @SqlStr nvarchar(max),
    @totalsql nvarchar(max),
    @PageSql nvarchar(max),
    @TableName nvarchar(max);
    set @TableName='dbo.[GrowthInfo]';
    set @SqlStr='select 
                  Row_Number() over(order by '+@SortField+' '+@SortMethod+') as Row,';
                Set @SqlStr=@SqlStr+'[RecordId],'
                Set @SqlStr=@SqlStr+'[PumpId],'
                Set @SqlStr=@SqlStr+'[CropsId],'
                Set @SqlStr=@SqlStr+'[GrowthName],'
                Set @SqlStr=@SqlStr+'[LandName],'
                Set @SqlStr=@SqlStr+'[PlantHeight],'
                Set @SqlStr=@SqlStr+'[DBH],'
                Set @SqlStr=@SqlStr+'[NumberOfBlades],'
                Set @SqlStr=@SqlStr+'[EmergenceRate],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[ModifiedDateTime]'
    Set @SqlStr=@SqlStr+' from '+@TableName;
    if @SqlWhere<>'' 
    Begin
       set @SqlStr=@SqlStr+' Where '+@SqlWhere;
    End
    set @totalsql='with Result as('+@SqlStr+')select @t=count(*) from Result';
    EXEC sp_executesql
        @totalsql, 
        N'@t AS INT OUTPUT',
        @t = @TotalNumber OUTPUT;
    set @PageSql='with Result as('+@SqlStr+')select * from Result where Row>='+cast(@StartRow as varchar)+' and Row<='+cast(@EndRow as varchar);
    print @PageSql;
    exec(@PageSql);	
END
if exists(select 1 from sysobjects where id=object_id('dbo.Create_PumpHouseInfo') and xtype='P')
   drop PROCEDURE dbo.Create_PumpHouseInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Update_PumpHouseInfo') and xtype='P')
   drop PROCEDURE dbo.Update_PumpHouseInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_PumpHouseInfo') and xtype='P')
   drop PROCEDURE dbo.Query_PumpHouseInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_PumpHouseInfo_Page') and xtype='P')
   drop PROCEDURE dbo.Query_PumpHouseInfo_Page   
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Create_PumpHouseInfo
(
                  @PumpId bigint,
                  @CompanyId int,
                    @PumpHouseName nvarchar(150),
                    @PumpHouseClass nvarchar(50),
                    @PersonIinCharge nvarchar(50),
                  @CreatedDateTime datetime,
                  @ModifiedDateTime datetime,
                  @MonitoringAddress nvarchar(350)
)
as
begin
     insert into PumpHouseInfo
     (
               [PumpId],
               [CompanyId],
               [PumpHouseName],
               [PumpHouseClass],
               [PersonIinCharge],
               [CreatedDateTime],
               [ModifiedDateTime],
               [MonitoringAddress]
     )
     values
     (
               @PumpId,
               @CompanyId,
               @PumpHouseName,
               @PumpHouseClass,
               @PersonIinCharge,
               @CreatedDateTime,
               @ModifiedDateTime,
               @MonitoringAddress
     )
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Update_PumpHouseInfo
(
                  @PumpId bigint,
                  @CompanyId int,
                    @PumpHouseName nvarchar(150),
                    @PumpHouseClass nvarchar(50),
                    @PersonIinCharge nvarchar(50),
                  @CreatedDateTime datetime,
                  @ModifiedDateTime datetime,
                  @MonitoringAddress nvarchar(350),
         @SqlWhere NVARCHAR(max)
)
as
begin
     Declare @SqlStr nvarchar(max);
     Set @SqlStr='Update PumpHouseInfo Set ';
              Set @SqlStr=@SqlStr+'PumpId='+rtrim(ltrim(cast(@PumpId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CompanyId='+rtrim(ltrim(cast(@CompanyId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'PumpHouseName='''+@PumpHouseName+''',';
              Set @SqlStr=@SqlStr+'PumpHouseClass='''+@PumpHouseClass+''',';
              Set @SqlStr=@SqlStr+'PersonIinCharge='''+@PersonIinCharge+''',';
              Set @SqlStr=@SqlStr+'CreatedDateTime='''+CONVERT(varchar,@CreatedDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'ModifiedDateTime='''+CONVERT(varchar,@ModifiedDateTime,120)+''; 
              Set @SqlStr=@SqlStr+'MonitoringAddress='''+@MonitoringAddress+'''';
    if @SqlWhere Is Not Null And @SqlWhere<>''
       Set @SqlStr=@SqlStr+' where '+@SqlWhere;
    exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_PumpHouseInfo
(
    @SqlWhere Nvarchar(max)
)
as
begin
    Declare @SqlStr nvarchar(max);
	Set @SqlStr='select ';
                Set @SqlStr=@SqlStr+'[PumpId],'
                Set @SqlStr=@SqlStr+'[CompanyId],'
                Set @SqlStr=@SqlStr+'[PumpHouseName],'
                Set @SqlStr=@SqlStr+'[PumpHouseClass],'
                Set @SqlStr=@SqlStr+'[PersonIinCharge],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[ModifiedDateTime],'
                Set @SqlStr=@SqlStr+'[MonitoringAddress]'
    Set @SqlStr=@SqlStr+' from PumpHouseInfo';
	if @SqlWhere is not Null Or @SqlWhere<>''
	begin
	   Set @SqlStr=@SqlStr+' where '+@SqlWhere; 
	end
	exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_PumpHouseInfo_Page
(
  @StartRow int, --开始位置
  @EndRow int, --结束位置
  @TotalNumber int out,--总数据量
  @SortField nvarchar(max),--排序字段
  @SortMethod nvarchar(10),--排序方法
  @SqlWhere nvarchar(max) --查询条件
)
as
BEGIN
	declare @SqlStr nvarchar(max),
    @totalsql nvarchar(max),
    @PageSql nvarchar(max),
    @TableName nvarchar(max);
    set @TableName='dbo.[PumpHouseInfo]';
    set @SqlStr='select 
                  Row_Number() over(order by '+@SortField+' '+@SortMethod+') as Row,';
                Set @SqlStr=@SqlStr+'[PumpId],'
                Set @SqlStr=@SqlStr+'[CompanyId],'
                Set @SqlStr=@SqlStr+'[PumpHouseName],'
                Set @SqlStr=@SqlStr+'[PumpHouseClass],'
                Set @SqlStr=@SqlStr+'[PersonIinCharge],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[ModifiedDateTime],'
                Set @SqlStr=@SqlStr+'[MonitoringAddress]'
    Set @SqlStr=@SqlStr+' from '+@TableName;
    if @SqlWhere<>'' 
    Begin
       set @SqlStr=@SqlStr+' Where '+@SqlWhere;
    End
    set @totalsql='with Result as('+@SqlStr+')select @t=count(*) from Result';
    EXEC sp_executesql
        @totalsql, 
        N'@t AS INT OUTPUT',
        @t = @TotalNumber OUTPUT;
    set @PageSql='with Result as('+@SqlStr+')select * from Result where Row>='+cast(@StartRow as varchar)+' and Row<='+cast(@EndRow as varchar);
    print @PageSql;
    exec(@PageSql);	
END
if exists(select 1 from sysobjects where id=object_id('dbo.Create_SeedInfo') and xtype='P')
   drop PROCEDURE dbo.Create_SeedInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Update_SeedInfo') and xtype='P')
   drop PROCEDURE dbo.Update_SeedInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_SeedInfo') and xtype='P')
   drop PROCEDURE dbo.Query_SeedInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_SeedInfo_Page') and xtype='P')
   drop PROCEDURE dbo.Query_SeedInfo_Page   
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Create_SeedInfo
(
                  @CropsId bigint,
                  @CompanyId int,
                    @SeedName nvarchar(200),
                    @SeedVariety nvarchar(50),
                    @SeedClass nvarchar(50),
                  @PlantArea numeric,
                  @CreatedDateTime datetime,
                  @ModifiedDateTime datetime,
                    @Introduce nvarchar(300)
)
as
begin
     insert into SeedInfo
     (
               [CropsId],
               [CompanyId],
               [SeedName],
               [SeedVariety],
               [SeedClass],
               [PlantArea],
               [CreatedDateTime],
               [ModifiedDateTime],
               [Introduce]
     )
     values
     (
               @CropsId,
               @CompanyId,
               @SeedName,
               @SeedVariety,
               @SeedClass,
               @PlantArea,
               @CreatedDateTime,
               @ModifiedDateTime,
               @Introduce
     )
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Update_SeedInfo
(
                  @CropsId bigint,
                  @CompanyId int,
                    @SeedName nvarchar(200),
                    @SeedVariety nvarchar(50),
                    @SeedClass nvarchar(50),
                  @PlantArea numeric,
                  @CreatedDateTime datetime,
                  @ModifiedDateTime datetime,
                    @Introduce nvarchar(300),
         @SqlWhere NVARCHAR(max)
)
as
begin
     Declare @SqlStr nvarchar(max);
     Set @SqlStr='Update SeedInfo Set ';
              Set @SqlStr=@SqlStr+'CropsId='+rtrim(ltrim(cast(@CropsId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CompanyId='+rtrim(ltrim(cast(@CompanyId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'SeedName='''+@SeedName+''',';
              Set @SqlStr=@SqlStr+'SeedVariety='''+@SeedVariety+''',';
              Set @SqlStr=@SqlStr+'SeedClass='''+@SeedClass+''',';
              Set @SqlStr=@SqlStr+'PlantArea='+rtrim(ltrim(cast(@PlantArea as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CreatedDateTime='''+CONVERT(varchar,@CreatedDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'ModifiedDateTime='''+CONVERT(varchar,@ModifiedDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'Introduce='''+@Introduce+'';
    if @SqlWhere Is Not Null And @SqlWhere<>''
       Set @SqlStr=@SqlStr+' where '+@SqlWhere;
    exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_SeedInfo
(
    @SqlWhere Nvarchar(max)
)
as
begin
    Declare @SqlStr nvarchar(max);
	Set @SqlStr='select ';
                Set @SqlStr=@SqlStr+'[CropsId],'
                Set @SqlStr=@SqlStr+'[CompanyId],'
                Set @SqlStr=@SqlStr+'[SeedName],'
                Set @SqlStr=@SqlStr+'[SeedVariety],'
                Set @SqlStr=@SqlStr+'[SeedClass],'
                Set @SqlStr=@SqlStr+'[PlantArea],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[ModifiedDateTime],'
                Set @SqlStr=@SqlStr+'[Introduce]'
    Set @SqlStr=@SqlStr+' from SeedInfo';
	if @SqlWhere is not Null Or @SqlWhere<>''
	begin
	   Set @SqlStr=@SqlStr+' where '+@SqlWhere; 
	end
	exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_SeedInfo_Page
(
  @StartRow int, --开始位置
  @EndRow int, --结束位置
  @TotalNumber int out,--总数据量
  @SortField nvarchar(max),--排序字段
  @SortMethod nvarchar(10),--排序方法
  @SqlWhere nvarchar(max) --查询条件
)
as
BEGIN
	declare @SqlStr nvarchar(max),
    @totalsql nvarchar(max),
    @PageSql nvarchar(max),
    @TableName nvarchar(max);
    set @TableName='dbo.[SeedInfo]';
    set @SqlStr='select 
                  Row_Number() over(order by '+@SortField+' '+@SortMethod+') as Row,';
                Set @SqlStr=@SqlStr+'[CropsId],'
                Set @SqlStr=@SqlStr+'[CompanyId],'
                Set @SqlStr=@SqlStr+'[SeedName],'
                Set @SqlStr=@SqlStr+'[SeedVariety],'
                Set @SqlStr=@SqlStr+'[SeedClass],'
                Set @SqlStr=@SqlStr+'[PlantArea],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[ModifiedDateTime],'
                Set @SqlStr=@SqlStr+'[Introduce]'
    Set @SqlStr=@SqlStr+' from '+@TableName;
    if @SqlWhere<>'' 
    Begin
       set @SqlStr=@SqlStr+' Where '+@SqlWhere;
    End
    set @totalsql='with Result as('+@SqlStr+')select @t=count(*) from Result';
    EXEC sp_executesql
        @totalsql, 
        N'@t AS INT OUTPUT',
        @t = @TotalNumber OUTPUT;
    set @PageSql='with Result as('+@SqlStr+')select * from Result where Row>='+cast(@StartRow as varchar)+' and Row<='+cast(@EndRow as varchar);
    print @PageSql;
    exec(@PageSql);	
END
if exists(select 1 from sysobjects where id=object_id('dbo.Create_ShowFields') and xtype='P')
   drop PROCEDURE dbo.Create_ShowFields
if exists(select 1 from sysobjects where id=object_id('dbo.Update_ShowFields') and xtype='P')
   drop PROCEDURE dbo.Update_ShowFields
if exists(select 1 from sysobjects where id=object_id('dbo.Query_ShowFields') and xtype='P')
   drop PROCEDURE dbo.Query_ShowFields
if exists(select 1 from sysobjects where id=object_id('dbo.Query_ShowFields_Page') and xtype='P')
   drop PROCEDURE dbo.Query_ShowFields_Page   
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Create_ShowFields
(
                  @RecordId bigint,
                  @CompanyId int,
                    @Device nvarchar(64),
                  @PointId bigint,
                    @FieldName nvarchar(150),
                    @ShowFieldName nvarchar(150),
                    @Unit nvarchar(16),
                  @IsShow int
)
as
begin
     insert into ShowFields
     (
               [RecordId],
               [CompanyId],
               [Device],
               [PointId],
               [FieldName],
               [ShowFieldName],
               [Unit],
               [IsShow]
     )
     values
     (
               @RecordId,
               @CompanyId,
               @Device,
               @PointId,
               @FieldName,
               @ShowFieldName,
               @Unit,
               @IsShow
     )
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Update_ShowFields
(
                  @RecordId bigint,
                  @CompanyId int,
                    @Device nvarchar(64),
                  @PointId bigint,
                    @FieldName nvarchar(150),
                    @ShowFieldName nvarchar(150),
                    @Unit nvarchar(16),
                  @IsShow int,
         @SqlWhere NVARCHAR(max)
)
as
begin
     Declare @SqlStr nvarchar(max);
     Set @SqlStr='Update ShowFields Set ';
              Set @SqlStr=@SqlStr+'RecordId='+rtrim(ltrim(cast(@RecordId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CompanyId='+rtrim(ltrim(cast(@CompanyId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'Device='''+@Device+''',';
              Set @SqlStr=@SqlStr+'PointId='+rtrim(ltrim(cast(@PointId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'FieldName='''+@FieldName+''',';
              Set @SqlStr=@SqlStr+'ShowFieldName='''+@ShowFieldName+''',';
              Set @SqlStr=@SqlStr+'Unit='''+@Unit+''',';
              Set @SqlStr=@SqlStr+'IsShow='+rtrim(ltrim(cast(@IsShow as nvarchar(max))));
    if @SqlWhere Is Not Null And @SqlWhere<>''
       Set @SqlStr=@SqlStr+' where '+@SqlWhere;
    exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_ShowFields
(
    @SqlWhere Nvarchar(max)
)
as
begin
    Declare @SqlStr nvarchar(max);
	Set @SqlStr='select ';
                Set @SqlStr=@SqlStr+'[RecordId],'
                Set @SqlStr=@SqlStr+'[CompanyId],'
                Set @SqlStr=@SqlStr+'[Device],'
                Set @SqlStr=@SqlStr+'[PointId],'
                Set @SqlStr=@SqlStr+'[FieldName],'
                Set @SqlStr=@SqlStr+'[ShowFieldName],'
                Set @SqlStr=@SqlStr+'[Unit],'
                Set @SqlStr=@SqlStr+'[IsShow]'
    Set @SqlStr=@SqlStr+' from ShowFields';
	if @SqlWhere is not Null Or @SqlWhere<>''
	begin
	   Set @SqlStr=@SqlStr+' where '+@SqlWhere; 
	end
	exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_ShowFields_Page
(
  @StartRow int, --开始位置
  @EndRow int, --结束位置
  @TotalNumber int out,--总数据量
  @SortField nvarchar(max),--排序字段
  @SortMethod nvarchar(10),--排序方法
  @SqlWhere nvarchar(max) --查询条件
)
as
BEGIN
	declare @SqlStr nvarchar(max),
    @totalsql nvarchar(max),
    @PageSql nvarchar(max),
    @TableName nvarchar(max);
    set @TableName='dbo.[ShowFields]';
    set @SqlStr='select 
                  Row_Number() over(order by '+@SortField+' '+@SortMethod+') as Row,';
                Set @SqlStr=@SqlStr+'[RecordId],'
                Set @SqlStr=@SqlStr+'[CompanyId],'
                Set @SqlStr=@SqlStr+'[Device],'
                Set @SqlStr=@SqlStr+'[PointId],'
                Set @SqlStr=@SqlStr+'[FieldName],'
                Set @SqlStr=@SqlStr+'[ShowFieldName],'
                Set @SqlStr=@SqlStr+'[Unit],'
                Set @SqlStr=@SqlStr+'[IsShow]'
    Set @SqlStr=@SqlStr+' from '+@TableName;
    if @SqlWhere<>'' 
    Begin
       set @SqlStr=@SqlStr+' Where '+@SqlWhere;
    End
    set @totalsql='with Result as('+@SqlStr+')select @t=count(*) from Result';
    EXEC sp_executesql
        @totalsql, 
        N'@t AS INT OUTPUT',
        @t = @TotalNumber OUTPUT;
    set @PageSql='with Result as('+@SqlStr+')select * from Result where Row>='+cast(@StartRow as varchar)+' and Row<='+cast(@EndRow as varchar);
    print @PageSql;
    exec(@PageSql);	
END
if exists(select 1 from sysobjects where id=object_id('dbo.Create_StorageInfo') and xtype='P')
   drop PROCEDURE dbo.Create_StorageInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Update_StorageInfo') and xtype='P')
   drop PROCEDURE dbo.Update_StorageInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_StorageInfo') and xtype='P')
   drop PROCEDURE dbo.Query_StorageInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_StorageInfo_Page') and xtype='P')
   drop PROCEDURE dbo.Query_StorageInfo_Page   
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Create_StorageInfo
(
                  @CropsId bigint,
                  @BatchNumber bigint,
                  @CompanyId int,
                  @InQuantity decimal,
                  @OutQuantity decimal,
                  @InDateTime datetime,
                  @OutDateTime datetime,
                    @WarehouseNumber nvarchar(100),
                  @WarehouseTemperature decimal,
                  @WarehouseHumidity decimal,
                  @CreatedDateTime datetime,
                  @validityDateTime datetime,
                  @State int
)
as
begin
     insert into StorageInfo
     (
               [CropsId],
               [BatchNumber],
               [CompanyId],
               [InQuantity],
               [OutQuantity],
               [InDateTime],
               [OutDateTime],
               [WarehouseNumber],
               [WarehouseTemperature],
               [WarehouseHumidity],
               [CreatedDateTime],
               [validityDateTime],
               [State]
     )
     values
     (
               @CropsId,
               @BatchNumber,
               @CompanyId,
               @InQuantity,
               @OutQuantity,
               @InDateTime,
               @OutDateTime,
               @WarehouseNumber,
               @WarehouseTemperature,
               @WarehouseHumidity,
               @CreatedDateTime,
               @validityDateTime,
               @State
     )
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Update_StorageInfo
(
                  @CropsId bigint,
                  @BatchNumber bigint,
                  @CompanyId int,
                  @InQuantity decimal,
                  @OutQuantity decimal,
                  @InDateTime datetime,
                  @OutDateTime datetime,
                    @WarehouseNumber nvarchar(100),
                  @WarehouseTemperature decimal,
                  @WarehouseHumidity decimal,
                  @CreatedDateTime datetime,
                  @validityDateTime datetime,
                  @State int,
         @SqlWhere NVARCHAR(max)
)
as
begin
     Declare @SqlStr nvarchar(max);
     Set @SqlStr='Update StorageInfo Set ';
              Set @SqlStr=@SqlStr+'CropsId='+rtrim(ltrim(cast(@CropsId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'BatchNumber='+rtrim(ltrim(cast(@BatchNumber as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CompanyId='+rtrim(ltrim(cast(@CompanyId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'InQuantity='+rtrim(ltrim(cast(@InQuantity as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'OutQuantity='+rtrim(ltrim(cast(@OutQuantity as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'InDateTime='''+CONVERT(varchar,@InDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'OutDateTime='''+CONVERT(varchar,@OutDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'WarehouseNumber='''+@WarehouseNumber+''',';
              Set @SqlStr=@SqlStr+'WarehouseTemperature='+rtrim(ltrim(cast(@WarehouseTemperature as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'WarehouseHumidity='+rtrim(ltrim(cast(@WarehouseHumidity as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'CreatedDateTime='''+CONVERT(varchar,@CreatedDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'validityDateTime='''+CONVERT(varchar,@validityDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'State='+rtrim(ltrim(cast(@State as nvarchar(max))));
    if @SqlWhere Is Not Null And @SqlWhere<>''
       Set @SqlStr=@SqlStr+' where '+@SqlWhere;
    exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_StorageInfo
(
    @SqlWhere Nvarchar(max)
)
as
begin
    Declare @SqlStr nvarchar(max);
	Set @SqlStr='select ';
                Set @SqlStr=@SqlStr+'[CropsId],'
                Set @SqlStr=@SqlStr+'[BatchNumber],'
                Set @SqlStr=@SqlStr+'[CompanyId],'
                Set @SqlStr=@SqlStr+'[InQuantity],'
                Set @SqlStr=@SqlStr+'[OutQuantity],'
                Set @SqlStr=@SqlStr+'[InDateTime],'
                Set @SqlStr=@SqlStr+'[OutDateTime],'
                Set @SqlStr=@SqlStr+'[WarehouseNumber],'
                Set @SqlStr=@SqlStr+'[WarehouseTemperature],'
                Set @SqlStr=@SqlStr+'[WarehouseHumidity],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[validityDateTime],'
                Set @SqlStr=@SqlStr+'[State]'
    Set @SqlStr=@SqlStr+' from StorageInfo';
	if @SqlWhere is not Null Or @SqlWhere<>''
	begin
	   Set @SqlStr=@SqlStr+' where '+@SqlWhere; 
	end
	exec(@SqlStr); 
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Query_StorageInfo_Page
(
  @StartRow int, --开始位置
  @EndRow int, --结束位置
  @TotalNumber int out,--总数据量
  @SortField nvarchar(max),--排序字段
  @SortMethod nvarchar(10),--排序方法
  @SqlWhere nvarchar(max) --查询条件
)
as
BEGIN
	declare @SqlStr nvarchar(max),
    @totalsql nvarchar(max),
    @PageSql nvarchar(max),
    @TableName nvarchar(max);
    set @TableName='dbo.[StorageInfo]';
    set @SqlStr='select 
                  Row_Number() over(order by '+@SortField+' '+@SortMethod+') as Row,';
                Set @SqlStr=@SqlStr+'[CropsId],'
                Set @SqlStr=@SqlStr+'[BatchNumber],'
                Set @SqlStr=@SqlStr+'[CompanyId],'
                Set @SqlStr=@SqlStr+'[InQuantity],'
                Set @SqlStr=@SqlStr+'[OutQuantity],'
                Set @SqlStr=@SqlStr+'[InDateTime],'
                Set @SqlStr=@SqlStr+'[OutDateTime],'
                Set @SqlStr=@SqlStr+'[WarehouseNumber],'
                Set @SqlStr=@SqlStr+'[WarehouseTemperature],'
                Set @SqlStr=@SqlStr+'[WarehouseHumidity],'
                Set @SqlStr=@SqlStr+'[CreatedDateTime],'
                Set @SqlStr=@SqlStr+'[validityDateTime],'
                Set @SqlStr=@SqlStr+'[State]'
    Set @SqlStr=@SqlStr+' from '+@TableName;
    if @SqlWhere<>'' 
    Begin
       set @SqlStr=@SqlStr+' Where '+@SqlWhere;
    End
    set @totalsql='with Result as('+@SqlStr+')select @t=count(*) from Result';
    EXEC sp_executesql
        @totalsql, 
        N'@t AS INT OUTPUT',
        @t = @TotalNumber OUTPUT;
    set @PageSql='with Result as('+@SqlStr+')select * from Result where Row>='+cast(@StartRow as varchar)+' and Row<='+cast(@EndRow as varchar);
    print @PageSql;
    exec(@PageSql);	
END