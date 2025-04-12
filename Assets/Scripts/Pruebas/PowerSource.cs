using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public BossController boss;

    public void TakeDamage(int amount)
    {
        // Simula daño, podrías agregar vida si quieres
        boss.PowerSourceDestroyed();
        Destroy(gameObject);
    }
}