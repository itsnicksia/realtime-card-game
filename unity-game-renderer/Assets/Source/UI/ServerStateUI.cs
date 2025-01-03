using Source;
using TMPro;
using UnityEngine;

public class ServerStateUI : MonoBehaviour
{
    public ServerState serverState;
    public TextMeshProUGUI textMesh;

    // Update is called once per frame
    void Update()
    {
        textMesh.text = $"enemyhp={serverState.enemyHp.Value}";
    }
}
