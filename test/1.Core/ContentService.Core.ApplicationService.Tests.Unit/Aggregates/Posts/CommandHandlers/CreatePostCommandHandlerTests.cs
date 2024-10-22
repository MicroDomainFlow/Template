using ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.Commands;
using ContentService.Core.Domain.Aggregates.Posts;

using MDF.Resources.Common.Messages;

using Moq;

namespace ContentService.Core.ApplicationService.Tests.Unit.Aggregates.Posts.CommandHandlers;
public class CreatePostCommandHandlerTests
{
	[Fact]
	public async Task ShouldBe_ReturnsPostId_When_ValidCommand()
	{
		// Arrange
		var postRepositoryMock = new Mock<IPostCommandRepository>();
		var cancellationToken = new CancellationToken();
		var command = new CreatePostCommand
		{
			Title = "Test Title".PadLeft(50, '-'),
			Description = "Test Description".PadLeft(150, '-'),
			Text = "Test Text".PadLeft(255, '-')
		};
		var post = new Post().Create(command.Title, command.Description, command.Text);
		postRepositoryMock.Setup(x => x.InsertByAsync(It.IsAny<Post>(), cancellationToken)).Returns(Task.CompletedTask);
		postRepositoryMock.Setup(x => x.CommitAsync(cancellationToken));
		var commandHandler = new CreatePostCommandHandler(postRepositoryMock.Object);

		// Act
		var result = await commandHandler.Handle(command, cancellationToken);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotEqual(Guid.Empty, result.Value);
	}

	[Fact]
	public async Task ShouldBe_ReturnsError_When_InvalidCommand()
	{
		// Arrange
		var postRepositoryMock = new Mock<IPostCommandRepository>();
		var cancellationToken = new CancellationToken();
		var command = new CreatePostCommand
		{
			Title = null, // Invalid command with null title
			Description = "Test Description",
			Text = "Test Text"
		};
		var post = new Post().Create(command.Title, command.Description, command.Text);
		var error = Errors.UnexpectedError;
		post.Result.WithError(error);
		postRepositoryMock.Setup(x => x.InsertByAsync(It.IsAny<Post>(), cancellationToken)).Returns(Task.CompletedTask);
		postRepositoryMock.Setup(x => x.CommitAsync(cancellationToken));
		var commandHandler = new CreatePostCommandHandler(postRepositoryMock.Object);

		// Act
		var result = await commandHandler.Handle(command, cancellationToken);

		// Assert
		Assert.True(result.IsFailed);
		Assert.Equal(4, result.Errors.Count);
		Assert.Contains(error, result.Errors[3].Message);
	}
}
