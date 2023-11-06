using UnityEngine;
using TMPro;

public class CollectedCrystalsCountRenderer : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TMP_Text _crystalsCounter;

    void Update() =>    
        _crystalsCounter.text = _base.CollectedCrystals.ToString();    
}