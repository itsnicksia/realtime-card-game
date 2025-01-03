using System;
using Unity.Netcode;
using UnityEngine;

namespace Source
{
    public class ServerState : NetworkBehaviour
    {
        public NetworkVariable<int> enemyHp = new(1_000_000);

        public void HurtEnemy(int amount)
        {
            enemyHp.Value -= amount;
        }
    }
}
