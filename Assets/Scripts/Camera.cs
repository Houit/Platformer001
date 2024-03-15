using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Vector3 currentVelocity = Vector3.zero;
    [SerializeField] private float smoothing = 0.5f;
    [SerializeField] private GameObject Player;

    void Update()
    {
        Vector3 target = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.x);
        transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, smoothing);
    }
}