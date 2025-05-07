using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DAL.Models;
using Pizzashop.BAL.Interfaces;
using BAL.Services;
using DAL.Interfaces;
using Pizzashop.Data.Repositories;
using BAL.Interfaces;
using DAL.Repository;
using BAL.Models.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserDetails, UserDetails>();
builder.Services.AddScoped<IUserList, UserList>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRolesAndPermissions, UserRolesAndPermissions>();
builder.Services.AddScoped<IUserRolesAndPermissionsRepository, UserRolesAndPermissionsRepository>();
builder.Services.AddScoped<IUserMenu, UserMenu>();
builder.Services.AddScoped<IUserMenuRepository, UserMenuRepository>();
builder.Services.AddScoped<ITable, BAL.Services.Table>();
builder.Services.AddScoped<IUserTableRepository, UserTableRepository>();
builder.Services.AddScoped<ITaxesAndFessService, TaxesAndFessService>();
builder.Services.AddScoped<ITaxesAndFessRepository, TaxesAndFess>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderservice, Orderservice>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IPermissions, Permissions>();
builder.Services.AddScoped<IUserroleService, UserRoleService>();
builder.Services.AddScoped<IKotRepository, KotRepository>();
builder.Services.AddScoped<IKotService, KotService>();
builder.Services.AddScoped<IKotTableRepository, KotTableRepository>();
builder.Services.AddScoped<IKotTableService, KotTableService>();
builder.Services.AddScoped<IKotTableRepository, KotTableRepository>();
builder.Services.AddScoped<IKotTableService, KotTableService>();
builder.Services.AddScoped<IWaitingRepository, WaitingRepository>();
builder.Services.AddScoped<IWaitingService, WaitingService>();
builder.Services.AddScoped<IOrderAppMenu, OrderAppMenu>();
builder.Services.AddScoped<IOrderAppMenuRepository, OrderAppMenuRepository>();
builder.Services.AddScoped<IDashboard, Dashboard>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<Permissions>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddControllersWithViews();

var conn = builder.Configuration.GetConnectionString("defaults");
builder.Services.AddDbContext<PizzaShopContext>(options => options.UseNpgsql(conn));
builder.Services.AddMvc().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {

                var token = context.Request.Cookies["jwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers["Authorization"] = "Bearer " + token;
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
                {
                    var path = context.Request.Path.Value;
                    if (path != "/Login/Login" && !context.Response.HasStarted)
                    {
                        context.Response.Cookies.Delete("jwtToken");
                        context.Response.Redirect("/Login/Login");
                        context.HandleResponse();
                    }
                    return Task.CompletedTask;
                },
            OnForbidden = context =>
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.Redirect("/Auth/AccessDenied");
                }
                return Task.CompletedTask;
            }


        };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
