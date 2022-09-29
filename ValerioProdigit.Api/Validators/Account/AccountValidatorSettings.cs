using System.Text.RegularExpressions;

namespace ValerioProdigit.Api.Validators.Account;

public class AccountValidatorSettings
{
	public readonly Regex NameRegex = new(@"^[a-z]+( [a-z])*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	public int MaxPasswordLength { get; set; }
	public int MaxEmailLength { get; set; }
	public int MaxNameLength { get; set; }
	public int MaxSurnameLength { get; set; }

}