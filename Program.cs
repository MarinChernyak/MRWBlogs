using Microsoft.EntityFrameworkCore;
using MRWBlogs.Components;
using MRWBlogs.Services;
using MRWBlogs.Utilities;
using MRWBlogs_DAL.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var conString = builder.Configuration.GetConnectionString("MRWBlogsConnection") ??
     throw new InvalidOperationException("Connection string 'MRWBlogsConnection'" +
    " not found.");
builder.Services.AddDbContext<MRWBlogsContext>(options =>
    options.UseSqlServer(conString,
      x => x.MigrationsAssembly(typeof(MRWBlogsContext).Assembly.FullName)));

builder.Services.AddScoped<ICookie, Cookie>();
builder.Services.AddScoped<DataTransferService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
