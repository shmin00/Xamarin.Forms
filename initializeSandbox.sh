#!/bin/bash

echo "Initializing Sandbox.."
echo "Applying a patch to Xamarin.Forms.."
cd Xamarin.Forms
git checkout profiling
git apply < ../Sandbox/patches/Xamarin.patch
echo "Applying a patch to Tizen.CircularUI.."
cd ../Sandbox/Tizen.CircularUI
git checkout master
git apply < ../patches/CircularUI.patch
cd ../../
echo "Done, enjoy experiments in sandbox"
