using UnityEngine;

public class RisingWater : MonoBehaviour
{
    public float riseSpeed = 0.2f;
    public float maxHeight = 6.0f;
    public int damage = 1;

    void Update()
    {
        if (transform.position.y < maxHeight)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller =
            other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ChangeHealth(-damage);
        }
    }
}
