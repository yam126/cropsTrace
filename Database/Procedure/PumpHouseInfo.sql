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
                  @PumpId bigint, --泵房编号
                  @CompanyId int, --公司编号
                    @PumpHouseName nvarchar(150), --泵房名称
                    @PumpHouseClass nvarchar(50), --泵房种类
                    @PersonIinCharge nvarchar(50), --负责人
                  @CreatedDateTime datetime, --创建时间
                  @ModifiedDateTime datetime, --修改时间
                    @MonitoringAddress nvarchar(350) --监控地址
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
                  @PumpId bigint, --泵房编号
                  @CompanyId int, --公司编号
                    @PumpHouseName nvarchar(150), --泵房名称
                    @PumpHouseClass nvarchar(50), --泵房种类
                    @PersonIinCharge nvarchar(50), --负责人
                  @CreatedDateTime datetime, --创建时间
                  @ModifiedDateTime datetime, --修改时间
                    @MonitoringAddress nvarchar(350), --监控地址
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
              Set @SqlStr=@SqlStr+'ModifiedDateTime='''+CONVERT(varchar,@ModifiedDateTime,120)+''',';              
              Set @SqlStr=@SqlStr+'MonitoringAddress='''+@MonitoringAddress+'';
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