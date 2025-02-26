using VersaLog.Server.Startup;
using VersaLog.Startup;
using VersaLog.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder2 =>
{
    builder2.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

builder.Services.AddCors(o => o.AddPolicy("Swagger", builder2 =>
{
    builder2.WithOrigins("http://localhost:8080")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var options = builder.Configuration.GetSection("DbOptions").Get<DbOptions>();

builder.Services.AddControllers();
builder.Services.RegisterServices(options);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Swagger");
}

app.UseCors("MyPolicy");
app.MapUserEndpoints(); // Method to list all endpoints of API
app.UseHttpsRedirection();
app.SetupMigration();
app.Run();
