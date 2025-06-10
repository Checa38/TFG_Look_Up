using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    public static Main Instance;
    [SerializeField] private AudioSource myAudioSource;

    public Web Web;

    // Use this for initialization
    void Start () {
        float volume = PlayerPrefs.GetFloat("volumenEfectos", 0.5f);
        myAudioSource.volume = volume;
        Instance = this;
        Web = GetComponent<Web>();
    }
}
