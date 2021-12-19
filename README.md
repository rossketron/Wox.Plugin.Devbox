# Wox.Plugin.Devbox

## Usage

Open Wox and use one of the following keywords:

- `db`: Configure this plugin and list options
- `gh`: Open a repo in github
- `c`: Open a repo in VSCode
- `ember`: Search Ember imports and copy to clipboard

## Installation

Copy the files located in the `.7z` file attached to the release

To this directory:

- `~\AppData\Local\Wox\app-*\Plugins\Wox.Plugin.Devbox`

Restart Wox

On first run, the plugin will prompt you to enter your Github API token.

## Build

> Note: If you modify the ActionKeywords in plugin.json, the ID will need to be changed for Wox to pick up the new keywords

Run `npm install` to download the ember import mappings

Build the solution

Package these files together:

- `plugin.json`
- `mappings.json (node_modules/ember-rfc176-data)`
- `Wox.Plugin.Devbox.dll (bin/Release)`
- `Newtonsoft.Json.dll (lib)`
- `Prompt.png`
