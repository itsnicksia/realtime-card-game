using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Source
{
    public class Server : NetworkBehaviour
    {
        public NetworkVariable<int> enemyHp = new(1_000_000);

        private float _timeUntilNextDraw;

        void Update()
        {
            _timeUntilNextDraw -= NetworkManager.Singleton.LocalTime.FixedDeltaTime;
            if (HasAuthority && _timeUntilNextDraw <= 0f)
            {
                // TODO: Well, each player should draw their own card, based on server authority.
                GetPlayerStates()
                    .ToList()
                    .ForEach(ps => ps.DrawCard());
                _timeUntilNextDraw = 5f;
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
