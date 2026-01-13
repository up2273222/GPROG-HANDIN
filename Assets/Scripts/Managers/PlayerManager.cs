using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  private static PlayerManager _instance; 
  public static PlayerManager Instance {get{return _instance;}}
  
  public static event Action<int,int> OnHealthChanged;
  public static event Action<int> OnBlockChanged;
  
  public static event Action OnPlayerDeath;
  
  private int _playerMaxHp = 20;
  private int _playerCurrentHp;

  private int _block;
  
  private void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      _instance = this;
      DontDestroyOnLoad(this.gameObject);
      
      
    }
    
  }

  private void Start()
  {
    SetPlayerCurrentHp(_playerMaxHp);
  }


  public void DamagePlayer(int damage)
  {
    Debug.Log("Player damaged for" + damage);
    if (_block != 0)
    {
      Debug.Log("Block is " + _block);
      if (_block >= damage)
      {
        Debug.Log("Attack fully blocked!");
        _block -= damage;
      }
      else if (_block < damage)
      {
        Debug.Log("Attack partially blocked!");
        var overflow = damage - _block;
        _block -= (damage - overflow);
        _playerCurrentHp -= overflow;
      }
    }
    else
    {
      _playerCurrentHp -= damage;
    }
    BroadcastHealth();
    BroadcastBlock();
    if (_playerCurrentHp <= 0)
    {
      OnPlayerDeath?.Invoke();
    }
    Debug.Log("Player HP is: " + _playerCurrentHp);
  }

  private void BroadcastHealth()
  {
    OnHealthChanged?.Invoke(_playerCurrentHp, _playerMaxHp);
  }

  private void BroadcastBlock()
  {
    OnBlockChanged?.Invoke(_block);
  }

  public void GainBlock(int blockValue)
  {
    _block += blockValue;
    BroadcastBlock();
  }

  private void SetPlayerCurrentHp(int playerCurrentHp)
  {
    _playerCurrentHp = playerCurrentHp;
    BroadcastHealth();
  }
  
  public int GetPlayerCurrentHp()
  {
    return _playerCurrentHp;
  }
  
  
}
