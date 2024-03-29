using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private Health _health;
    public void InflictDamage(float damage)
    {
        if (_health == null)
        {
            return;
        }
        var totalDamage = damage;
        
        _health.TakeDamage(totalDamage);        
    }
}
