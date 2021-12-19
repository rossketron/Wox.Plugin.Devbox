namespace Wox.Plugin.Devbox.Helpers
{
  class ApiResultRepo
  {
    public int id { get; set; }
    public string name { get; set; }
    public string full_name { get; set; }
    public string description { get; set; }
    public bool @private { get; set; }
    public string html_url { get; set; }
    public int stargazers_count { get; set; }
    public ApiResultOwner owner { get; set; }
  }
}
