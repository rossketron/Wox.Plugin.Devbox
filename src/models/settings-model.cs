namespace Flow.Launcher.Plugin.DevBox.PluginCore
{
  class SettingsModel
  {
    public string apiToken { get; set; }
    public string gitFolder { get; set; }
    public string wslGitFolder { get; set; }
    public string wslDistroName { get; set; }
  }
}
