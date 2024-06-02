using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private DynamicJoystick joystick;
    public static DynamicJoystick Joystick { get; set; }
}
