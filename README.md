<img  align= "right" width="400" src = "./RayTracing/Examples/threesphere.png"/>


# RayTracing_GEM

[![MIT License](https://img.shields.io/badge/License-MIT-blue.svg)](./LICENSE)
![Shield](https://img.shields.io/badge/build-passing-brightgreen.svg)
![Maintenance](https://img.shields.io/github/commit-activity/m/aprea333/RayTracing_GEM?color=yellow)
![Top Language](https://img.shields.io/github/languages/top/aprea333/RayTracing_GEM?color=red)
![release](https://img.shields.io/badge/version-0.3.0-violet)
![GitHub contributors](https://img.shields.io/github/contributors/aprea333/RayTracing_GEM?color=white)

Welcome to RayTracing_Gem!
This is a raytracing library written in C#. It was developed for the course _Numerical Methods for Photorealistic Image Generation_ held by Prof. [Maurizio Tomasi](https://github.com/ziotom78) at Università degli Studi di Milano (A.Y. 2022-2023).

The contibutors to the project are [Giacomo Aprea](https://github.com/Aprea333), [Emanuele Ricci](https://github.com/emaskusku) and [Michela Dinatolo](https://github.com/micheladinatolo).

## Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Usage](#usage)
    - [Render mode](#render-mode)
    - [How to create input files](#how-to-create-input-files)
    - [Demo mode](#demo-mode)
- [Documentation](#documentation)
- [License](#license)
- [Gallery](#gallery)
- [Issue tracking](#issue-tracking)
## Overview
👀

The main functionality of this library is to produce photorealistic images from input files describing a certain scene.
The scene is made of geometric shapes (see the list of available shapes), each one defined by its transformation and its material. Our code offers the possibility to choose between a diffusive, emissive or reflective material.
The code implements three different _backwards ray tracing_ algorithms to simulate how light rays propagate. A camera (perspective or orthogonal) representing the observer will see the world through a 2D screen placed ahead of it and it is defined by its position, the distance from the screen and the aspect ratio.

Once everything (shapes and observer) is defined and in place, the code solves the rendering equation (with different assumptions, depending on the chosen algorithm) and produces an HDR image (in `.pfm` format). Later, the HDR image is converted into LDR formats, in particular `.png`.

## Prerequisites
💻

RayTracing_GEM requires .NET 7.0.201 to run. It is possible to download the latest version [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
This library uses some external libraries. The user should not worry as .NET automatically imports them with the repository download. The libraries are:
- [CommandLineParser](https://github.com/commandlineparser/commandline) to build the Command Line Interface
- [ShellProgressBar](https://www.nuget.org/packages/ShellProgressBar/) to show a progress bar during while rendering
- [ImageSharp](https://www.nuget.org/packages/SixLabors.ImageSharp/2.1.1) to converta pfm image into an LDR image

This library is available for Windows, Linux and MacOS systems.

## Usage
In order to use the library you can clone this repository through the command

`git@github.com:Aprea333/RayTracing_GEM.git`

Alternatively, you can download the latest version of the code from the [releases page](https://github.com/Aprea333/RayTracing_GEM/releases).
To check that the code works as expected, you can run the suite of tests using the following command:

`dotnet test`

To get command line help for the usage, and see all the commands for the different mode, use the command:

`dotnet run`

### Demo mode
<img  align= "right" width="300" src = "./RayTracing/Examples/demo.png"/>
Use this mode to create a simple image. In order to use this mode, you must go to the `RayTracing_GEM/RayTracing` directory and run the following command:

`dotnet run demo[options]`

the image on the right will be generated. This image is composed of randomly positioned and randomly colored spheres using the PCG random number generator implemented in the code.

To view all the possible options of the demo command you can run
`dotnet run demo -help`

### Render mode
You can create your own images using this mode: it reads an input file describing the scene to render and creates the image. Use this  [tutorial](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/Examples/Tutorial.md) to learn how to write an imput file.

In order to use this mode, you must go to the `RayTracing_GEM/RayTracing` directory and run the following command:

`dotnet run render [arguments][options]`

Here you ccan find some examples:
- [Cornell example](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/texture/Cornell.txt) & [Result](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/Examples/Cornell_sphere.png)
- [Csg example](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/texture/CsgScene.txt) & [Result](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/Examples/csg.mp4)
- [Scene example](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/texture/FirstScene.txt) & [Result](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/Examples/coke.png)
- [Four spheres example](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/texture/SecondScene.txt) & [Result](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/Examples/threesphere.png)    
- [Another scene example](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/texture/spheres.txt) & [Result](https://github.com/Aprea333/RayTracing_GEM/blob/master/RayTracing/Examples/first_image.png)

### Convert mode
Use this mode if you need to convert a .pfm file into a LDR file running the following command:

`dotnet run pfm2png --input_file [NAME] --output_file [NAME]`

To view all the possible options you can run

`dotnet run pfm2png -help`

## License
The code is released under GNU General Public License. See the file [REFERENCE](https://github.com/Aprea333/RayTracing_GEM/blob/master/LICENSE) for further informations.

## CHANGELOG
See the file [REFERENCE](https://github.com/Aprea333/RayTracing_GEM/blob/master/CHANGELOG.md) for history

## Gallery

<p float = "center">

<img  src="./RayTracing/Examples/moon_earth.gif" width="350"/>
<img  src="./RayTracing/Examples/csg.gif" width="350"/>
<img  src="./RayTracing/Examples/threesphere.png" height = "250" />
<img  src="./RayTracing/Examples/Cornell_sphere.png" height = "250" />
<img  src="./RayTracing/Examples/coke.png" height = "250" />
<img  src="./RayTracing/Examples/demo.png" height = "250" />
<img  src="./RayTracing/Examples/first_image.png" height = "250" />

</p>

## Issue tracking
If you happen to find any issue or bug with our code, you're more than welcome to let us know.
Either [contact us via email](mailto:michelamaria.dinatolo@studenti.unimi.it,giacomo.aprea@studenti.unimi.it,emanuele.ricci@studenti.unimi.it).

To contribute to RayTracing_GEM, clone this repository locally and commit your code on a separate branch. Please write unit test for your code and then open a _pull request_. If you find any bug in our code, let us kno by opening an _issue_. We will be grateful to any contribution!
