using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Wox.Plugin.Devbox.Helpers
{

  static class GithubApi
  {
    public static ApiResult QueryGithub(Query query, SettingsModel settings)
    {
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls12
        | SecurityProtocolType.Ssl3;

      String url = $"https://api.github.com/search/repositories?access_token={settings.apiToken}&q={query.Search}";
      HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
      request.ProtocolVersion = HttpVersion.Version10;
      request.Method = "GET";
      request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
      WebResponse response = request.GetResponse();
      using(Stream stream = response.GetResponseStream())
      {
        StreamReader objReader = new StreamReader(stream);
        var json = objReader.ReadToEnd();
        ApiResult result = JsonConvert.DeserializeObject<ApiResult>(json);
        return result;
      }
    }
  }
}
