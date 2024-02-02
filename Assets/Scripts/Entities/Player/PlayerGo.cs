using UnityEngine;

namespace Entities.Player
{
    public class PlayerGo : IEntityGo
    {
        public GameObject View { get; private set; }

        public PlayerGo(GameObject playerPrefab) : base()
        {
            View = playerPrefab;
        }

        public void Init()
        {
            View = Object.Instantiate(View);
        }
    }
}