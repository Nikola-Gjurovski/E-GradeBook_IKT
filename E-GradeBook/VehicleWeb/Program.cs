using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reposiotry.Implementation;
using Reposiotry.Interface;
using Services.Implementation;
using Services.Interface;
using VehicleReposiotry;
using VehicleReposiotry.Implementation;
using VehicleReposiotry.Interface;
using VehicleReposiotry.Migrations;
using VehicleServices.Implementation;
using VehicleServices.Interface;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddTransient<IRoles, RoleRepository>();
builder.Services.AddScoped(typeof(ISubjectInt), typeof(SubjectImp));
builder.Services.AddScoped(typeof(IGrades), typeof(GradesRepository));
builder.Services.AddScoped(typeof(ISubjectProfessor), typeof(SubjectProfessorRepository));
builder.Services.AddScoped(typeof(ISubjectStudent), typeof(SubjectStudentRepository));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<ISubject, SubjectRepository>();
builder.Services.AddTransient<IGradesService, GradeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
