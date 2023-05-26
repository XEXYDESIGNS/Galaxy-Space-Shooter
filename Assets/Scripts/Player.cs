using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int startPositionY = -3;

    [SerializeField]
    private float _speed = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        // Take the current position of the Player and set the position
        // Set the new position to center/lower/middle of the screen
        transform.position = new Vector3(0, startPositionY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        float horizonalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        

        Vector3 direction = new Vector3(horizonalMovement, verticalMovement, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        float maxHeight = 5.9f;
        float minHeight = -3.8f;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minHeight, maxHeight), 0);


        float maxBoundary = 11f;
        float minBoundary = -11f;
        
        if (transform.position.x > maxBoundary)
        {
            transform.position = new Vector3(minBoundary, transform.position.y, 0);
        }
        else if (transform.position.x < minBoundary)
        {
            transform.position = new Vector3(maxBoundary, transform.position.y, 0);
        }
    }
}
