using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Flow.Launcher.Infrastructure.Storage;
using Flow.Launcher.Plugin.DevBox.PluginCore;
using Flow.Launcher.Plugin.DevBox.Plugins;

namespace Flow.Launcher.Plugin.DevBox
{
  public class Main : IPlugin, ISettingProvider
  {
    private PluginInitContext context;
    const string ico = "prompt.png";
    const string defaultgitFolder = "C:\\git";
    const string defaultwslGitFolder = "/git";
    private readonly Exception startupException = null;

    private readonly Settings settings;
    private readonly PluginJsonStorage<Settings> storage;

    public Control CreateSettingPanel()
    {
      throw new NotImplementedException();
    }

    public Main()
    {
      try
      {
        storage = new PluginJsonStorage<Settings>();
        settings = storage.Load();
        if (string.IsNullOrEmpty(settings.gitFolder))
        {
          settings.gitFolder = defaultgitFolder;
          storage.Save();
        }
        if (string.IsNullOrEmpty(settings.wslGitFolder))
        {
          settings.wslGitFolder = defaultwslGitFolder;
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
        else if (query.ActionKeyword.Equals("ember"))
        {
          return Ember.Query(query, settings, context);
        }
        else if (query.ActionKeyword.Equals("c"))
        {
          return VSCode.Query(query, settings, context);
        }
        else if (query.ActionKeyword.Equals("gh"))
        {
          return Github.Query(query, settings, context);
        }
        else if (query.ActionKeyword.Equals("cl"))
        {
          return Github.Clone(query, settings, context);
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
