using UnityEngine;

[RequireComponent(typeof(Collector))]
[RequireComponent(typeof(CollectorMovement))]
[RequireComponent(typeof(TargetReceiver))]
[RequireComponent(typeof(AudioSource))]
public class CollectorBrainController : MonoBehaviour
{
    [SerializeField] private Trunk _trunk;
    [SerializeField] private CollectorsBase _newBaseBlueprint;
    [SerializeField] private AudioSource _dischargeSound;
    [SerializeField] private AudioSource _completedWorkSound;

    private Collector _collector;
    private CollectorMovement _collectorMovement;
    private TargetReceiver _targetReceiver;

    private CollectorsBase _motherBase;    
    private CollectorsBase _newBase;        

    private Vector3 _target;
    private Vector3 _direction;

    private bool _isReached;

    public bool IsWorking { get; private set; }
    public bool IsBuilding { get; private set; }
    public bool IsCrystalOnBoard { get; private set; }

    private void Awake()
    {
        _collector = GetComponent<Collector>();
        _collectorMovement = GetComponent<CollectorMovement>();
        _targetReceiver = GetComponent<TargetReceiver>();
        _motherBase = GetComponentInParent<CollectorsBase>();

        IsCrystalOnBoard = false;
        IsWorking = false;
        IsBuilding = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Crystal crystal))
        {
            if (_target == crystal.transform.position)
            {
                _isReached = true;
                Collect(crystal);
            }
        }

        if (collision.TryGetComponent(out HomePoint homePoint))
        {
            _isReached = true;
        }

        if (collision.TryGetComponent(out BuildPoint buildPoint))
        {
            _isReached = true;

            Destroy(buildPoint.gameObject);
            Destroy(_motherBase.GetComponent<FounderOfNewCollectorsBase>().
                CollectorsBase.gameObject);

            BuildNewBase();

            _completedWorkSound.Play();
        }
    }

    void Update()
    {
        if (_isReached)
            _collectorMovement.Stop();

        if (IsWorking)
        {
            if (IsCrystalOnBoard == false)
                ReachDestination(Targets.Target);
            else
                ReachDestination(Targets.BaseLocation);
        }
        else
        {
            if (_isReached == false)
                ReachDestination(Targets.BaseLocation);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out Crystal crystal))
        {
            if (_target == crystal.transform.position)
                _isReached = false;
        }
    }

    public void SetWorkingStatus(bool status) =>
        IsWorking = status;

    public void SetBuilderMode(bool isTurnOn) =>
        IsBuilding = isTurnOn;

    public int GiveCrystals()
    {
        Crystal crystal = GetComponentInChildren<Crystal>();

        Destroy(crystal.gameObject);

        _dischargeSound.Play();

        SetCrystalInTrunkStatus(false);
        SetWorkingStatus(false);

        return crystal.RecourceCapacity;
    }

    private void Collect(Crystal crystal)
    {
        if (IsCrystalOnBoard)
            return;

        crystal.transform.position = _trunk.transform.position;
        crystal.transform.SetParent(_trunk.transform);
        crystal.SetColliderActive(false);

        IsCrystalOnBoard = true;
    }

    private void BuildNewBase()
    {
        SetWorkingStatus(false);
        SetBuilderMode(false);

        _motherBase.GiveAwayCollector(_collector);

        _newBase = Instantiate(_newBaseBlueprint, transform.position, Quaternion.identity);
        _newBase.AppropriateCollector(_collector);


        _collector.transform.SetParent(_newBase.GetComponentInChildren<CollectorsHolder>().transform);
        _collector.GetComponent<TargetReceiver>().SetBase(_newBase);
    }

    private void ReachDestination(Targets target)
    {
        _isReached = false;

        SetTarget(target);
        SetRotation();
        SetDirection();
        _collectorMovement.Move(_direction);
    }

    private void SetTarget(Targets target)
    {
        if (target == Targets.Target)
            _target = _targetReceiver.Target;
        else if (target == Targets.BaseLocation)
            _target = _targetReceiver.BaseLocation;
    }

    private void SetDirection() =>
        _direction = (_target - transform.position).normalized;

    private void SetRotation() =>
        transform.LookAt(_target);

    private void SetCrystalInTrunkStatus(bool status) =>
        IsCrystalOnBoard = status;

    public enum Targets
    {
        Target,
        BaseLocation
    }
}