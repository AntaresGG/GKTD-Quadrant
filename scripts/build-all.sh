#!/bin/bash

echo "QuadrantGTD - 构建所有平台版本"
echo "================================"

# 获取脚本所在目录的上级目录
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"
cd "$PROJECT_DIR"

echo
echo "[1/4] 清理之前的构建..."
rm -rf publish
mkdir -p publish

echo
echo "[2/4] 构建 Windows x64 版本..."
cd src/QuadrantGTD
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ../../publish/win-x64
if [ $? -ne 0 ]; then
    echo "Windows 版本构建失败！"
    exit 1
fi

echo
echo "[3/4] 构建 Linux x64 版本..."
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ../../publish/linux-x64
if [ $? -ne 0 ]; then
    echo "Linux 版本构建失败！"
    exit 1
fi

echo
echo "[4/4] 构建 macOS x64 版本..."
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ../../publish/osx-x64
if [ $? -ne 0 ]; then
    echo "macOS 版本构建失败！"
    exit 1
fi

cd ../..

echo
echo "================================"
echo "构建完成！发布文件位于 publish/ 目录："
echo "- Windows: publish/win-x64/QuadrantGTD.exe"
echo "- Linux:   publish/linux-x64/QuadrantGTD"
echo "- macOS:   publish/osx-x64/QuadrantGTD"
echo

# 显示文件大小
if [ -f "publish/win-x64/QuadrantGTD.exe" ]; then
    winsize=$(du -h publish/win-x64/QuadrantGTD.exe | cut -f1)
    echo "Windows 版本大小: $winsize"
fi

if [ -f "publish/linux-x64/QuadrantGTD" ]; then
    linuxsize=$(du -h publish/linux-x64/QuadrantGTD | cut -f1)
    echo "Linux 版本大小: $linuxsize"
fi

if [ -f "publish/osx-x64/QuadrantGTD" ]; then
    macsize=$(du -h publish/osx-x64/QuadrantGTD | cut -f1)
    echo "macOS 版本大小: $macsize"
fi

echo
echo "所有平台版本构建完成！🚀"