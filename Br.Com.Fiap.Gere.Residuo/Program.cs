using Asp.Versioning;
using AutoMapper;
using Br.Com.Fiap.Gere.Residuo;
using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Agenda;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Bairro;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Caminhao;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Morador;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Motorista;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Notificacao;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Usuario;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Agenda;
using Br.Com.Fiap.Gere.Residuo.Service.Bairro;
using Br.Com.Fiap.Gere.Residuo.Service.Caminhao;
using Br.Com.Fiap.Gere.Residuo.Service.Morador;
using Br.Com.Fiap.Gere.Residuo.Service.Motorista;
using Br.Com.Fiap.Gere.Residuo.Service.Notificacao;
using Br.Com.Fiap.Gere.Residuo.Service.Usuario;
using Br.Com.Fiap.Gere.Residuo.Swagger;
using Br.Com.Fiap.Gere.Residuo.Utils.Email;
using Br.Com.Fiap.Gere.Residuo.Utils.Hash;
using Br.Com.Fiap.Gere.Residuo.Utils.Token;
using Br.Com.Fiap.Gere.Residuo.Validation.ActionFilters;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Morador;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Motorista;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Notificacao;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Usuario;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


#region DEFAULT
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Informe um Token JWT para o Swagger utilizar nas requisições",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
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
    options.SchemaFilter<EnumSchemaFilter>();
    options.OperationFilter<SwaggerDefaultValues>();

    

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});


builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true); // Desabilita a validação automática (necessário para implementação de um ValidationFilterAttribute Personalizado
#endregion

#region BANCO DE DADOS
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
var enableSensitiveDataLogging = builder.Environment.IsStaging() || builder.Environment.IsDevelopment();
builder.Services.AddDbContext<DatabaseContext>(
    opt => opt.UseOracle(connectionString).EnableSensitiveDataLogging(enableSensitiveDataLogging)
);
#endregion


#region ISERVICECOLLECTION

#region VALIDATION
builder.Services.AddScoped<ValidationFilterAttribute>();
#endregion

#region OTHERS
builder.Services.AddScoped<SHA256>(provider => SHA256.Create());
builder.Services.AddScoped<StringBuilder>();
builder.Services.AddScoped<IHashUtils, HashUtils>();
builder.Services.AddScoped<IJWTUtils, JWTUtils>();
builder.Services.AddTransient<ISmtpUtils, SmtpUtils>(provider => new SmtpUtils(
    builder.Configuration.GetConnectionString("MailTrapUser"),
    builder.Configuration.GetConnectionString("MailTrapPassword"),
    builder.Configuration.GetConnectionString("MailTrapHost"),
    Int32.Parse(builder.Configuration.GetConnectionString("MailTrapPort")),
    builder.Configuration.GetConnectionString("MailTrapSender")
    ));
#endregion

#region SERVICES
builder.Services.AddScoped<IAgendaService, AgendaService>();
builder.Services.AddScoped<IBairroService, BairroService>();
builder.Services.AddScoped<ICaminhaoService, CaminhaoService>();
builder.Services.AddScoped<IMoradorService, MoradorService>();
builder.Services.AddScoped<IMotoristaService, MotoristaService>();
builder.Services.AddScoped<INotificacaoService, NotificacaoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
#endregion

#region REPOSITORIES
builder.Services.AddScoped<IAgendaRepository, AgendaRepository>();
builder.Services.AddScoped<IBairroRepository, BairroRepository>();
builder.Services.AddScoped<ICaminhaoRepository, CaminhaoRepository>();
builder.Services.AddScoped<IMoradorRepository, MoradorRepository>();
builder.Services.AddScoped<IMotoristaRepository, MotoristaRepository>();
builder.Services.AddScoped<INotificacaoRepository, NotificacaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
#endregion

#region VIEWMODEL
builder.Services.AddScoped<ErrorViewModel>();
#endregion
#endregion

#region AUTENTICACAO
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("f+ujXAKHk00L5jlMXo2XhAWawsOoihNP1OiAM25lLSO57+X7uBMQgwPju6yzyePi")), // Secret de geração do Token JWT
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
#endregion


#region AUTOMAPPER
// Configuração do AutoMapper
var mapperConfig = new AutoMapper.MapperConfiguration(c => {
    c.AllowNullCollections = true;
    c.AllowNullDestinationValues = true;

    #region BAIRRO
    c.CreateMap<BairroModel, BairroViewModel>();
    c.CreateMap<BairroViewModel, BairroModel>();
    c.CreateMap<BairroCreateViewModel, BairroModel>();
    c.CreateMap<BairroUpdateViewModel, BairroModel>();
    c.CreateMap<BairroViewModelComPaginacaoReference, BairroModel>();
    c.CreateMap<BairroViewModelForResponse, BairroModel>();
    c.CreateMap<BairroModel, BairroViewModelForResponse > ();
    #endregion

    #region MORADOR
    c.CreateMap<MoradorModel, MoradorViewModel>();
    c.CreateMap<MoradorViewModel, MoradorModel>();
    c.CreateMap<MoradorCreateViewModel, MoradorModel>();
    c.CreateMap<MoradorUpdateViewModel, MoradorModel>();
    c.CreateMap<MoradorViewModelComPaginacaoReference, MoradorModel>();
    c.CreateMap<MoradorViewModelForResponse, MoradorModel>();
    c.CreateMap<MoradorModel, MoradorViewModelForResponse>();
    #endregion

    #region CAMINHAO
    c.CreateMap<CaminhaoModel, CaminhaoViewModel>();
    c.CreateMap<CaminhaoViewModel, CaminhaoModel>();
    c.CreateMap<CaminhaoCreateViewModel, CaminhaoModel>();
    c.CreateMap<CaminhaoUpdateViewModel, CaminhaoModel>();
    c.CreateMap<CaminhaoViewModelComPaginacaoReference, CaminhaoModel>();
    c.CreateMap<CaminhaoViewModelForResponse, CaminhaoModel>();
    c.CreateMap<CaminhaoModel, CaminhaoViewModelForResponse>();
    #endregion

    #region MOTORISTA
    c.CreateMap<MotoristaModel, MotoristaViewModel>();
    c.CreateMap<MotoristaViewModel, MotoristaModel>();
    c.CreateMap<MotoristaCreateViewModel, MotoristaModel>();
    c.CreateMap<MotoristaUpdateViewModel, MotoristaModel>();
    c.CreateMap<MotoristaViewModelComPaginacaoReference, MotoristaModel>();
    c.CreateMap<MotoristaViewModelForResponse, MotoristaModel>();
    c.CreateMap<MotoristaModel, MotoristaViewModelForResponse>();
    #endregion

    #region AGENDA
    c.CreateMap<AgendaModel, AgendaViewModel>();
    c.CreateMap<AgendaViewModel, AgendaModel>();
    c.CreateMap<AgendaCreateViewModel, AgendaModel>();
    c.CreateMap<AgendaUpdateViewModel, AgendaModel>();
    c.CreateMap<AgendaViewModelComPaginacaoReference, AgendaModel>();
    c.CreateMap<AgendaViewModelForResponse, AgendaModel>();
    c.CreateMap<AgendaModel, AgendaViewModelForResponse>();
    #endregion

    #region NOTIFICACAO
    c.CreateMap<NotificacaoModel, NotificacaoViewModel>();
    c.CreateMap<NotificacaoViewModel, NotificacaoModel>();
    c.CreateMap<NotificacaoViewModelComPaginacaoReference, NotificacaoModel>();
    c.CreateMap<NotificacaoViewModelForResponse, NotificacaoModel>();
    c.CreateMap<NotificacaoModel, NotificacaoViewModelForResponse>();
    #endregion

    #region USUARIO
    c.CreateMap<UsuarioModel, UsuarioViewModel>();
    c.CreateMap<UsuarioViewModel, UsuarioModel>();
    c.CreateMap<UsuarioCreateViewModel, UsuarioModel>();
    c.CreateMap<UsuarioUpdateViewModel, UsuarioModel>();
    c.CreateMap<UsuarioViewModelComPaginacaoReference, UsuarioModel>();
    c.CreateMap<UsuarioViewModelForResponse, UsuarioModel>();
    c.CreateMap<UsuarioModel, UsuarioViewModelForResponse>();
    c.CreateMap<UsuarioModel, UsuarioLoginViewModel>();
    c.CreateMap<UsuarioLoginViewModel, UsuarioModel>();
    #endregion
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion


#region VERSIONAMENTO
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
// Classes presentes no SwagggerConfig.cs
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline. - Swagger Versionamento API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
