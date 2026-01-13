using System;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]

public class TestEnemy : BaseEnemy
{
  private SpriteRenderer _spriteRenderer;
  private BoxCollider _boxCollider;
  [SerializeField] public TextMeshProUGUI intentDescriptor;

  private void OnEnable()
  {
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _boxCollider = GetComponent<BoxCollider>();
  }

  private void Start()
  {
    SetEnemyName("Test enemy");
    SetEnemyHealth(100);
    
    
  }

  public override IEnemyIntent DecideIntent()
  {
    intentDescriptor.text = "Dealing 10 damage!";
    return new DealDamageIntent(10);
    
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

  public override void Die()
  {
    Debug.Log(GetEnemyName() + " is dead");
    Destroy(gameObject);
  }

  public override void OnCardPlayed()
  {
    if (DeckManager.Instance.GetSelectedCard() != null)
    {
      DeckManager.Instance.GetCardContext(DeckManager.Instance.GetSelectedCard(), this);
    }
  }


}
