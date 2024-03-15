using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlateform : MonoBehaviour
{
    private Collider2D ColPlatform;
    [SerializeField] private float ResetDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ColPlatform = GetComponent<Collider2D>();
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
        ColPlatform.enabled = false;
        yield return new WaitForSeconds(ResetDelay);
        ColPlatform.enabled = true;
    }

    
}
