using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
	options.Cookie.HttpOnly = true;  // Cookie chỉ có thể được truy cập từ server
	options.Cookie.IsEssential = true; // Cookie luôn có mặt trong ứng dụng
});

// Cấu hình Authentication JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				var token = context.HttpContext.Request.Cookies["access_token"];
				if (!string.IsNullOrEmpty(token))
				{
					context.Token = token;
				}
				return Task.CompletedTask;
			}
		};

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = "Issuer", // Issuer
			ValidAudience = "Audience", // Audience
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super_secret_key_123!")) // Secret key
		};
	});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseSession(); // Thêm middleware Session vào pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

// Sử dụng middleware Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=TrangChu}/{action=Index}/{id?}");

app.Run();
