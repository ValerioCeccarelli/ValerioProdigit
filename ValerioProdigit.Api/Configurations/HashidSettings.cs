namespace ValerioProdigit.Api.Configurations;

public sealed class HashidSettings
{
	public string Salt { get; set; } = default!;
	public int MinHashLength { get; set; }
}