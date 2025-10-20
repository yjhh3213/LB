using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class CountTimer : MonoBehaviour
{
    [Header("타이머 설정")]
    public int startTime = 60; // 시작 시간 (초)
    private int currentTime;

    [Header("UI 연결")]
    public Text timerText;
    public Text waveText;

    public int CurrentWave { get; private set; } = 1;
    public bool WaveEnded { get; private set; } = true;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerUI();
        UpdateWaveUI();
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            while (currentTime > 0)
            {
                yield return new WaitForSeconds(1f);
                currentTime--;

                // 안전하게 음수 방지
                if (currentTime < 0)
                    currentTime = 0;

                UpdateTimerUI();
            }

            // 0이 되면 웨이브 종료 처리
            TimerEnd();
            yield return null;
        }
    }

    void UpdateTimerUI()
    {
        int minutes = currentTime / 60;
        int seconds = currentTime % 60;
        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateWaveUI()
    {
        if (waveText != null)
            waveText.text = "Wave " + CurrentWave;
    }

    void TimerEnd()
    {
        Debug.Log("웨이브 종료! 다음 웨이브 시작");
        WaveEnded = true;

        CurrentWave++;
        UpdateWaveUI();

        currentTime = startTime; // 다음 웨이브 타이머 초기화
        UpdateTimerUI();
    }

    // 외부에서 WaveEnded 플래그 초기화 가능
    public void ResteWaveFlag()
    {
        WaveEnded = false;
    }
}
