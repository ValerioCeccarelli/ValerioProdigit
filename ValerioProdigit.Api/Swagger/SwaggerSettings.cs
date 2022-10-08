namespace ValerioProdigit.Api.Swagger;

public class SwaggerSettings
{
	public SwaggerContactSettings Contact { get; set; } = default!;
	public SwaggerLicenseSettings License { get; set; } = default!;
	public SwaggerInfoSettings Info { get; set; } = default!;

	//Todo: add more checks
	public bool IsValid(out string error)
	{
		error = "";
		if (Contact is null)
		{
			error = "Contact is null";
			return false;
		}
		if (License is null)
		{
			error = "License is null";
			return false;
		}
		if (Info is null)
		{
			error = "Info is null";
			return false;
		}
		return true;
	}
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