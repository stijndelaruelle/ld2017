using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sjabloon;
using UnityEngine.UI;

public class RegisterInput : MonoBehaviour
{
    [SerializeField]
    private Text m_Text;

    private InputManager m_InputManger;

    private void Start()
    {
        m_InputManger = InputManager.Instance;

        m_InputManger.BindButton("Submit", KeyCode.Return, InputManager.ButtonState.OnRelease);
        m_InputManger.BindButton("Submit", 0, ControllerButtonCode.A, InputManager.ButtonState.OnRelease);

        m_InputManger.BindButton("Cancel", KeyCode.Escape, InputManager.ButtonState.OnRelease);
        m_InputManger.BindButton("Cancel", 0, ControllerButtonCode.B, InputManager.ButtonState.OnRelease);
    }

    private void Update()
    {
        if (m_InputManger.GetButton("Submit"))
        {
            Debug.Log("Submit!");

            if (m_Text != null)
                m_Text.text = "Submit";
        }

        if (m_InputManger.GetButton("Cancel"))
        {
            Debug.Log("Cancel!");

            if (m_Text != null)
                m_Text.text = "Cancel";
        }
    }
    private void OnDestroy()
    {
        if (m_InputManger == null)
            return;

        m_InputManger.UnbindButton("Submit");
        m_InputManger.UnbindButton("Cancel");
    }
}
