using System.Text.RegularExpressions;

namespace ValerioProdigit.Api.Validators.Lesson;

public class LessonValidatorSettings
{
	public readonly Regex DateRegex = new(@"^\d\d\d\d-\d\d-\d\d", RegexOptions.Compiled);
	
	public int MaxNameLength { get; set; }
	public int MaxDescriptionLength { get; set; }
	public int MinStartHourLength { get; set; }
	public int MaxStartHourLength { get; set; }
	public int MinFinishHourLength { get; set; }
	public int MaxFinishHourLength { get; set; }
}