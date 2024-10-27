using ContentService.Resources;
using ContentService.Resources.FormattedMessages;

using EventBus.Messages.Aggregates.Categories.Events;

using MassTransit;

using MDF.Framework.LayersContracts.ApplicationServices;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.ApplicationService.Aggregates.Categories.EventHandlers;
public sealed class CategoryCreatedEventHandler : IDomainEventHandler<CategoryCreatedEvent>
{
	public Task Consume(ConsumeContext<CategoryCreatedEvent> context)
	{
		//todo pak konam in ha baraye test bug vs mibashad.
		Console.OutputEncoding = System.Text.Encoding.UTF8; // Set the console output encoding to UTF-8
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(context.Message);
		Console.WriteLine(ErrorMessages.CanNotDelete("test"));
		Console.WriteLine(ContentErrorMessages.ErrorSample);
		Console.WriteLine(DataDictionary.Name);
		Console.WriteLine(ContentServiceDataDictionary.Sample);
		Console.ResetColor();

		return Task.CompletedTask;
	}
}
