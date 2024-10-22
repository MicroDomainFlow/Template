using EventBus.Messages.Aggregates.Categories.Events;

using MassTransit;

using MDF.Framework.LayersContracts.ApplicationServices;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.EventHandlers;
public sealed class CategoryParentAddedEventHandler : IDomainEventHandler<CategoryParentAddedEvent>
{
	public Task Consume(ConsumeContext<CategoryParentAddedEvent> context)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8; // Set the console output encoding to UTF-8
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(context.Message);
		Console.ResetColor();

		return Task.CompletedTask;
	}
}
