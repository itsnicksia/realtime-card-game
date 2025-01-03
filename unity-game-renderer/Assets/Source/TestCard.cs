using UnityEngine;

namespace Source
{

    public class TestCard : MonoBehaviour
    {
        public GameActionHandler gameActionHandler;

        public void Activate()
        {
            gameActionHandler.HurtEnemyRpc(5);
            gameActionHandler.HurtEnemyRpc(5);
        }
    }
}
