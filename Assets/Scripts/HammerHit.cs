
using UnityEngine;

public class HammerHit : MonoBehaviour
{
    [SerializeField] private Transform hitPoint;
    [SerializeField] private Vector2 hitSize = new Vector2(0.7f, 0.7f);

    public void Hit()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            hitPoint.position,
            hitSize,
            hitPoint.eulerAngles.z
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Box"))
            {
                Destroy(hit.gameObject);
            }
        }
    }
}
