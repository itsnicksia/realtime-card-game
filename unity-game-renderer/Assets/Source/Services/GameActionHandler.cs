using Unity.Netcode;
using UnityEngine;

namespace Source
{
    public class GameActionHandler : NetworkBehaviour
    {
        public ServerState serverState;
        public GameObject playerUI;

        [Rpc(SendTo.Server)]
        public void HurtEnemyRpc(int amount)
        {
            serverState.HurtEnemy(amount);
        }
    }
}
