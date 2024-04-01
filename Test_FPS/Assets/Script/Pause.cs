using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _winMenu;
    private InputActionAsset _inputActionAsset;
    private InputAction _pauseAction;
    private bool _isPaused = false;

    private void Start()
    {
        _inputActionAsset = Resources.Load<InputActionAsset>("Input/Input");
        _pauseAction = _inputActionAsset.FindAction("Pause");
        _pauseAction.Enable();
    }

    private void Update()
    {
        if(_pauseAction.triggered)
        {
            if(_isPaused)
            {
                ResumeGame();
            }
            else
            {
                StopGame();
            }
        }
    }

    private void StopGame()
    {
        Cursor.visible = true;
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
        _isPaused = true; ;
    }

    private void ResumeGame()
    {
        Cursor.visible = false;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        _isPaused = false;
    }

    private void OnDisable()
    {
        _pauseAction.Disable();
    }
}
