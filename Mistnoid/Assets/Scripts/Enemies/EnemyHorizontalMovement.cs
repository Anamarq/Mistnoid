using UnityEngine;

public class EnemyHorizontalMovement : MonoBehaviour
{
    private float leftLimitX = -7.8f, rightLimitX = 4.4f;
    [SerializeField] private float speed = 2f;

    private bool movingRight = true;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float direction = movingRight ? 1f : -1f;

        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (movingRight && transform.position.x >= rightLimitX)
        {
            transform.position = new Vector3(
                rightLimitX,
                transform.position.y,
                transform.position.z
            );

            ChangeDirection(false);
        }
        else if (!movingRight && transform.position.x <= leftLimitX)
        {
            transform.position = new Vector3(
                leftLimitX,
                transform.position.y,
                transform.position.z
            );

            ChangeDirection(true);
        }
    }

    private void ChangeDirection(bool moveRight)
    {
        movingRight = moveRight;
        spriteRenderer.flipX = !moveRight;
    }
}