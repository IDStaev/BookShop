using BookShop.Common;
using BookShop.Data;
using BookShop.Repositories;
using BookShop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EntityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(EntityContext)));
});
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<Hasher>();
builder.Services.AddScoped<BookRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.Run();