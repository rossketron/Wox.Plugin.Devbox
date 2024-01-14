#!/usr/bin/env bash

BUILD_DIR=dist
PLUGIN="Flow.Launcher.Plugin.DevBox"
WIN_APPDATA=$(wslpath $(wslvar APPDATA))


build() {
  echo "========================= Building Project ============================"
  echo ""
  if [ ! -d node_modules ]; then
    echo "Installing dependencies..."
    npm install
    echo ""
    echo "Building project..."
    echo ""
  fi
  dotnet build -c Release
  echo ""
}

bundle() {
  if [[ ! -d $BUILD_DIR ]]; then
    build
  fi
  echo "===================== Creating Release Bundle ========================="
  echo ""
  tar -czvf $PLUGIN.tar $BUILD_DIR && \
    gzip -9vf $PLUGIN.tar && \
      echo "Finished bundling package files..." && \
        echo "" && \
          echo "The package is located at ./$PLUGIN.tar.gz"
  echo ""
}

_extract_bundle() {
  echo "==================== Unbundling Release Files ========================"
  echo ""
  gunzip -vkf $PLUGIN.tar.gz && \
    tar -xvf $PLUGIN.tar ; rm -rf $PLUGIN.tar && \
      echo "Finished unbundling project..." && \
        echo "" && \
          echo "The files are located at ./dist"
  echo ""
}

_install() {
  echo "============== Installing DevBox to Flow Launcher ===================="
  echo ""
  echo "Copying files to Flow Launcher directory..."
  echo ""
  if [[ ! -d $WIN_APPDATA/FlowLauncher/Plugins/$PLUGIN ]]; then
    mkdir $WIN_APPDATA/FlowLauncher/Plugins/$PLUGIN
  fi
  cp -v $BUILD_DIR/* $WIN_APPDATA/FlowLauncher/Plugins/$PLUGIN && \
    echo "" && \
      echo "Plugins are now installed, reload plugin data in Flow Launcher to see the changes..."
  echo ""
}

install() {
  if [[ "$1" == "--from-archive" ]]; then
    _extract_bundle
  else
    build
  fi
  _install
}

uninstall() {
  echo "============= Uninstalling DevBox from Flow Launcher ================="
  echo ""
  rm -rfv $WIN_APPDATA/FlowLauncher/Plugins/$PLUGIN && \
    echo "Plugins are now uninstalled, reload plugin data in Flow Launcher to see the changes..."
  echo ""
}

clean() {
  echo "======================== Cleaning Project ============================"
  echo ""
  local obj_dirs=$(find . -type d -name obj)
  local bin_dirs=$(find . -type d -name bin)
  rm -rf node_modules $BUILD_DIR $obj_dirs $bin_dirs && \
    echo "Finished cleaning project..."
  echo ""
}

case $1 in
  (build|bundle|clean|install|uninstall) "$@" ;;
  (*) echo "Usage: $0 {build|bundle|clean|install|uninstall}" && exit 1 ;;
esac
