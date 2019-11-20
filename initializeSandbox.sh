#!/bin/bash

echo "Initializing Sandbox.."
cd Xamarin.Forms
echo "Applying patch to Xamarin.Forms.."
git apply < ../Xamarin.patch
cd ../Sandbox/Tizen.CircularUI
git apply < ../CircularUI.patch
cd ../../

echo "Done, enjoy experiments in sandbox"
