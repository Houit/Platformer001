using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following_Cam : MonoBehaviour
{
    [SerializeField] private Transform Player;
    private Vector3 CurrentVelocity = Vector3.zero;
    [SerializeField] private float Smoothing = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position = new Vector3(Player.position.x + 6, 0, -10); // Camera follows the player but 6 to the right
        Vector3 Target = new Vector3(Player.position.x, Player.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, Target, ref CurrentVelocity, Smoothing);
    }
}
