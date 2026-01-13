using UnityEngine;

public interface ICardEffect
{
    void PlayEffect(CardContext cardContext);
}



public class DamageEffect : ICardEffect
{
    private int _damage;

    public DamageEffect(int damage)
    {
        this._damage = damage;
    }
    

    public void PlayEffect(CardContext cardContext)
    {
       cardContext.SingleEnemy.TakeDamage(_damage);
    }
}

public class DrawCardEffect : ICardEffect
{
    private int _drawValue;

    public DrawCardEffect(int drawValue)
    {
        this._drawValue = drawValue;
    }

    public void PlayEffect(CardContext cardContext)
    {
        DeckManager.Instance.DrawCard(_drawValue);
    }
}

public class GainBlockEffect : ICardEffect
{
    private int _blockValue;

    public GainBlockEffect(int blockValue)
    {
        this._blockValue = blockValue;
    }

    public void PlayEffect(CardContext cardContext)
    {
        PlayerManager.Instance.GainBlock(_blockValue);
    }
}


