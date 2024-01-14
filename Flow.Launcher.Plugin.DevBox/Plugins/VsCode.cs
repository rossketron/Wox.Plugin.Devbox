using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Flow.Launcher.Plugin.DevBox.Helpers;

namespace Flow.Launcher.Plugin.DevBox.Plugins
{
  static class VSCode
  {
    private static readonly string ico = "Prompt.png";

    public static void OpenVSCode(string folder, Boolean useWsl, SettingsModel settings)
    {

      string command = $"code {folder}";
      if (useWsl)
      {
        string wslFolder = folder.Replace($"\\\\wsl$\\{settings.WslDistroName}", "");
        wslFolder = wslFolder.Replace("\\", "/");
        command = $"code --folder-uri vscode-remote://wsl+{settings.WslDistroName}{wslFolder}";
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
          Title = "Open VSCode",
          SubTitle = "...or keep typing to search for repositories",
          Action = (e) =>
          {
            OpenVSCode("", false, settings);
            return true;
          },
          IcoPath = ico
        });
        return list;
      }
      var searchString = query.Search;
      string[] splitQuery = searchString.Split(' ');
      if (splitQuery.Length > 1)
      {
        searchString = string.Join("*", splitQuery);
      }
      string[] wslResults = new string[0];
      if (!string.IsNullOrEmpty(settings.WslDistroName))
      {
        wslResults = Directory.GetDirectories($"\\\\wsl$\\{settings.WslDistroName}{settings.WslGitFolder}", $"*{searchString}*", SearchOption.TopDirectoryOnly);
      }
      string[] localResults = Directory.GetDirectories(settings.GitFolder, $"*{searchString}*", SearchOption.TopDirectoryOnly);

      if (wslResults.Length > 0 || localResults.Length > 0)
      {
        foreach (string result in wslResults)
        {
          list.Add(new Result
          {
            Title = Path.GetFileName(result),
            SubTitle = "WSL",
            IcoPath = ico,
            Action = (e) =>
            {
              OpenVSCode(result, true, settings);
              return true;
            }
          });
        }
        foreach (string result in localResults)
        {
          list.Add(new Result
          {
            Title = Path.GetFileName(result),
            SubTitle = "Windows",
            IcoPath = ico,
            Action = (e) =>
            {
              OpenVSCode(result, false, settings);
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
