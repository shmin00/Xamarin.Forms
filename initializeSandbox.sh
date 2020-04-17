#!/bin/bash

echo "Initializing Sandbox.."

echo "Applying a patch to Xamarin.Forms.."
cd Xamarin.Forms
git stash && git checkout profiling
git apply < ../Sandbox/patches/Xamarin.patch

echo "Applying a patch to Tizen.CircularUI.."
cd ../Sandbox/Tizen.CircularUI
git stash && git checkout master
git apply < ../patches/CircularUI.patch
cd ../../

echo "Applying a patch to Tizen.TV.UIControls.."
cd ./Sandbox/Tizen.TV.UIControls
git checkout master
git apply < ../patches/TVUIControls.patch
cd ../../

echo "Done, enjoy experiments in sandbox"
