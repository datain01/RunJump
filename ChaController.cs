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
    [SerializeField] private bool isGrounded = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color originalColor;

    [Header("Audio Sources")]
    public AudioSource jumpAudioSource;
    public AudioSource hitAudioSource;
    public AudioSource scoreAudioSource;
    public AudioSource healAudioSource;

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
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && jumpCount < 2)
        {
            Jump();
        }
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
            case "HealthItem":
                Heal(1);
                PlayAudio(healAudioSource);
                Destroy(other.gameObject);
                break;
            case "ScoreItem":
                GameManager.instance.IncreaseScore(1000);
                PlayAudio(scoreAudioSource);
                Destroy(other.gameObject);
                break;
        }
    }

    void TakeDamage()
    {
        hp--;
        HPUIManager.instance.UpdateHPUI(hp);
        PlayAudio(hitAudioSource);

        if (hp <= 0)
        {
            GameOver();
        }
        else
        {
            if (SettingManager.instance?.IsVibrationOn() == true)
            {
                Handheld.Vibrate();
            }
            StartCoroutine(HitEffect());
        }
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHP);
        HPUIManager.instance.UpdateHPUI(hp);
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

    public void ResetHP()
    {
        hp = maxHP;
        HPUIManager.instance.UpdateHPUI(hp);
    }

    private void PlayAudio(AudioSource audioSource)
    {
        audioSource?.Play();
    }
}
