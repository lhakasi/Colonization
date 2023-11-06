using UnityEngine;

public class TargetReceiver : MonoBehaviour
{
    [SerializeField] private Base _base;

    private Transform _target;
    
    public Transform Target => _target;
    public Transform BaseLocation => _base.transform;

    public void SetTarget(Crystal crystal) =>
        _target = crystal.transform;
}