using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ToggleGameObjectButton : MonoBehaviour
{
    public GameObject ObjectToToggle;
    public bool ResetSelectionAfterClick;
    private InputAction cancelAction;

    void Awake()
    {
        cancelAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
        cancelAction.Enable();
    }

    void Update()
    {
        if (ObjectToToggle.activeSelf && cancelAction.triggered)
        {
            SetGameObjectActive(false);
        }
    }

    public void SetGameObjectActive(bool active)
    {
        ObjectToToggle.SetActive(active);

        if (ResetSelectionAfterClick)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnDestroy()
    {
        cancelAction.Disable();
    }
}