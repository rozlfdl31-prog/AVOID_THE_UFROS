using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSpeed = 3f;
    public float maxSpeed = 7f;
    public float deadSpeed = 1.5f;
    public Sprite[] sprites;

    Rigidbody2D rb;
    SpriteRenderer sr;

    float wallContactTime = 0f;
    bool isTouchingWall = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (sprites != null && sprites.Length > 0)
        {
            int index = Random.Range(0, sprites.Length);
            sr.sprite = sprites[index];
            sr.color = Color.white;
        }

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        rb.AddForce(randomDir * randomSpeed, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-5f, 5f));
    }

    void FixedUpdate()
    {
        // 완전 멈춤 방지
        if (rb.linearVelocity.magnitude < deadSpeed)
        {
            Vector2 dir = rb.linearVelocity.magnitude < 0.01f
                ? Random.insideUnitCircle.normalized
                : rb.linearVelocity.normalized;
            rb.linearVelocity = dir * deadSpeed;
        }

        // 벽에 0.1초 이상 끼어있으면 강제로 떼어내기
        if (isTouchingWall)
        {
            wallContactTime += Time.fixedDeltaTime;
            if (wallContactTime > 0.1f)
            {
                Vector2 toCenter = ((Vector2)Vector3.zero - rb.position).normalized;

                // 티 안 나게 살짝만 떼어놓기
                rb.position += toCenter * 0.15f;
                rb.linearVelocity = toCenter * Random.Range(minSpeed, maxSpeed);
                wallContactTime = 0f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            isTouchingWall = true;
            wallContactTime = 0f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            isTouchingWall = false;
            wallContactTime = 0f;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
