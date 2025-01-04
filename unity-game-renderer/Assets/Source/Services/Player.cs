using Unity.Netcode;
using UnityEngine;

namespace Source
{
    public class Player : NetworkBehaviour
    {
        public void DrawCard()
        {
            Debug.Log("Draw Card!!");
        }
    }
}
