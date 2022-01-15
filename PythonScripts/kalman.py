# -*- coding: UTF-8 -*-
"""
@Project ：kalman-filter-in-single-object-tracking 
@File ：kalman.py
@Date ：9/15/21 3:36 PM 
"""
import random
import numpy as np
import utils
from matcher import Matcher

GENERATE_SET = 1  # set the trajectory starting frame
TERMINATE_SET = 7  # set the trajectory ending frame


class Kalman:
    def __init__(self, A, B, H, Q, R, X, P):
        # fix arguments
        self.A = A  # state transistion matrix
        self.B = B  # control matrix
        self.H = H  # measurement matrix
        self.Q = Q  # process noise covariance matrix
        self.R = R  # measurement noise covariance matrix
        # iternation variables
        self.X_posterior = X  # posterior state, defined as [center x,center y, width w, height h, dx, dy]
        self.P_posterior = P  # posterior state srror matrix
        self.X_prior = None  # prior state
        self.P_prior = None  # prior state error matrix
        self.K = None  # kalman gain
        self.Z = None  # measurement, defined as [center x, center y, width w, height h]
        # starting and ending strategy
        self.terminate_count = TERMINATE_SET
        # save the trajectory
        self.track = []  # save current trajectory [(p1_x,p1_y),()]
        self.track_color = (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255))
        self.__record_track()

    def predict(self):
        """
        Make prediction
        :return:
        """
        self.X_prior = np.dot(self.A, self.X_posterior)
        self.P_prior = np.dot(np.dot(self.A, self.P_posterior), self.A.T) + self.Q
        return self.X_prior, self.P_prior

    @staticmethod
    def association(kalman_list, mea_list):
        """
        Data association: Associate different objects, using maximum weighted matching
        :param kalman_list: state list，storing each Kalman object，which has already finished prediction
        :param mea_list: measurement list，storing object measurement in the form of matrix, ndarray [c_x, c_y, w, h].T
        :return:
        """
        # recording states and measurements that needed to match
        state_rec = {i for i in range(len(kalman_list))}
        mea_rec = {i for i in range(len(mea_list))}

        # converting state object for better matching with measurement object
        state_list = list()  # [c_x, c_y, w, h].T
        for kalman in kalman_list:
            state = kalman.X_prior
            state_list.append(state[0:4])

        # matching to get a matching dictionary
        match_dict = Matcher.match(state_list, mea_list)

        # based on the matching dictionary, updating matched pairs of state and measurement, return un-matched pairs
        state_used = set()
        mea_used = set()
        match_list = list()
        for state, mea in match_dict.items():
            state_index = int(state.split('_')[1])
            mea_index = int(mea.split('_')[1])
            match_list.append([utils.state2box(state_list[state_index]), utils.mea2box(mea_list[mea_index])])
            kalman_list[state_index].update(mea_list[mea_index])
            state_used.add(state_index)
            mea_used.add(mea_index)

        # calculate un-matched state and measurement pairs and return them
        return list(state_rec - state_used), list(mea_rec - mea_used), match_list

    def update(self, mea=None):
        """
        finishing kalman filter once
        :param mea:
        :return:
        """
        status = True
        if mea is not None:  # if the measurement is matched with a state
            self.Z = mea
            self.K = np.dot(np.dot(self.P_prior, self.H.T),
                            np.linalg.inv(np.dot(np.dot(self.H, self.P_prior), self.H.T) + self.R))  # calculate kalman gain
            self.X_posterior = self.X_prior + np.dot(self.K, self.Z - np.dot(self.H, self.X_prior))  # update posterior estimation
            self.P_posterior = np.dot(np.eye(6) - np.dot(self.K, self.H), self.P_prior)  # update posterior error matrix
            status = True
        else: # if the measurement is NOT matched with a state
            if self.terminate_count == 1:
                status = False
            else:
                self.terminate_count -= 1
                self.X_posterior = self.X_prior
                self.P_posterior = self.P_prior
                status = True
        if status:
            self.__record_track()

        return status, self.X_posterior, self.P_posterior

    def __record_track(self):
        self.track.append([int(self.X_posterior[0]), int(self.X_posterior[1])])


if __name__ == '__main__':
    # state transistion matrix, transiting prior state into current state
    A = np.array([[1, 0, 0, 0, 1, 0],
                  [0, 1, 0, 0, 0, 1],
                  [0, 0, 1, 0, 0, 0],
                  [0, 0, 0, 1, 0, 0],
                  [0, 0, 0, 0, 1, 0],
                  [0, 0, 0, 0, 0, 1]])
    # control input matrix B
    B = None
    # process noise covariance matrix Q，p(w)~N(0,Q)，noise comes from uncertainty in real world,
    # in tracking process，process noise comes from the uncertainty of objects' movement (sudden increase/decrease of speed, sharp turns, etc.)
    Q = np.eye(A.shape[0]) * 0.1
    # mesurement matrix
    H = np.array([[1, 0, 0, 0, 0, 0],
                  [0, 1, 0, 0, 0, 0],
                  [0, 0, 1, 0, 0, 0],
                  [0, 0, 0, 1, 0, 0]])
    # measurement noise covariance matrix R，p(v)~N(0,R)
    # measurement noise comes from object overlapping or detection loss, etc
    R = np.eye(H.shape[0]) * 1
    # initialization of state covariance matrix P
    P = np.eye(A.shape[0])

    box = [729, 238, 764, 339]
    X = utils.box2state(box)

    k1 = Kalman(A, B, H, Q, R, X, P)
    print(k1.predict())

    mea = [730, 240, 766, 340]
    mea = utils.box2meas(mea)
    print(k1.update(mea))
