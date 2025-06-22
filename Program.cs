using HongDucFashion.Models;
using HongDucFashion.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Đăng ký HttpClient cho DI (dùng cho Blazor Server)
builder.Services.AddHttpClient();

// Thêm xác thực người dùng
builder.Services.AddAuthorizationCore();



// Đăng ký các dịch vụ cần thiết

// Dịch vụ quản lý san pham
builder.Services.AddScoped<ProductsService>();

// Dich vụ xác thực người dùng
builder.Services.AddScoped<AuthApiService>();

builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<ApiAuthenticationStateProvider>());

var connectionString = builder.Configuration.GetConnectionString("HongDucFashionDb");

builder.Services.AddDbContext<HongDucFashionV1Context>(options =>
    options.UseSqlServer(connectionString));

// Đăng ký controller và API routing
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Đăng ký các route cho API controllers
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
