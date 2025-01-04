using Source;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ServerStateUI : MonoBehaviour
{
    [FormerlySerializedAs("server")] public Host host;
    public TextMeshProUGUI textMesh;

    // Update is called once per frame
    void Update()
    {
        textMesh.text = $"enemyhp={host.enemyHp.Value}";
    }
}
