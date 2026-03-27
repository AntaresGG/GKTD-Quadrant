@echo off
echo QuadrantGTD - 构建所有平台版本
echo ================================

cd /d "%~dp0.."

echo.
echo [1/4] 清理之前的构建...
if exist publish rmdir /s /q publish
mkdir publish

echo.
echo [2/4] 构建 Windows x64 版本...
cd src\QuadrantGTD
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ..\..\publish\win-x64
if %ERRORLEVEL% neq 0 (
    echo Windows 版本构建失败！
    goto :end
)

echo.
echo [3/4] 构建 Linux x64 版本...
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ..\..\publish\linux-x64
if %ERRORLEVEL% neq 0 (
    echo Linux 版本构建失败！
    goto :end
)

echo.
echo [4/4] 构建 macOS x64 版本...
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ..\..\publish\osx-x64
if %ERRORLEVEL% neq 0 (
    echo macOS 版本构建失败！
    goto :end
)

cd ..\..

echo.
echo ================================
echo 构建完成！发布文件位于 publish\ 目录：
echo - Windows: publish\win-x64\QuadrantGTD.exe
echo - Linux:   publish\linux-x64\QuadrantGTD
echo - macOS:   publish\osx-x64\QuadrantGTD
echo.

for /f %%i in ('powershell -command "(Get-Item publish\win-x64\QuadrantGTD.exe).Length / 1MB"') do set "winsize=%%i"
echo Windows 版本大小: %winsize% MB

:end
pause