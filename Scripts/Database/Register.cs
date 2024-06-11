using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour {
    public InputField UsernameInput;
    public InputField PasswordInput;
    public InputField PasswordRepeatInput;
    public Button RegisterButton;

    // Use this for initialization
    void Start () {
        RegisterButton.onClick.AddListener(() => {
            StartCoroutine(Main.Instance.Web.Register(UsernameInput.text, PasswordInput.text, PasswordRepeatInput.text));
        });
    }
}
