using UnityEngine;

public abstract class View : MonoBehaviour
{
    public abstract void Initialize();
    public void Show() => gameObject.SetActive(false);
    public void Hide() => gameObject.SetActive(false);
}