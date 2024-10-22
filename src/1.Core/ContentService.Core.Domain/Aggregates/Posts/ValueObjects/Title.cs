using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MDF.Framework.Extensions.Results;
using MDF.Framework.SeedWork;
using MDF.Resources.Common;

namespace ContentService.Core.Domain.Aggregates.Posts.ValueObjects;
public class Title : BaseValueObject<string?>
{
	public const byte Minimum = 50;
	public const byte Maximum = 60;

	private Title()
	{

	}
	private Title(string? value) : base(value)
	{
	}

	public static Result<Title?> Create(Result<object?> guardResult)
	{

		var result = Create(guardResult.Value.ToString());
		return result;

	}

	public static Result<Title?> Create(string? value)
	{
		//استفاده از گارد
		//var guardResult = Guard.CheckIf(value,DataDictionary.Title)
		//			.NotEmpty<string>()
		//			.MinimumLength(Minimum) 
		//			.MaximumLength(Maximum)
		//			.AsResult();

		//استفاده از Validation Result
		Result<Title?> result = new(); // ایجاد یک نمونه از کلاس Result با نوع داده Title

		TitleValidator validator = new TitleValidator(); // ایجاد یک نمونه از کلاس TitleValidator
		ValidationResult validationResult = validator.Validate(new Title(value)); // اعتبارسنجی عنوان با استفاده از ولیدیتور

		result = result.AddValidationErrors(validationResult); // افزودن خطاهای اعتبارسنجی به نتیجه

		if (result.IsFailed) // اگر عملیات ناموفق بود
		{
			return result; // بازگشت نتیجه
		}

		var returnValue = new Title(value); // ایجاد یک نمونه جدید از عنوان
		result.WithValue(returnValue); // تنظیم مقدار نتیجه به عنوان ایجاد شده
		return result; // بازگشت نتیجه
	}


	public class TitleValidator : AbstractValidator<Title>
	{
		public TitleValidator()
		{
			RuleFor(t => t.Value)
				.NotEmpty()
				.Length(Title.Minimum, Title.Maximum)
				.WithName(DataDictionary.Title);
		}
	}
}