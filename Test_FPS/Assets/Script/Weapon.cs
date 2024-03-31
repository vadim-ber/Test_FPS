using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/create weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Transform _WeaponPrefab;
    [SerializeField] private Transform _shootVFX;
    [SerializeField] private float _modelScaleMultipler = 1f;
    [SerializeField] private int _animationLayer;
    [SerializeField] private int _maxBulletsNumber = 30;
    [SerializeField] private int _hpDamagePerShot;
    [SerializeField] private float _secondsBetweenShots = 1f;
    [SerializeField] private int _bulletsPerShot = 1;
    [SerializeField][Range(0, 1)] float _recoil;
    [SerializeField] private AudioClip _shootSound;

    private int _currentBulletsNumber;

    public string Name
    {
        get => _name;
    }
    public int AnimationLayer
    {
        get => _animationLayer;
    }
    public int MaxBulletsNumber
    {
        get => _maxBulletsNumber;
    }
    public int HPDamagePerShot
    {
        get => _hpDamagePerShot;
    }
    public float ShotsPerSecond
    {
        get => _secondsBetweenShots;
    }
    public float Recoil
    {
        get => _recoil;
    }
    public Transform ShootVFX
    {
        get => _shootVFX;
    }
    public AudioClip AudioClip
    {
        get => _shootSound;
    }
    public int CurrentBulletsNumber
    { 
        get => _currentBulletsNumber; 
    }
    public int BulletsPerShot
    {
        get => _bulletsPerShot;
    }

    public void SetBulletNumber(int value)
    {
        _currentBulletsNumber += value;
        if(_currentBulletsNumber < 0)
        {
            _currentBulletsNumber = 0;
        }
        if(_currentBulletsNumber > _maxBulletsNumber)
        {
            _currentBulletsNumber = _maxBulletsNumber;
        }
    }

    public void Initialize()
    {
        _currentBulletsNumber = _maxBulletsNumber;
    }

    public Transform Create(Transform slot)
    {
        var weapon = GameObject.Instantiate(_WeaponPrefab, slot.position, slot.rotation);
        weapon.SetParent(slot);
        weapon.localScale = new Vector3(_modelScaleMultipler, _modelScaleMultipler, _modelScaleMultipler);
        return weapon;
    }
}
