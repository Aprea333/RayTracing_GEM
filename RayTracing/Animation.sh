#!/bin/bash


 output_dir="./output"
 rm -rf "$output_dir"
 mkdir "$output_dir"
  
    for angle in $(seq 0 359); do
   
        angleNNN=$(printf "%03d" $angle)
        
      dotnet run -- demo --width 640 --height 480 --angle_deg $angle
      dotnet run -- pfm2png  --input_file image.pfm "/image.pfm" --output_file "image_$angleNNN.png"
      
    done
      
    ffmpeg -r 25 -f image2 -s 640x480 -i image_%03d.png -vcodec libx264 -pix_fmt yuv420p spheres-perspective.mp4  
    
    