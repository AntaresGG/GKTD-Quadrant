# PowerShell脚本：验证EXE文件图标
param(
    [string]$ExePath = "..\publish\win-x64-new\QuadrantGTD.exe"
)

Write-Host "验证QuadrantGTD.exe图标..." -ForegroundColor Green

if (Test-Path $ExePath) {
    # 获取文件信息
    $fileInfo = Get-ItemProperty $ExePath
    Write-Host "文件路径: $($fileInfo.FullName)" -ForegroundColor Cyan
    Write-Host "文件大小: $([math]::Round($fileInfo.Length / 1MB, 2)) MB" -ForegroundColor Cyan
    Write-Host "创建时间: $($fileInfo.CreationTime)" -ForegroundColor Cyan
    Write-Host "修改时间: $($fileInfo.LastWriteTime)" -ForegroundColor Cyan
    
    # 尝试获取版本信息
    try {
        $versionInfo = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($ExePath)
        Write-Host "产品名称: $($versionInfo.ProductName)" -ForegroundColor Yellow
        Write-Host "产品版本: $($versionInfo.ProductVersion)" -ForegroundColor Yellow
        Write-Host "文件描述: $($versionInfo.FileDescription)" -ForegroundColor Yellow
        Write-Host "公司名称: $($versionInfo.CompanyName)" -ForegroundColor Yellow
    } catch {
        Write-Host "无法获取版本信息" -ForegroundColor Red
    }
    
    Write-Host "`n图标验证完成！" -ForegroundColor Green
    Write-Host "请在Windows资源管理器中查看文件图标以确认效果。" -ForegroundColor Green
    Write-Host "如果图标未显示，请尝试：" -ForegroundColor Yellow
    Write-Host "1. 刷新文件夹 (F5)" -ForegroundColor Cyan
    Write-Host "2. 重启资源管理器进程" -ForegroundColor Cyan
    Write-Host "3. 清除图标缓存：ie4uinit.exe -show" -ForegroundColor Cyan
} else {
    Write-Host "错误：未找到EXE文件 $ExePath" -ForegroundColor Red
}

# 创建快捷方式用于测试图标
try {
    $desktopPath = [Environment]::GetFolderPath("Desktop")
    $shortcutPath = "$desktopPath\QuadrantGTD.lnk"
    
    $WshShell = New-Object -ComObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut($shortcutPath)
    $Shortcut.TargetPath = (Resolve-Path $ExePath).Path
    $Shortcut.WorkingDirectory = Split-Path (Resolve-Path $ExePath).Path
    $Shortcut.Description = "四象限GTD任务管理工具"
    $Shortcut.Save()
    
    Write-Host "`n已在桌面创建快捷方式用于测试图标显示" -ForegroundColor Green
} catch {
    Write-Host "创建桌面快捷方式失败: $($_.Exception.Message)" -ForegroundColor Red
}