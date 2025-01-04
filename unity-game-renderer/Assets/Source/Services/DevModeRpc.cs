using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source
{
    public class DevModeRpc : NetworkBehaviour
    {
        [FormerlySerializedAs("serverState")] public Host hostState;
        public GameObject playerUI;

        [Rpc(SendTo.Server)]
        public void HurtEnemyRpc(int amount)
        {
            hostState.HurtEnemy(amount);
        }
    }
}
