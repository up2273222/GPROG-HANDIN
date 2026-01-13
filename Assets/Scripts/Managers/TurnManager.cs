using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public interface IState
{
   void Enter();
   void Exit();
}

public abstract class State : IState
{
    protected TurnManager Manager;

    protected State(TurnManager manager)
    {
        this.Manager = manager;
    }



    public abstract void Enter();
    public abstract void Exit();
}










public class StatePlayerTurn : State
{
    public StatePlayerTurn(TurnManager manager) : base(manager)
    {
      this.Manager = manager;
    }

    public override void Enter()
    {
        //Resolve action queue for start of turn?

        DeckManager.Instance.DrawCard(DeckManager.Instance.GetTurnStartDrawCount());
        
    }
    public override void Exit()
    {
        //Resolve action queue for end of turn actions?
        DeckManager.Instance.DiscardHand();
        
        
    }
}









public class StateEnemyTurn : State
{
    public static Action CallResolveEnemyIntents;
    public StateEnemyTurn(TurnManager manager) : base(manager)
    {
        this.Manager = manager;
    }

    public override void Enter()
    {
        //Resolve start of enemy turn action queue?
        CallResolveEnemyIntents?.Invoke();
        
    }
    public override void Exit()
    {
        //Resolve end of enemy turn action queue?
    }
}









public class StateDecideEnemyIntent : State
{
    public static Action CallGetAllEnemyIntents;
    public StateDecideEnemyIntent(TurnManager manager) : base(manager)
    {
        this.Manager = manager;
    }
    public override void Enter()
    {
        CallGetAllEnemyIntents?.Invoke();
    }
    public override void Exit()
    {
        
    }
}

public class StateEndCombat : State
{
    private bool _playerWon;

    public StateEndCombat(TurnManager manager, bool playerWon) : base(manager)
    {
        this.Manager = manager;
        this._playerWon = playerWon;
    }

    public override void Enter()
    {
        DeckManager.Instance.DiscardHand();
        TurnManager.Instance.SetEndGameText(_playerWon ? "You won!" : "You have been defeated!");
    }

    public override void Exit()
    {
        
    }
}


public class TurnManager : MonoBehaviour
{ 
   private static TurnManager _instance; 
   public static TurnManager Instance {get {return _instance;}}

   [SerializeField] private TextMeshProUGUI endGameText;

   
   
   private State _currentState;

   private void Awake()
   {
       if (_instance != null && _instance != this)
       {
           Destroy(this.gameObject);
       }
       else
       {
           _instance = this;
       }
   }

   private void OnEnable()
   {
       PlayerManager.OnPlayerDeath += PlayerHasDied;
       EnemyManager.AllEnemiesDied += PlayerHasWon;
       EnemyManager.SuggestNewState += ChangeState;
   }

   private void OnDisable()
   {
       PlayerManager.OnPlayerDeath -= PlayerHasDied;
       EnemyManager.AllEnemiesDied -= PlayerHasWon;
       EnemyManager.SuggestNewState -= ChangeState;
   }

   private void Start() 
   {
       ChangeState(new StateDecideEnemyIntent(this));
   }
   
   public void ChangeState(State newState)
   {
       _currentState?.Exit();
       _currentState = newState;
       _currentState?.Enter();
   }

   public void SetEndGameText(string text)
   {
       endGameText.text = text;
   }
   

   public State GetCurrentState()
   {
       return _currentState;
   }
   
   private void PlayerHasDied()
   {
       ChangeState(new StateEndCombat(this,false));
   }

   private void PlayerHasWon()
   {
      ChangeState(new StateEndCombat(this,true));
   }
}
