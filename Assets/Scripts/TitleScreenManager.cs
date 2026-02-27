using UnityEngine;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject titleScreenUI;      // 타이틀 화면 전체 (배경 이미지 + 텍스트)
    public TextMeshProUGUI pressKeyText;  // "PRESS ANY KEY TO START" 텍스트

    private bool gameStarted = false;
    private float blinkTimer = 0f;

    void Update()
    {
        if (gameStarted) return;

        // 텍스트 깜빡이기 (0.5초 간격)
        blinkTimer += Time.unscaledDeltaTime;
        if (pressKeyText != null)
        {
            // sin 함수로 부드럽게 깜빡 (알파값 0.2 ~ 1.0 반복)
            float alpha = Mathf.Lerp(0.2f, 1f, (Mathf.Sin(blinkTimer * 3f) + 1f) / 2f);
            Color c = pressKeyText.color;
            c.a = alpha;
            pressKeyText.color = c;
        }

        // 아무 키 누르면 게임 시작!
        if (Input.anyKeyDown)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;

        // 타이틀 화면 숨기기
        if (titleScreenUI != null)
            titleScreenUI.SetActive(false);

        // 게임 시작 알림 (GameManager에게)
        if (GameManager.instance != null)
            GameManager.instance.StartGame();
    }
}
