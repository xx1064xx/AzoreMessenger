using AzoreMessanger.DbAccess;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

using Microsoft.EntityFrameworkCore;
using AzoreMessanger.Models;
using AzoreMessanger.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AzoreMessanger.Controller;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<MessengerAppContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<DbInitializer>();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".js"] = "application/javascript";
provider.Mappings[".css"] = "text/css";

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,
    OnPrepareResponse = context =>
    {
        if (!context.File.Name.EndsWith(".html")) // Hier anpassen für die Dateitypen, die du bearbeiten möchtest
        {
            context.Context.Response.Headers["Cache-Control"] = "no-cache, no-store";
            context.Context.Response.Headers["Pragma"] = "no-cache";
            context.Context.Response.Headers["Expires"] = "-1";
        }
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

using (var azore = app.Services.CreateScope()) 
{
    azore.ServiceProvider.GetRequiredService<DbInitializer>().Run();
}

app.Run();

