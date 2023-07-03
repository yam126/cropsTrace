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

//��ȡ��վ�����ļ�
IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

string MOIConnStr = configuration["ConnectionStrings:CropsTraceConnStr"];

//���������ַ���
builder.Services.AddDbContext(options => options.AddMtrlSqlServer(configuration["ConnectionStrings:CropsTraceConnStr"]));

//����AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

#region ����ʱע�ͣ�����ʱ����
////setting authetication 
//builder.Services.AddAuthentication(builder.Configuration["Ids4:Scheme"])
//          .AddIdentityServerAuthentication(options =>
//          {
//              options.RequireHttpsMetadata = false;
//              options.Authority = $"http://{builder.Configuration["Ids4:IP"]}:{builder.Configuration["Ids4:Port"]}";  //IdentityServer��Ȩ·��
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
//                                             Description = "���¿�����JWT���ɵ�TOKEN,��ʽΪBearer ��TOKEN��",
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

//// Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
//var basePath = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
//var xmlPath = System.IO.Path.Combine(basePath, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
//if (!File.Exists(xmlPath))
//{
//    XmlDocument document = new XmlDocument();
//    XmlElement xmlElement = document.CreateElement("root");
//    XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");//xml�ĵ�����������
//    document.AppendChild(declaration);
//    document.AppendChild(xmlElement);
//    document.Save(xmlPath);
//}
//option.IncludeXmlComments(xmlPath);

//option.DocumentFilter<HiddenApiFilter>();
//});
#endregion

#region ���ÿ���
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

#region ����ʱע�ͣ�����ʱ����
//app.UseSwagger(c =>
//{
//    //Change the path of the end point , should also update UI middle ware for this change                
//    c.RouteTemplate = "api-doc/crops-trace/{documentName}/swagger.json";
//});
////�����м�������swagger-ui��ָ��Swagger JSON�ս��
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/api-doc/crops-trace/v1/swagger.json", "Crops_Trace_Api v1");
//    //c.RoutePrefix = "swagger";  //Ĭ��
//    c.RoutePrefix = "api-doc/crops-trace";
//});
#endregion

app.Run();
