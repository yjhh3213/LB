using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource audioSource;
    public AudioClip[] Sound; // 0 사격, 1 발사장전, 2 장전, 3 대쉬, 4 카드, 5 좀비, 6 총알없음, 7 탄피
    private int currentPlayingIndex = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Player_SFX(int num)
    {
        if (num == currentPlayingIndex) return;

        currentPlayingIndex = num;

        float volume = (num == 9) ? 0.5f : 1f;
        audioSource.PlayOneShot(Sound[num], volume);

        // PlayOneShot 길이만큼 후에 초기화
        StartCoroutine(ResetPlayingIndex(Sound[num].length));
    }

    private IEnumerator ResetPlayingIndex(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentPlayingIndex = -1;
    }
}
