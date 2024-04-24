using K_meansAlgorithmWebsite.Server;
using K_meansAlgorithmWebsite.Server.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddScoped<ClusterService>();
builder.Services.AddScoped<RegressionService>();
//builder.Services.AddSingleton<DataSaver>();
builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseCors("AllowAll");
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
