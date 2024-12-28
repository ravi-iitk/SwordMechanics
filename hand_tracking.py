import cv2
import mediapipe as mp
import socket
import json

mp_hands = mp.solutions.hands
hands = mp_hands.Hands(min_detection_confidence=0.9, min_tracking_confidence=0.9)  # Increased confidence
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server_address = ("127.0.0.1", 12345)

# Initialize video capture (camera)
cap = cv2.VideoCapture(0)

# Check if the camera is accessible
if not cap.isOpened():
    print("Error: Could not access the camera")
    exit()

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        print("Error: Failed to capture frame")
        break

    frame = cv2.flip(frame, 1)  # Flip the frame horizontally (mirror view)
    rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)  # Convert to RGB for Mediapipe processing

    # Process the frame for hand tracking
    results = hands.process(rgb_frame)

    # Check if any hands were detected
    if results.multi_hand_landmarks:
        print("Hands detected!")
        hand_data = {"left": None, "right": None}
        for hand_landmarks, hand_classification in zip(results.multi_hand_landmarks, results.multi_handedness):
            label = hand_classification.classification[0].label
            # Get wrist coordinates
            cx, cy = hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].x, hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].y
            if label == "Left":
                hand_data["left"] = {"x": cx, "y": cy}
            elif label == "Right":
                hand_data["right"] = {"x": cx, "y": cy}
        
        # Send the hand data over UDP
        sock.sendto(json.dumps(hand_data).encode(), server_address)
    else:
        print("No hands detected")

    # Draw landmarks if hands are detected
    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            mp.solutions.drawing_utils.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)

    # Show the frame with hand landmarks drawn (for debugging)
    cv2.imshow("Hand Tracking", frame)

    # Exit the loop when 'q' is pressed
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release resources
cap.release()
cv2.destroyAllWindows()