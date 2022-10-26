using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedScreen : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _failedClip;

    void OnEnable()
    {
        _source.PlayOneShot(_failedClip);
    }

}
