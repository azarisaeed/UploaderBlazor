using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UploaderBlazor.Security;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UploaderBlazorContextConnection") ?? throw new InvalidOperationException("Connection string 'UploaderBlazorContextConnection' not found.");

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultSignInScheme = "External";
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("External").AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "780718390029-8p97a1e9jl357kqch86erf9ah8558sik.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
});

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.TryAddSingleton(builder.Configuration.GetSection("PermissionModel").Get<PermissionModel>());
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}


app.UseStaticFiles();

app.UseRouting();


app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseAuthentication(); ;
app.UseAuthorization();

app.Run();
