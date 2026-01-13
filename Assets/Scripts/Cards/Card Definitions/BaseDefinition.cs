using UnityEngine;

public enum CardCategory
{
  Attack,
  Skill  
}

public abstract class BaseDefinition : ScriptableObject
{
    public string cardID;
    
    public BaseCard prefab;
    
    public CardCategory category;
}
