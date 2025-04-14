using Application;
using Infrastructure;
using Infrastructure.SignalR.Hubs;
using Persistence;
using Scalar.AspNetCore;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddApplication()
    .AddInfrastructure();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors(PresentationServiceRegistration.AllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ReportHub>("/report-hub");

app.Run();
