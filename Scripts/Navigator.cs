using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Navigator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toRegister()
    {
        SceneManager.LoadScene("Register");
    }

    public void toLogIn()    
    {
        SceneManager.LoadScene("LogIn");
    }

}
