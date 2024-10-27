using ContentService.Resources.Messages;

namespace ContentService.Resources.FormattedMessages;
public static class ContentServiceValidationMessages
{
	public static string ValidationSample() => string.Format(ContentServiceValidations.ValidationSample);
	//	public static string SuccessCreate(string input) => string.Format(Successes.SuccessCreate, input);
}
