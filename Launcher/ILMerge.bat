:: This script requires https://www.nuget.org/packages/ilmerge
@ECHO OFF
ECHO ILMerge Helper Script, by Summer (https://github.com/alerithe)
ECHO:
ECHO This script is a modified version of the one provided in
ECHO - https://github.com/dotnet/ILMerge#to-run-ilmerge-in-a-batch-file
ECHO:

:: What is the name of our application?
SET /P IL_EXE_NAME="Set your executable name, used for pathing/output. (ex: <ProjectName>.exe): "

:: What build type are we making portable?
SET /P IL_BUILD="Set build, used for pathing. (ex: [Release]/Debug): "
IF NOT DEFINED IL_BUILD (
	ECHO No build defined, defaulting to Release.
	SET IL_BUILD=Release
)

:: What platform are we exporting for?
SET /P IL_ILMERGE_PLATFORM="OPTIONAL: Set platform, used for pathing. (ex: x64/x86): "

:: What version of ILMerge is to be used?
SET /P IL_ILMERGE_VERSION="Set your ILMerge version, used for pathing. (ex: 3.0.41): "

:: Grab ILMerge from project-wide NuGet, if available
SET IL_ILMERGE_PATH=.\packages\ILMerge.%IL_ILMERGE_VERSION%\tools\net452
IF NOT EXIST "%IL_ILMERGE_PATH%" (
	ECHO Could not locate project-wide ILMerge.%IL_ILMERGE_VERSION%, trying NuGet cache instead.

	:: Grab ILMerge from user-wude NuGet cache, if available
	SET IL_ILMERGE_PATH=%USERPROFILE%\.nuget\packages\ilmerge\%IL_ILMERGE_VERSION%\tools\net452
	IF NOT EXIST "%IL_ILMERGE_PATH%" (
		ECHO Could not locate ILMerge.%IL_ILMERGE_VERSION% NuGet Package, terminating!

		PAUSE
		EXIT
	)
)

:: Merge into standalone application
ECHO Merging %IL_EXE_NAME%...

IF NOT DEFINED IL_ILMERGE_PLATFORM (
	"%IL_ILMERGE_PATH%"\ILMerge.exe bin\%IL_BUILD%\%IL_EXE_NAME%  ^
	/wildcards ^
	/lib:bin\%IL_BUILD%\ ^
	/out:.\%IL_EXE_NAME% ^
	*.dll
) ELSE (
	ECHO Merging for platform %IL_ILMERGE_PLATFORM%...

	"%IL_ILMERGE_PATH%"\ILMerge.exe bin\%IL_ILMERGE_PLATFORM%\%IL_BUILD%\%IL_EXE_NAME%  ^
	/wildcards ^
	/lib:bin\%IL_ILMERGE_PLATFORM%\%IL_BUILD%\ ^
	/out:.\%IL_EXE_NAME% ^
	*.dll
)

DIR %IL_EXE_NAME%

PAUSE