# BonZeb CSharp — CSCD94 Project
  Computer Science Project - Fall 2021/Winter 2022: Machine Learning in real-time analysis of multi-animal tracking experiments
![](/BonZeb-Miro.jpg)

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

I re-wrote the key tracking methods [cdist](https://docs.scipy.org/doc/scipy/reference/generated/scipy.spatial.distance.cdist.html) and [linear_sum_assignment](https://docs.scipy.org/doc/scipy-0.18.1/reference/generated/scipy.optimize.linear_sum_assignment.html) using C# ([cDist](https://github.com/ymart1n/BonZeb_CSharp/blob/ed6bf23d46f0fe3f00d363c65e7134ff64846346/C%23/multi-animal-tracking.cs#L27), [LinearSumAssignment](https://github.com/ymart1n/BonZeb_CSharp/blob/ed6bf23d46f0fe3f00d363c65e7134ff64846346/C%23/multi-animal-tracking.cs#L69)) and integrated them into BonZeb​

## [cDist](https://github.com/ymart1n/BonZeb_CSharp/blob/ed6bf23d46f0fe3f00d363c65e7134ff64846346/C%23/multi-animal-tracking.cs#L27)


# Kalman Filter

# YOLOv5

# DeepSORT

# Project Overview

# Reference


# Credits