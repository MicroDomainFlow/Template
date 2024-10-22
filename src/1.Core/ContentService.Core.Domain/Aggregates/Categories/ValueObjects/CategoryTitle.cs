using FluentResults;

using MDF.Framework.Extensions.Guards;
using MDF.Framework.Extensions.Guards.GuardClauses;
using MDF.Framework.SeedWork;
using MDF.Resources.Common;

namespace ContentService.Core.Domain.Aggregates.Categories.ValueObjects;
public class CategoryTitle : BaseValueObject<string>
{
	public const byte Minimum = 5;
	public const byte Maximum = 20;

	private CategoryTitle()
	{

	}
	private CategoryTitle(string? value) : base(value)
	{
	}
	public static Result<CategoryTitle?> Create(string? value)
	{
		//استفاده از گارد
		var guardResult = Guard.CheckIf(value, DataDictionary.Title)
					.NotEmpty()
					.MinimumLength(Minimum)
					.MaximumLength(Maximum);


		//استفاده از Validation Result
		Result<CategoryTitle?> result = new(); // ایجاد یک نمونه از کلاس Result با نوع داده Title

		result.WithErrors(guardResult._result.Errors);

		if (result.IsFailed) // اگر عملیات ناموفق بود
		{
			return result; // بازگشت نتیجه
		}

		var returnValue = new CategoryTitle(guardResult._result.Value); // ایجاد یک نمونه جدید از عنوان
		result.WithValue(returnValue); // تنظیم مقدار نتیجه به عنوان ایجاد شده
		return result; // بازگشت نتیجه
	}
}