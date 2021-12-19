using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Wox.Plugin.Devbox.Helpers;

namespace Wox.Plugin.Devbox.Plugins
{
  static class VSCode
  {
    private static readonly string ico = "Prompt.png";

    public static void openResultInVSCode(string result, SettingsModel settings)
    {
      openVSCode(result, settings);
    }

    public static void openVSCode(String folder, SettingsModel settings)
    {
      String command = $"code {folder}";
      if (!string.IsNullOrEmpty(settings.wslName) && !string.IsNullOrEmpty(folder))
      {
        command = $"code --folder-uri vscode-remote://wsl+{settings.wslName}{folder}";
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
              openVSCode("", settings);
              return true;
            },
            IcoPath = ico
        });
        return list;
      }

      string searchFolder = settings.gitFolder;
      if (!string.IsNullOrEmpty(settings.wslName))
      {
        searchFolder = $"\\\\wsl$\\{settings.wslName}{settings.gitFolder}";
      }
      string[] results = Directory.GetDirectories(searchFolder, $"*{query.Search}*", SearchOption.TopDirectoryOnly);

      if (results.Length > 0)
      {
        foreach (string result in results)
        {
          list.Add(new Result
          {
            Title = Path.GetFileName(result),
              IcoPath = ico,
              Action = (e) =>
              {
                string folderName = result;
                if (!string.IsNullOrEmpty(settings.wslName))
                {
                  folderName = folderName.Replace($"\\\\wsl$\\{settings.wslName}", "");
                  folderName = folderName.Replace("\\", "/");
                }
                openResultInVSCode(folderName, settings);
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
