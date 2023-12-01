using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Json Serializer
builder.Services.AddControllers().AddNewtonsoftJson(options =>options.SerializerSettings.ReferenceLoopHandling= Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(options =>options.SerializerSettings.ContractResolver= new DefaultContractResolver());
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Test API",
        Description = "A Simple Example of Swagger Integration"
    });


// This should be added so that all the summary will be reflected on Swagger.
    var filename = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
var filepath = Path.Combine(AppContext.BaseDirectory, filename);
options.IncludeXmlComments(filepath);
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable CORS
app.UseCors(options => options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

}
if (app.Environment.IsDevelopment())
{

}
app.UseAuthorization();

app.MapControllers();

app.Run();
