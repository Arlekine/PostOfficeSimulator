using System;
 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using Random = UnityEngine.Random;
 
 public class ClientDesire : MonoBehaviour
 {
     [Serializable]
     private class DesireWithModel
     {
         public Package DesiredPackage;
         public GameObject ModelPart;
     }
 
     public Package CurrentDesire => _currentDesire.DesiredPackage;
     public UiClientDesire DesireView => _desireUI;
     
     [SerializeField] private List<DesireWithModel> _possibleDesires = new List<DesireWithModel>();
     [SerializeField] private UiClientDesire _desireUI;
 
     private DesireWithModel _currentDesire;
 
     public void SelectRandomDesire()
     {
         _currentDesire = _possibleDesires[Random.Range(0, _possibleDesires.Count)];
         
         if (_currentDesire.ModelPart != null)
             _currentDesire.ModelPart.SetActive(false);
         
         _desireUI.SetIcon(_currentDesire.DesiredPackage.PackageIcon);
     }
 
     public bool TryPerformDesireWithPackage(Package package)
     {
         bool desireFulfilled = _currentDesire.DesiredPackage == package;
         
         if (_currentDesire.ModelPart != null)
             _currentDesire.ModelPart.SetActive(desireFulfilled);
         
         return desireFulfilled;
     }
 }