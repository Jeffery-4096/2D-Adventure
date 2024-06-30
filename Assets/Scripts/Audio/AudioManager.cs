using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource Jump1;
    public AudioSource Jump2;

    public void JumpSound1()
    {
        if (Jump1 != null)
        {
            Jump1.Play();
        }
        else
        {
            Debug.LogWarning("FXSource is not assigned.");
        }
    }

    public void JumpSound2()
    {
        if (Jump2 != null)
        {
            Jump2.Play();
        }
        else
        {
            Debug.LogWarning("FXSource is not assigned.");
        }
    }
}
