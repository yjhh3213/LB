using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject inGameUI;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;

    [Header("Game Over Texts")]
    public TMP_Text deadWaveText;
    public TMP_Text playTimeText;
    public TMP_Text killCountText;

    [Header("Game Clear Texts")]
    public TMP_Text clearRecordText;  // 최고 기록
    public TMP_Text clearTimeText;    // 이번 클리어 시간
    public TMP_Text clearKillText;    // 처치 수

    [Header("Game Data")]
    public int currentWave;
    public int killCount;
    public float playTime;

    private bool isGameOver = false;
    private float bestRecord = 0f; //최단 클리어 시간 저장용

    [Header("Player Reference")]
    public PlayerCtrl pc; // Inspector에서 연결

    private void Start()
    {
        if (pc == null)
            Debug.LogError("? PlayerCtrl(pc)가 Inspector에 연결되어 있지 않습니다!");

        // 저장된 최고 기록 로드
        bestRecord = PlayerPrefs.GetFloat("BestRecord", 0f);
    }

    private void Update()
    {
        if (!isGameOver)
            playTime += Time.deltaTime;
    }

    // ==========================================
    //  ?? 게임 오버 (PlayerCtrl 호출)
    // ==========================================
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // 플레이어 데스 애니메이션을 보여주기 위해 지연 실행
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        // 0.6초 동안 플레이어 데스 애니메이션 재생
        yield return new WaitForSecondsRealtime(0.6f);

        // UI 표시
        ShowGameOverUI();
    }

    // 기존 GameOver 로직을 분리한 부분
    private void ShowGameOverUI()
    {
        if (inGameUI != null)
            inGameUI.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (deadWaveText != null)
            deadWaveText.text = $"당신이 죽은 웨이브: {currentWave}";

        if (killCountText != null)
            killCountText.text = $"당신이 죽인 몬스터 수: {killCount} 마리";

        if (playTimeText != null)
        {
            int min = Mathf.FloorToInt(playTime / 60);
            int sec = Mathf.FloorToInt(playTime % 60);
            playTimeText.text = $"플레이 시간: {min:00}:{sec:00}";
        }

        Time.timeScale = 0f;
    }

    // ==========================================
    //  ?? 게임 클리어 (모든 웨이브 완료 시 SpawnManager 등에서 호출)
    // ==========================================
    public void GameClear()
    {
        if (isGameOver) return;
        isGameOver = true;

        inGameUI.SetActive(false);
        gameClearPanel.SetActive(true);

        // 최고 기록 갱신 (최단 클리어 시간)
        if (bestRecord == 0f || playTime < bestRecord)
        {
            bestRecord = playTime;
            PlayerPrefs.SetFloat("BestRecord", bestRecord);
            PlayerPrefs.Save();
        }

        // 텍스트 출력
        if (clearRecordText != null)
            clearRecordText.text = $"최고 기록 : {FormatTime(bestRecord)}";

        if (clearKillText != null)
            clearKillText.text = $"당신이 죽인 몬스터 수 : {killCount} 마리";

        if (clearTimeText != null)
            clearTimeText.text = $"플레이 시간 : {FormatTime(playTime)}";

        Time.timeScale = 0f;
    }

    // ==========================================
    //  ?? 다시하기
    // ==========================================
    public void ReplayGame()
    {
        Time.timeScale = 1f;

        // ?? 모든 코루틴 정지 → 씬 로드 중 오류 방지
        StopAllCoroutines();

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }


    // ==========================================
    //  ?? 타이틀로
    // ==========================================
    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    // ==========================================
    //  ?? 시간 포맷 함수 (00:00)
    // ==========================================
    private string FormatTime(float t)
    {
        int min = Mathf.FloorToInt(t / 60);
        int sec = Mathf.FloorToInt(t % 60);
        return $"{min:00}:{sec:00}";
    }
}
