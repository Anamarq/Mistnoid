using UnityEngine;

public class EnemyHorizontalMovement : MonoBehaviour
{
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
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

        if (movingRight && transform.position.x >= rightPoint.position.x)
        {
            transform.position = new Vector3(
                rightPoint.position.x,
                transform.position.y,
                transform.position.z
            );

            ChangeDirection(false);
        }
        else if (!movingRight && transform.position.x <= leftPoint.position.x)
        {
            transform.position = new Vector3(
                leftPoint.position.x,
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