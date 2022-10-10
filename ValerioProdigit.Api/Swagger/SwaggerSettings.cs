namespace ValerioProdigit.Api.Swagger;

public class SwaggerSettings
{
	public SwaggerContactSettings? Contact { get; set; } = default!;
	public SwaggerLicenseSettings? License { get; set; } = default!;
	public SwaggerInfoSettings? Info { get; set; } = default!;
	
}

public class SwaggerContactSettings
{
	public string Name { get; set; } = default!;
	public string Email { get; set; } = default!;
	public string Url { get; set; } = default!;
}

public class SwaggerLicenseSettings
{
	public string Name { get; set; } = default!;
	public string Url { get; set; } = default!;
}

public class SwaggerInfoSettings
{
	public string Version { get; set; } = default!;
	public string Title { get; set; } = default!;
	public string Description { get; set; } = default!;
}