using Entities.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private float activationDelay; 
    [SerializeField] private int damage = 20; 
    [SerializeField] private float damageInterval = 1.0f;
    [SerializeField] private Animator animator;

    private bool isActive = false;

    private void Awake()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "SpikesAnimation")
            {
                activationDelay = clip.length;
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            isActive = true;
            StartCoroutine(ActivateTrap(other));
        }
    }

    private IEnumerator ActivateTrap(Collider2D player)
    {
        while (isActive)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.GetHit(damage, gameObject);
            }


            if (animator != null)
            {
                animator.SetTrigger("Activate");
            }
            yield return new WaitForSeconds(activationDelay * 2);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false;
            StopAllCoroutines();
        }
    }
}
