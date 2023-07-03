using cropsTraceApi.Swagger;
using cropsTraceDataAccess;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//读取网站配置文件
IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

string MOIConnStr = configuration["ConnectionStrings:CropsTraceConnStr"];

//配置连接字符串
builder.Services.AddDbContext(options => options.AddMtrlSqlServer(configuration["ConnectionStrings:CropsTraceConnStr"]));

//配置AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

#region 开发时注释，发布时启用
////setting authetication 
//builder.Services.AddAuthentication(builder.Configuration["Ids4:Scheme"])
//          .AddIdentityServerAuthentication(options =>
//          {
//              options.RequireHttpsMetadata = false;
//              options.Authority = $"http://{builder.Configuration["Ids4:IP"]}:{builder.Configuration["Ids4:Port"]}";  //IdentityServer授权路径
//              options.ApiName = builder.Configuration["Ids4:ApiName"];
//              options.TokenRetriever = new Func<HttpRequest, string>(req =>
//              {
//                  var fromHeader = TokenRetrieval.FromAuthorizationHeader();
//                  var fromQuery = TokenRetrieval.FromQueryString();
//                  return fromHeader(req) ?? fromQuery(req);

//              });

//          });

////swagger web api doc 
//builder.Services.AddSwaggerGen(option =>
//{
//    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                                         {
//                                             Description = "在下框输入JWT生成的TOKEN,格式为Bearer 【TOKEN】",
//                                             Name = "Authorization",
//                                             In = ParameterLocation.Header,
//                                             Type = SecuritySchemeType.ApiKey,
//                                             BearerFormat = "JWT",
//                                             Scheme = "Bearer"
//                                         });
//option.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        { new OpenApiSecurityScheme{Reference=new OpenApiReference{ Type=ReferenceType.SecurityScheme,Id="Bearer"} }, new string[]{ } }

//    });

//option.SwaggerDoc("v1", new OpenApiInfo
//{
//    Title = "Crops_Trace_Api",
//    Version = "v1",
//    Description = "http://{ip}:{port}/api/crops-trace/v1{uri}"
//});

//// 为 Swagger JSON and UI设置xml文档注释路径
//var basePath = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
//var xmlPath = System.IO.Path.Combine(basePath, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
//if (!File.Exists(xmlPath))
//{
//    XmlDocument document = new XmlDocument();
//    XmlElement xmlElement = document.CreateElement("root");
//    XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");//xml文档的声明部分
//    document.AppendChild(declaration);
//    document.AppendChild(xmlElement);
//    document.Save(xmlPath);
//}
//option.IncludeXmlComments(xmlPath);

//option.DocumentFilter<HiddenApiFilter>();
//});
#endregion

#region 配置跨域
builder.Services.AddCors(cors =>
{
    cors.AddPolicy("Cors", policy =>
    {
        policy.WithOrigins("*", "*");
        policy.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS");
        policy.AllowAnyHeader()
              .AllowAnyOrigin()
              .AllowAnyMethod();
    });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

#region 开发时注释，发布时启用
//app.UseSwagger(c =>
//{
//    //Change the path of the end point , should also update UI middle ware for this change                
//    c.RouteTemplate = "api-doc/crops-trace/{documentName}/swagger.json";
//});
////启用中间件服务对swagger-ui，指定Swagger JSON终结点
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/api-doc/crops-trace/v1/swagger.json", "Crops_Trace_Api v1");
//    //c.RoutePrefix = "swagger";  //默认
//    c.RoutePrefix = "api-doc/crops-trace";
//});
#endregion

app.Run();
