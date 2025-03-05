using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpDelay = 0.3f;  // 🔹 플레이어보다 늦게 점프하는 딜레이

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
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
    }
}
