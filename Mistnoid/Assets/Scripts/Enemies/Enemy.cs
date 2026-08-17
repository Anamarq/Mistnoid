using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int soulReward = 10;
    [SerializeField] private float flashDuration = 0.05f;

    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHealth--;
        AudioManager.Instance.PlayHitEnemy();
        StartCoroutine(HitFlash());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFlash()
    {
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.enabled = true;
    }

    private void Die()
    {
        isDead = true;

        ScoreManager.Instance.AddSouls(soulReward);

        Destroy(gameObject);
    }
}

