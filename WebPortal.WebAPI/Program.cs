using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebPortal.Application.Auth;
using WebPortal.Persistence.Context;
using WebPortal.WebAPI.Middlewares;
using WebPortal.WebAPI.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;
if (environment.IsProduction())
{
    //add logging
}
builder.Services.InstallServicesInAssembly(configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMyAuthentication();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApplyAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseCustomExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors("ApplyAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

/*using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var context = serviceProvider.GetRequiredService<WebPortalDbContext>();
context.Database.EnsureCreated();*/

app.Run();