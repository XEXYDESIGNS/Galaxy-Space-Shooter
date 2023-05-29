using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8;
    
    void Update()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        
        if (transform.position.y > 7f)
        {
            if (transform.parent == true)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
