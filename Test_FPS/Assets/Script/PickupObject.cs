using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private ShootingSystem _shootingSystem;

    private void OnTriggerEnter(Collider other)
    {        
        if (!other.gameObject.CompareTag("Player"))
        {           
            return;
        }
        _shootingSystem = other.gameObject.GetComponent<ShootingSystem>();
        _shootingSystem.ActiveWeapon.AddBullets((int)(_shootingSystem.ActiveWeapon.MaxBulletsNumber / 3));
        Destroy(gameObject);
    }
}
