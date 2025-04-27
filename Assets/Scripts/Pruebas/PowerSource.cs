using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public BossController boss;

    public void TakeDamage(int amount)
    {
        
        boss.PowerSourceDestroyed();
        Destroy(gameObject);
    }
}