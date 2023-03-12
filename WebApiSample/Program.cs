using AutoMapper;
using Common;
using Serilog;
using Serilog.Enrichers.Span;
using WebApiSample;
using WebApiSample.Swashbuckle;
using WebApiSample.SystemTextJson;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithSpan()
    .Enrich.FromLogContext()
    //.WriteTo.Console(new LogPropertiesFormatter())
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.Configure(options =>
    {
        options.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId | ActivityTrackingOptions.ParentId;
    });

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(o => o.InputFormatters.Add(new TreatNonNullableAsRequiredInputFormatter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    /*...*/
    c.SchemaFilter<RequireNonNullablePropertiesSchemaFilter>();
    c.SupportNonNullableReferenceTypes(); // Sets Nullable flags appropriately.              
    c.UseAllOfToExtendReferenceSchemas(); // Allows $ref enums to be nullable
    c.UseAllOfForInheritance();  // Allows $ref objects to be nullable

});

builder.Services.ConfigureHttpJsonOptions(x => { });

builder.Services.AddMediatR(x => { x.RegisterServicesFromAssemblyContaining<Program>(); });
builder.Services.AddAutoMapper(x => { x.AddProfile<MappingProfile>(); });

var app = builder.Build();

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
