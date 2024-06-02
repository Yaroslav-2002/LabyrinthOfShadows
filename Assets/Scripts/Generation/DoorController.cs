using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(Collider2D))]
public class DoorController: MonoBehaviour
{
    private Collider2D _collider2D;
    private Animator _animator;

    private bool _isOpened;
    public bool IsOpened
    {
        get
        {
            return _isOpened;
        }
        private set
        {
            _isOpened = value;
        }
    }

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isOpened)
        {
            return;
        }
        if (collision.GetComponent<PlayerController>().HasKey)
        {
            Open();
        }
    }

    public void Open()
    {
        IsOpened = true;
        _collider2D.enabled = false;
        _animator.SetBool("Open", true);
    }
}