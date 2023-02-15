using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * 0.5f * Time.deltaTime);
        if (transform.position.x > 3f)
        {
            transform.position = new Vector3(-1.024f, 0, 0);
        }
    }
}
