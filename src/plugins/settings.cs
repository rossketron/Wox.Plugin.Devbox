using System.Collections.Generic;
using Flow.Launcher.Infrastructure.Storage;
using Flow.Launcher.Plugin.DevBox.PluginCore;

namespace Flow.Launcher.Plugin.DevBox.Plugins
{
  static class Settings
  {
    private static readonly string ico = "Prompt.png";

    public static List<Result> Query(Query query, SettingsModel settings, PluginInitContext context, PluginJsonStorage<SettingsModel> storage)
    {

      List<Result> list = new List<Result>();
      if (query.Search.Length == 0)
      {
        list.Add(new Result
        {
          Title = "Set Github API Token",
          SubTitle = $"Currently: \"{settings.apiToken}\"",
          Action = (e) =>
          {
            context.API.ChangeQuery("db apiToken ");
            return false;
          },
          IcoPath = ico
        });
        list.Add(new Result
        {
          Title = "Set Github API Token",
          SubTitle = $"Currently: \"{settings.apiToken}\"",
          Action = (e) =>
          {
            context.API.ChangeQuery("db apiToken ");
            return false;
          },
          IcoPath = ico
        });
        list.Add(new Result
        {
          Title = "Set Git Folder",
          SubTitle = $"Currently: \"{settings.gitFolder}\"",
          Action = (e) =>
          {
            context.API.ChangeQuery("db gitFolder ");
            return false;
          },
          IcoPath = ico
        });
        list.Add(new Result
        {
          Title = "Set WSL Git Folder",
          SubTitle = $"Currently: \"{settings.wslGitFolder}\"",
          Action = (e) =>
          {
            context.API.ChangeQuery("db wslgitFolder ");
            return false;
          },
          IcoPath = ico
        });
        list.Add(new Result
        {
          Title = "Set WSL Distro Name",
          SubTitle = $"Currently: \"{settings.wslDistroName}\"",
          Action = (e) =>
          {
            context.API.ChangeQuery("db wslDistroName ");
            return false;
          },
          IcoPath = ico
        });

        // Other sub-plugins

        list.Add(new Result
        {
          Title = "Open a Repo in Gitub",
          SubTitle = "gh",
          Action = (e) =>
          {
            context.API.ChangeQuery("gh ");
            return false;
          },
          IcoPath = "github.png"
        });
        list.Add(new Result
        {
          Title = "Open a Repo in VSCode",
          SubTitle = "c",
          Action = (e) =>
          {
            context.API.ChangeQuery("c ");
            return false;
          },
          IcoPath = "vscode.png"
        });
        list.Add(new Result
        {
          Title = "Search Ember imports and copy to clipboard",
          SubTitle = "ember",
          Action = (e) =>
          {
            context.API.ChangeQuery("ember ");
            return false;
          },
          IcoPath = "ember.png"
        });
        list.Add(new Result
        {
          Title = "Clone a Repo to Windows",
          SubTitle = "wincl",
          Action = (e) =>
          {
            context.API.ChangeQuery("wincl ");
            return false;
          },
          IcoPath = "github.png"
        });
        list.Add(new Result
        {
          Title = "Clone a Repo to WSL",
          SubTitle = "cl",
          Action = (e) =>
          {
            context.API.ChangeQuery("cl ");
            return false;
          },
          IcoPath = "github.png"
        });
        return list;
      }

      string[] searchStrings = query.Search.Split(' ');

      if ("apiToken".Equals(searchStrings[0]))
      {
        string apiToken = "";
        if (searchStrings.Length > 1)
        {
          apiToken = searchStrings[1];
        }
        list.Add(new Result
        {
          Title = $"Set Github API Token to \"{apiToken}\"",
          SubTitle = $"Currently: \"{settings.apiToken}\"",
          Action = (e) =>
          {
            settings.apiToken = apiToken;
            storage.Save();
            return true;
          },
          IcoPath = ico
        });
        return list;
      }

      if ("gitFolder".Equals(searchStrings[0]))
      {
        string gitFolder = "";
        if (searchStrings.Length > 1)
        {
          gitFolder = searchStrings[1];
        }
        list.Add(new Result
        {
          Title = $"Set git folder to \"{gitFolder}\"",
          SubTitle = $"Currently: \"{settings.gitFolder}\"",
          Action = (e) =>
          {
            settings.gitFolder = gitFolder;
            storage.Save();
            return true;
          },
          IcoPath = ico
        });
        return list;
      }

      if ("wslgitFolder".Equals(searchStrings[0]))
      {
        string gitFolder = "";
        if (searchStrings.Length > 1)
        {
          gitFolder = searchStrings[1];
        }
        list.Add(new Result
        {
          Title = $"Set WSL git folder to \"{gitFolder}\"",
          SubTitle = $"Currently: \"{settings.wslGitFolder}\"",
          Action = (e) =>
          {
            settings.wslGitFolder = gitFolder;
            storage.Save();
            return true;
          },
          IcoPath = ico
        });
        return list;
      }

      if ("wslDistroName".Equals(searchStrings[0]))
      {
        string distroName = "";
        if (searchStrings.Length > 1)
        {
          distroName = searchStrings[1];
        }
        list.Add(new Result
        {
          Title = $"Set WSL Distro Name to \"{distroName}\"",
          SubTitle = $"Currently: \"{settings.wslDistroName}\"",
          Action = (e) =>
          {
            settings.wslDistroName = distroName;
            storage.Save();
            return true;
          },
          IcoPath = ico
        });
        return list;
      }

      return list;
    }
  }
}
