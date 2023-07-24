using CleanArchMvc.Infra.IoC;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;

var AllowOrigin = "_AllowOrigin";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowOrigin,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000/")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});
// Add services to the container.
builder.Services.AddInfrastructureAPI(builder.Configuration);
//ativar autenticacao e validar o token
builder.Services.AddInfrastructureJWT(builder.Configuration);
builder.Services.AddInfrastructureSwagger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "CleanArchMvc API",
        Description = "Projeto API",
        TermsOfService = new Uri("https://examplp.com/termoservico"),
        Contact = new OpenApiContact
        {
            Name = "Contato",
            Url = new Uri("https://examplo.com/contato")
        },
        License = new OpenApiLicense
        {
            Name = "Licen�a",
            Url = new Uri("https://examplo.com/licenca")
        }
    });

    // usando System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
// builder.Services.AddCors(options =>
// {
//     options.AddDefaultPolicy(builder =>
//     {
//         builder.AllowAnyOrigin()
//                .AllowAnyMethod()
//                .AllowAnyHeader();
//     });
// });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

app.UseHttpsRedirection();

app.UseStatusCodePages();
app.UseRouting();
app.UseCors(AllowOrigin);
app.UseAuthentication();
app.UseAuthorization();
//app.UseCors(option => option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();

app.Run();
