using UnityEngine;

public class ParryCard : BaseCard
{
    protected override void PopulateEffects()
    {
        if (Definition is ParryDefinition cardDef)
        {
            int block = cardDef.blockGained;
            int damage = cardDef.parryDamage;
            
            AddEffect(new GainBlockEffect(block));
            AddEffect(new DamageEffect(damage));
        }
        else
        {
            Debug.Log("Card definition mismatch");
        }
    }
}
