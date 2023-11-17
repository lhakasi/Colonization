using UnityEngine;

[RequireComponent (typeof(CollectorBrainController))]
public class Collector : MonoBehaviour
{
    [SerializeField] private float _price;

    private CollectorBrainController _colllectorBrainController;

    public float Price => _price;
    public bool IsWorking => 
        _colllectorBrainController.IsWorking;
    public bool IsCrystalOnBoard => 
        _colllectorBrainController.IsCrystalOnBoard;

    private void Awake()
    {
        _colllectorBrainController = GetComponent<CollectorBrainController>();
    }
}