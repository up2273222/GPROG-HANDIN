using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StrikeCard : BaseCard
{
    protected override void PopulateEffects()
    {
        if (Definition is StrikeDefinition cardDef)
        {
            int damage = cardDef.strikedamage;
            AddEffect(new DamageEffect(damage));
        }
        else
        {
            Debug.Log("Card definition mismatch");
        }
    }
}
