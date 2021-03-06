#!/usr/bin/env bash

set -e
set -u

brew tap caskroom/cask
brew tap wooga/unityversions
brew update

brew cask install spatial

if [ ! -d "$UNITY_FOLDER"/Unity.app ]; then
  brew cask install unity@$UNITY_VERSION
  brew cask install unity-linux-support-for-editor@$UNITY_VERSION
  brew cask install unity-standardassets@$UNITY_VERSION
  brew cask install unity-windows-support-for-editor@$UNITY_VERSION

  mv "$UNITY_FOLDER-$UNITY_VERSION" $UNITY_FOLDER
fi
