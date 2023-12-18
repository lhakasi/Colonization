using UnityEngine;

[RequireComponent (typeof(CollectorBrainController))]
public class Collector : MonoBehaviour
{
    [SerializeField] private int _price;

    private CollectorBrainController _collectorBrainController;

    public int Price => _price;
    public bool IsWorking => 
        _collectorBrainController.IsWorking;
    public bool IsBuilding => 
        _collectorBrainController.IsBuilding;
    public bool IsCrystalOnBoard =>
        _collectorBrainController.IsCrystalOnBoard;

    private void Awake()
    {
        _collectorBrainController = GetComponent<CollectorBrainController>();
    }
}