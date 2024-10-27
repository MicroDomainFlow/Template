using ContentService.Core.Domain.Aggregates.Posts.ValueObjects;

using FluentResults;

using MDF.Framework.SeedWork;
using MDF.Resources.Common;
using MDF.Resources.Common.FormattedMessages;

namespace ContentService.Core.Domain.Aggregates.Posts.Entities;
public class Comment : Entity
{
	public DisplayName Name { get; private set; }
	public Email Email { get; private set; }
	public CommentText CommentText { get; private set; }
	public Post Post { get; private set; }
	private Comment()
	{

	}
	private Comment(Post post, DisplayName name, Email email, CommentText text) : this()
	{
		Post = post;
		Name = name;
		Email = email;
		CommentText = text;
	}

	public static Result<Comment> Create(Post? post, string? name, string? email, string? text)
	{
		Result<Comment> result = new();

		if (post is null)
		{
			var errorMessage = ValidationMessages.Required(DataDictionary.Post);
			result.WithError(errorMessage);
		}

		var displayNameResult = DisplayName.Create(name);
		result.WithErrors(displayNameResult.Errors);

		var emailResult = Email.Create(email);
		result.WithErrors(emailResult.Errors);

		var textResult = CommentText.Create(text);
		result.WithErrors(textResult.Errors);

		if (result.IsFailed)
		{
			return result;
		}

		var returnValue = new Comment(post!, displayNameResult.Value, emailResult.Value, textResult.Value);
		result.WithValue(returnValue);
		return result;
	}

}
