using VersaLog_server.Startup;
using VersaLog_server.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.RegisterServices(builder.Configuration.GetConnectionString("AWSConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapUserEndpoints(); // Method to list all endpoints of API
app.UseHttpsRedirection();
app.Run();
