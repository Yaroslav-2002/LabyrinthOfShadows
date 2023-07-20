using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Store the start and current positions for each touch
    private Dictionary<int, Vector2> startTouchPositions = new Dictionary<int, Vector2>();
    private Dictionary<int, Vector2> currentTouchPositions = new Dictionary<int, Vector2>();

    void Update()
    {
        // Iterate over all current touches
        foreach (Touch touch in Input.touches)
        {
            // Check the phase of the touch
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // The touch just started, store the initial position
                    startTouchPositions[touch.fingerId] = touch.position;
                    currentTouchPositions[touch.fingerId] = touch.position;
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    // The touch is ongoing, update the current position
                    currentTouchPositions[touch.fingerId] = touch.position;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // The touch has ended, remove it from the dictionaries
                    startTouchPositions.Remove(touch.fingerId);
                    currentTouchPositions.Remove(touch.fingerId);
                    break;
            }
        }
    }

    // You can add methods to access the touch positions, for example:
    public Vector2 GetStartTouchPosition(int fingerId)
    {
        Debug.Log(fingerId);
        return startTouchPositions.TryGetValue(fingerId, out var position) ? position : Vector2.zero;
    }

    public Vector2 GetCurrentTouchPosition(int fingerId)
    {
        return currentTouchPositions.TryGetValue(fingerId, out var position) ? position : Vector2.zero;
    }
}