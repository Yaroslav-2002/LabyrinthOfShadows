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

        public void Init()
        {
            View = Object.Instantiate(View);
        }
    }
}