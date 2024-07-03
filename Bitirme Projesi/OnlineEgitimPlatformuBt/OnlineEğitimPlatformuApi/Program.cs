using Core.IImagesAndIVideosSevices;
using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using Data.Context;
using Data.Repositories;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OnlineEðitimPlatformuApi.Seeds;
using Service.ImagesAndVideosSevices;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);


//AddScopedýn burda ki amacý þu her interface gördüðünde onun clasýndan bir nesne oluþtur
builder.Services.AddScoped(typeof(IRepostory<>), typeof(Repostory<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddScoped<IUserRepostory, UserRepostory>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUserRoleRepostory, UserRoleRepostory>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

builder.Services.AddScoped<IBolumRepostory, BolumRepostory>();
builder.Services.AddScoped<IBolumService, BolumService>();

builder.Services.AddScoped<ISatinAlinanKursService, SatinAlinanKursService>();
builder.Services.AddScoped<ISatinAlinanKursRepostory, SatinAlinanKursRepostory>();

builder.Services.AddScoped<IDersRepostory, DersRepostory>();
builder.Services.AddScoped<IDersService, DersService>();

builder.Services.AddScoped<IKategoriRepostory, KategoriRepostory>();
builder.Services.AddScoped<IKategoriService, KategoriService>();

builder.Services.AddScoped<IKursRepostory, KursRepostory>();
builder.Services.AddScoped<IKursService, KursService>();

builder.Services.AddScoped<IPuanlamaRepostory, PuanlamaRepostory>();
builder.Services.AddScoped<IPuanlamaService, PuanlamaService>();

builder.Services.AddScoped<IYorumRepostory, YorumRepostory>();
builder.Services.AddScoped<IYorumService, YorumService>();

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IVideoSevices, VideoService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//sepet için eklendi
builder.Services.AddMemoryCache();

//dbContext sql baðlantýsý 
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
        sqlOptions =>
        {
            //migrationun yerini belirtiyorum
            sqlOptions.MigrationsAssembly("Data");
            //alt kýsým baðlantý hatalarýnda otomatik tekrar deneme iþlemi yapmamýz için var
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,//tekrar sayisi
                maxRetryDelay: TimeSpan.FromSeconds(30),//denemeler arasý süre
                errorNumbersToAdd: null);//hata numaralarý için null ise  varsayýlan hata numaralarýný kullanmamzý saðlar
        });
});

//cors asp.nette güvenlik sistemidir
builder.Services.AddCors(opt =>
{
    //cors yapýsý her wep sitesi istek gönderince kabul edilecek þekilde acýlmýstýr
    opt.AddDefaultPolicy(x => { x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Database oluþturma ve sabit deðerleri atama
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Veritabaný yoksa oluþtur ve sabit deðerleri atama iþlemi
    if (context.Database.EnsureCreated())
    {
        await PermissionSeed.Seed(services);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
