using UnityEngine;

[RequireComponent(typeof(Collector))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TargetReceiver))]
public class CollectorMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Collector _collector;
    private Rigidbody _rigidbody;
    private TargetReceiver _targetReceiver;

    private Transform _target;
    private Vector3 _direction;

    private bool _isReached;

    private void Awake()
    {
        _collector = GetComponent<Collector>();
        _rigidbody = GetComponent<Rigidbody>();
        _targetReceiver = GetComponent<TargetReceiver>();
    }

    void Update()
    {
        if (_collector.IsWorking)
        {
            _isReached = false;

            if (_isReached)
                Stop();

            if (_isReached == false)
            {
                if (_collector.IsCrystalOnBoard == false)
                    ReachDestination(Targets.Target);
                else
                    ReachDestination(Targets.BaseLocation);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Crystal crystal))
        {
            if (_target == crystal.transform)
            {
                _isReached = true;
                _collector.Collect(crystal);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out Crystal crystal))
        {
            if (_target == crystal.transform)            
                _isReached = false;            
        }
    }

    public void SetTarget(Targets target)
    {
        if (target == Targets.Target)        
            _target = _targetReceiver.Target;        
        else if (target == Targets.BaseLocation)        
            _target = _targetReceiver.BaseLocation;        
    }

    private void SetDirection() =>
        _direction = (_target.transform.position - transform.position).normalized;

    private void SetRotation() =>
        transform.LookAt(_target);

    private void Move() =>    
        _rigidbody.velocity = _direction * _speed;    

    private void Stop() =>
        _rigidbody.velocity = Vector3.zero;

    private void ReachDestination(Targets target)
    {
        SetTarget(target);
        SetRotation();
        SetDirection();
        Move();
    }

    public enum Targets
    {
        Target,
        BaseLocation
    }
}
