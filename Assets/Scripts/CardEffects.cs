using UnityEngine;

public class CardEffects : MonoBehaviour
{
    public static void Critical(Card cardFrom, Card cardTo)
    {
        int defaultDamage = 1;
        cardTo.DealDamage(defaultDamage * 2);
    }
}