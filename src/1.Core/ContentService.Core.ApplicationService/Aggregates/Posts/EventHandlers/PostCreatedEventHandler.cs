using EventBus.Messages.Aggregates.Posts.Events;

using MassTransit;

using MDF.Framework.LayersContracts.ApplicationServices;
using MDF.Framework.Logging;

using Microsoft.Extensions.Logging;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.EventHandlers;
public class PostCreatedEventHandler : IDomainEventHandler<PostCreatedEvent>
{
	private readonly ILogger _logger;

	public PostCreatedEventHandler(ILoggerFactory logger)
	{
		_logger = logger.CreateLogger(LogEventCategory.DomainEventHandler.ToString());//از ToString استفاده شده تا نام عضو را برگرداند
	}

	public Task Consume(ConsumeContext<PostCreatedEvent> context)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8; // Set the console output encoding to UTF-8
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(context.Message);
		Console.ResetColor();

		return Task.CompletedTask;
	}
}
