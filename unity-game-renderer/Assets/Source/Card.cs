using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source
{

    public class Card : MonoBehaviour
    {
        public DevModeRpc devModeRpc;
        public Player player;
        public int index;
        private TextMeshProUGUI _label;

        private void Start()
        {
            _label = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (player is not null)
            {
                _label.text = $"cardId: {player.card1.Value}";
            }
        }

        public void Activate()
        {
            devModeRpc.HurtEnemyRpc(5);
            devModeRpc.HurtEnemyRpc(5);
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }
    }
}
