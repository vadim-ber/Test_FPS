using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed = 2.0f;
    [SerializeField] private float _sprintSpeed = 5.335f;
    [SerializeField][Range(0.0f, 0.3f)] private float _rotationSmoothTime = 0.12f;
    [SerializeField] private float _speedChangeRate = 10.0f;
    [SerializeField] private float _jumpHeight = 1.2f;
    [SerializeField] private float _gravity = -15.0f;
    [SerializeField] private float _jumpTimeout = 0.50f;
    [SerializeField] private float _fallTimeout = 0.15f;
    [SerializeField] private bool _grounded = true;
    [SerializeField] private float _groundedOffset = -0.14f;
    [SerializeField] private float _groundedRadius = 0.28f;
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private GameObject _cinemachineCameraTarget;
    [SerializeField] private float _tcopClamp = 70.0f;
    [SerializeField] private float _bottomClamp = -30.0f;
    [SerializeField] private float _cameraAngleOverride = 0.0f;
    [SerializeField] private float _sensitivity = 1.0f;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private readonly float _terminalVelocity = 53.0f;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    private CharacterController _controller;
    private GameObject _mainCamera;
    private InputActionAsset _inputActionAsset;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    private const float _threshold = 0.01f;
    private bool _rotateOnMove = true;

    public InputActionAsset InputActionAsset
    {
        get => _inputActionAsset;
    }
    public Animator Animator
    {
        get => _animator;
    }

    public void SetSensitivity(float sensitivity)
    {
        _sensitivity = sensitivity;
    }

    public void SetRotateOnMove(bool newRotate)
    {
        _rotateOnMove |= newRotate;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void Initialize()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _controller = GetComponent<CharacterController>();        
        _jumpTimeoutDelta = _jumpTimeout;
        _fallTimeoutDelta = _fallTimeout;
        _inputActionAsset = Resources.Load<InputActionAsset>("Input/Input");
        _moveAction = _inputActionAsset.FindAction("Move");
        _lookAction = _inputActionAsset.FindAction("Look");
        _jumpAction = _inputActionAsset.FindAction("Jump");
        _sprintAction = _inputActionAsset.FindAction("Sprint");
        _moveAction.Enable();
        _lookAction.Enable();
        _sprintAction.Enable();
        _jumpAction.Enable();
        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
        _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers, QueryTriggerInteraction.Ignore);
        _animator.SetBool(_animIDGrounded, _grounded);
    }

    private void CameraRotation()
    {
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();
        if (lookInput.sqrMagnitude >= _threshold)
        {
            float deltaTimeMultiplier = 0.1f;
            _cinemachineTargetYaw += lookInput.x * deltaTimeMultiplier * _sensitivity;
            _cinemachineTargetPitch += -lookInput.y * deltaTimeMultiplier * _sensitivity;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _tcopClamp);

        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private void Move()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        float targetSpeed = _sprintAction.ReadValue<float>() > 0 ? _sprintSpeed : _moveSpeed;
        if (moveInput == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _speedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * _speedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
        Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

        if (moveInput != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotationSmoothTime);

            if(!_rotateOnMove)
            {
                return;
            }
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        _animator.SetFloat(_animIDSpeed, _animationBlend);
        _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
    }

    private void JumpAndGravity()
    {
        if (_grounded)
        {
            _fallTimeoutDelta = _fallTimeout;
            _animator.SetBool(_animIDJump, false);
            _animator.SetBool(_animIDFreeFall, false);
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (_jumpAction.triggered && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

                _animator.SetBool(_animIDJump, true);
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = _jumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _animator.SetBool(_animIDFreeFall, true);
            }
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (_grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z), _groundedRadius);
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
        _sprintAction.Disable();
        _jumpAction.Disable();
    }
}