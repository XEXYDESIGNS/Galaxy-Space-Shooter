using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int startPositionY = 3;

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
        //if press right arrow on keyboard, move right
        if(Input.GetKey(KeyCode.RightArrow))
            //new Vector3(-5, 0, 0) * 5 * real time
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        //if press left arrow on keyboard, move left
        if(Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }
}
