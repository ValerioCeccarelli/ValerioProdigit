namespace ValerioProdigit.Api.Configurations;

public class HashidSettings
{
	public string Salt { get; set; } = default!;
	public int MinLength { get; set; }
}