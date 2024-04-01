using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string SceneName = "";
    private InputAction submitAction;

    void Awake()
    {
        submitAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/enter");
        submitAction.Enable();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject
            && submitAction.triggered)
        {
            LoadTargetScene();
        }
    }

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(SceneName);
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        submitAction.Disable();
    }
}