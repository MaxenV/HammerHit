using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameManager.LoseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            gameManager.LoseGame();
        }
    }
}
