using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Source
{
    public class Host : NetworkBehaviour
    {
        public NetworkVariable<int> enemyHp = new(1_000_000);

        public float timeUntilNextDraw;

        void Update()
        {
            if (HasAuthority)
            {
                RunServerLogic();
            }

        }

        private void RunServerLogic()
        {
            // FIXME: Move to some kind of network-safe timer.
            timeUntilNextDraw -= Time.deltaTime;
            if (timeUntilNextDraw <= 0f)
            {
                // TODO: Well, each player should draw their own card, based on server authority.
                GetPlayerStates()
                    .ToList()
                    .ForEach(ps => ps.DrawCard());
                timeUntilNextDraw = 5f;
            }
        }

        public void HurtEnemy(int amount)
        {
            enemyHp.Value -= amount;
        }

        private IEnumerable<Player> GetPlayerStates()
        {
            return GameObject
                .FindGameObjectsWithTag("Player")
                .Select(go => go.GetComponent<Player>());
        }
    }
}
