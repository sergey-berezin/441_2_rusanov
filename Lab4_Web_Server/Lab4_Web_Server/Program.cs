var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(builder =>
{
    builder
        .WithOrigins("*")
        .WithHeaders("*")
        .WithMethods("*");
});

app.MapControllers();
app.Run();

public partial class Program
{

}