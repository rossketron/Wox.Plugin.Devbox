namespace Wox.Plugin.Devbox.Helpers
{
  class SettingsModel
  {
    public string ApiToken { get; set; }
    public string GitFolder { get; set; }
    public string WslGitFolder { get; set; }
    public string WslDistroName { get; set; }

    public string GithubUserName { get; set; }
  }
}
