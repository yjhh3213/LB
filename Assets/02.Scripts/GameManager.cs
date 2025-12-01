using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject inGameUI;        // 기존 게임 UI (Wave, 체력, 타이머 등)
    public GameObject gameOverPanel;   // 게임 오버 패널
    public Image redFade;              // 붉은 화면 효과 이미지

    [Header("Game Info")]
    public TMP_Text deadWaveText;
    public TMP_Text playTimeText;
    public TMP_Text killCountText;

    public int currentWave;
    public int killCount;
    public float playTime;

    private bool isGameOver = false;

    public PlayerCtrl pc;

    private void Start()
    {

    }
    void Update()
    {
        if (!isGameOver)
            playTime += Time.deltaTime;
    }

    // 외부에서 GameOver() 호출만 하면 모든 연출이 자동 작동됨
    public void GameOver()
    {
        if (pc.dead) return;  // 중복 방지
        isGameOver = true;

        // 1. 기존 UI 비활성화
        inGameUI.SetActive(false);

        // 2. GameOver Panel 활성화
        gameOverPanel.SetActive(true);

        // 3. 통계 표시
        deadWaveText.text = "당신이 죽은 웨이브: " + currentWave;
        killCountText.text = "당신이 죽인 몬스터 수: " + killCount + " 마리";

        int minute = Mathf.FloorToInt(playTime / 60);
        int second = Mathf.FloorToInt(playTime % 60);
        playTimeText.text = $"플레이 시간: {minute:D2}:{second:D2}";

        // 4. 붉은 화면 페이드 인
        StartCoroutine(FadeRedScreen());
    }

    IEnumerator FadeRedScreen()
    {
        float t = 0;
        Color c = redFade.color;

        while (t < 1f)
        {
            t += Time.deltaTime * 0.5f; // 연출 속도
            c.a = Mathf.Lerp(0, 0.6f, t);
            redFade.color = c;
            yield return null;
        }
    }

    // 다시하기
    public void ReplayGame()
    {
        // 현재 씬 다시 로드
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
