using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthFillImage;
    [SerializeField] private Health _playerHealth;

    void Update()
    {
        _healthFillImage.fillAmount = _playerHealth.CurrentHealtValue / _playerHealth.MaxHealthValue;
    }
}