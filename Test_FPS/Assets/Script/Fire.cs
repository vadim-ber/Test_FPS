using UnityEngine;
using UnityEngine.InputSystem;

public class Fire
{
    private Aim _aim;
    private InputAction _action;

    public Fire(Aim aim, InputAction action)
    {
       _aim = aim;
       _action = action;
    }
    public void SingleShoot(Transform hitTransform, Vector3 hitPoint, Transform blood, Transform explosion)
    {
        if(_aim.ActiveWeapon == null || !_aim.AimModeOn)
        {
            return;
        }
        if(_action.triggered && hitTransform != null)
        {
            _aim.Player.Animator.SetTrigger("Hit");
            if (hitTransform.CompareTag("BulletTarget"))
            {
                GameObject.Instantiate(blood, hitPoint, Quaternion.identity);
            }
            else
            {
                GameObject.Instantiate(explosion, hitPoint, Quaternion.identity);
            }
        }
    }
}
