using Domain.Config;
using Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envFilePath))
{
    foreach (var line in File.ReadAllLines(envFilePath))
    {
        if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
            continue;

        var normalized = line.Trim();
        if (normalized.StartsWith("export ", StringComparison.OrdinalIgnoreCase))
            normalized = normalized[7..].Trim();

        var separatorIndex = normalized.IndexOf('=');
        if (separatorIndex <= 0)
            continue;

        var key = normalized[..separatorIndex].Trim();
        var value = normalized[(separatorIndex + 1)..].Trim();

        if (value.Length >= 2 && value.StartsWith('"') && value.EndsWith('"'))
            value = value[1..^1];

        Environment.SetEnvironmentVariable(key, value);
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
Container.Register(builder.Services,builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IIdentificationTypeService, IdentificationTypeService>();
builder.Services.AddTransient<ISaleService, SaleService>();
builder.Services.AddTransient<ISaleDetailsService, SaleDetailsService>();
builder.Services.AddTransient<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IQuotaService, QuotaService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer( options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", () => Results.Content(@"
<!DOCTYPE html>
<html lang=""es""><head>
    <meta charset=""utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <title>AdminCR Backend</title>
    <style>
        body { font-family: Arial, sans-serif; background: #111827; color: #f9fafb; display: grid; place-items: center; min-height: 100vh; margin: 0; }
        .card { background: #1f2937; padding: 2rem 2.5rem; border-radius: 14px; box-shadow: 0 10px 30px rgba(0,0,0,0.35); text-align: center; }
        h1 { margin-top: 0; }
        p { color: #d1d5db; }
        code { background: #0f172a; padding: 0.2rem 0.45rem; border-radius: 6px; }
    </style>
</head>
<body>
    <div class="card">
        <h1>AdminCR Backend</h1>
        <p>API funcionando correctamente.</p>
        <p>Base URL: <code>https://manageyourdreams-backend.onrender.com</code></p>
    </div>
</body>
</html>
", "text/html"));

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "AdminCR Backend" }));
app.MapControllers();
app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

app.Run();