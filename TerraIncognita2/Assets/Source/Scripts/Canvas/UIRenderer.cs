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

    private bool _isBaseSelected;
    private bool _isWorking;

    private void Awake()
    {
        _elapsedTime = 0.0f;
        _isWorking = true;

        StartCoroutine(RuntimeTimerCoroutine());
    }

    void Update()
    {        
        if (_isBaseSelected)
        {
            _collectorsCounter.text = _collectorsBase.CollectorsCount.ToString();
            _crystalsCounter.text = _collectorsBase.CollectedCrystals.ToString();
        }        
    }

    public void SetSelectedBase(CollectorsBase collectorsBase)
    {
        _collectorsBase = collectorsBase;

        _isBaseSelected = true;
    }

    private IEnumerator RuntimeTimerCoroutine()
    {
        WaitForSeconds oneSecond = new WaitForSeconds(1f);
        while (_isWorking)
        {
            _elapsedTime += Time.deltaTime;

            string timeString = string.Format("{0:00}:{1:00}",
                Mathf.Floor(_elapsedTime / 60), Mathf.Floor(_elapsedTime % 60));

            _timer.text = timeString;

            yield return oneSecond; 
        }
    }
}