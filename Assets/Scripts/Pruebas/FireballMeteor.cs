using UnityEngine;

public class FireballMeteor : MonoBehaviour
{
      public float fallSpeed = 20f;
    public int damage = 100; // Daño que aplicará al jugador

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y <= 0.2f)
        {
            Destroy(gameObject);
        }
    }

    // Detectar colisión con el jugador
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThirdPersonController player = other.GetComponent<ThirdPersonController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // Destruir el proyectil al impactar
        }
    }
}