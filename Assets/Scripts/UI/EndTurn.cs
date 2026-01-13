using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTurn : MonoBehaviour
{
   public void OnClickEndTurn()
   {
       if (PlayerManager.Instance.GetPlayerCurrentHp() > 0)
       {
           TurnManager.Instance.ChangeState(new StateEnemyTurn(TurnManager.Instance));
       }
     
   }
}
