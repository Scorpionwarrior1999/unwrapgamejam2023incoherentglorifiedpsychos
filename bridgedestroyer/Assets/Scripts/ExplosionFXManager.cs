using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _smallExplosionPrefab;
    [SerializeField] private ParticleSystem _middleExplosionPrefab;
    [SerializeField] private ParticleSystem _bigExplosionPrefab;

    private ParticleSystem _smallExplosion;
    private ParticleSystem _middleExplosion;
    private ParticleSystem _bigExplosion;

    public void EmitSmallExplosion(Vector3 spawnPosition)
    {
        _smallExplosion = Instantiate(_smallExplosionPrefab, spawnPosition, Quaternion.identity);
        _smallExplosion.Play();
    }

    public void EmitMiddleExplosion(Vector3 spawnPosition)
    {
        _middleExplosion = Instantiate(_middleExplosionPrefab, spawnPosition, Quaternion.identity);
        _middleExplosion.Play();
    }

    public void EmitBigExplosion(Vector3 spawnPosition)
    {
        _bigExplosion = Instantiate(_bigExplosionPrefab, spawnPosition, Quaternion.identity);
        _bigExplosion.Play();
    }
}
