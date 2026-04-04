using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            Block block = collision.GetComponent<Block>();

            if (block != null)
            {
                block.TakeHit();
            }

            Destroy(gameObject);
        }

        if (collision.CompareTag("LimitTop"))
        {
            Destroy(gameObject);
        }
    }
}