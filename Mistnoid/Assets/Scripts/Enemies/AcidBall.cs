using UnityEngine;

public class AcidBall : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 4f;

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Paddle"))
        {
            // TODO: Hit paddle

            Destroy(gameObject);
        }

        if (collision.CompareTag("Limit"))
        {
            Destroy(gameObject);
        }
    }
}