using HRIS.Application.Services;
using HRIS.Domain;
using HRIS.Domain.Interfaces;
using HRIS.Domain.Models;
using HRIS.Infrastructure;
using HRIS.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Konfigurasi koneksi ke PostgreSQL
var connectionStringPostgre = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<hrisDbContext>(options => options.UseNpgsql(connectionStringPostgre));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = true;

})
.AddEntityFrameworkStores<hrisDbContext>();

builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])),
    };

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllowedRolesPolicy", policy =>
        policy.RequireRole("Employee", "Employee Supervisor", "HR Manager"));
});

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService,DepartmentService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>()
    ;
builder.Services.AddScoped<IProjempService, ProjempService>();
builder.Services.AddScoped<IProjempRepository, ProjempRepository>();

builder.Services.AddScoped<IDependentService, DependentService>();
builder.Services.AddScoped<IDependentRepository, DependentRepository>();

builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IProcessRepository, ProcessRepository>();

builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
