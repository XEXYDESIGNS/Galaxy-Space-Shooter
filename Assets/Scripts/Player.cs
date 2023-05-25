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
        //makes it easier to control movement on Player
        float horizonalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        
        /*
        transform.Translate(Vector3.right * horizonalMovement * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalMovement *_speed * Time.deltaTime);
        */
        
        //More efficient! but add a variable to clean up more
        //transform.Translate(new Vector3(horizonalMovement, verticalMovement, 0) * _speed * Time.deltaTime);
        
        Vector3 direction = new Vector3(horizonalMovement, verticalMovement, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
    }
}
