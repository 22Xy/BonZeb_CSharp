# -*- coding: utf-8 -*-
"""
@Project: kalman-filter-in-single-object-tracking
@File   : main.py
@Author : Zhang P.H
@Date   : 2021/9/20
@Desc   :
"""
import cv2
import numpy as np
import const
import utils
import measure
from kalman import Kalman

# --------------------------------Kalman Configuration---------------------------------------
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
# -------------------------------------------------------------------------------


def main():
    # 1. Loading videos and object location data
    cap = cv2.VideoCapture(const.VIDEO_PATH)
    meas_list_all = measure.load_measurement(const.FILE_DIR)
    sz = (int(cap.get(cv2.CAP_PROP_FRAME_WIDTH)),
          int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT)))
    fourcc = cv2.VideoWriter_fourcc('m', 'p', '4', 'v')  # opencv3.0
    video_writer = cv2.VideoWriter(const.VIDEO_OUTPUT_PATH, fourcc, const.FPS, sz, True)
    # 2. filtering each frame
    state_list = []  # state information in ONE frame, storing Kalman object
    frame_cnt = 1
    for meas_list_frame in meas_list_all:
        # --------------------------------------------Load current frame image------------------------------------
        ret, frame = cap.read()
        if not ret:
            break

        # ---------------------------------------Kalman Filter for multi-objects-------------------
        # prediction
        for target in state_list:
            target.predict()
        # association
        mea_list = [utils.box2meas(mea) for mea in meas_list_frame]
        state_rem_list, mea_rem_list, match_list = Kalman.association(state_list, mea_list)
        # for un-matched state, update the list, if it triggers termination, then delete it
        state_del = list()
        for idx in state_rem_list:
            status, _, _ = state_list[idx].update()
            if not status:
                state_del.append(idx)
        state_list = [state_list[i] for i in range(len(state_list)) if i not in state_del]
        # for un-matched measurement, make it a new starting point of a trajectory
        for idx in mea_rem_list:
            state_list.append(Kalman(A, B, H, Q, R, utils.mea2state(mea_list[idx]), P))

        # -----------------------------------------------Visualization-----------------------------------
        # displaying every measurement
        for mea in meas_list_frame:
            cv2.rectangle(frame, tuple(mea[:2]), tuple(mea[2:]), const.COLOR_MEA, thickness=1)
        # displaying every state
        for kalman in state_list:
            pos = utils.state2box(kalman.X_posterior)
            cv2.rectangle(frame, tuple(pos[:2]), tuple(pos[2:]), const.COLOR_STA, thickness=2)
        # draw matching association
        for item in match_list:
            cv2.line(frame, tuple(item[0][:2]), tuple(item[1][:2]), const.COLOR_MATCH, 3)
        # draw trajectories
        for kalman in state_list:
            tracks_list = kalman.track
            for idx in range(len(tracks_list) - 1):
                last_frame = tracks_list[idx]
                cur_frame = tracks_list[idx + 1]
                # print(last_frame, cur_frame)
                cv2.line(frame, last_frame, cur_frame, kalman.track_color, 2)

        cv2.putText(frame, str(frame_cnt), (0, 50), color=const.RED, fontFace=cv2.FONT_HERSHEY_SIMPLEX, fontScale=1.5)
        cv2.imshow('Demo', frame)
        cv2.imwrite("./image/{}.jpg".format(frame_cnt), frame)
        video_writer.write(frame)
        cv2.waitKey(100)  # displaying for 1000ms (1s)
        frame_cnt += 1

    cap.release()
    cv2.destroyAllWindows()
    video_writer.release()


if __name__ == '__main__':
    main()
