using Lab4_Web_Server;

var builder = WebApplication.CreateBuilder(args);

var bertModelService = new BertModelService();
bertModelService.GetBertModel();

builder.Services.AddControllers();
builder.Services.AddSingleton<BertModelService>(bertModelService);
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