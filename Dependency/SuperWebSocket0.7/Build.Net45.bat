@echo off

set fdir=%WINDIR%\Microsoft.NET\Framework64

if not exist %fdir% (
	set fdir=%WINDIR%\Microsoft.NET\Framework
)

set msbuild=%fdir%\v4.0.30319\msbuild.exe

%msbuild% SuperWebSocket\SuperWebSocket.NET45.csproj /p:Configuration=Release /t:Rebuild /p:OutputPath=..\bin\Net45\Release

FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

%msbuild% SuperWebSocket\SuperWebSocket.NET45.csproj /p:Configuration=Debug /t:Rebuild /p:OutputPath=..\bin\Net45\Debug

FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

pause