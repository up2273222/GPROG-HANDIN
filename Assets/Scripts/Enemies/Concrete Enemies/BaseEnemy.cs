using System;
using System.Collections;
using TMPro;
using UnityEngine;


public interface ICanTakeDamage
{
    void TakeDamage(int damage);
    void Die();
}

public interface IEnemyIntent
{
    public abstract void Execute();
}


public abstract class BaseEnemy : MonoBehaviour, ICanTakeDamage
{
    protected string _enemyName;
    protected int _enemyHealth;
    protected string _intentDescriptor;

    public static Action<BaseEnemy> EnemyHasDied;
    
    
    
    protected string GetEnemyName()
    {
        return _enemyName;
    }

    protected void SetEnemyName(string enemyName)
    {
        this._enemyName = enemyName;
    }
    
    protected int GetEnemyHealth()
    {
        return _enemyHealth;
    }

    protected void SetEnemyHealth(int enemyHealth)
    {
        this._enemyHealth = enemyHealth;
    }

    public abstract void TakeDamage(int damage);

    public abstract void OnCardPlayed();
    
    public abstract IEnemyIntent DecideIntent();


    public virtual void Die()
    {
        Debug.Log(GetEnemyName() + " is dead");
        EnemyHasDied?.Invoke(this);
        Destroy(gameObject);
    }

}


public class DealDamageIntent : IEnemyIntent
{
    private int damage;

    public DealDamageIntent(int damage)
    {
        this.damage = damage;
    }
  
    public void Execute()
    {
        PlayerManager.Instance.DamagePlayer(damage);
    }
}


