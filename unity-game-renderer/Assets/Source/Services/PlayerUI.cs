using System.Linq;
using Source;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public Card[] cards;

    public void SetPlayer(Player player)
    {
        gameObject.SetActive(true);
        cards.ToList().ForEach(card => card.SetPlayer(player));
    }
}
