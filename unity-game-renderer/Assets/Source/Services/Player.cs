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

        private void OnValueChanged(int previousvalue, int newvalue)
        {
            Debug.Log($"Drew card: {newvalue}");
        }

        public void DrawCard()
        {
            card1.Value = 1;
        }

        public override void OnNetworkSpawn()
        {
            GameObject.FindGameObjectWithTag("PlayerUI").SetActive(true);
            Debug.Log("activating player stuff");
            base.OnNetworkSpawn();
        }
    }
}
