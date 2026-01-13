using System.Collections.Generic;
using UnityEngine;

public class PrepareCard : BaseCard
{
    protected override void PopulateEffects()
    {
        if (Definition is PrepareDefinition cardDef)
        {
            int drawNum = cardDef.cardsDrawn;
            AddEffect(new DrawCardEffect(drawNum));
        }
        else
        {
            Debug.Log("Card definition mismatch");
        }
    }
}
