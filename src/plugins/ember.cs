using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using Flow.Launcher.Plugin.DevBox.PluginCore;

namespace Flow.Launcher.Plugin.DevBox.Plugins
{
  class Ember
  {
    private static readonly string ico = "prompt.png";
    private static List<EmberImportObject> _imports = null;

    private static void LoadImports(PluginInitContext context)
    {
      using var stream = new StreamReader($"{context.CurrentPluginMetadata.PluginDirectory}\\mappings.json");
      _imports = JsonSerializer.Deserialize<List<EmberImportObject>>(stream.ReadToEnd());
    }

    private static List<EmberImportObject> SearchImports(Query query)
    {
      return _imports.FindAll(result =>
        {
          bool found = true;
          string[] searchStrings = query.Search.Split(' ');
          foreach (string searchSegment in searchStrings)
          {
            found = found && result.global.ToLower().Contains(searchSegment.ToLower());
          }
          return found;
        });
    }

    private static string GetClipboardText(EmberImportObject item)
    {
      string module = item.module;
      string export = item.export;
      string localName = item.localName;
      if (item.deprecated && item.replacement != null)
      {
        module = item.replacement.module;
        export = item.replacement.export;
        if (export.Equals("default"))
        {
          EmberImportObject nonDeprecatedImport = _imports.Find(import =>
          {
            return import.module.Equals(module) && import.export.Equals(export);
          });
          localName = nonDeprecatedImport.localName;
        }
      }

      string importText = export.Equals("default") ? localName : "{ " + export + " }";
      return "import " + importText + " from '" + module + "';";
    }

    public static List<Result> Query(Query query, Settings settings, PluginInitContext context)
    {
      if (_imports == null)
      {
        LoadImports(context);
      }

      if (query.Search.Length == 0)
      {
        return new List<Result>
        {
          new() {
            Title = "Lookup Ember Import",
            SubTitle = "Lookup Ember import strings and copy to clipboard",
            IcoPath = ico
          }
        };
      }

      var imports = SearchImports(query);
      if (imports.Count == 0)
      {
        return new List<Result>
        {
          new() {
            Title = "No Results Found",
            IcoPath = ico
          }
        };
      }

      return imports.ConvertAll(import =>
      {
        string clipboardText = GetClipboardText(import);
        return new Result
        {
          Title = import.global,
          SubTitle = clipboardText,
          IcoPath = ico,
          Action = (e) =>
          {
            Clipboard.SetText(clipboardText);
            return true;
          }
        };
      });
    }
  }
}
