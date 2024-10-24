﻿using EventBus.Messages.Aggregates.Posts.Events;

using MassTransit;

using MDF.Framework.LayersContracts.ApplicationServices;

namespace ContentService.Core.ApplicationService.Aggregates.Posts.EventHandlers;
public class CommentEditedEventHandler : IDomainEventHandler<CommentEditedEvent>
{
	public Task Consume(ConsumeContext<CommentEditedEvent> context)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8; // Set the console output encoding to UTF-8
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(context.Message);
		Console.ResetColor();

		return Task.CompletedTask;
	}
}
