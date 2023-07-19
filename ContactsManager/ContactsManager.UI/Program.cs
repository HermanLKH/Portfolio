using ContactsManager.Core.Domain.IdentityEntities;
using DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

// logging
builder.Host.ConfigureLogging(loggingProvider =>
{
	loggingProvider.ClearProviders();
	loggingProvider.AddConsole();
	loggingProvider.AddDebug();
	loggingProvider.AddEventLog();
});

builder.Services.AddControllersWithViews(options =>
{
    // to prevent xsrf attack
    // only for applicable methods, eg: post
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// create users & roles table with these properties
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
	{
		options.Password.RequiredLength = 5;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireLowercase = true;
		options.Password.RequireDigit = false;
		options.Password.RequiredUniqueChars = 3;
	})
	// store the data using this dbcontext
	.AddEntityFrameworkStores<ApplicationDbContext>()
	// generate tokens
	.AddDefaultTokenProviders()
	// create repositories for user and role
	.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
	.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

// 
builder.Services.AddAuthorization(options =>
{
	// enforces authorization policy in which
	// users must be logged in to access every action method of entire application
	options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
	options.AddPolicy("NotAuthorized", policy =>
	{
		policy.RequireAssertion(context =>
		{
			return context.User.Identity?.IsAuthenticated == false;
		});
	});
});

// fallback url if authorization fails
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Account/Login";
});


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

// enable http logging
//app.UseHttpLogging();

if (!builder.Environment.IsEnvironment("Test"))
{
	// help to locate wkhtmltopdf.exe -> to convert view to pdf
	Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

// inform client & server to enable http for all requests & responses
app.UseHsts();
// to enstablish https connection between client & server
app.UseHttpsRedirection();

app.UseStaticFiles();
// identify action method based on route
app.UseRouting();
// enable authentication & read authentication cookie
app.UseAuthentication();
// enable authorization & validate access permissions of user
app.UseAuthorization();
// execute the filter pipeline (actions + filters)
app.MapControllers();

app.Run();

public partial class Program { } // make the auto-generated program accessible programmatically