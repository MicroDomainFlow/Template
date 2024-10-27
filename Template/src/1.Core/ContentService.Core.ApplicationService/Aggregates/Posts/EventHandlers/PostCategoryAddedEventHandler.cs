using EventBus.Messages.Aggregates.Posts.Events;

using MassTransit;

using MDF.Framework.LayersContracts.ApplicationServices;

using Microsoft.Extensions.Logging;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.EventHandlers;
public class PostCategoryAddedEventHandler : IDomainEventHandler<PostCategoryAddedEvent>
{
	private readonly ILogger<PostCategoryAddedEventHandler> _logger;
	public Task Consume(ConsumeContext<PostCategoryAddedEvent> context)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8; // Set the console output encoding to UTF-8
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(context.Message);
		Console.ResetColor();

		return Task.CompletedTask;
	}
}
