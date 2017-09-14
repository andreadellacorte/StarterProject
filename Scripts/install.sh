#!/bin/bash

set +e

mkdir -p ~/.improbable/oauth2
mv ./secret ~/.improbable/oauth2/oauth2_refresh_token

brew tap caskroom/cask
brew tap improbable-io/spatialos
brew update

brew cask install java

brew cask install spatial

brew cask install unity@5.6.0
brew cask install unity-ios-support-for-editor@5.6.0
brew cask install unity-linux-support-for-editor@5.6.0
brew cask install unity-standard-assets@5.6.0
brew cask install unity-windows-support-for-editor@5.6.0

spatial diagnose --log_level=debug

set -e
