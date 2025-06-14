using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Button LoginButton;

    // Use this for initialization
    void Start () {
        LoginButton.onClick.AddListener(() => {
            StartCoroutine(Main.Instance.Web.Login(UsernameInput.text, PasswordInput.text));
        });
    }
}
