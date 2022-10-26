using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonScreen : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _wonClip, _choirClip;

    void OnEnable()
    {
        _source.PlayOneShot(_wonClip);
        _source.PlayOneShot(_choirClip);
    }

}
