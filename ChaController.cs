using UnityEngine;
using System.Collections;

public class ChaController : MonoBehaviour
{
    public static ChaController instance;

    [Header("Character Stats")]
    public float jumpForce = 5f;
    private int jumpCount = 0;
    public int hp = 3;
    private int maxHP = 3;
    private bool isInvincible = false;
    private bool isGrounded = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color originalColor;

    [Header("HP UI Reference")] 
    [SerializeField] private HPBarManager hpBarManager; // ✅ 인스펙터에서 직접 할당

    [Header("Animation Debug")]
    [SerializeField] private float animSpeed;

    [Header("Audio Sources")]
    public AudioSource jumpAudioSource;
    public AudioSource hitAudioSource;
    public AudioSource scoreAudioSource;
    public AudioSource healAudioSource;
    public AudioSource maxAudioSource;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalColor = spriteRenderer.color;

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        ResetHP();
        UpdateAnimationSpeed();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && jumpCount < 2)
        {
            Jump();
        }

        UpdateAnimationSpeed();
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount++;

        isGrounded = false;
        animator.SetTrigger("Jump");
        animator.SetBool("isGrounded", false);
        PlayAudio(jumpAudioSource);

        foreach (var follower in GameObject.FindGameObjectsWithTag("Follower"))
        {
            follower.GetComponent<Follower>()?.JumpWithDelay(jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            isGrounded = true;
            animator.SetBool("isGrounded", true);
            animator.ResetTrigger("Jump");

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                animator.Play("Idle");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Obstacle":
                if (!isInvincible) TakeDamage();
                Destroy(other.gameObject);
                break;

            case "HealthItem": // 기존 HP 회복 아이템
                Heal(1);
                PlayAudio(healAudioSource);
                Destroy(other.gameObject);
                break;

            case "MaxHealthItem": // ✅ HP 최대치 증가 아이템
                IncreaseMaxHP(1);
                PlayAudio(maxAudioSource);
                Destroy(other.gameObject);
                break;

            case "ScoreItem":
                GameManager.instance.IncreaseScore(1000);
                PlayAudio(scoreAudioSource);
                Destroy(other.gameObject);
                break;
        }
    }

    // ✅ 최대 HP 증가 & 풀 체력 회복
    public void IncreaseMaxHP(int amount)
    {
        maxHP += amount; // 최대 HP 증가
        hp = maxHP; // 현재 HP를 최대 HP로 설정

        if (hpBarManager != null)
        {
            hpBarManager.UpdateHPUI(hp, maxHP);
        }
        else
        {
            Debug.LogError("hpBarManager is not assigned in the Inspector!");
        }
    }

    void TakeDamage()
    {
        hp--;

        if (hpBarManager != null)
        {
            hpBarManager.UpdateHPUI(hp, maxHP);
        }

        if (hp <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(HitEffect());
        }
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHP);

        if (hpBarManager != null)
        {
            hpBarManager.UpdateHPUI(hp, maxHP);
        }
    }

    public void ResetHP()
    {
        hp = maxHP;

        if (hpBarManager != null)
        {
            hpBarManager.UpdateHPUI(hp, maxHP);
        }
        else
        {
            Debug.LogError("hpBarManager is not assigned in the Inspector!");
        }
    }

    IEnumerator HitEffect()
    {
        isInvincible = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
        isInvincible = false;
    }

    void GameOver()
    {
        GameManager.instance.GameOver();
    }

    private void PlayAudio(AudioSource audioSource)
    {
        audioSource?.Play();
    }

    private void UpdateAnimationSpeed()
    {
        if (animator == null || GameManager.instance == null) return;

        float baseSpeed = 2.0f; // 기본 애니메이션 속도
        animSpeed = GameManager.instance.speed / baseSpeed;
        animSpeed = Mathf.Clamp(animSpeed, 0.5f, 3.0f);

        animator.speed = animSpeed; 
    }
}
