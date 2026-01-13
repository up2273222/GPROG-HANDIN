using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class SpikedSlimeEnemy : BaseEnemy
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider _boxCollider;
    [SerializeField] private TextMeshProUGUI intentDescriptor;

    
    public override IEnemyIntent DecideIntent()
    {
        intentDescriptor.text = "2 damage";
        return new DealDamageIntent(2);
    }

    public SpikedSlimeEnemy(string enemyName, int enemyHealth)
    {
        this._enemyHealth = enemyHealth;
        this._enemyName = enemyName;
    }

    private void OnEnable()
    {
        if (_enemyName == null)
        {
            SetEnemyName("SpikedSlimeEnemy");
            SetEnemyHealth(25);
        }
    }
    
    
    public override void TakeDamage(int damage)
    {
        var spikesDamage = 2;
        damage = Mathf.Max(0, damage);
        var newHealth = GetEnemyHealth() - damage;
        PlayerManager.Instance.DamagePlayer(spikesDamage);
        if (newHealth <= 0)
        {
            SetEnemyHealth(0);
            Die();
        }
        else
        {
            SetEnemyHealth(newHealth);
            Debug.Log(GetEnemyName() + "now has" + newHealth + "hp");
        }
    }
    
    public override void OnCardPlayed()
    {
        if (DeckManager.Instance.GetSelectedCard() != null)
        {
            DeckManager.Instance.GetCardContext(DeckManager.Instance.GetSelectedCard(), this);
        }
    }
    
    
   
}
