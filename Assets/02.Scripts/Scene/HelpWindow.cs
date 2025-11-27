using UnityEngine;

public class HelpWindow : MonoBehaviour
{
    public GameObject helpPanel;
    public GameObject startButton;
    public GameObject quitButton;

    public void OpenHelp()
    {
        helpPanel.SetActive(true);

        // 버튼 숨기기
        startButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void CloseHelp()
    {
        helpPanel.SetActive(false);

        // 버튼 다시 보이게
        startButton.SetActive(true);
        quitButton.SetActive(true);
    }
}
