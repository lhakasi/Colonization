using System;
using TMPro;
using UnityEngine;

public class RealTimeRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;

    void Update()
    {
        DateTime currentTime = DateTime.Now;

        string timeString = currentTime.ToString("HH:mm");

        _timeText.text = timeString;
    }
}