# What is Object Pooling?
Object Pooling is optimization technique that focus on reusable objects instead of creating and destroying them. In this solution objects are only displayed if they are inside camera view and released after being outside.

[More info + example](https://www.youtube.com/watch?v=tdSmKaJvCoA)

 
![Alt Text](https://media2.giphy.com/media/lQbFj9l7T2lAuyTWjM/giphy.gif)

[https://www.youtube.com/watch?v=vGq9RtnU9tg](https://www.youtube.com/watch?v=vGq9RtnU9tg)

## Real life example
In our game we have created a lot of GameObjects (trees, rocks, buildings). Even with built-in Frustum Culling there was a big drop in game performance. After investigation we figured out that Unity have problem even with empty GameObjects if they count reach hundreds of thousands. 

Object Pooling gave us **3x more FPS than before** (from 40 to 120).


# About this repository
This demo project shows custom implementation of Object Pooling used in our upcoming game: [Pelagos: Rise of Greece](https://www.reddit.com/r/Pelagos/).

It contains working example scene and commented source code.


## Requirements

Unity 2019.3.2: go to [Unity download archive](https://unity3d.com/get-unity/download/archive) page and download **Unity 2019.3.2** version.
## Installation

Just download ZIP of this repo or use GIT client and open in Unity. That's all :)
## Implementation

This is demonstration of solution that we're using in our project. **This is not** out of the box plugin that you can download and make working in 2 minutes.
Object Pooling requires specific code architecture approach that is unfortunately not taught on Unity tutorials.
In this example we're using **Dependency Injection**. We also try to use Unity components only if necessary.

It may be scary, but don't worry and take a look :)

## Where to start
Good places to start understanding the code are:

`ObjectPooler.Application.ObjectPoolerDisplayer`

`ObjectPooler.Application.Displayers.BuildingsDisplayer`

`ObjectPooler.Application.Displayers.PeopleDisplayer`

and

`Map.Application.MapGenerator`

# Benchmark
Buildings: 22064

People: 16914

|  Object Pooling ON|Object Pooling OFF  |
|--|--|
|  148 FPS| 30 FPS |

Click on screenshot to resize
![Object Pooling ON](https://i.imgur.com/eeLE7Du.png)
![Object Pooling OFF](https://i.imgur.com/lKphxcR.png)
