using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float _missleSpeed = 15.0f;
    
    void Update()
    {
        transform.Translate(Vector3.up * _missleSpeed * Time.deltaTime);
        
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
