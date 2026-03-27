#!/usr/bin/env python3
"""
SVG to ICO converter script for QuadrantGTD
Requires: pip install pillow cairosvg
"""

import os
import sys
from pathlib import Path

try:
    from PIL import Image
    import cairosvg
    import io
except ImportError as e:
    print(f"Error: Missing required package. Please install:")
    print("pip install pillow cairosvg")
    print(f"Missing: {e.name}")
    sys.exit(1)

def svg_to_ico(svg_path, ico_path, sizes=[16, 32, 48, 64, 128, 256]):
    """
    Convert SVG file to ICO with multiple sizes
    """
    try:
        print(f"Converting {svg_path} to {ico_path}...")
        
        # Read SVG file
        with open(svg_path, 'rb') as svg_file:
            svg_data = svg_file.read()
        
        # Create images for different sizes
        images = []
        for size in sizes:
            print(f"  Creating {size}x{size} image...")
            
            # Convert SVG to PNG in memory
            png_data = cairosvg.svg2png(
                bytestring=svg_data,
                output_width=size,
                output_height=size,
                background_color='transparent'
            )
            
            # Create PIL Image
            image = Image.open(io.BytesIO(png_data))
            images.append(image)
        
        # Save as ICO with multiple sizes
        print(f"  Saving ICO file with {len(images)} sizes...")
        images[0].save(
            ico_path,
            format='ICO',
            sizes=[(img.width, img.height) for img in images],
            append_images=images[1:]
        )
        
        print(f"Successfully created: {ico_path}")
        return True
        
    except Exception as e:
        print(f"Error converting SVG to ICO: {e}")
        return False

def main():
    # Get script directory
    script_dir = Path(__file__).parent
    project_root = script_dir.parent
    assets_dir = project_root / "src" / "QuadrantGTD" / "Assets"
    
    # Input and output paths
    svg_premium = assets_dir / "quadrant-premium.svg"
    svg_modern = assets_dir / "quadrant-modern.svg"
    ico_output = assets_dir / "quadrant-icon.ico"
    
    print("QuadrantGTD SVG to ICO Converter")
    print("=" * 40)
    
    # Check if SVG files exist
    if not svg_premium.exists() and not svg_modern.exists():
        print("No SVG files found in Assets directory")
        return False
    
    # Choose which SVG to convert (prefer premium)
    svg_input = svg_premium if svg_premium.exists() else svg_modern
    print(f"Input file: {svg_input.name}")
    print(f"Output file: {ico_output.name}")
    
    # Convert SVG to ICO
    success = svg_to_ico(svg_input, ico_output)
    
    if success:
        print("\nConversion completed successfully!")
        print("Next steps:")
        print("   1. Rebuild the project: dotnet build src/QuadrantGTD")
        print("   2. Publish the application: dotnet publish src/QuadrantGTD -c Release -r win-x64 --self-contained")
        print("   3. Check the new icon in Windows Explorer!")
    else:
        print("\nConversion failed!")
        print("Alternative solutions:")
        print("   1. Use online converter: https://convertio.co/svg-ico/")
        print("   2. Use GIMP or Paint.NET to manually convert")
        print("   3. Install ImageMagick: https://imagemagick.org/script/download.php")
    
    return success

if __name__ == "__main__":
    main()