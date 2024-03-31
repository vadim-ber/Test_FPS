using UnityEngine;
using UnityEngine.UI;

public class WorldspaceHealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Transform _healthBarPivot;
    [SerializeField] private bool _hideFullHealthBar = true;

    void Update()
    {        
        _healthBarImage.fillAmount = _health.CurrentHealtValue / _health.MaxHealthValue;       
        _healthBarPivot.LookAt(Camera.main.transform.position);        
        if (_hideFullHealthBar)
        {
            _healthBarPivot.gameObject.SetActive(_healthBarImage.fillAmount != 1);
        }           
    }
}