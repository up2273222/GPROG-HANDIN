using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using UnityEngine.U2D;
using Spline = UnityEngine.Splines.Spline;


public class DeckManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    
    [Header("Starter deck")]
    [SerializeField] private List<BaseDefinition> _playerStartingDeck;
    private List<BaseDefinition> _playerHand = new List<BaseDefinition>();
    private List<BaseDefinition> _playerDraw = new List<BaseDefinition>();
    private List<BaseDefinition> _playerDiscard = new List<BaseDefinition>();
    
    private List<BaseCard> _playerCardObjects = new List<BaseCard>();
    

    private Spline _handSpline;


    private readonly int _maxHandSize = 10;
    private int _turnStartDrawCount = 3;
    
    private BaseCard _selectedCard;


    private static DeckManager _instance; 
    public static DeckManager Instance {get{return _instance;}}
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            
            CreateHandSpline();
            
           // _playerStartingDeck.Add(CardDatabase.Instance.GetCard("Strike"));
           
            _playerDraw.AddRange(_playerStartingDeck);
            ShuffleList(_playerDraw);
        }
    }

    private void OnEnable()
    {
        BaseCard.OnCardSelected += SetSelectedCard;
    }

    private void OnDisable()
    {
        BaseCard.OnCardSelected -= SetSelectedCard;
    }
    
    public void GetCardContext(BaseCard card, BaseEnemy target) 
    {
        CardContext cardContext = new CardContext()
        {
        PlayedCard = card,
        SingleEnemy = target,
        AllEnemies = EnemyManager.Instance.GetAllEnemies()
        };
        PlayCard(cardContext);
        
        DiscardCard(card);
        _playerCardObjects.Remove(card);
        
        SetSelectedCard(null);
        RecalculateCardPositions();
    }
    
    private void PlayCard(CardContext cardContext)
    {
        cardContext.PlayedCard.Play(cardContext);
        RecalculateCardPositions();
    }
    
    private void DiscardCard(BaseCard card)
    {
        _playerHand.Remove(card.Definition);
        _playerDiscard.Add(card.Definition);
        Destroy(card.gameObject);
        RecalculateCardPositions();
    }
    
    public void DrawCard(int numDrawn)
    {
        for (int i = 0; i < numDrawn; i++)
        {
            if (_playerDraw.Count == 0)
            {
                if (_playerDiscard.Count == 0)
                {
                    break;
                }
                Reshuffle();
            }
            BaseDefinition cardDef = _playerDraw[0];
            _playerDraw.RemoveAt(0);
            if (_playerHand.Count == _maxHandSize)
            {
                _playerDiscard.Add(cardDef);
            }
            else
            { 
                _playerHand.Add(cardDef);
                BaseCard card = Instantiate(cardDef.prefab, this.transform, true);
                card.Initialize(cardDef);
                _playerCardObjects.Add(card);

                RecalculateCardPositions();
            }
        }
    }
    
    private void ShuffleList(List<BaseDefinition> inList)
    {
        for (int i = inList.Count - 1; i >= 1; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (inList[i], inList[j]) = (inList[j], inList[i]);
        }
    }
    
    private void Reshuffle()
    {
        _playerDraw.AddRange(_playerDiscard);
        _playerDiscard.Clear();
        ShuffleList(_playerDraw);
    }
    
    public void DiscardHand()
    {
        _playerDiscard.AddRange(_playerHand);
        _playerHand.Clear();
        foreach (var t in _playerCardObjects)
        {
            Destroy(t.gameObject);
        }
        _playerCardObjects.Clear();
    }
    
    private void RecalculateCardPositions()
    {
        for (int i = 0; i < _playerCardObjects.Count; i++)
        {
            float splinePos = (i + 1f)  / (_playerCardObjects.Count + 1f);
            Vector3 cardPos = _handSpline.EvaluatePosition(splinePos);
            
            _playerCardObjects[i].transform.position = new Vector3(cardPos.x, cardPos.y, 1);
        }
    }
    
    private void CreateHandSpline() {
        GameObject splineObject = new GameObject("HandSpline");
        splineObject.transform.SetParent(this.transform);
        SplineContainer splineContainer = splineObject.AddComponent<SplineContainer>();

        _handSpline = new Spline();
        _handSpline.Add(new BezierKnot(mainCamera.ViewportToWorldPoint(new Vector3(0,0,0))));
        _handSpline.Add(new BezierKnot(mainCamera.ViewportToWorldPoint(new Vector3(1/3f,1/3f,0))));
        _handSpline.Add(new BezierKnot(mainCamera.ViewportToWorldPoint(new Vector3(2/3f,1/3f,0))));
        _handSpline.Add(new BezierKnot(mainCamera.ViewportToWorldPoint(new Vector3(1,0,0))));
        
        _handSpline.SetTangentMode(TangentMode.AutoSmooth);
        
        splineContainer.AddSpline(_handSpline);

    }

    public int GetCurrentHandSize()
    {
        return _playerHand.Count;
    }
    
    public int GetMaxHandSize()
    {
        return _maxHandSize;
    }
    
    public void SetSelectedCard(BaseCard card)
    {
        _selectedCard = card;
    }

    public BaseCard GetSelectedCard()
    {
        return _selectedCard;
    }

    public int GetTurnStartDrawCount()
    {
        return _turnStartDrawCount;
    }

    public void SetTurnStartDrawCount(int turnStartDrawCount)
    {
        _turnStartDrawCount = turnStartDrawCount;
    }
    
}
