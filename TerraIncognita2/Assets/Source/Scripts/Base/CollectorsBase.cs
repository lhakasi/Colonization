using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(CollectorsSpawner))]
public class CollectorsBase : MonoBehaviour
{
    private const string FutureBuildings = nameof(FutureBuildings); 
    
    [SerializeField] private float _price;
    [SerializeField] private AudioSource _insufficientFundsSound;
    [SerializeField] private List<Collector> _collectors;

    private readonly object _queueLock = new object();

    private Scanner _scanner;
    private CollectorsSpawner _collectorsSpawner;
    private FounderOfNewCollectorsBase _founderOfNewCollectorsBase;

    private List<Collider> _foundCrystals;

    private Queue<GameObject> _workQueue;
    private Queue<Collector> _workersQueue;

    private bool _isNewBaseBuilding;

    public int CollectedCrystals { get; private set; }
    public int CollectorsCount => _collectors.Count;

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _collectorsSpawner = GetComponent<CollectorsSpawner>();
        _founderOfNewCollectorsBase = GetComponent<FounderOfNewCollectorsBase>();

        _foundCrystals = new List<Collider>();
        _workQueue = new Queue<GameObject>();
        _workersQueue = new Queue<Collector>();

        _isNewBaseBuilding = false;

        CollectedCrystals = 0;
    }

    private void Update()
    {
        if (_scanner.IsFinded)
            FillCrystalQueue();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Collector collector))
        {
            if (collector.IsCrystalOnBoard)
                CollectedCrystals +=
                    collector.GetComponent<CollectorBrainController>().GiveCrystals();
        }
    }

    public void FindCrystals() =>
        _foundCrystals = _scanner.GetFindedCrystals();

    public void CreateNewCollector()
    {
        if (CollectedCrystals >= _collectorsSpawner.GetCollectorPrice())
            _collectors.Add(_collectorsSpawner.CreateNewCollector());
        else
            _insufficientFundsSound.Play();
    }

    public void StartEstablishNewCollectorsBaseCoroutine() =>    
        StartCoroutine(EstablishNewCollectorsBase());       

    public void AppropriateCollector(Collector collector) =>
        _collectors.Add(collector);

    public void GiveAwayCollector(Collector collector) =>
        _collectors.Remove(collector);

    private void CancelBuilding()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(FutureBuildings);

        foreach (GameObject gameObject in objects)        
            Destroy(gameObject);
        
        foreach (Collector collector in _collectors)
        {
            if(collector.IsBuilding)
            {
                collector.GetComponent<CollectorBrainController>().SetBuilderMode(false);
                collector.GetComponent<CollectorBrainController>().SetWorkingStatus(false);
            }
        }
    }

    private void FillCrystalQueue()
    {
        lock (_queueLock)
        {
            _scanner.TurnOff();

            foreach (Collider crystal in _foundCrystals)
                _workQueue.Enqueue(crystal.gameObject);

            StartCoroutine(PlaneWork());
        }
    }

    private void FillWorkersQueue()
    {
        lock (_queueLock)
        {
            foreach (Collector collector in _collectors)
            {
                if (collector.IsWorking == false)
                    _workersQueue.Enqueue(collector);
            }
        }
    }

    private IEnumerator PlaneWork()
    {
        while (_workQueue.Count > 0)
        {
            lock (_queueLock)
            {
                FillWorkersQueue();

                while (_workersQueue.Count > 0)
                {
                    GameObject currentWork = _workQueue.Peek();

                    if (_workQueue.Peek().TryGetComponent<FutureBuilding>(out FutureBuilding futureBuilding))
                    {
                        futureBuilding = currentWork.GetComponent<FutureBuilding>();

                        SetUpWork(futureBuilding.transform.position, true);
                    }
                    else if (_workQueue.Peek().TryGetComponent<Crystal>(out Crystal crystal))
                    {
                        crystal = currentWork.GetComponent<Crystal>();

                        SetUpWork(crystal.transform.position, false);
                    }

                    _workQueue.Dequeue();
                }
            }

            yield return null;
        }
    }

    private IEnumerator EstablishNewCollectorsBase()
    {
        if (_isNewBaseBuilding)
            CancelBuilding();

        _founderOfNewCollectorsBase.EstablishNewCollectorsBase();
        
        _isNewBaseBuilding = true;

        while (!_founderOfNewCollectorsBase.IsPlaceChosen())
            yield return null;

        while (CollectedCrystals < _price)
            yield return null;

        if (_workQueue.Count > 0)
        {
            _workQueue = new Queue<GameObject>(new[]
            { _founderOfNewCollectorsBase.CollectorsBase.gameObject }.Concat(_workQueue));
        }
        else
        {
            _workQueue.Enqueue(_founderOfNewCollectorsBase.CollectorsBase.gameObject);
        }

        StartCoroutine(PlaneWork());
    }

    private void SetUpWork(Vector3 target, bool isBuilding)
    {
        _workersQueue.Peek().GetComponent<TargetReceiver>().
                    SetTarget(target);

        _workersQueue.Peek().GetComponent<CollectorBrainController>().
            SetWorkingStatus(true);

        if (isBuilding)
        {
            _workersQueue.Peek().GetComponent<CollectorBrainController>().
                SetBuilderMode(true);
        }        

        _workersQueue.Dequeue();
    }
}