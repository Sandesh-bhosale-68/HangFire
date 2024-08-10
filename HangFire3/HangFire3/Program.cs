using Hangfire;
using HangFire3.IService;
using HangFire3.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(x => x.UseSqlServerStorage(@"data source =192.168.0.31;uid=sa; password=sa@123;Initial Catalog = HRMIS_Report; Integrated security = false;TrustServerCertificate=True;"));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<IReprocess, Reprocess>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting(); 

app.UseAuthorization();




app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard(); 
});
app.Run();
