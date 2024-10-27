using ContentService.Core.ApplicationService.Aggregates.Posts.CommandHandlers;
using ContentService.Core.Contracts.Aggregates.Posts.CommandRepositories;
using ContentService.Core.Contracts.Aggregates.Posts.Commands;
using ContentService.Core.Domain.Aggregates.Posts;

using MDF.Resources.Common.FormattedMessages;

using Moq;

namespace ContentService.Core.ApplicationService.Tests.Unit.Aggregates.Posts.CommandHandlers;
public class UpdatePostCommandHandlerTests
{
	[Fact]
	public async Task Should_UpdatePostAndReturnPostId_When_PostFound()
	{
		// Arrange
		var post = new Post()
			.Create("-".PadLeft(60), "-".PadLeft(160), "-".PadLeft(255));

		var request = new UpdatePostCommand { PostId = post.Id, Title = "a".PadLeft(60), Description = "a".PadLeft(160), Text = "a".PadLeft(255) };
		var cancellationToken = CancellationToken.None;

		var postCommandRepositoryMock = new Mock<IPostCommandRepository>();
		postCommandRepositoryMock.Setup(repo => repo.GetByAsync(post.Id, cancellationToken))
			.ReturnsAsync(post);

		var handler = new UpdatePostCommandHandler(postCommandRepositoryMock.Object);

		// Act
		var result = await handler.Handle(request, cancellationToken);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess);
		Assert.Equal(post.Id, result.Value);
		//postCommandRepositoryMock.Verify(repo => repo.UpdateBy(post.ResultObject.Value), Times.Once);
		postCommandRepositoryMock.Verify(repo => repo.CommitAsync(cancellationToken), Times.Once);
	}

	[Fact]
	public async Task Should_ReturnError_When_PostNotFound()
	{
		//copilot question
		// fix آیا این تست با توجه به  کلاس های #file:'Posts\Post.cs'  و #file:'UpdatePostCommandHandler.cs'  درست است؟
		// Arrange
		var post = new Post()
			.Create("-".PadLeft(60), "-".PadLeft(160), "-".PadLeft(255));

		var notExistPostId = Guid.NewGuid();
		var request = new UpdatePostCommand { PostId = notExistPostId, Title = "-".PadLeft(60), Description = "-".PadLeft(160), Text = "-".PadLeft(255) };

		var cancellationToken = CancellationToken.None;

		var postCommandRepositoryMock = new Mock<IPostCommandRepository>();
		postCommandRepositoryMock.Setup(repo => repo.GetByAsync(post.Id, cancellationToken))
						.ReturnsAsync(post);


		var handler = new UpdatePostCommandHandler(postCommandRepositoryMock.Object);

		// Act
		var result = await handler.Handle(request, cancellationToken);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsFailed);
		Assert.Contains(result.Errors, error => error.Message == ErrorMessages.NotFound(request.ToString()));
		postCommandRepositoryMock.Verify(repo => repo.UpdateBy(It.IsAny<Post>()), Times.Never);
		postCommandRepositoryMock.Verify(repo => repo.CommitAsync(cancellationToken), Times.Never);
	}
}
