using UnityEngine;

public class TargetReceiver : MonoBehaviour
{
    [SerializeField] private CollectorsBase _base;

    private Vector3 _target;
    
    public Vector3 Target => _target;
    public Vector3 BaseLocation => 
        _base.GetComponentInChildren<HomePoint>().transform.position;

    public void SetTarget(Vector3 transform) =>
        _target = transform;

    public void SetBase(CollectorsBase collectorsBase) =>
        _base = collectorsBase;
}