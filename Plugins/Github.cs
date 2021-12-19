using System;
using System.Collections.Generic;
using System.Diagnostics;
using Wox.Plugin.Devbox.Helpers;

namespace Wox.Plugin.Devbox.Plugins
{
  static class Github
  {
    private static readonly string ico = "Prompt.png";

    private static void openUrl(String url)
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
              openUrl("http://github.com/");
              return true;
            },
            IcoPath = ico
        });
        return list;
      }

      ApiResult results = GithubApi.QueryGithub(query, settings);

      if (results.total_count > 0)
      {
        foreach (ApiResultRepo result in results.items)
        {
          list.Add(new Result
          {
            Title = result.full_name,
              SubTitle = result.description,
              IcoPath = ico,
              Action = (e) =>
              {
                openUrl(result.html_url);
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
