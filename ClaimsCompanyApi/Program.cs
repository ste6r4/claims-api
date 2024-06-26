using ClaimsCompanyApi.Common;
using ClaimsCompanyApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context); 
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

void RegisterServices(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddControllers();
    webApplicationBuilder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(Constants.InMemoryConnectionString),
        ServiceLifetime.Transient);
    webApplicationBuilder.Services.AddEndpointsApiExplorer();
    webApplicationBuilder.Services.AddSwaggerGen();
    webApplicationBuilder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
}