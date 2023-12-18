using System.Collections;
using TMPro;
using UnityEngine;

public class UIRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text _collectorsCounter;
    [SerializeField] private TMP_Text _crystalsCounter;
    [SerializeField] private TMP_Text _timer;

    private CollectorsBase _collectorsBase;

    private float _elapsedTime;

    private bool _isWorking;

    private void Awake()
    {
        _elapsedTime = 0f;
        _isWorking = true;

        StartCoroutine(RuntimeTimerCoroutine());
    }

    void Update()
    {
        _collectorsCounter.text = _collectorsBase.CollectorsCount.ToString();
        _crystalsCounter.text = _collectorsBase.CollectedCrystals.ToString();
    }

    public void SetSelectedBase(CollectorsBase collectorsBase)
    {
        _collectorsBase = collectorsBase;
    }

    private IEnumerator RuntimeTimerCoroutine()
    {
        float second = 1f;
        WaitForSeconds oneSecond = new WaitForSeconds(second);

        while (_isWorking)
        {
            _elapsedTime += second;

            string timeString = string.Format("{0:00}:{1:00}",
                Mathf.Floor(_elapsedTime / 60), Mathf.Floor(_elapsedTime % 60));

            _timer.text = timeString;

            yield return oneSecond;
        }
    }
}