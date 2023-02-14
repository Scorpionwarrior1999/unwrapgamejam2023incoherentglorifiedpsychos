using RayFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager instance { get; private set; }

    List<RayfireBomb> bombs = new List<RayfireBomb>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    public void ExplosionButton()
    {
        if (bombs.Count > 0)
        {
            foreach (var bomb in bombs)
            {
                if (bomb != null)
                {
                    Debug.Log("Boom");
                    bomb.Explode(0);
                    Destroy(bomb.gameObject);
                }
            }
        }

        bombs.Clear();
    }

    public void AddBombToList(RayfireBomb bomb)
    {
        if (bomb != null)
            bombs.Add(bomb);
    }
}
