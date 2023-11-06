using UnityEngine;
using TMPro;

public class CollectorStatusRenderer : MonoBehaviour
{
    private const string Работает = nameof(Работает); 
    private const string Ожидает = nameof(Ожидает); 

    [SerializeField] private Collector _collector;
    [SerializeField] private TMP_Text _statusRenderer;    
    
    void Update()
    {
        if (_collector.IsWorking)        
            _statusRenderer.text = Работает;        
        else        
            _statusRenderer.text = Ожидает;        
    }
}
