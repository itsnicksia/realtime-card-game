using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class ConnectedHack : NetworkBehaviour
{
    private bool _hasNotifiedSessionManagerOfConnection = false;

    private void Start()
    {

    }

    void Update()
    {
        if (_hasNotifiedSessionManagerOfConnection)
        {
            GameObject.FindGameObjectWithTag("GameState").GetComponent<SessionManager>().NotifyConnectedHack();
            _hasNotifiedSessionManagerOfConnection = true;
        }
    }

}
