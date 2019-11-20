# Xamarin.Forms.Tizen.Sandbox
`Xamarin.Forms.Tizen.Sandbox` provides the application build environement that is optimized for ttrace measurement.

## Cloning the repository
Clone repositories using recursive parameter:

	git clone git@github.sec.samsung.net:dotnet/Xamarin.Forms.Tizen.Sandox.git --recursive

Or Update submodules at `Xamarin.Forms.Tizen.Sandox` directory

	git submodule update --init --recursive

## Initizlize Sandbox
Run initialize script to apply patches to submodules.

	~/Xamarin.Forms.Tizen.Sandbox $ ./initializeSandbox.sh

## Build Sandbox

	1. Build `Xamarin.Forms.Build.Tasks` project.
	2. Build an application.