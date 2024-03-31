using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealthValue;
    private float _currentHealtValue;
    private bool _isDead;
    public UnityAction OnDie;
    public UnityAction OnTakeDamage;
    public bool IsDead
    {
        get => _isDead;
    }
    public float MaxHealthValue
    {
        get => _maxHealthValue;
    }
    public float CurrentHealtValue
    {
        get => _currentHealtValue;
    }

    private void Start()
    {
        _currentHealtValue = _maxHealthValue;
    }

    public void TakeDamage(float damage)
    {
        if (_isDead)
        {
            return;
        }

        _currentHealtValue -= damage;
        OnTakeDamage?.Invoke();
        if (_currentHealtValue <= 0)
        {
            _isDead = true;
            OnDie?.Invoke();
        }
    }
}
