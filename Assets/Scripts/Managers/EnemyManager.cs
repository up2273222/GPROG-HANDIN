using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance; 
    public static EnemyManager Instance {get{return _instance;}}
    
    private Queue<IEnemyIntent> _intentsQueue;
    private List<BaseEnemy> _enemies;

    public static Action AllEnemiesDied;
    public static Action StartPlayerTurn;
    public static Action DecideEnemyIntent;
    public static Action<State> SuggestNewState;
    
    
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
            _intentsQueue = new Queue<IEnemyIntent>();
            _enemies = new List<BaseEnemy>();
            _enemies = FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None).ToList();
        }
    }

    private void OnEnable()
    {
        StateDecideEnemyIntent.CallGetAllEnemyIntents += GetAllEnemyIntents;
        StateEnemyTurn.CallResolveEnemyIntents += ResolveEnemyIntents;
        BaseEnemy.EnemyHasDied += OnEnemyDeath;
    }

    private void OnDisable()
    {
        StateDecideEnemyIntent.CallGetAllEnemyIntents -= GetAllEnemyIntents;
        StateEnemyTurn.CallResolveEnemyIntents -= ResolveEnemyIntents;
        BaseEnemy.EnemyHasDied -= OnEnemyDeath;
    }

    private void AddEnemyIntent(IEnemyIntent intent)
    {
        _intentsQueue.Enqueue(intent);
        Debug.Log("Intent added");
    }

    private void GetAllEnemyIntents()
    {
        _enemies = FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None).ToList();
        foreach (BaseEnemy e in _enemies)
        {
            AddEnemyIntent(e.DecideIntent());
        }

        SuggestNewState?.Invoke(new StatePlayerTurn(TurnManager.Instance));
    
      
    }
    
    private void ResolveEnemyIntents()
    {
        foreach (IEnemyIntent intent in _intentsQueue.ToList())
        {
            intent.Execute();
            _intentsQueue.Dequeue();
        }

        if (PlayerManager.Instance.GetPlayerCurrentHp() > 0)
        {
            SuggestNewState?.Invoke(new StateDecideEnemyIntent(TurnManager.Instance));
        }
        
    }

    private void OnEnemyDeath(BaseEnemy enemy)
    {
        _enemies.Remove(enemy);
        if (_enemies.Count == 0)
        {
            AllEnemiesDied?.Invoke();
        }
    }

    public List<BaseEnemy> GetAllEnemies()
    {
        return _enemies;
    }
    
}
