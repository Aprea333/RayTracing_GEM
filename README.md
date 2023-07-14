# RayTracing_GEM

[![MIT License](https://img.shields.io/badge/License-MIT-blue.svg)](./LICENSE)

Welcome to RayTracing_Gem!
This is a raytracing library written in C#. It was developed for the course _Numerical Methods for Photorealistic Image Generation_ held by Prof. [Maurizio Tomasi][1] at Università degli Studi di Milano (A.Y. 2022-2023).

The contibutors to the project are [Giacomo Aprea][2], [Emanuele Ricci][3] and [Michela Dinatolo][4].

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


## License
The code is released under GNU General Public License. See the file [REFERENCE](https://github.com/Aprea333/RayTracing_GEM/blob/master/LICENSE) for further informations.

## CHANGELOG
See the file [REFERENCE](https://github.com/Aprea333/RayTracing_GEM/blob/master/CHANGELOG.md) for history
