using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public float speed = 20;

    private ClientNetworkTransform _transform;

    private void Start()
    {
        _transform = GetComponent<ClientNetworkTransform>();
    }
    private void Update()
    {
        if (!IsOwner) return;
        var moveVec2 = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        var movement = new Vector3(moveVec2.x, moveVec2.y, 0);

        _transform.transform.position += movement * speed * Time.deltaTime;
    }

}
