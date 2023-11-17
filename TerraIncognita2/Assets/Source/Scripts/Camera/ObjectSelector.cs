using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] private SelectionRenderer _selectionRendererBlueprint;
    [SerializeField] private UIRenderer _UIRenderer;
    [SerializeField] private UIController _UIController;

    [SerializeField] private CollectorsBase _firstBase;

    [SerializeField] private float _selectionRendererHeight = 0.01f;
    [SerializeField] private float _eulerAngleX = 90;

    private SelectionRenderer _currentSelection;
    private CollectorsBase _selectedBase;

    private void Awake()
    {
        SelectBase(_firstBase);
    }

    private void Update()
    {
        TrySelect();
    }

    private void TrySelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CollectorsBase collectorsBase = hit.collider.GetComponent<CollectorsBase>();

                if (collectorsBase != _selectedBase)
                {
                    SelectBase(collectorsBase);
                }
            }
        }
    }

    private void SelectBase(CollectorsBase collectorsBase)
    {
        _selectedBase = collectorsBase;

        _UIRenderer.SetSelectedBase(collectorsBase);
        _UIController.SetSelectedBase(collectorsBase);

        RenderSelection(collectorsBase.gameObject);
    }

    private void RenderSelection(GameObject gameObject)
    {
        RenderDeselection();

        _currentSelection = Instantiate(_selectionRendererBlueprint,
                new Vector3(
                    gameObject.transform.position.x,
                    _selectionRendererHeight,
                    gameObject.transform.position.z),
                Quaternion.Euler(_eulerAngleX, 0, 0));

    }

    private void RenderDeselection()
    {
        if (_currentSelection != null && _currentSelection.gameObject != null)
        {
            Destroy(_currentSelection.gameObject);
        }
    }
}