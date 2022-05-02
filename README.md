# BonZeb CSharp — CSCD94 Project
  Computer Science Project - Fall 2021/Winter 2022:
  Machine Learning in real-time analysis of multi-animal tracking experiments
  
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
Goal: Rewrote tracking algorithms in C# (Bonsai) to make the project lightweight

Since we want good real-time tracking experience, I have to keep the application as lightweight as possible. I need to program in C# which is the language used in Bonsai. However, I cannot just import methods from off-the-shelf C# libraries since it will import other unnecessary content and violate our goal to keep the software lightweight.

I re-wrote the key tracking methods [cdist](https://docs.scipy.org/doc/scipy/reference/generated/scipy.spatial.distance.cdist.html) and [linear_sum_assignment](https://docs.scipy.org/doc/scipy-0.18.1/reference/generated/scipy.optimize.linear_sum_assignment.html) using C# and integrated them into BonZeb

- [cDist](https://github.com/ymart1n/BonZeb_CSharp/blob/3272d82fb6e031bc9ac36ca9d650d6abf6ee99d0/CSharp/multi-animal-tracking.cs#L27)
  - double[,] cDist(double[][] XA, double[][] XB) computes the distance between m points using Euclidean distance (2-norm) as the distance metric between the points. The points are arranged as m n-dimensional row vectors in the matrix X.

- [LinearSumAssignment](https://github.com/ymart1n/BonZeb_CSharp/blob/3272d82fb6e031bc9ac36ca9d650d6abf6ee99d0/CSharp/multi-animal-tracking.cs#L69)
  - The *LinearSumAssignment* method solves the linear sum assignment problem (using Hungarian method). The linear sum assignment problem is also known as minimum weight matching in bipartite graphs. A problem instance is described by a matrix C, where each C[i,j] is the cost of matching vertex i of the first partite set (a “worker”) and vertex j of the second set (a “job”). The goal is to find a complete assignment of workers to jobs of minimal cost.

## Result
- Short demo of labelling objects in real-time after integrating these two methods:


https://user-images.githubusercontent.com/56213581/166180961-183f3af4-a46b-499a-b132-3f8d8b3e6b21.mp4


# Kalman Filter
You can use a Kalman filter in any place where you have uncertain information about some dynamic system, and you can make an educated guess about what the system is going to do next. Even if messy reality comes along and interferes with the clean motion you guessed about, the Kalman filter will often do a very good job of figuring out what actually happened [1].

<div align="center">
  <img src="https://www.bzarg.com/wp-content/uploads/2015/08/kalflow.png" width="400" ></img>
</div>


## Result
- Predicting trajectories of two randomly moving objects ([Source code](https://github.com/ymart1n/BonZeb_CSharp/tree/main/KalmanFilterTwoObjectsDemo/Kalman%20Filter), [4]):

https://user-images.githubusercontent.com/56213581/166180964-3446e92d-45f5-4d40-a9b6-ebccc904e369.mp4


- Python Kalman Filter tracking human movement ([Source code](https://github.com/ymart1n/BonZeb_CSharp/tree/main/PythonScripts), [5]):

https://user-images.githubusercontent.com/56213581/166180919-b333f667-f108-4109-82f1-d7fd3091c7c3.mp4

___
In the first half (4 months) of the project, I completed implemented lightweight C# algorithms/programs for real-time tracking and prospected the second half of the project in the first presentation to the lab:
- Utilize more Machine Learning models to improve real-time identity tracking when objects overlap
- Integrate all features together and get a robust tracking software
___

# YOLOv5
[YOLO](https://github.com/ultralytics/yolov5) is one of the most famous object detection algorithms due to its speed and accuracy. It is a novel convolutional neural network (CNN) that detects objects in real-time with great accuracy [6].

I wrote a [Python script](https://github.com/ymart1n/BonZeb_CSharp/blob/main/yolov5_deepsort/parse_vott.py) to retrive the [annotation data](https://github.com/ymart1n/BonZeb_CSharp/blob/main/yolov5_deepsort/data_vott.csv) in batch (8251 frames, 12 labelled objects in each frame).

![](/Final%20Presentation/data-retriving-pipeline.png)

I used [Roboflow](https://roboflow.com/) to export annotated dataset for training the YOLOv5 object detection model to detect zebrafish in each image/frame.

![](/Final%20Presentation/roboflow1.png) ![](/Final%20Presentation/roboflow2.png) ![](/Final%20Presentation/roboflow3.png)

## Result
- Graph of the trained model performance:
![](/Final%20Presentation/results.png)
(Note: refer to [this article](https://towardsdatascience.com/the-practical-guide-for-object-detection-with-yolov5-algorithm-74c04aac4843) for detail of each graph)

- Ground truth augmented training data:
![](/Final%20Presentation/train_batch2.jpg)

# YOLOv5 + DeepSORT

[DeepSORT](https://github.com/nwojke/deep_sort) is an algorithm for tracking that extends Simple Online and Real-time Tracking (SORT) with a Deep Association Metric. 
![](/Final%20Presentation/deepsort1.png)
DeepSORT uses a custom deep convolutional neural network (CNN) as the appearance descriptor (identifying each unique object) [7].
![](/Final%20Presentation/deepsort2.png)
(Image Credit: [DeepSORT — Deep Learning applied to Object Tracking](https://medium.com/augmented-startups/deepsort-deep-learning-applied-to-object-tracking-924f59f99104) [8])

## Result
Using the implementation of [Yolov5 + Deep Sort with OSNet](https://github.com/mikel-brostrom/Yolov5_DeepSort_OSNet) [9] and my trained YOLOv5 model, we obtained the following result: 

https://user-images.githubusercontent.com/56213581/166181075-fae61442-6bbb-4e51-a71f-99870fc01127.mp4

(Note: the detection model (YOLOv5) was trained with a dataset of 12736 annotated images (after augmenting 7749 original frames​ extracted from a video) and the DeepSORT model is a pre-trained model -- osnet_x0_5_market1501).


# Project Overview

## Completed
- Lightweight C# algorithms/programs for real-time tracking
- Trained Machine Learning models to detect and track zebrafish in real-time
- Integrate detection and tracking features together and get a tracking software​

## What can be improved?
- For YOLOv5
  - Better Hardware (GPU or on the cloud)​
    - 35.665s to detect 778 images​
    - 0.04 second inference time​
    - Could done better if accelerated with better GPU (current testing GPU is Tesla K80, which is provided by Google Colab for free)​
  - More robust dataset​
    - Objects are labelled correctly when they are overlapping​
    - More images and videos for training​
    - Using idtracker.ai or some other fish tracking algorithms to obtain dataset for training​
  - Train other YOLOv5 models (e.g., YOLOv5x6, YOLOv5l6, due to limited time and resource, I chose to train YOLOv5s6) with better performance ![](/Final%20Presentation/diff-yolo-models.png)

- For DeepSORT
  - Train a deep association metric model that learns zebrafish appearance 
    - for identifying each zebrafish based on their appearance​ (P.S. Though it seems there is no much appearance feature we can get for each zebrafish (they look alike), technically speaking, the high resolution cameras in the lab provides enough information for identification)
    - according to the [DeepSORT GitHub repo](https://github.com/nwojke/deep_sort), we can use a novel [cosine metric learning](https://github.com/nwojke/cosine_metric_learning) approach to train a deep association metric model on a custom dataset (annotated zebrafish images, in our case)

## Prospect
- Explore & study other tracking algorithms (e.g., [T-Rex](https://github.com/mooch443/trex), [xyTracker](https://github.com/maljoras/xyTracker)) to learn how they implemented real-time tracking (mostly in C++ and MATLAB).

# Reference
[1] How a Kalman filter works, in pictures: https://www.bzarg.com/p/how-a-kalman-filter-works-in-pictures/

[2] SciPy official documentations: 
https://docs.scipy.org/doc/scipy/reference/generated/scipy.spatial.distance.cdist.html
https://docs.scipy.org/doc/scipy-0.18.1/reference/generated/scipy.optimize.linear_sum_assignment.html

[3] Linear sum assignment implementation reference: https://github.com/fuglede/linearassignment/blob/master/src/LinearAssignment/ShortestPathSolver.cs

[4] Kalman filter implementation derived from EMGU KF demo: https://www.emgu.com/wiki/index.php/Kalman_Filter

[5] Python Kalman filter implementation derived from ZhangPHEngr's implementation: https://github.com/ZhangPHEngr/Kalman-in-MOT

[6] How to Use Yolo v5 Object Detection Algorithm for Custom Object Detection: https://www.analyticsvidhya.com/blog/2021/12/how-to-use-yolo-v5-object-detection-algorithm-for-custom-object-detection-an-example-use-case/

[7] DeepSORT original paper: https://arxiv.org/abs/1703.07402

[8] DeepSORT — Deep Learning applied to Object Tracking https://medium.com/augmented-startups/deepsort-deep-learning-applied-to-object-tracking-924f59f99104

[9] Yolov5 + Deep Sort with OSNet https://github.com/mikel-brostrom/Yolov5_DeepSort_OSNet

# Credits
Big thanks to Prof. Tod R. Thiele and Prof. Anya Tafliovich for their supervision and providing such a valuable opportunity.

Special thanks to Dr. Nicholas Guilbeault for his continuous support and help in the last 8 months. I learned a lot when we were collaboratively working on this project.

Thanks to all Thiele Lab members for asking insightful questions and showing support for my presentation.

Thanks to the authors/organizations referred in the reference section for helpful resource and information.