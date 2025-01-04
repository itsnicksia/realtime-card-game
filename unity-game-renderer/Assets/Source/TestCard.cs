using UnityEngine;
using UnityEngine.Serialization;

namespace Source
{

    public class TestCard : MonoBehaviour
    {
        [FormerlySerializedAs("playerActionHandler")] [FormerlySerializedAs("gameActionHandler")] public DevModeRpc devModeRpc;

        public void Activate()
        {
            devModeRpc.HurtEnemyRpc(5);
            devModeRpc.HurtEnemyRpc(5);
        }
    }
}
