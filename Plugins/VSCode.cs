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

        public static void openResultInVSCode(ApiResultRepo result, SettingsModel settings)
        {
            openVSCode($"{settings.gitFolder}\\{result.name}");
        }

        public static void openVSCode(String folder)
        {
            String command = $"code {folder}";

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

        public static void cloneRepo(ApiResultRepo result, SettingsModel settings)
        {
            if (Directory.Exists($"{settings.gitFolder}\\{result.name}"))
            {
                return;
            }
            var cloneUrl = $"git@github.com:{result.owner.login}/{result.name}.git";
            var command = $"git clone {cloneUrl}";

            ProcessStartInfo info;
            var arguments = $"/c \"{command}\"";
            info = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = arguments,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = settings.gitFolder
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
                        openVSCode("");
                        return true;
                    },
                    IcoPath = ico
                });
                return list;
            }

            ApiResult results = GithubApi.QueryGithub(query, settings);

            if (results.total_count > 0)
            {
                foreach (ApiResultRepo result in results.items)
                {
                    list.Add(new Result
                    {
                        Title = result.full_name,
                        SubTitle = result.description,
                        IcoPath = ico,
                        Action = (e) =>
                        {
                            cloneRepo(result, settings);
                            openResultInVSCode(result, settings);
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
