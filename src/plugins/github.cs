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

    public static void CloneGithubRepo(string clone_url, Boolean useWsl, string name, Settings settings)
    {
      var ssh_url = $"git@github.com:{clone_url.Remove(0, 19)}";
      var command = useWsl
        ? $"wsl --cd {settings.wslGitFolder} git clone {ssh_url}"
        : $"git clone {ssh_url} {settings.gitFolder}\\{name}";

      Process.Start(new ProcessStartInfo
      {
        FileName = "cmd.exe",
        Arguments = $"/c \"{command}\"",
        UseShellExecute = true,
        WindowStyle = ProcessWindowStyle.Hidden
      });
    }

    public static List<Result> Query(Query query, Settings settings, PluginInitContext context)
    {
      var list = new List<Result>();

      if (query.Search.Length == 0)
      {
        return new List<Result> {
          new() {
            Title = "Open Github",
            SubTitle = "...or keep typing to search for repositories",
            Action = (e) =>
            {
              OpenUrl("https://github.com/");
              return true;
            },
            IcoPath = ico
          }
        };
      }

      var results = GithubApi.QueryGithub(query.Search, settings);
      if (results.Count == 0)
      {
        return new List<Result> {
          new() {
            Title = "No Results Found",
            IcoPath = ico
          }
        };
      }

      return results.ConvertAll(result => new Result
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

    public static List<Result> Clone(Query query, Settings settings, PluginInitContext context)
    {
      if (query.Search.Length == 0)
      {
        return new List<Result>()
        {
          new() {
            Title = "Clone Repo",
            SubTitle = "...keep typing to search for repository",
            IcoPath = ico
          }
        };
      }

      var useWsl = true;
      var search = query.Search;
      if (query.FirstSearch.Equals("win"))
      {
        useWsl = false;
        search = query.SecondToEndSearch;
      }

      var repos = GithubApi.QueryGithub(search, settings);
      if (repos.Count == 0)
      {
        return new List<Result>()
        {
          new() {
            Title = "No Results Found",
            IcoPath = ico
          }
        };
      }

      return repos.ConvertAll(repo => new Result
      {
        Title = repo.full_name,
        SubTitle = repo.description,
        IcoPath = ico,
        Action = (e) =>
        {
          CloneGithubRepo(repo.clone_url, useWsl, repo.name, settings);
          return true;
        }
      });
    }
  }
}
