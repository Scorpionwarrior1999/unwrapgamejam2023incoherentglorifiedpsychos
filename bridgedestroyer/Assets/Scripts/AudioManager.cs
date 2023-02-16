using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _source;
    [SerializeField] private AudioClip _clip;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayExplosionSound()
    {
        _source.PlayOneShot(_clip, 7);
    }
}
