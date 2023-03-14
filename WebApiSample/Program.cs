using AutoMapper;
using Common;
using Serilog;
using Serilog.Enrichers.Span;
using System.Text.Json.Serialization.Metadata;
using WebApiSample;
using WebApiSample.Swashbuckle;
using WebApiSample.SystemTextJson;
using Common.Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithSpan()
    .Enrich.FromLogContext()
    .Enrich.WithCustomLogLevel()
    .MinimumLevel.Verbose()
    //.WriteTo.Console(new LogPropertiesFormatter())
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {MyLogLevel}] {Message:lj}{NewLine}{Properties}{NewLine}{Exception}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose) // TODO: Can we change how log levels are printed (to fit standard naming and not use Serilog's chosen loglevel names), but keep the color logic stuff?
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.Configure(options =>
    {
        options.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId | ActivityTrackingOptions.ParentId;
    });

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers =
            {
                JsonModifiers.MakeNonNullableValueTypesRequired
            }
        };
    });

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
