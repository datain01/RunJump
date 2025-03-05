using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public float jumpDelay = 0.3f;  // 🔹 플레이어보다 늦게 점프하는 딜레이

    [Header("Animation Debug")]
    [SerializeField] private float animSpeed; // ✅ 인스펙터에서 애니메이션 속도 확인

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // ✅ 애니메이터 가져오기
        rb.freezeRotation = true;
    }

    void Update()
    {
        UpdateAnimationSpeed(); // ✅ 게임 속도에 따라 애니메이션 속도 업데이트
    }

    public void JumpWithDelay(float jumpForce)
    {
        StartCoroutine(DelayedJump(jumpForce));
    }

    IEnumerator DelayedJump(float jumpForce)
    {
        yield return new WaitForSeconds(jumpDelay);  // 🔹 일정 시간 후 점프
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        animator.SetTrigger("Jump"); // ✅ 점프 애니메이션 실행
        animator.SetBool("isGrounded", false); // ✅ 공중 상태로 설정
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isGrounded", true); // ✅ 착지 시 Idle로 변경
            animator.ResetTrigger("Jump"); // ✅ 점프 트리거 초기화

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                animator.Play("Idle"); // 즉시 Idle 상태로 변경
            }
        }
    }

    /// <summary>
    /// ✅ GameManager의 속도에 맞춰 애니메이션 속도 조절 (인스펙터에서 확인 가능)
    /// </summary>
    private void UpdateAnimationSpeed()
    {
        if (animator == null || GameManager.instance == null) return;

        float baseSpeed = 2.0f; // 기본 애니메이션 속도
        animSpeed = GameManager.instance.speed / baseSpeed; // ✅ 인스펙터에서 확인 가능하도록 업데이트
        animSpeed = Mathf.Clamp(animSpeed, 0.5f, 3.0f); // 너무 느리거나 빠르지 않게 제한

        animator.speed = animSpeed; // ✅ 애니메이션 속도 적용
    }
}
