# Xamarin.Forms.Tizen.Sandbox
`Xamarin.Forms.Tizen.Sandbox` provides the application build environement that is optimized for ttrace measurement.

## Cloning the repository
Clone repositories using recursive parameter:

	git clone git@github.sec.samsung.net:dotnet/Xamarin.Forms.Tizen.Sandbox.git --recursive

Or Update submodules at `Xamarin.Forms.Tizen.Sandox` directory

	git submodule update --init --recursive

## Initizlize Sandbox
Run initialize script to apply patches to submodules.

	~/Xamarin.Forms.Tizen.Sandbox $ ./initializeSandbox.sh

## Build Sandbox

	1. Build `Xamarin.Forms.Build.Tasks` project.
	2. Build an application.
	
### When build on remote Windows machine
 Visual Studio does not allow to load assembly from network location by default. <br/>
 To fix the issue, add `loadFromRemoteSources` property to `MSBuild.exe.config` file which is normally located at
`C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe.config`.
 ```xml
 <configuration>
    <runtime>
	....
       <loadFromRemoteSources enabled="true"/>
   </runtime>
</configuration>
 ```
 Visit [the link](https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/runtime/loadfromremotesources-element) to know more about `loadFromRemoteSources`.
