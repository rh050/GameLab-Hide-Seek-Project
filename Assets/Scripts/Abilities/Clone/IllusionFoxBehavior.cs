using UnityEngine;

public class IllusionFoxBehavior : MonoBehaviour
{
    [HideInInspector] public float lifetime = 7f;
    private Vector2 moveDirection = Vector2.zero;

    public float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // אם לא הוגדר כיוון חיצוני — ניישם ברירת מחדל
        if (moveDirection == Vector2.zero)
            moveDirection = Vector2.up;

        if (lifetime > 0f)
            Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        UpdateAnimation(moveDirection);
    }

    private void UpdateAnimation(Vector2 dir)
    {
        bool isMoving = dir != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("InputX", dir.x);
        animator.SetFloat("InputY", dir.y);
    }

    /// <summary>
    /// קובע את הכיוון שבו תנוע האשליה.
    /// חובה להיקרא מ-FoxAbility מיד אחרי Instantiate.
    /// </summary>
    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
    }
}
