using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float thrustForce = 1f;

    public GameObject boosterFlame;
    public GameObject explosionEffect;

    Rigidbody2D rb;
    bool wasBoosting = false; // 이전 프레임 부스터 상태

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 게임 시작 전이면 입력 무시
        if (GameManager.instance != null && !GameManager.instance.IsGameStarted()) return;

        bool isBoosting = Mouse.current.leftButton.isPressed;

        // 부스터 이펙트
        if (boosterFlame != null)
            boosterFlame.SetActive(isBoosting);

        // 부스터 소리 (켜짐/꺼짐 전환 시에만!)
        if (isBoosting && !wasBoosting)
        {
            if (SoundManager.instance != null)
                SoundManager.instance.PlayBooster();
        }
        else if (!isBoosting && wasBoosting)
        {
            if (SoundManager.instance != null)
                SoundManager.instance.StopBooster();
        }
        wasBoosting = isBoosting;

        // 이동
        if (isBoosting)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // 부스터 소리 끄기
            if (SoundManager.instance != null)
            {
                SoundManager.instance.StopBooster();
                SoundManager.instance.PlaySFX(SoundManager.instance.explosionClip);
            }

            GameManager.instance.GameOver();

            if (explosionEffect != null)
                Instantiate(explosionEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}