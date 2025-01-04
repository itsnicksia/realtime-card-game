using Source;
using TMPro;
using UnityEngine;

public class ServerStateUI : MonoBehaviour
{
    public Server server;
    public TextMeshProUGUI textMesh;

    // Update is called once per frame
    void Update()
    {
        textMesh.text = $"enemyhp={server.enemyHp.Value}";
    }
}
