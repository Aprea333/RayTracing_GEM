## Tutorial for the Input Scene File

In this file we provide the instructions to build the input file describing the scene. At the end of this file you will be able to understand [the examples] and to build your own images.

All lines starting with the # symbol are considered comments, they are just skipped by the compiler.

### Float variables
We can define float variables as follows:

`float name(value)`

where `value` is a floating-point number and `name` is the name of the variable.
Example: `float angle(45)`
Defining float variables is useful for particular transformations.

### Materials
In order to create our scene, we must first of all define the materials of the shapes.
A material characterizes the color of an object and is declared like this:

`material name = (Brdf, Pigment)`

where `name` is the name assigned to the material. Once created a material, you can refer to it using its name. We must specify the `BRDF` type and the `Pigment` type.
The `BRDF` type needs a pigment. The `Pigment` specify the emitted radiance.

#### BRDF types: diffuse, specular
To define each type of BRDF we have to specify a Pigment between brackets, as follows:

`Brdf(Pigment)`

#### Pigment types: uniform, checkered, image
To define a Pigment we need to know how to define a Color. Color are represented by triplets of floating numbers enclosed by angular brackets

`color = <R,G,B>`

where `R` is red color, `G` is green color and `B` is blue color. 
Example: `<0.5, 0.1, 0.3>`
We recommend to use values from 0 to 1 for the three color components.

Now we report how to define the different pigment giving some examples

`uniform (color)`

Example: `uniform(<0.5, 0.1, 0.3>)`

`checkered (color1, color2, NumOfSteps)`

where `NumOfSteps` is the number of iterations (integer) of the checkered pattern.
Example: `checkered(<0.5, 0.1, 0.3>, <0.2, 0.7, 0.1>, 5)`

`image (filename)`

where `filename` is the file in pfm format.
Example: `image("moon.pfm")`

⚠️ __If the file of the image is not in the directory where you are executing the code, you must specify the path of the file__ ⚠️

Now we can show some examples of Brdf:

`diffuse(image("texture.txt"))`
`specular(uniform(<0.5, 0.1, 0.3>))`

We can now define materials:

`material blu_material(specular(uniform(<0, 0, 1>)), uniform(<0,0,0>))`
`material light_material(diffuse(uniform(<1, 1, 1>)), uniform(<1,1,1>))`

If the second pigment is uniform with color `<0,0,0>`, which is black, the object does not radiate light.


### Shapes
Shapes are the objects in our world. We can declare a shape in the following way:

`shape_type = (material_name, transformation)`

We must specify the name of the material and also the transformation. 

⚠️ __The `material_name` must be already define above, you cannot directly construct a material here__ ⚠️

This is the way we use for the shapes: `Sphere`, `Cylinder`, `Plane`.

The `Box` requires a different definition:

`box = (max_point, min_point, material_name, transformation)`

where `max_point` and `min_point` are 3D points representing the maximum and the minimum extent of each axis of the box.

The last shape left is the `union_shape`, which is obtained from the difference between the intersection of a sphere and a box and the union of three cylinder. You can see an example [here]. It requires this definition:

`union_shape = (transformation)`

where `transformation` is the transformation that can be add to the shape.

Now we need to know what transformation are allowed and how to use them.

__Transformation types: scale, translation, rotation_x, rotation_y, rotation_z, identity__

In order to create a transformation we need to know how to define vector. Vectors are represented by triples of numbers enclosed by squared brackets:

`vector = [X, Y, Z]`

where `X`, `Y`, `Z` are floating numbers.
Example `[1.2, 3.6, 4.1]`

Now we report how to define the different transformations giving some examples:

`scale(x, y, z)`

where `X`, `Y`, `Z` are floating numbers. Example `scale(1, 2, 3)`

`translation(vector)`

Example `translation([0.1, 3, 5])`

`rotation_x(angle)`

Example `rotation_x(15)`. Similarly to `rotation_x` we can obtain `rotation_y` and `rotation_z`

⚠️ __The angle must be specified in degrees__ ⚠️

The variable `angle` could be used in different rotations of different shapes. 

Transformations can be combined with the `*` symbol, but some transformations are __not__ commutative, so pay attention to the order. If you don't need any transformation, write `identity`.

Here are the characteristics of the default shapes:
- __Sphere__: it is centered at the origin and has a unit radius
- __Plane__: xy plane, passing through the origin
- __Cylinder__: it is centered at the origin. The z-coordinate range is -0.5 to 0.5 and it has unit radius
- __Box__: it has faces parallel to the axes, as just seen we can define the extent of each face

Example `box([-1,-1,-1],[1,1,1], blue_material, rotation_x(60))`

We can also construct more complicated shapes using Constructive Solid Geometry (CSG) just doing this:

`CSGOperation = (shape1, shape2, transformation)`

__CSGOperation: union (1 + 2), difference (1 - 2), intersection (1 ∩ 2)__

Example `union = (sphere(blue_material, identity), sphere(light_material, translation([0.1, 3, 5])), identity)`

### Cameras
The camera describes the position of the observer and the viewing direction.

⚠️ __For each scene you must define one ad only one camera!__ ⚠️

We declare a camera in the following way: 

`camera = (type, transformation, aspect_ratio, distance)`

where `aspect_ratio` is a floating number defining how larger than the height is the image, `distance` represents the distance between the observer and the screen.
__Type: perspective, orthogonal__

Examples: 

`camera (perspective, translation([-5,0,0.5]), 1, 1)`

`camera (orthogonal, translation([-5,0,0.5]), 1, 1)`

And now it's time to create your first image!
