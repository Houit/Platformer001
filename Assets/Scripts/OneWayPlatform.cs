using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D colPlatform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetAxisRaw("Vertical") < 0)
        {
            StartCoroutine(ChangeColliderState());
        }
    }

    private IEnumerator ChangeColliderState()
    {
        colPlatform.enabled = false;
        yield return new WaitForSeconds(0.2f);
        colPlatform.enabled = true;
    }
}
