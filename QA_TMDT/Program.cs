
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QA_TMDT.Helper;
using QA_TMDT.Model;
using QA_TMDT.Repository;
using QA_TMDT.Repository.Impl;
using QA_TMDT.Service;
using QA_TMDT.Service.Impl;
using QA_TMDT.Utils;
using Microsoft.OpenApi.Models;
using System.Text;


namespace QA_TMDT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập token vào đây"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            //AddDBContext
            builder.Services.AddDbContext<QaTmdtContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection")));
            //AddCloudinaryAcc
            builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));
            builder.Services.AddSingleton<CloudinaryService>();
            //AddJWT
            builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            builder.Services.AddScoped<IDanhMucRepository, DanhMucRepository>();
            builder.Services.AddScoped<IDanhMucService, DanhMucService>();

            builder.Services.AddScoped<ICuaHangRepository, CuaHangRepository>();
            builder.Services.AddScoped<ICuaHangService, CuaHangService>();

            builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
            builder.Services.AddScoped<IVoucherService, VoucherService>();

            builder.Services.AddScoped<IPhuongThucTTRepository, PhuongThucTTRepository>();
            builder.Services.AddScoped<IPhuongThucTTService, PhuongThucTTService>();

            builder.Services.AddScoped<IPhuongThucVCRepository, PhuongThucVCRepository>();
            builder.Services.AddScoped<IPhuongThucVCService, PhuongThucVCService>();

            builder.Services.AddScoped<IKhuyenMaiRepository, KhuyenMaiRepository>();
            builder.Services.AddScoped<IKhuyenMaiService, KhuyenMaiService>();

            builder.Services.AddScoped<IAnhRepository, AnhRepository>();
            builder.Services.AddScoped<IAnhService, AnhService>();

            builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
            builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();

            builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
            builder.Services.AddScoped<ISanPhamService, SanPhamService>();

            builder.Services.AddScoped<IGioHangRepository, GioHangRepository>();
            builder.Services.AddScoped<IGioHangService, GioHangService>();

            builder.Services.AddScoped<IDonHangRepository, DonHangRepository>();
            builder.Services.AddScoped<IDonHangService, DonHangService>();

            builder.Services.AddScoped<IChiTietSPRepository, ChiTietSPRepository>();

            builder.Services.AddScoped<IThongKeRepositoy, ThongKeRepository>();
            builder.Services.AddScoped<IThongKeService, ThongKeService>();

            var app = builder.Build();
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
