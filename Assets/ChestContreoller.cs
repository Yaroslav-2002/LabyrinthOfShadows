using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestContreoller : MonoBehaviour
{
    [SerializeField] Sprite ChestOpendSprite;

    private SpriteRenderer _spriteRenderer;
    private bool _isChestOpened;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsChestOpened
    {
        get
        {
            return _isChestOpened;
        }
        private set
        {
            _isChestOpened = value;
        }
    }

    public void OpenChest()
    {
        if (IsChestOpened)
        {
            IsChestOpened = true;
            _spriteRenderer.sprite = ChestOpendSprite;
        }
    }
}
