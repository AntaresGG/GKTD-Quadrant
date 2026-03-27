# PowerShell脚本：创建现代化的GTD图标
# 需要安装ImageMagick或使用在线转换工具

param(
    [string]$OutputPath = "..\src\QuadrantGTD\Assets\quadrant-modern.ico"
)

Write-Host "创建QuadrantGTD应用图标..." -ForegroundColor Green

# 检查ImageMagick是否可用
try {
    $magickPath = Get-Command magick -ErrorAction Stop
    Write-Host "找到ImageMagick: $($magickPath.Source)" -ForegroundColor Green
    
    # 从SVG创建多尺寸ICO文件
    $svgPath = "..\src\QuadrantGTD\Assets\quadrant-modern.svg"
    
    if (Test-Path $svgPath) {
        Write-Host "正在转换SVG到ICO..." -ForegroundColor Yellow
        
        # 创建多个尺寸的ICO文件
        & magick $svgPath -background transparent -define icon:auto-resize=256,128,64,48,32,16 $OutputPath
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "图标创建成功: $OutputPath" -ForegroundColor Green
        } else {
            Write-Host "图标创建失败" -ForegroundColor Red
        }
    } else {
        Write-Host "SVG文件不存在: $svgPath" -ForegroundColor Red
    }
    
} catch {
    Write-Host "未找到ImageMagick，请使用以下替代方案：" -ForegroundColor Yellow
    Write-Host "1. 安装ImageMagick: https://imagemagick.org/script/download.php" -ForegroundColor Cyan
    Write-Host "2. 使用在线转换工具:" -ForegroundColor Cyan
    Write-Host "   - https://convertio.co/svg-ico/" -ForegroundColor Cyan
    Write-Host "   - https://www.aconvert.com/icon/svg-to-ico/" -ForegroundColor Cyan
    Write-Host "3. 使用GIMP或其他图像编辑软件手动转换" -ForegroundColor Cyan
}

Write-Host "完成!" -ForegroundColor Green