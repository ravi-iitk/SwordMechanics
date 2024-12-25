import cv2
import mediapipe as mp
import numpy as np
import socket

def send_data(data,host = "127.0.0.1", port=25001):
    try:
        sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        sock.sendto(data.encode("utf-8"),(host,port))
    except Exception as e:
        print(f"Error: {e}")
def get_angle(a, b):
    angle=np.degrees(np.arctan2((b.y-a.y),b.x-a.x))
    return angle


def get_angle(a, b): #function to calculate angle between two points
    angle = np.degrees(np.arctan2((-b.y + a.y), b.x - a.x))
    return angle

mp_hands = mp.solutions.hands
mp_drawing = mp.solutions.drawing_utils

cap = cv2.VideoCapture(0)

hands = mp_hands.Hands(
    static_image_mode=False,
    max_num_hands=2, # Change to 2 if you want to detect two hands
    min_detection_confidence = 0.5,
    min_tracking_confidence=0.5
)

while cap.isOpened():
    success, frame = cap.read()
    if not success:
        print(" ignoring empty camera frame.")
        continue

    frame = cv2.resize(frame, (320, 240))
    frame = cv2.flip(frame, 1)
    frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

    results = hands.process(frame_rgb)
    if results.multi_hand_landmarks:
        data_list = []
        for i, hand_landmarks in enumerate(results.multi_hand_landmarks):
            mp_drawing.draw_landmarks(frame,hand_landmarks, mp_hands.HAND_CONNECTIONS)
            handedness_label = results.multi_handedness[i].classification[0].label

            index_mcp = hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_MCP]
            pinky_mcp = hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_MCP]

            handposx = (index_mcp.x + pinky_mcp.x) / 2
            handposy = (index_mcp.y + pinky_mcp.y) / 2
            angle = get_angle(index_mcp, pinky_mcp) #angle between index and pinky finger

            data_list.extend([handedness_label, handposx, handposy, angle]) #data_list contains the hand data

        if(len(data_list) < 8):                #if only one hand is detected
            if data_list[0] == "Left":
                data_list.extend(["Right", 0, 0, 0])
            else:
                data_list.extend(["Left", 0, 0, 0])

        data = " ".join(map(str,data_list))
        # print(data)
        send_data(data)

    cv2.imshow("Hand Tracking", frame)
    if cv2.waitKey(5) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
            
            
