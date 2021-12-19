using System;
using System.Collections.Generic;
using Wox.Infrastructure.Storage;
using Wox.Plugin.Devbox.Helpers;

namespace Wox.Plugin.Devbox.Plugins
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
          Title = "Set WSL Distro Name",
            SubTitle = $"Causes vscode shortcut to use WSL - Currently: \"{settings.wslName}\"",
            Action = (e) =>
            {
              context.API.ChangeQuery("db wslName ");
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
        return list;
      }

      String[] searchStrings = query.Search.Split(' ');

      if ("apiToken".Equals(searchStrings[0]))
      {
        String apiToken = "";
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
        String gitFolder = "";
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

      if ("wslName".Equals(searchStrings[0]))
      {
        String wslName = "";
        if (searchStrings.Length > 1)
        {
          wslName = searchStrings[1];
        }
        list.Add(new Result
        {
          Title = $"Set WSL2 distro to \"{wslName}\"",
            SubTitle = $"Currently: \"{settings.wslName}\"",
            Action = (e) =>
            {
              settings.wslName = wslName;
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
