using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public int amount = 1;
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller =
            other.GetComponent<PlayerController>();

        if (controller != null
            && controller.health < controller.maxHealth)
        {
            controller.ChangeHealth(amount);
            controller.PlaySound(collectedClip);
            Destroy(gameObject);
        }
    }
}
