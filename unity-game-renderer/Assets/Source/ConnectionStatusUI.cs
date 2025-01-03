using TMPro;
using UnityEngine;

public class ConnectionStatusUI : MonoBehaviour
{
    public SessionManager sessionManager;
    public TextMeshProUGUI textMesh;

    // Update is called once per frame
    void Update()
    {
        textMesh.text = $"connection status={sessionManager.statusString}";
    }
}
