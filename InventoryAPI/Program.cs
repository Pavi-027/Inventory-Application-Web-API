using InventoryEntities.IdentityEntities;
using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Implementation;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services;
using InventoryServices.Services.ServiceInterface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryConnectionString"))
);

//able to inject jwtservice class inside controllers
builder.Services.AddScoped<JWTService>();

//injecting emailservice to our application 
builder.Services.AddScoped<EmailService>();

//service to seed the data's to db
builder.Services.AddScoped<ContextSeedService>();

//Defining identityCore services
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    //Password Configuration
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;

    //Email Comfirmation
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddRoles<IdentityRole>() //able to add roles
    .AddRoleManager<RoleManager<IdentityRole>>() //able to make use of RoleManager
    .AddEntityFrameworkStores<InventoryDbContext>() //providing our dbcontext
    .AddSignInManager<SignInManager<ApplicationUser>>() //make use of signin manager
    .AddUserManager<UserManager<ApplicationUser>>() //make use of user manager to create users
    .AddDefaultTokenProviders(); //able to create tokes for email confirmation

//able to authenticate users using jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //validate the token based on the key we have rovided inside appsetting.development.json JWT:Key
            ValidateIssuerSigningKey = true,
            //the issuer signing key based on JWT:Kery
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            //issuer which in here is the api project url we are using
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            //validate the issuer(who ever is issusing the jwt)
            ValidateIssuer = true,
            //don't validate audience (angular side) 
            ValidateAudience = false
        };
    });

builder.Services.AddCors();

//services for errors
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
        .Where(x => x.Value.Errors.Count > 0)
        .SelectMany(x => x.Value.Errors)
        .Select(x => x.ErrorMessage).ToArray();

        var toReturn = new
        {
            Errors = errors
        };
        return new BadRequestObjectResult(toReturn);
    };
});

//services for roles, claims, & policy
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
    opt.AddPolicy("SalesPersonPolicy", policy => policy.RequireRole("SalesPerson"));

    opt.AddPolicy("AdminOrManagerPolicy", policy => policy.RequireRole("Admin", "Manager")); //Admin or Manager role
    opt.AddPolicy("AdminAndManagerPolicy", policy => policy.RequireRole("Admin").RequireRole("Manager")); //Admin and Manager role
    opt.AddPolicy("AllRolePolicy", policy => policy.RequireRole("Admin", "Manager", "SalesPerson"));

    opt.AddPolicy("AdminEmailPolicy", policy => policy.RequireClaim(ClaimTypes.Email, "admin@gmail.com"));
    opt.AddPolicy("salesPersonFullNamePolicy", policy => policy.RequireClaim(ClaimTypes.GivenName, "salesPerson"));
    opt.AddPolicy("ManagerEmailandFullnamePolicy", policy => policy.RequireClaim(ClaimTypes.GivenName, "manager")
    .RequireClaim(ClaimTypes.Email, "manager@gmail.com"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>(); 
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});
app.UseHttpsRedirection();

//adding UseAuthentication into our pipline and this should come before UseAuthorization
//Authentication verifies the identity of a user or service, and authorization determines their access rights.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//inject a service into our program.cs
#region ContextSeed
using var scope = app.Services.CreateScope();
try
{
    var contextSeedService= scope.ServiceProvider.GetService<ContextSeedService>();
    await contextSeedService.InitializeContextAsync();
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
    logger.LogError(ex.Message, "Failed to initialize and seen the database");

}
#endregion

app.Run();
