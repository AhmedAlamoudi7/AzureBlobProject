using Azure.Storage.Blobs;
using AzureBlobStorage.Web.Services;

var builder = WebApplication.CreateBuilder(args);
// Register BlobServiceClient as a singleton before building the app
builder.Services.AddSingleton(u => new BlobServiceClient(
    builder.Configuration.GetValue<string>("BlobConnection")
));
builder.Services.AddSingleton<IContainerService, ContainerService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
