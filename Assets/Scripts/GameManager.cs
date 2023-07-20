using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private void Start()
        {
            Instantiate(player, new Vector2(-1f, -2.5f), Quaternion.Euler(0,0,0));
        }
    }
}