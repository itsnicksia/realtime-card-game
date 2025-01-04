using Unity.Netcode;
using UnityEngine;

namespace Source
{
    public class DevModeRpc : NetworkBehaviour
    {
        public Server serverState;
        public GameObject playerUI;

        [Rpc(SendTo.Server)]
        public void HurtEnemyRpc(int amount)
        {
            serverState.HurtEnemy(amount);
        }
    }
}
