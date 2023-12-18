using UnityEngine;

[RequireComponent (typeof(CollectorBrainController))]
public class Collector : MonoBehaviour
{
    [SerializeField] private float _price;

    private CollectorBrainController _collectorBrainController;

    public float Price => _price;
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