using EventBus.Messages.Aggregates.Posts.Events;

using MassTransit;

using MDF.Framework.LayersContracts.ApplicationServices;

using Microsoft.Extensions.Logging;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.EventHandlers;
public class CommentRemovedEventHandler : IDomainEventHandler<CommentRemovedEvent>
{
	private ILogger<CommentRemovedEventHandler> _logger;

	public CommentRemovedEventHandler(ILogger<CommentRemovedEventHandler> logger)
	{
		_logger = logger;
	}

	public Task Consume(ConsumeContext<CommentRemovedEvent> context)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8; // Set the console output encoding to UTF-8
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(context.Message);
		Console.ResetColor();

		return Task.CompletedTask;
	}
}
