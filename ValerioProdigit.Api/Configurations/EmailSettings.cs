using System.Text.RegularExpressions;

namespace ValerioProdigit.Api.Configurations;

public sealed class EmailSettings
{
    public readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    public HashSet<string> AllowedStudentsDomains { get; set; } = default!;
    public HashSet<string> AllowedTeacherDomains { get; set; } = default!;
    public HashSet<string> AllowedAdminDomains { get; set; } = default!;

    public HashSet<string> AllowedDomains { get; set; } = default!;
    public HashSet<string> AllowedNonStudentDomains { get; set; } = default!;
}