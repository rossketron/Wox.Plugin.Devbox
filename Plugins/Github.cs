using System;
using System.Collections.Generic;
using System.Diagnostics;
using Wox.Plugin.Devbox.Helpers;

namespace Wox.Plugin.Devbox.Plugins
{
  static class Github
  {
    private static readonly string ico = "Prompt.png";

    private static void OpenUrl(string url)
    {
      Process.Start(url);
    }

    public static List<Result> Query(Query query, SettingsModel settings, PluginInitContext context)
    {
      List<Result> list = new List<Result>();

      if (query.Search.Length == 0)
      {
        list.Add(new Result
        {
          Title = "Open Github",
          SubTitle = "...or keep typing to search for repositories",
          Action = (e) =>
          {
            OpenUrl("http://github.com/");
            return true;
          },
          IcoPath = ico
        });
        return list;
      }

      List<ApiResultRepo> results = GithubApi.QueryGithub(query, settings);

      if (results.Count > 0)
      {
        foreach (ApiResultRepo result in results)
        {
          list.Add(new Result
          {
            Title = result.full_name,
            SubTitle = result.description,
            IcoPath = ico,
            Action = (e) =>
            {
              OpenUrl(result.html_url);
              return true;
            }
          });
        }
      }
      else
      {
        list.Add(new Result
        {
          Title = "No Results Found",
          IcoPath = ico
        });
      }

      return list;
    }
  }
}
