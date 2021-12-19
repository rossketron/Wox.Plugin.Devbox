using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using Wox.Plugin.Devbox.Helpers;

namespace Wox.Plugin.Devbox.Plugins
{
  class Ember
  {
    private static readonly string ico = "Prompt.png";

    private static List<EmberImportObject> _imports = null;

    private static List<EmberImportObject> LoadImports(PluginInitContext context)
    {
      if (_imports == null)
      {
        using(StreamReader stream = new StreamReader(context.CurrentPluginMetadata.PluginDirectory + "\\mappings.json"))
        {
          var json = stream.ReadToEnd();
          _imports = JsonConvert.DeserializeObject<List<EmberImportObject>>(json);
        }
      }

      return _imports;
    }

    public static List<Result> Query(Query query, SettingsModel settings, PluginInitContext context)
    {
      List<Result> list = new List<Result>();
      List<EmberImportObject> imports = LoadImports(context);

      if (query.Search.Length == 0)
      {
        list.Add(new Result
        {
          Title = "Lookup Ember Import",
            SubTitle = "Lookup Ember import strings and copy to clipboard",
            IcoPath = ico
        });
        return list;
      }
      List<EmberImportObject> results = imports.FindAll(result =>
      {
        bool found = true;
        String[] searchStrings = query.Search.Split(' ');
        foreach (String searchSegment in searchStrings)
        {
          found = found && result.global.ToLower().Contains(searchSegment.ToLower());
        }
        return found;
      });

      if (results.Count > 0)
      {
        foreach (EmberImportObject item in results)
        {
          String module = item.module;
          String export = item.export;
          String localName = item.localName;
          if (item.deprecated && item.replacement != null)
          {
            module = item.replacement.module;
            export = item.replacement.export;
            if (export.Equals("default"))
            {
              EmberImportObject nonDeprecatedImport = imports.Find(import =>
              {
                return import.module.Equals(module) && import.export.Equals(export);
              });
              localName = nonDeprecatedImport.localName;
            }
          }

          String importText = export.Equals("default") ? localName : "{ " + export + " }";
          String clipboardText = "import " + importText + " from '" + module + "';";
          list.Add(new Result
          {
            Title = item.global,
              SubTitle = clipboardText,
              IcoPath = ico,
              Action = (e) =>
              {
                Clipboard.SetText(clipboardText);
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
