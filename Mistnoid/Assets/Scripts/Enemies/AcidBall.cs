using UnityEngine;

public class AcidBall : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 4f;

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // TODO: Hit paddle
            Debug.Log("HIT");
            PlayerController.Instance.LosePaddleLive();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Limit"))
        {
            Destroy(gameObject);
        }
    }
}