using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class SlimeEnemy : BaseEnemy
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider _boxCollider;
    [SerializeField] private TextMeshProUGUI intentDescriptor;
    public override IEnemyIntent DecideIntent()
    {
        int random = Random.Range(1, 4);
        if (random != 3)
        {
            intentDescriptor.text = "3 damage";
            return new DealDamageIntent(3);
        }
        else
        {
            intentDescriptor.text = "5 damage";
            return new DealDamageIntent(5);
        }
    }
    
    
    public SlimeEnemy(string enemyName, int enemyHealth)
    {
        this._enemyHealth = enemyHealth;
        this._enemyName = enemyName;
    }

    private void OnEnable()
    {
        if (_enemyName == null)
        {
            SetEnemyName("SlimeEnemy");
            SetEnemyHealth(30);
        }
    }
    
    public override void TakeDamage(int damage)
    {
        damage = Mathf.Max(0, damage);
        var newHealth = GetEnemyHealth() - damage;
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
