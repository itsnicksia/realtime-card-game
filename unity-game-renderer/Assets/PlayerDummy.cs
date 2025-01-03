using Unity.Netcode;
using UnityEngine;

public class PlayerDummy : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        GameObject.FindGameObjectWithTag("PlayerUI").SetActive(true);
        Debug.Log("activating player stuff");
        base.OnNetworkSpawn();
    }
}
