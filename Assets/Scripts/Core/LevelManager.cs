using Unity.Cinemachine;
using Entities.Player;
using Generation;
using UnityEngine;
using VContainer;

namespace LevelManagement
{
    public class LevelManager : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private CinemachineTargetGroup targetGroup;
        [SerializeField] private MapGenerationManager _worldGenerationManager;
        [SerializeField] private HUDView hudView;
        [Inject] private PlayerGo _playerGo;
        private GameData _gameData;

        public void Start()
        {
            if (_gameData.playerData == null)
            {
                _worldGenerationManager.InitMap();
            }
            SetPlayer(_gameData.playerData);
            hudView.Setup();
        }

        private void SetPlayer(PlayerData playerData)
        {
            Vector3 pawnPos = playerData == null ? _worldGenerationManager.GetPLayerSpawnPosition() : new Vector3(playerData.x, playerData.y, 0);
            _playerGo.Init(pawnPos);
            targetGroup.AddMember(_playerGo.View.transform, 2, 2);

            if (playerData != null)
            {
                _playerGo.View.GetComponent<Health>().currentHealth = playerData.health;
                _playerGo.View.GetComponent<PlayerController>().damage = playerData.damage;
            }
        }

        public void LoadData(GameData data)
        {
            _gameData = data;
        }

        public void SaveData(GameData data)
        {
            var player = _playerGo.View;
            data.playerData = player.GetComponent<PlayerController>().GetPlayerData();
        }
    }
}