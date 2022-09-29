using System.Text.RegularExpressions;

namespace ValerioProdigit.Api.Validators.Classroom;

public class ClassroomValidatorSettings
{
	public readonly Regex CodeRegex = new(@"^[a-z0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
	
	public int MaxCodeLength { get; set; }
}