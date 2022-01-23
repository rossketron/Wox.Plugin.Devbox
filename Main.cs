using System;
using System.Collections.Generic;
using Wox.Infrastructure.Storage;
using Wox.Plugin.Devbox.Helpers;
using Wox.Plugin.Devbox.Plugins;

namespace Wox.Plugin.Devbox
{
  public class Main : IPlugin
  {
    private PluginInitContext context;
    const string ico = "Prompt.png";
    const string defaultGitFolder = "C:\\git";
    const string defaultWslGitFolder = "/git";
    private readonly Exception startupException = null;

    private readonly SettingsModel settings;
    private readonly PluginJsonStorage<SettingsModel> storage;

    public Main()
    {
      try
      {
        storage = new PluginJsonStorage<SettingsModel>();
        settings = storage.Load();
        if (string.IsNullOrEmpty(settings.GitFolder))
        {
          settings.GitFolder = defaultGitFolder;
          storage.Save();
        }
        if (string.IsNullOrEmpty(settings.WslGitFolder))
        {
          settings.WslGitFolder = defaultWslGitFolder;
          storage.Save();
        }
      }
      catch (Exception e)
      {
        startupException = e;
      }
    }

    public void Init(PluginInitContext pluginContext)
    {
      context = pluginContext;
    }

    public List<Result> Query(Query query)
    {
      List<Result> list = new List<Result>();
      try
      {
        if (startupException != null)
        {
          list.Add(new Result
          {
            Title = "Devbox Plugin",
            SubTitle = "Error during initialization: " + startupException.Message,
            IcoPath = ico
          });
        }
        else if (query.ActionKeyword.Equals("db"))
        {
          return Settings.Query(query, settings, context, storage);
        }
        else if (query.ActionKeyword.Equals("ember"))
        {
          return Ember.Query(query, settings, context);
        }
        else if (query.ActionKeyword.Equals("c"))
        {
          return VSCode.Query(query, settings, context);
        }
        else if (string.IsNullOrEmpty(settings.ApiToken))
        {
          list.Add(new Result
          {
            Title = "Set Github API Token",
            SubTitle = "Set this before using this plugin",
            Action = (e) =>
            {
              context.API.ChangeQuery("db apiToken ");
              return false;
            },
            IcoPath = ico
          });
        }
        else if (query.ActionKeyword.Equals("gh"))
        {
          return Github.Query(query, settings, context);
        }
        else if (query.ActionKeyword.Equals("wincl"))
        {
          Boolean useWsl = false;
          return Github.Clone(query, useWsl, settings, context);
        }
        else if (query.ActionKeyword.Equals("cl"))
        {
          Boolean useWsl = true;
          return Github.Clone(query, useWsl, settings, context);
        }
        else
        {
          return Settings.Query(query, settings, context, storage);
        }
      }
      catch (Exception e)
      {
        list.Add(new Result
        {
          Title = "Error: " + e.Message,
          IcoPath = ico
        });
      }
      return list;
    }
  }
}
