using UnityEngine;
using UnityEngine.InputSystem;

public class Fire
{
    private Aim _aim;
    private InputAction _action;
    private Transform _previousTransform;
    private Damageable _previousDamageable;

    public Fire(Aim aim, InputAction action)
    {
       _aim = aim;
       _action = action;
    }
    public void Shoot(Transform hitTransform, Vector3 hitPoint, Transform blood, Transform explosion)
    {
        if(_aim.ActiveWeapon == null || !_aim.AimModeOn || _aim.ActiveWeapon.CurrentBulletsNumber <= 0)
        {
            return;
        }
        if(_action.triggered && hitTransform != null)
        {
            _aim.Player.Animator.SetTrigger("Hit");
            _aim.Player.Animator.SetLayerWeight(5, _aim.ActiveWeapon.Recoil);
            _aim.PlaySound.clip = _aim.ActiveWeapon.AudioClip;
            int bulletsFire = _aim.ActiveWeapon.CurrentBulletsNumber >= _aim.ActiveWeapon.BulletsPerShot ?
                _aim.ActiveWeapon.BulletsPerShot : _aim.ActiveWeapon.CurrentBulletsNumber;
            _aim.ActiveWeapon.SetBulletNumber(-bulletsFire);
            _aim.PlaySound.Play();
            _aim.SetFireTimer(0);
            GameObject.Instantiate(_aim.ActiveWeapon.ShootVFX, _aim.ActiveWeaponModel);
            for (int i = 0; i < bulletsFire; i++)
            {                
                if (hitTransform.CompareTag("Damageable"))
                {                    
                    if (hitTransform != _previousTransform)
                    {
                        _previousDamageable = hitTransform.GetComponent<Damageable>();
                    }
                    _previousTransform = hitTransform;
                    _previousDamageable.InflictDamage(_aim.ActiveWeapon.HPDamagePerShot);
                    GameObject.Instantiate(blood, hitPoint, Quaternion.identity);
                }
                else
                {
                    GameObject.Instantiate(explosion, hitPoint, Quaternion.identity);
                }
            }            
        }
    }
}
