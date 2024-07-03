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
using OnlineE�itimPlatformuApi.Seeds;
using Service.ImagesAndVideosSevices;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);


//AddScoped�n burda ki amac� �u her interface g�rd���nde onun clas�ndan bir nesne olu�tur
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

//sepet i�in eklendi
builder.Services.AddMemoryCache();

//dbContext sql ba�lant�s� 
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
        sqlOptions =>
        {
            //migrationun yerini belirtiyorum
            sqlOptions.MigrationsAssembly("Data");
            //alt k�s�m ba�lant� hatalar�nda otomatik tekrar deneme i�lemi yapmam�z i�in var
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,//tekrar sayisi
                maxRetryDelay: TimeSpan.FromSeconds(30),//denemeler aras� s�re
                errorNumbersToAdd: null);//hata numaralar� i�in null ise  varsay�lan hata numaralar�n� kullanmamz� sa�lar
        });
});

//cors asp.nette g�venlik sistemidir
builder.Services.AddCors(opt =>
{
    //cors yap�s� her wep sitesi istek g�nderince kabul edilecek �ekilde ac�lm�st�r
    opt.AddDefaultPolicy(x => { x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Database olu�turma ve sabit de�erleri atama
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Veritaban� yoksa olu�tur ve sabit de�erleri atama i�lemi
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
