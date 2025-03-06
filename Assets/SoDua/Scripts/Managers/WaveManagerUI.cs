using TMPro;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    // ==============================
    // === Props & Fields
    // ==============================

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timerText;

    // ==============================
    // === Methods
    // ==============================

    public void UpdateWaveText(string waveString) => waveText.text = waveString;
    public void UpdateTimerText(string timerString) => timerText.text = timerString;
}
