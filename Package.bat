@echo off

:: https://discussions.unity.com/t/how-to-find-installation-path-of-given-unity-version-outside-of-unity-editor/850322

set "UnityVersion=2022.3.10f1"
set "RegPath=HKEY_CURRENT_USER\SOFTWARE\Unity Technologies\Installer\Unity %UnityVersion%"
set "RegKey=Location x64"

for /F "usebackq tokens=3,*" %%A IN (`reg query "%RegPath%" /v "%RegKey%" 2^>nul ^| find "%RegKey%"`) do (
    set UnityPath=%%B
)
set UnityPath=%UnityPath%\Editor\Unity.exe

echo %UnityPath%

pause

:: https://docs.unity3d.com/530/Documentation/Manual/CommandLineArguments.html
"%UnityPath%" -quit -batchmode -nographics -projectPath . -buildWindows64Player Builds\Window\PlayerCharacters.exe

:: https://docs.unity3d.com/ScriptReference/BuildTarget.html
::"%UnityPath%" -quit -batchMode -executeMethod ProjectBuilder.BuildWindows

pause