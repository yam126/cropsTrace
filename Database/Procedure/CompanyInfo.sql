if exists(select 1 from sysobjects where id=object_id('dbo.Create_CompanyInfo') and xtype='P')
   drop PROCEDURE dbo.Create_CompanyInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Update_CompanyInfo') and xtype='P')
   drop PROCEDURE dbo.Update_CompanyInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_CompanyInfo') and xtype='P')
   drop PROCEDURE dbo.Query_CompanyInfo
if exists(select 1 from sysobjects where id=object_id('dbo.Query_CompanyInfo_Page') and xtype='P')
   drop PROCEDURE dbo.Query_CompanyInfo_Page   
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Create_CompanyInfo
(
                  @companyId bigint, --公司编号
                    @companyName nvarchar(300), --公司名称
                    @barcodeUrl nvarchar(800), --二维码URL
                    @Backup01 nvarchar(300), --备用字段01
                    @Backup02 nvarchar(300), --备用字段02
                    @Backup03 nvarchar(300), --备用字段03
                    @Backup04 nvarchar(300), --备用字段04
                    @Backup05 nvarchar(300), --备用字段05
                    @Backup06 nvarchar(300), --备用字段06
                    @created nvarchar(50), --创建人
                  @CreatedTime datetime, --创建时间
                    @Modifier nvarchar(50), --修改人
                  @ModifiedTime datetime --修改时间
)
as
begin
     insert into CompanyInfo
     (
               [companyId],
               [companyName],
               [barcodeUrl],
               [Backup01],
               [Backup02],
               [Backup03],
               [Backup04],
               [Backup05],
               [Backup06],
               [created],
               [CreatedTime],
               [Modifier],
               [ModifiedTime]
     )
     values
     (
               @companyId,
               @companyName,
               @barcodeUrl,
               @Backup01,
               @Backup02,
               @Backup03,
               @Backup04,
               @Backup05,
               @Backup06,
               @created,
               @CreatedTime,
               @Modifier,
               @ModifiedTime
     )
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE Update_CompanyInfo
(
                  @companyId bigint, --公司编号
                    @companyName nvarchar(300), --公司名称
                    @barcodeUrl nvarchar(800), --二维码URL
                    @Backup01 nvarchar(300), --备用字段01
                    @Backup02 nvarchar(300), --备用字段02
                    @Backup03 nvarchar(300), --备用字段03
                    @Backup04 nvarchar(300), --备用字段04
                    @Backup05 nvarchar(300), --备用字段05
                    @Backup06 nvarchar(300), --备用字段06
                    @created nvarchar(50), --创建人
                  @CreatedTime datetime, --创建时间
                    @Modifier nvarchar(50), --修改人
                  @ModifiedTime datetime, --修改时间
         @SqlWhere NVARCHAR(max)
)
as
begin
     Declare @SqlStr nvarchar(max);
     Set @SqlStr='Update CompanyInfo Set ';
              Set @SqlStr=@SqlStr+'companyId='+rtrim(ltrim(cast(@companyId as nvarchar(max))))+',';
              Set @SqlStr=@SqlStr+'companyName='''+@companyName+''',';
              Set @SqlStr=@SqlStr+'barcodeUrl='''+@barcodeUrl+''',';
              Set @SqlStr=@SqlStr+'Backup01='''+@Backup01+''',';
              Set @SqlStr=@SqlStr+'Backup02='''+@Backup02+''',';
              Set @SqlStr=@SqlStr+'Backup03='''+@Backup03+''',';
              Set @SqlStr=@SqlStr+'Backup04='''+@Backup04+''',';
              Set @SqlStr=@SqlStr+'Backup05='''+@Backup05+''',';
              Set @SqlStr=@SqlStr+'Backup06='''+@Backup06+''',';
              Set @SqlStr=@SqlStr+'created='''+@created+''',';
              Set @SqlStr=@SqlStr+'CreatedTime='''+CONVERT(varchar,@CreatedTime,120)+''',';              
              Set @SqlStr=@SqlStr+'Modifier='''+@Modifier+''',';
              Set @SqlStr=@SqlStr+'ModifiedTime='''+CONVERT(varchar,@ModifiedTime,120)+'';              
    if @SqlWhere Is Not Null And @SqlWhere<>''
       Set @SqlStr=@SqlStr+' where '+@SqlWhere;
    exec(@SqlStr); 
end
