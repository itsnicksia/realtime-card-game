using System;
using Unity.Netcode;
using UnityEngine;

namespace Source
{
    public class Player : NetworkBehaviour
    {
        public NetworkVariable<int> playerHp = new(1_000_000);

        // TODO: NetworkSerializable list type for cards? Just do 5 cards for now.
        public NetworkVariable<int> card1 = new(-1);

        private void Start()
        {
            card1.OnValueChanged += OnValueChanged;
        }



        public void DrawCard()
        {
            card1.Value = 1;
        }

        private void OnValueChanged(int previousValue, int newValue)
        {
            Debug.Log($"Drew card: {newValue}");
        }

        public override void OnNetworkSpawn()
        {
            var playerUI = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>();
            playerUI.SetPlayer(this);
            Debug.Log("activating player stuff");
            base.OnNetworkSpawn();
        }
    }
}
