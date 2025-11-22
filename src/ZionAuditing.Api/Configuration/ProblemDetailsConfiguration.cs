namespace ZionAuditing.Api.Configuration;

public sealed class ProblemDetailsConfiguration
{
    public int Status { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
}
