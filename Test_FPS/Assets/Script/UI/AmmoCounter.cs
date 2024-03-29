using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private Aim _aim;
    [SerializeField] private Text _text;

    private void Update()
    {
        DisplayAmmoCount(); 
    }
    private void DisplayAmmoCount()
    {
        _text.text = _aim.ActiveWeapon.CurrentBulletsNumber.ToString();
    }
}
