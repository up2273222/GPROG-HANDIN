using System.Collections.Generic;
using UnityEngine;

public class GuardCard : BaseCard
{
    protected override void PopulateEffects()
    {
        if (Definition is GuardDefinition cardDef)
        {
            int block = cardDef.blockGained;
            AddEffect(new GainBlockEffect(block));
        }
        else
        {
            Debug.Log("Card definition mismatch");
        }
    }
}
