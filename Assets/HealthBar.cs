using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetValue(int health)
    {
        slider.value = health;
    }
}
