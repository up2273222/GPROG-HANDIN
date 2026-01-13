using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IPlayable
{
    void Play(CardContext cardContext);
}

public abstract class BaseCard : MonoBehaviour, IPlayable
{
    protected List<ICardEffect> CardEffects;
    
    public static event Action<BaseCard> OnCardSelected;
    
    private Vector3 _startposition;
    
    public BaseDefinition Definition { get; private set; }

    public void Initialize(BaseDefinition definition)
    {
        this.Definition = definition;
        CardEffects = new List<ICardEffect>();
        PopulateEffects();
    }

   
    
    
    
    
    public void Play(CardContext context)
    {
        ApplyEffects(context);
    }

    private void ApplyEffects(CardContext context)
    {
        foreach (var i in CardEffects)
        {
            i.PlayEffect(context);
        }
    }
    protected void AddEffect(ICardEffect effect)
    {
        CardEffects.Add(effect);
    }
    protected abstract void PopulateEffects();
    
   
   
   
   
   
   
   
    public void OnMouseDown()
    {
        BroadcastSelectedCard(this);
        Debug.Log(Definition.cardID + " has been selected");
        _startposition = transform.position;
    }

    public void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, mousePosition.z + 1);
    }

    public void OnMouseUp()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.forward * 1000000, out hit,100000f))
        {
            switch (Definition.category)
            {
               case CardCategory.Attack:
                   if (hit.collider.gameObject.CompareTag("Enemy"))
                   {
                       hit.collider.gameObject.GetComponent<BaseEnemy>().OnCardPlayed();
                   }
                   break;
               default:
                   if (hit.collider.gameObject.CompareTag("PlayZone") || hit.collider.gameObject.CompareTag("Enemy") )
                   {
                       DeckManager.Instance.GetCardContext(this, null);
                   }

                   break;
                          
            }
            transform.position = _startposition;
        }
        else
        {
            transform.position = _startposition;
        }
    }

    private void BroadcastSelectedCard(BaseCard card)
    {
        OnCardSelected?.Invoke(card);
    }
    


    

 








}
