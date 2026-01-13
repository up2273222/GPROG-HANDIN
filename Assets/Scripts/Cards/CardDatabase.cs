using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CardDatabase : MonoBehaviour
{
  private static CardDatabase _instance; 
  public static CardDatabase Instance {get{return _instance;}}
  

  private Dictionary<string, BaseDefinition> Allcards;


  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      DontDestroyOnLoad(this.gameObject);
      _instance = this;
    }
    Allcards = Resources.LoadAll<BaseDefinition>("Cards").ToDictionary(def => def.cardID, def => def);
  }

  public BaseDefinition GetCardByID(string cardID)
  {
    return Allcards[cardID];
  }

  public BaseDefinition GetRandomCardDefinition()
  {
    int randomNumber = UnityEngine.Random.Range(0, Allcards.Count);
    {
      return Allcards.ElementAt(randomNumber).Value;
    };
  }
  
  
  
  
}
