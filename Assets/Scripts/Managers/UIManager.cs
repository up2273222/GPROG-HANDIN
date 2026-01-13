using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
 private static UIManager _instance; 
 public static UIManager Instance {get{return _instance;}}
 
 [SerializeField] private Slider playerHpslider;
 [SerializeField] private TextMeshProUGUI block;

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

 void OnEnable()
 {
  PlayerManager.OnHealthChanged += SetHPSliderValues;
  PlayerManager.OnBlockChanged += SetBlockCounter;
 }

 void OnDisable()
 {
  PlayerManager.OnHealthChanged -= SetHPSliderValues;
  PlayerManager.OnBlockChanged -= SetBlockCounter;
 }

 private void SetHPSliderValues(int current, int max)
 {
  playerHpslider.maxValue = max;
  playerHpslider.value = current;
 }



 private void SetBlockCounter(int value)
 {
  block.text = value.ToString();
 }
 
 
 
}
