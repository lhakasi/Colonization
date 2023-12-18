using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button _buildNewCollectorButton;
    [SerializeField] private Button _scanningButton;
    [SerializeField] private Button _buildNewBaseButton;

    private CollectorsBase _collectorsBase;

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
}