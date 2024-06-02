using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class HUDView : View
{
    [Inject] PlayerGo player;
    [SerializeField] private Button menuButton;
    [SerializeField] private Slider HealthBar;

    public override void Initialize()
    {
        menuButton.onClick.AddListener(() => ViewManager.Show<GameMenuView>());
        HealthBar.value = player.View.GetComponent<Health>().maxHealth / 100;
    }
    
    public void Setup()
    {
        player.View.GetComponent<Health>().OnHitWithReference += UpdateHealthBar;
    }

    private void UpdateHealthBar(float health)
    {
        HealthBar.value = health;
    }
}
