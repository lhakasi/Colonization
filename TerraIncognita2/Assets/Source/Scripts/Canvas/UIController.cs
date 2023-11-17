using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button _buildNewCollectorButton;
    [SerializeField] private Button _scanningButton;
    [SerializeField] private Button _buildNewBaseButton;

    private CollectorsBase _collectorsBase;

    private void Start()
    {
        _buildNewCollectorButton.onClick.AddListener(CreateNewCollector);
        _scanningButton.onClick.AddListener(FindCrystals);
        _buildNewBaseButton.onClick.AddListener(EstablishNewCollectorsBase);
    }

    public void SetSelectedBase(CollectorsBase collectorsBase)
    {
        _collectorsBase = collectorsBase;

        UpdateButtonFunctions();
    }

    private void UpdateButtonFunctions()
    {
        _scanningButton.onClick.RemoveAllListeners();
        _scanningButton.onClick.AddListener(_collectorsBase.FindCrystals);

        _buildNewCollectorButton.onClick.RemoveAllListeners();
        _buildNewCollectorButton.onClick.AddListener(_collectorsBase.CreateNewCollector);

        _buildNewBaseButton.onClick.RemoveAllListeners();
        _buildNewBaseButton.onClick.AddListener(_collectorsBase.StartEstablishNewCollectorsBaseCoroutine);
    }

    private void FindCrystals() =>
        _collectorsBase.FindCrystals();

    private void CreateNewCollector() =>
        _collectorsBase.CreateNewCollector();

    private void EstablishNewCollectorsBase() =>
        _collectorsBase.StartEstablishNewCollectorsBaseCoroutine();
}