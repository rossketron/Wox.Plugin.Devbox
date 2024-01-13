# Flow.Launcher.Plugin.DevBox

## Usage

Open Wox and use one of the following keywords:

- `db`: Configure this plugin and list options
- `gh`: Open a repo in GitHub
- `c`: Open a repo in VSCode
- `ember`: Search Ember imports and copy to clipboard
- `wincl`: Clone a repo from GitHub to Windows
- `cl`: Clone a repo from GitHub to WSL

## Installation

Copy the files located in the `.7z` file attached to the release

To this directory:

- `~\AppData\Local\Wox\app-*\Plugins\Flow.Launcher.Plugin.DevBox`

Restart Wox

On first run, the plugin will prompt you to enter your Github API token.

## Contributing -- Build, Test, and Release

> Note: If you modify the ActionKeywords in plugin.json, the ID will need to be changed for Wox to pick up the new keywords

Run `npm install` to download the ember import mappings

Build the solution:

- If using VSCode, `CNTL-Shift-B` will launch the build process

In order to test the Plugin while working:

- Build the solution
- If wox is currently running, you will have to `exit` wox before continuing
- Run the provided `release.sh` script
  - Update the `WOX_VERSION` variable at the top of the script to your wox version
  - This bundles all needed files and places them here: `~\AppData\Local\Wox\app-*\Plugins\Flow.Launcher.Plugin.DevBox`
- Restart wox and test your changes

Create a Release:

- Use 7-zip to archive the plugin files located in the `Flow.Launcher.Plugin.DevBox` directory
- Use this archive to create the release
