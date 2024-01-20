using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Automation.Peers;
using Flow.Launcher.Plugin.DevBox.PluginCore;

namespace Flow.Launcher.Plugin.DevBox.Plugins
{
  static class VSCode
  {
    private static readonly string ico = "prompt.png";

    private class SearchResult
    {
      public string path { get; set; }
      public string type { get; set; }

      public SearchResult(string path, string type)
      {
        this.path = path;
        this.type = type;
      }
    }

    private static void OpenVSCode(SearchResult result, Settings settings)
    {
      string command = $"code {result.path}";
      if (result.type.Equals("WSL"))
      {
        string wslFolder = result.path.Replace($"\\\\wsl$\\{settings.wslDistroName}", "");
        wslFolder = wslFolder.Replace("\\", "/");
        command = $"code --folder-uri vscode-remote://wsl+{settings.wslDistroName}{wslFolder}";
      }

      Process.Start(new ProcessStartInfo
      {
        FileName = "cmd.exe",
        Arguments = $"/c \"{command}\"",
        UseShellExecute = true,
        WindowStyle = ProcessWindowStyle.Hidden
      });
    }

    private static List<SearchResult> SearchGitDirectories(Query query, Settings settings)
    {
      var searchString = string.Join("*", query.SearchTerms);
      var results = string.IsNullOrEmpty(settings.wslDistroName)
        ? new List<SearchResult>()
        : Directory.GetDirectories($"\\\\wsl$\\{settings.wslDistroName}{settings.wslGitFolder}", $"*{searchString}*", SearchOption.TopDirectoryOnly).ToList().ConvertAll(item => new SearchResult(item, "WSL"));
      results.AddRange(Directory.GetDirectories(settings.gitFolder, $"*{searchString}*", SearchOption.TopDirectoryOnly).ToList().ConvertAll(item => new SearchResult(item, "Windows")));

      return results;
    }

    public static List<Result> Query(Query query, Settings settings, PluginInitContext context)
    {
      if (query.Search.Length == 0)
      {
        return new List<Result> {
          new() {
            Title = "Open VSCode",
            SubTitle = "...or keep typing to search for repositories",
            Action = (e) =>
            {
              OpenVSCode(new SearchResult("", "Windows"), settings);
              return true;
            },
            IcoPath = ico
          }
        };
      }

      var directories = SearchGitDirectories(query, settings);
      if (directories.Count == 0)
      {
        return new List<Result> {
          new() {
            Title = "No Results Found",
            IcoPath = ico
          }
        };
      }

      return directories.ConvertAll(directory => new Result
      {
        Title = Path.GetFileName(directory.path),
        SubTitle = directory.type,
        IcoPath = ico,
        Action = (e) =>
        {
          OpenVSCode(directory, settings);
          return true;
        }
      });
    }
  }
}
