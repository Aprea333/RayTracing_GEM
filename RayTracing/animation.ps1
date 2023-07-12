
$output_dir = '.\output'
Remove-Item -LiteralPath $output_dir -Recurse -Force -ErrorAction SilentlyContinue
New-Item -Path $output_dir -ItemType Directory

$max_angle = 360
$step = 1
for (($angle = 0); $angle -lt $max_angle; $angle+=$step)
{
    $prog = $angle*100/360
    $prog = [math]::round($prog,2)
    $angleNNN = '{0:d3}' -f $angle
    Write-Progress -Activity "Rendering in progress" -Status "$prog% Complete:" -PercentComplete $prog
    $file = "image_$angle.png"
    dotnet run -- demo --angle_deg $angle --width 1024 --height 640  --output_file "output\image_$angleNNN.png"
    #dotnet run -- pfm2png --input_file ".\image.pfm" --output_file "output\image_$angleNNN.png"
}

ffmpeg -r 20 -s 10240x640 -f image2 -i .\output\image_%03d.png -vcodec libx264 -pix_fmt yuv420p "output\spheres-perspective.mp4"
