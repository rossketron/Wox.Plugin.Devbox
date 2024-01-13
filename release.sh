#!/usr/bin/env bash

#################################
WOX_VERSION="1.3.524"
#################################

if [ -d release ]; then
  rm -r release
fi

mkdir release
cp ./bin/Debug/*.dll* release/
cp ./bin/Debug/*.xml release/
cp ./node_modules/ember-rfc176-data/mappings.json release/
cp ./plugin.json release/
cp ./Prompt.png release/

if [ -d ~/AppData/Local/Wox/app-$WOX_VERSION/Plugins/Flow.Launcher.Plugin.DevBox ]; then
  rm -r ~/AppData/Local/Wox/app-$WOX_VERSION/Plugins/Flow.Launcher.Plugin.DevBox
fi

mkdir ~/AppData/Local/Wox/app-$WOX_VERSION/Plugins/Flow.Launcher.Plugin.DevBox
mv release/* ~/AppData/Local/Wox/app-$WOX_VERSION/Plugins/Flow.Launcher.Plugin.DevBox
rmdir release
