using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] private float _normalSensitivity = 1.0f;
    [SerializeField] private float _aimSensitivity = 1.0f;
    [SerializeField] private LayerMask _aimColliderMask = new();
    [SerializeField] private Transform _bloodVFX;
    [SerializeField] private Transform _explosionVFX;
    [SerializeField] private Transform _handSlot;
    [SerializeField] private List<Weapon> _weapons;
    private InputAction _aimAction;
    private InputAction _fireAction;
    private InputAction _weapon1Action;
    private InputAction _weapon2Action;
    private InputAction _weapon3Action;
    private Fire _fire;
    private Transform _hitTransform;
    private Transform _target;
    private Weapon _activeWeapon;
    private Transform _activeWeaponModel;
    private bool _aimModeOn;

    public Weapon ActiveWeapon
    {
        get => _activeWeapon;
    }
    public bool AimModeOn
    {
        get => _aimModeOn;
    }

    private void Start()
    {
        Initialize(); 
    }

    private void Update()
    {
        Vector3 hitPoint = GetHitPoint();
        _fire.SingleShoot(_hitTransform, hitPoint, _bloodVFX, _explosionVFX);
        HandleAimAction(hitPoint);
        SwitchWeapon();
    }

    private void Initialize()
    {
        Cursor.visible = false;
        _target = new GameObject("Target").transform;
        _aimAction = _player.InputActionAsset.FindAction("Aim");
        _fireAction = _player.InputActionAsset.FindAction("Fire");
        _weapon1Action = _player.InputActionAsset.FindAction("Weapon1");
        _weapon2Action = _player.InputActionAsset.FindAction("Weapon2");
        _weapon3Action = _player.InputActionAsset.FindAction("Weapon3");
        _aimAction.Enable();
        _fireAction.Enable();
        _weapon1Action.Enable();
        _weapon2Action.Enable();
        _weapon3Action.Enable();
        _fire = new(this, _fireAction);
        SwitchToWeapon(0);
    }
    private void SwitchWeapon()
    {
        if (_aimModeOn)
        {
            return;
        }

        if (_weapon1Action.triggered)
        {
            SwitchToWeapon(0);
        }
        else if (_weapon2Action.triggered)
        {
            SwitchToWeapon(1);
        }
        else if (_weapon3Action.triggered)
        {
            SwitchToWeapon(2);
        }
    }

    private void SwitchToWeapon(int weaponIndex)
    {
        if (_activeWeaponModel != null)
        {
            Destroy(_activeWeaponModel.gameObject);
        }

        _activeWeapon = _weapons[weaponIndex];
        _activeWeaponModel = _activeWeapon.Initialize(_handSlot);
    }

    private Vector3 GetHitPoint()
    {
        Vector3 hitPoint = Vector3.zero;
        Vector2 screenCenter = new(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        _hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _aimColliderMask))
        {
            _target.position = raycastHit.point;
            hitPoint = raycastHit.point;
            _hitTransform = raycastHit.transform;
        }
        return hitPoint;
    }

    private void HandleAimAction(Vector3 hitPoint)
    {
        if (_aimAction.ReadValue<float>() > 0)
        {
            EnableAimMode(hitPoint);
        }
        else
        {
            DisableAimMode();
        }
    }

    private void EnableAimMode(Vector3 hitPoint)
    {
        if(_activeWeapon == null)
        {
            return;
        }
        _aimModeOn = true;
        _aimCamera.gameObject.SetActive(true);
        _player.SetSensitivity(_aimSensitivity);
        _player.SetRotateOnMove(false);

        Vector3 wolrdAim = hitPoint;
        wolrdAim.y = transform.position.y;
        Vector3 aimDirection = (wolrdAim - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20);
        _player.Animator.SetLayerWeight(_activeWeapon.AnimationLayer, 
            Mathf.Lerp(_player.Animator.GetLayerWeight(_activeWeapon.AnimationLayer), 1f, Time.deltaTime * 10f));
    }

    private void DisableAimMode()
    {
        if (_activeWeapon == null)
        {
            return;
        }
        _aimModeOn = false; 
        _aimCamera.gameObject.SetActive(false);
        _player.SetSensitivity(_normalSensitivity);
        _player.SetRotateOnMove(true);
        _player.Animator.SetLayerWeight(_activeWeapon.AnimationLayer, 
            Mathf.Lerp(_player.Animator.GetLayerWeight(_activeWeapon.AnimationLayer), 0f, Time.deltaTime * 10f));
    }

    private void OnDisable()
    {
        _aimAction.Disable();
        _fireAction.Disable();
        _weapon1Action.Disable();
        _weapon2Action.Disable();
        _weapon3Action.Disable();
    }
}
