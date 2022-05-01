# BonZeb CSharp — CSCD94 Project
  Computer Science Project - Fall 2021/Winter 2022: Machine Learning in real-time analysis of multi-animal tracking experiments
  
![](/Final%20Presentation/BonZeb%20-%20BonZeb.jpg)

# Table of Contents
1. [Introduction](#introduction)
2. [From Python to CSharp](#from-python-to-csharp)
3. [Kalman Filter](#kalman-filter)
4. [YOLOv5](#yolov5)
5. [DeepSORT](#deepsort)
6. [Project Overview](#project-overview)
7. [Reference](#reference)
8. [Credits](#credits)

# Introduction
[BonZeb](https://github.com/ncguilbeault/BonZeb) is a [Bonsai](https://bonsai-rx.org/) library for high-resolution zebrafish behavioural tracking and analysis. The project is to add the real-time tracking feature to current BonZeb library so that researchers can collect accurate behavioral data easier. This README gives an overview of the problem-solving process and results in the past 8 months.

# From Python to CSharp
Goal: Rewrote tracking algorithms in C# (Bonsai) to make the project lightweight​

Since we want good real-time tracking experience, I have to keep the application as lightweight as possible. I need to program in C# which is the language used in Bonsai. However, I cannot just import methods from off-the-shelf C# libraries since it will import other unnecessary content and violate our goal to keep the software lightweight.

I re-wrote the key tracking methods [cdist](https://docs.scipy.org/doc/scipy/reference/generated/scipy.spatial.distance.cdist.html) and [linear_sum_assignment](https://docs.scipy.org/doc/scipy-0.18.1/reference/generated/scipy.optimize.linear_sum_assignment.html) using C# and integrated them into BonZeb​

## [cDist](https://github.com/ymart1n/BonZeb_CSharp/blob/3272d82fb6e031bc9ac36ca9d650d6abf6ee99d0/CSharp/multi-animal-tracking.cs#L27)

double[,] cDist(double[][] XA, double[][] XB) computes the distance between m points using Euclidean distance (2-norm) as the distance metric between the points. The points are arranged as m n-dimensional row vectors in the matrix X.

## [LinearSumAssignment](https://github.com/ymart1n/BonZeb_CSharp/blob/3272d82fb6e031bc9ac36ca9d650d6abf6ee99d0/CSharp/multi-animal-tracking.cs#L69)

The *LinearSumAssignment* method solves the linear sum assignment problem (using Hungarian method)​. The linear sum assignment problem is also known as minimum weight matching in bipartite graphs. A problem instance is described by a matrix C, where each C[i,j] is the cost of matching vertex i of the first partite set (a “worker”) and vertex j of the second set (a “job”). The goal is to find a complete assignment of workers to jobs of minimal cost.

## Result
Short demo of labelling objects in real-time after integrating these two methods:


# Kalman Filter
You can use a Kalman filter in any place where you have uncertain information about some dynamic system, and you can make an educated guess about what the system is going to do next. Even if messy reality comes along and interferes with the clean motion you guessed about, the Kalman filter will often do a very good job of figuring out what actually happened [[1](https://www.bzarg.com/p/how-a-kalman-filter-works-in-pictures/)].

<div style="text-align: center">
  <img src="https://www.bzarg.com/wp-content/uploads/2015/08/kalflow.png" width="400" ></img>
</div>


## Result
Predicting trajectories of two randomly moving objects ([Source code](https://github.com/ymart1n/BonZeb_CSharp/tree/main/KalmanFilterTwoObjectsDemo/Kalman%20Filter), [4]):

Python Kalman Filter tracking human movement ([Source code](https://github.com/ymart1n/BonZeb_CSharp/tree/main/PythonScripts), [5]):

___
In the first half (4 months) of the project, I completed implemented lightweight C# algorithms/programs for real-time tracking​ and prospected the second half of the project in the first presentation to the lab:
- Utilize more Machine Learning models to improve real-time identity tracking when objects overlap​
- Integrate all features together and get a robust tracking software​
___

# YOLOv5
[YOLO](https://github.com/ultralytics/yolov5) is one of the most famous object detection algorithms due to its speed and accuracy. It is a novel convolutional neural network (CNN) that detects objects in real-time with great accuracy [6].

I wrote a [Python script](https://github.com/ymart1n/BonZeb_CSharp/blob/main/yolov5_deepsort/parse_vott.py) to retrive the [annotation data](https://github.com/ymart1n/BonZeb_CSharp/blob/main/yolov5_deepsort/data_vott.csv) in batch (8251 frames, 12 labelled objects in each frame).

![](/Final%20Presentation/data-retriving-pipeline.png)

I used [Roboflow](https://roboflow.com/) to export annotated dataset for training the YOLOv5 object detection model.

![](/Final%20Presentation/roboflow1.png) ![](/Final%20Presentation/roboflow2.png) ![](/Final%20Presentation/roboflow3.png)

## Result
Graph of the trained model performance:
![](/Final%20Presentation/results.png)
(Note: refer to [this article](https://towardsdatascience.com/the-practical-guide-for-object-detection-with-yolov5-algorithm-74c04aac4843) for detail of each graph)
Ground truth augmented training data:
![](/Final%20Presentation/train_batch2.jpg)

# DeepSORT

# Project Overview

# Reference
[1] How a Kalman filter works, in pictures: https://www.bzarg.com/p/how-a-kalman-filter-works-in-pictures/

[2] SciPy official documentations: 
https://docs.scipy.org/doc/scipy/reference/generated/scipy.spatial.distance.cdist.html
https://docs.scipy.org/doc/scipy-0.18.1/reference/generated/scipy.optimize.linear_sum_assignment.html

[3] Linear sum assignment implementation reference: https://github.com/fuglede/linearassignment/blob/master/src/LinearAssignment/ShortestPathSolver.cs

[4] Kalman filter implementation derived from EMGU KF demo: https://www.emgu.com/wiki/index.php/Kalman_Filter

[5] Python Kalman filter implementation derived from ZhangPHEngr's implementation: https://github.com/ZhangPHEngr/Kalman-in-MOT

[6] How to Use Yolo v5 Object Detection Algorithm for Custom Object Detection: https://www.analyticsvidhya.com/blog/2021/12/how-to-use-yolo-v5-object-detection-algorithm-for-custom-object-detection-an-example-use-case/

# Credits