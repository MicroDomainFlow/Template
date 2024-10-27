#if (IncludeExample)
using ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
using ContentService.Core.ApplicationService.Aggregates.Posts.EventHandlers;
#endif
using ContentService.Infrastructure.Persistence.Sql.Commands.Common;
using ContentService.Infrastructure.Persistence.Sql.Queries.Common;

#if (IncludeExample)
using EventBus.Messages.Aggregates.Posts.Events;
#endif
using FluentValidation;

using MassTransit;
using MassTransit.Logging;

using MDF.Framework.Extensions.ExtensionMethods;
using MDF.Framework.Infrastructure.Commands.Interceptors.Extensions;
using MDF.Framework.LayersContracts.ApplicationServices.MediatorExtensions;
using MDF.Framework.LayersContracts.Persistence.Commands;
using MDF.Framework.LayersContracts.Persistence.Queries;
using MDF.Framework.Middlewares;
using MDF.Framework.SeedWork;
using MDF.Resources.Extensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

#if (ThisProjectIsAspireNet == false)
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
#endif
var builder = WebApplication.CreateBuilder(args);
#if (ThisProjectIsAspireNet == false)
#region Remove This section if Using Aspire.net
builder.Services.AddLogging(option =>
{
	option.ClearProviders();
	option.AddOpenTelemetry(logging =>
	{
		var resourceBuilder = ResourceBuilder
			.CreateDefault()
			.AddService(builder.Environment.ApplicationName, serviceInstanceId: Environment.MachineName);

		logging.SetResourceBuilder(resourceBuilder)

			// ConsoleExporter is used for demo purpose only.
			// In production environment, ConsoleExporter should be replaced with other exporters (e.g. OTLP Exporter).
			.AddConsoleExporter();
	});
});

builder.Services.AddOpenTelemetry().ConfigureResource(cfg =>
{
	cfg.AddService(builder.Environment.ApplicationName, /*serviceInstanceId: Environment.MachineName,*/ autoGenerateServiceInstanceId: true);
})
.WithTracing(tracing =>
{
	tracing
		.AddSource(builder.Environment.ApplicationName)
		.AddSource(DiagnosticHeaders.DefaultListenerName)// MassTransit ActivitySource
		.ConfigureResource(resource =>
			resource.AddService(
				serviceName: builder.Environment.ApplicationName,
				autoGenerateServiceInstanceId: true))
		.AddAspNetCoreInstrumentation()
		.AddHttpClientInstrumentation()
		.AddEntityFrameworkCoreInstrumentation()
		.AddConsoleExporter();
	//.AddOtlpExporter(cfg => cfg.Endpoint = new("http://localhost:4317"));
})
.WithMetrics(metrics =>
{
	metrics
		.AddAspNetCoreInstrumentation()
		.AddHttpClientInstrumentation()
		.AddRuntimeInstrumentation()
		.AddProcessInstrumentation()
		.AddPrometheusExporter();
	//	.AddOtlpExporter(/*cfg => cfg.Endpoint = new("http://localhost:4317")*/);
});
#endregion End Remove This section if Using Aspire.net	
#endif
builder.Services.AddCommonLocalization(Path.Combine("ContentService", "Resources"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddValidatorsFromAssemblies(new[]
{
#if (IncludeExample)
	typeof(CreatePostCommandValidation).Assembly,
#endif
#if (!IncludeExample)
	typeof(OneClassFromCoreApplicaitonServiceLayer).Assembly,
#endif
	typeof(Entity).Assembly
});


builder.Services.AddMediatRWithNamespace("ContentService.Core");

//MediatR Pipelines
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<IUnitOfWork, CommonUnitOfWork>();

//command dbcontext
builder.Services.AddDbContext<ContentServiceCommandDbContext>((serviceProvider, options) =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), option =>
	{
		option.EnableRetryOnFailure(6);
	});

	options.UseCommonShadowPropertiesInterceptor()
	.UseDomainEventsDispatcherInterceptor();
});

//query dbcontext
builder.Services.AddDbContext<ContentServiceQueryDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), option =>
	{
		option.EnableRetryOnFailure(6);
	});
	options.UseCommonShadowPropertiesInterceptor()
		.UseDomainEventsDispatcherInterceptor();
});

builder.Services.AddAutoMapper(options => options.AddMaps(
	typeof(Program).Assembly
	, typeof(ContentServiceQueryDbContext).Assembly));

//ثبت خودکار ریپازیتوریها
builder.Services.Scan(s => s.
FromAssembliesOf(typeof(ContentServiceCommandDbContext))
	.AddClasses(classes => classes.AssignableTo(typeof(ICommandRepository<>)))
	.AsImplementedInterfaces()
	.WithScopedLifetime()

.FromAssembliesOf(typeof(ContentServiceQueryDbContext))
	.AddClasses(classes => classes.AssignableTo<IQueryRepository>())
	.AsImplementedInterfaces()
	.WithScopedLifetime()
);

#if (ThisProjectIsAspireNet)
//Masstransit Configuration if using Aspire.net you must setup masstransit settings for app host
//https://github.com/MassTransit/MassTransit/discussions/4780
//https://fiyaz-hasan-me-blog.azurewebsites.net/aspire-messaging-with-rabbitmq-and-masstransit/
//https://www.youtube.com/watch?v=GB4oAcja55w
#endif
#if (ThisProjectIsAspireNet == false)
#region Masstransit Configuration if using Aspire.net you must setup masstransit settings for app host
//https://github.com/MassTransit/MassTransit/discussions/4780
//https://fiyaz-hasan-me-blog.azurewebsites.net/aspire-messaging-with-rabbitmq-and-masstransit/
//https://www.youtube.com/watch?v=GB4oAcja55w

builder.Services.AddMassTransit(configurator =>
{
	configurator.SetKebabCaseEndpointNameFormatter();

	configurator.AddEntityFrameworkOutbox<ContentServiceCommandDbContext>(o =>
	{
		o.QueryDelay = TimeSpan.FromSeconds(1);
		o.UseSqlServer(false);//set false when using multiple DbContexts
		o.UseBusOutbox();
	});
	//The outbox can also be added to all consumers using a configure endpoints callback:
	// https://masstransit.io/documentation/configuration/middleware/outbox
	configurator.AddConfigureEndpointsCallback((context, name, cfg) =>
	{
		cfg.UseEntityFrameworkOutbox<ContentServiceCommandDbContext>(context);
	});
#if (IncludeExample)
	configurator.AddConsumers(typeof(CommentAddedEvent).Assembly, typeof(CommentAddedEventHandler).Assembly);
#endif
#if (!IncludeExample)
	configurator.AddConsumers(typeof(EventsMustCreateInEventBus.MessageLayer).Assembly, typeof(EventHandlerMustCreateInCoreApplicationServiceLayer).Assembly);
#endif

	configurator.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host("amqp://guest:guest@localhost:5672");
		cfg.AutoStart = true;
		cfg.ReceiveEndpoint("ContentService", c =>
		{
			c.ConfigureConsumers(context);
			c.UseEntityFrameworkOutbox<ContentServiceCommandDbContext>(context);
			c.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
			c.UseMessageRetry(r => r.Immediate(5));
		});
	});
});
//add masstransit source to this https://masstransit.io/documentation/configuration/observability#open-telemetry
builder.Services.AddOpenTelemetry()
	.ConfigureResource(resource => resource.AddService("content-service"))
	.WithTracing(tracing => tracing
		.AddAspNetCoreInstrumentation()
		.AddConsoleExporter())
	.WithMetrics(metrics => metrics
		.AddAspNetCoreInstrumentation()
		.AddConsoleExporter());
builder.Services.AddLogging(options =>
{
	options.SetMinimumLevel(LogLevel.Information);
	options.AddOpenTelemetry(cfg =>
	{
		cfg.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("content-service"));
	});
});
#endregion Masstransit Configuration if using Aspire.net you must setup masstransit settings for app host
#endif

var app = builder.Build();

app.UseRequestLocalization(options =>
{
	var supportedCultures = new[] { "fa-IR", "en-US", "es" };
	options.SetDefaultCulture(supportedCultures[0])
		.AddSupportedCultures(supportedCultures)
		.AddSupportedUICultures(supportedCultures)
		.ApplyCurrentCultureToResponseHeaders = true;
});
//app.UseRequestCulture();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();

app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
	app.UseGlobalExceptionResultHandler();
}



app.MapControllers();
#if (ThisProjectIsAspireNet == false)
ContentService.Endpoints.API.LoggerExtensions.StartingApp(app.Logger);// Remove this section if using Aspire.net
#endif
app.Run();
#if (ThisProjectIsAspireNet == false)
#region Remove this section if using Aspire.net
namespace ContentService.Endpoints.API
{
	internal static partial class LoggerExtensions
	{
		[LoggerMessage(LogLevel.Information, "Starting the app...")]
		public static partial void StartingApp(this ILogger logger);

		[LoggerMessage(LogLevel.Information, "Stoping the app...")]
		public static partial void StopingApp(this ILogger logger);

		//[LoggerMessage(LogLevel.Information, "Food `{name}` price changed to `{price}`.")]
		//public static partial void FoodPriceChanged(this ILogger logger, string name, double price);
	}
}
#endregion End Remove this section if using Aspire.net
#endif