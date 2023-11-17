using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollectorMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Rigidbody _rigidbody;

    private void Awake()
    {        
        _rigidbody = GetComponent<Rigidbody>();
    } 

    public void Move(Vector3 direction) =>    
        _rigidbody.velocity = direction * _speed;    

    public void Stop() =>    
        _rigidbody.velocity = Vector3.zero;       
}