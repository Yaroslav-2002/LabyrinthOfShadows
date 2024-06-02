using UnityEngine;

namespace Entities.Player
{
    public class PlayerGo
    {
        public GameObject View { get; private set; }

        public PlayerGo(GameObject playerPrefab)
        {
            View = playerPrefab;
        }

        public void Init(Vector3 pos)
        {
            View = Object.Instantiate(View, pos, Quaternion.identity);
        }
    }
}