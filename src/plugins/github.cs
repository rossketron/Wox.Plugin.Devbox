using System;
using System.Collections.Generic;
using System.Diagnostics;
using Flow.Launcher.Plugin.DevBox.PluginCore;

namespace Flow.Launcher.Plugin.DevBox.Plugins
{
  static class Github
  {
    private static readonly string ico = "Prompt.png";

    private static void OpenUrl(string url)
    {
      Process.Start(url);
    }

    public static void CloneGithubRepo(string clone_url, Boolean useWsl, string name, SettingsModel settings)
    {
      string command = "";
      String ssh_url = $"git@github.com:{clone_url.Remove(0, 19)}";
      if (useWsl)
      {
        command = $"wsl --cd {settings.wslGitFolder} git clone {ssh_url}";
      }
      else
      {
        command = $"git clone {ssh_url} {settings.gitFolder}\\{name}";
      }

      ProcessStartInfo info;
      var arguments = $"/c \"{command}\"";
      info = new ProcessStartInfo
      {
        FileName = "cmd.exe",
        Arguments = arguments,
        UseShellExecute = true,
        WindowStyle = ProcessWindowStyle.Hidden
      };

      Process.Start(info);
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

    public static List<Result> Clone(Query query, Boolean useWsl, SettingsModel settings, PluginInitContext context)
    {
      List<Result> list = new List<Result>();

      if (query.Search.Length == 0)
      {
        list.Add(new Result
        {
          Title = "Clone Repo",
          SubTitle = "...keep typing to search for repository",
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
              CloneGithubRepo(result.clone_url, useWsl, result.name, settings);
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
