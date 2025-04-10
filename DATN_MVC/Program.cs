using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Th?i gian session h?t h?n
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
IdentityModelEventSource.ShowPII = true;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();// thêm cái này 
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=GioHang}/{action=GioHang}/{id?}");

app.Run();
