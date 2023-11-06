using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCameraBase[] _virtualCameras;
    [SerializeField] private Button[] _cameraButtons;

    private void Start()
    {
        for (int i = 0; i < _cameraButtons.Length; i++)
        {
            int index = i;
            _cameraButtons[i].onClick.AddListener(() => SwitchCamera(index));
        }
    }

    private void SwitchCamera(int index)
    {
        foreach (CinemachineVirtualCamera camera in _virtualCameras)        
            camera.gameObject.SetActive(false);        

        _virtualCameras[index].gameObject.SetActive(true);
    }
}