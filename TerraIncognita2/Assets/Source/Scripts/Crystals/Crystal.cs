using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Crystal : MonoBehaviour
{
    [SerializeField] private int _recourceCapacity;

    private Collider _collider;

    public int RecourceCapacity => _recourceCapacity;

    private void Awake() =>    
        _collider = GetComponent<Collider>();    

    public void SetColliderActive(bool state) =>    
        _collider.enabled = state;    
}