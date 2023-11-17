using UnityEngine;

public class FounderOfNewCollectorsBase : MonoBehaviour
{
    [SerializeField] private FutureBuilding _collectorsBaseBlueprint;
    [SerializeField] private BuildPoint _buildPointBlueprint;
    [SerializeField] private Camera _camera;

    private bool _isChosen;
    private bool _isChoosePlaceForNewBase;

    public FutureBuilding CollectorsBase { get; private set; }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isChoosePlaceForNewBase == false)
            return;

        ChoosePlaceForDeploy();
    }

    public void EstablishNewCollectorsBase()
    {
        CollectorsBase = Instantiate(_collectorsBaseBlueprint, transform.position, Quaternion.identity);

        _isChoosePlaceForNewBase = true;
        _isChosen = false;
    }

    public bool IsPlaceChosen() =>
        _isChosen ? true : false;

    private void ChoosePlaceForDeploy()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            CollectorsBase.transform.position = hit.point;

        if (Input.GetMouseButton(0))
        {
            _isChoosePlaceForNewBase = false;
            _isChosen = true;
            Vector3 currentNewBaseLocation = CollectorsBase.transform.position;

            Instantiate(_buildPointBlueprint, currentNewBaseLocation, Quaternion.identity);
        }
    }
}