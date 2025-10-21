using CalorieTracker.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ����������� ���������� ���� ������
builder.Services.AddDbContext<CalorieContext>(options =>
    options.UseMySql("server=127.0.0.1;uid=root;pwd=;database=CalorieTracker",
        new MySqlServerVersion(new Version(8, 0, 11))));

builder.Services.AddDbContext<AuthContext>(options =>
    options.UseMySql("server=127.0.0.1;uid=root;pwd=;database=CalorieTracker",
        new MySqlServerVersion(new Version(8, 0, 11))));

builder.Services.AddDbContext<StatsContext>(options =>
    options.UseMySql("server=127.0.0.1;uid=root;pwd=;database=CalorieTracker",
        new MySqlServerVersion(new Version(8, 0, 11))));

// ��������� �������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ��������� pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CalorieTracker API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "CalorieTracker API V2");
    });
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// �������� ���� ������ ��� �������
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CalorieContext>();
    context.Database.EnsureCreated();
}

app.Run();