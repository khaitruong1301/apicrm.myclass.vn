using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SoloDevApp.Api.Filters;
using SoloDevApp.Api.Middlewares;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.AutoMapper;
using SoloDevApp.Service.Helpers;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.SignalR;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Text;

namespace SoloDevApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string CorsPolicy = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ===================== REPOSITORY ======================
            services.AddTransient<IQuyenRepository, QuyenRepository>();
            services.AddTransient<INhomQuyenRepository, NhomQuyenRepository>();

            services.AddTransient<ILoTrinhRepository, LoTrinhRepository>();
            services.AddTransient<ILopHocRepository, LopHocRepository>();
            services.AddTransient<IKhoaHocRepository, KhoaHocRepository>();
            services.AddTransient<IChuongHocRepository, ChuongHocRepository>();
            services.AddTransient<IBaiHocRepository, BaiHocRepository>();
            services.AddTransient<ILoaiBaiHocRepository, LoaiBaiHocRepository>();
            services.AddTransient<ICauHoiRepository, CauHoiRepository>();
            services.AddTransient<IBaiTapRepository, BaiTapRepository>();
            services.AddTransient<IBaiTapNopRepository, BaiTapNopRepository>();
            services.AddTransient<IChuyenLopRepository, ChuyenLopRepository>();

            services.AddTransient<INguoiDungRepository, NguoiDungRepository>();

            services.AddTransient<IKhachHangRepository, KhachHangRepository>();
            services.AddTransient<IHocPhiRepository, HocPhiRepository>();

            // ==================== SERVICE ====================
            services.AddTransient<IQuyenService, QuyenService>();
            services.AddTransient<INhomQuyenService, NhomQuyenService>();

            services.AddTransient<ILoTrinhService, LoTrinhService>();
            services.AddTransient<ILopHocService, LopHocService>();
            services.AddTransient<IKhoaHocService, KhoaHocService>();
            services.AddTransient<IKhoaHocService, KhoaHocService>();
            services.AddTransient<IChuongHocService, ChuongHocService>();
            services.AddTransient<IBaiHocService, BaiHocService>();
            services.AddTransient<ILoaiBaiHocService, LoaiBaiHocService>();
            services.AddTransient<ICauHoiService, CauHoiService>();
            services.AddTransient<IBaiTapService, BaiTapService>();
            services.AddTransient<IBaiTapNopService, BaiTapNopService>();
            services.AddTransient<IChuyenLopService, ChuyenLopService>();

            services.AddTransient<INguoiDungService, NguoiDungService>();

            services.AddTransient<IKhachHangService, KhachHangService>();
            services.AddTransient<IHocPhiService, HocPhiService>();

            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IUploadService, UploadService>();

            // ==================== HELPER ====================
            services.AddSingleton<IFacebookService, FacebookService>();
            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<ITranslator, Translator>();

            // ==================== AUTO MAPPER ====================
            services.AddSingleton(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToViewModelProfile());
                cfg.AddProfile(new ViewModelToEntityProfile());
            }).CreateMapper());

            services.AddMvc(opt => {
                opt.Filters.Add(typeof(ValidateModelFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // ==================== SWAGGER ====================
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SOLO DEV API", Version = "v1" });
            });

            // ==================== CORS ORIGIN ====================
            services.AddCors(
                options => options.AddPolicy(CorsPolicy,
                builder => {
                    builder.WithOrigins("http://localhost:3000","http://crm.myclass.vn", "*")
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .Build();
                }));

            // ==================== SIGNALR ====================
            //services.AddSignalR();

            // ==================== SECTION CONFIG ====================
            //froservices.AddSingleton<IFacebookSettings>(
            //    Configuration.GetSection("FacebookSettings").Get<FacebookSettings>());
            services.AddSingleton<IMailSettings>(
                Configuration.GetSection("MailSettings").Get<MailSettings>());
            services.AddSingleton<IFtpSettings>(
                Configuration.GetSection("FtpSettings").Get<FtpSettings>());
            services.AddSingleton<ICaptchaSettings>(
                Configuration.GetSection("CaptchaSettings").Get<CaptchaSettings>());

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.AddSingleton<IAppSettings>(
                Configuration.GetSection("AppSettings").Get<AppSettings>());

            // ==================== FACEBOOK LOGIN ====================
            //services.AddAuthentication().AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
            //    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //});

            // ==================== JWT AUTHENTICATION CONFIG ====================
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                // Đặt tiền tố cho header token (Sử dụng mặc định là Bearer)
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false; // Cấu hình không bắt buộc sử dụng https
                //Lưu bearer token trong Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties
                x.SaveToken = true; // Sau khi đăng nhập thành công
                // Set or get các tham số lưu vào token
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Bắt buộc phải có SigningKey
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Issuer không bắt buộc
                    ValidateAudience = false, // Audience không bắt buộc
                    ValidateLifetime = true // Thời gian hết hạn (expires) là bắt buộc
                };
                x.IncludeErrorDetails = true;
                x.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c => //
                    {
                        c.NoResult();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // ==================== HANDLER EXCEPTION ====================
            //app.UseExceptionHadler();

            // ==================== CORS ORIGIN ====================
            app.UseCors(CorsPolicy);

            // ==================== SIGNALR ====================
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<AppHub>("/apphub");
            //});

            // ==================== AUTHEN JWT ====================
            app.UseAuthentication();

            app.UseHttpsRedirection();

            //khai bao su dung  quyen folder hinh
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("/wwwroot"),

            }) ;

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "images")),
            //    RequestPath = new PathString("/images"),

            //});
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "files")),
            //    RequestPath = new PathString("/files"),

            //});
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot","cmnd")),
            //    RequestPath = new PathString("/cmnd"),

            //});

            app.UseStaticFiles();



            app.UseMvc();

            // ==================== SWAGGER ====================
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SOLO DEV API VERSION 01");
            });


            
        }
    }
}