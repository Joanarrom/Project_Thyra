using UnityEngine;

public class FireballMeteor : MonoBehaviour
{
    public float fallSpeed = 20f;
    public GameObject explosionPrefab;

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y <= 0.2f)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}