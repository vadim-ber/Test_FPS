using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/create weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Transform _WeaponPrefab;
    [SerializeField] private float _scaleMultipler = 1f;
    [SerializeField] private int _animationLayer;
    [SerializeField] private int _maxBulletsNumber;
    [SerializeField] private int _hpDamagePerShot;
    [SerializeField] private AudioClip _shootSound;

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
    public AudioClip AudioClip
    {
        get => _shootSound;
    }

    public Transform Initialize(Transform slot)
    {
        var weapon = GameObject.Instantiate(_WeaponPrefab, slot.position, slot.rotation);
        weapon.SetParent(slot);
        weapon.localScale = new Vector3(_scaleMultipler, _scaleMultipler, _scaleMultipler);
        return weapon;
    }
}
