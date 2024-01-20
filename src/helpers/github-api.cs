using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http;

namespace Flow.Launcher.Plugin.DevBox.PluginCore
{
  static class GithubApi
  {
    private static HttpClient _httpClient = new();
    public static List<ApiResultRepo> QueryGithub(string searchTerm, Settings settings)
    {
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls12;

      string url = $"https://api.github.com/user/repos?sort=updated";
      HttpWebRequest request = _httpClient.Requ
      request.Headers["Authorization"] = $"token {settings.apiToken}";
      request.ProtocolVersion = HttpVersion.Version10;
      request.Method = "GET";
      request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
      WebResponse response = request.GetResponse();
      using (Stream stream = response.GetResponseStream())
      {
        StreamReader objReader = new StreamReader(stream);
        var json = objReader.ReadToEnd();
        List<ApiResultRepo> results = JsonSerializer.Deserialize<List<ApiResultRepo>>(json);
        List<ApiResultRepo> filteredResultsList = new List<ApiResultRepo>();
        foreach (ApiResultRepo result in results)
        {
          if (result.name.ToLower().Contains(searchTerm.ToLower()))
          {
            filteredResultsList.Add(result);
          }
        }
        return filteredResultsList;
      }
    }
  }
}
