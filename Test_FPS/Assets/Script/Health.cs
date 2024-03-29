using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealthValue;
    private float _currentHealtValue;
    private bool _isDead;
    public UnityAction OnDie;
    public bool IsDead
    {
        get => _isDead;
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
        print($"{gameObject.name} ������� {damage} �����");

        if (_currentHealtValue <= 0)
        {
            _isDead = true;
            print($"{gameObject.name} �����");
            OnDie?.Invoke();
        }
    }
}
