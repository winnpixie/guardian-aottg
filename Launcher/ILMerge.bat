@echo off
color 0c
echo ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
echo +                       ILMerge Batch Script                       +
echo +                             by Summer                            +
echo +                    Based off of/modified from                    +
echo + https://github.com/dotnet/ILMerge#to-run-ilmerge-in-a-batch-file +
echo ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
echo.

:: This script needs https://www.nuget.org/packages/ilmerge

:: What is the name of our application?
set /P APP_NAME="Set your target executable name. (ex: [ProjectName].exe): "

:: What build type are we making portable?
set /P ILMERGE_BUILD="Set build, used for directory. (ex: Release / Debug): "

:: What platform are we exporting for?
set /P ILMERGE_PLATFORM="Set platform. (ex: x64) This can be left blank.: "

:: What version of ILMerge is going to be used?
set /P ILMERGE_VERSION="Set your ILMerge version. (latest: 3.0.41): "

:: Grab ILMerge from NuGet cache if available
set ILMERGE_PATH=%USERPROFILE%\.nuget\packages\ilmerge\%ILMERGE_VERSION%\tools\net452
dir "%ILMERGE_PATH%"\ILMerge.exe

:: Merge into standalone application
echo Merging %APP_NAME% ...
if not defined ILMERGE_PLATFORM (

  "%ILMERGE_PATH%"\ILMerge.exe bin\%ILMERGE_BUILD%\%APP_NAME%  ^
    /wildcards ^
    /lib:bin\%ILMERGE_BUILD%\ ^
    /out:%APP_NAME% ^
    *.dll

) else (

  echo Merging for platform %ILMERGE_PLATFORM%

  "%ILMERGE_PATH%"\ILMerge.exe Bin\%ILMERGE_PLATFORM%\%ILMERGE_BUILD%\%APP_NAME%  ^
    /wildcards ^
    /lib:bin\%ILMERGE_PLATFORM%\%ILMERGE_BUILD%\ ^
    /out:%APP_NAME% ^
    *.dll

)

dir %APP_NAME%
PAUSE