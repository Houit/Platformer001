using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMobile : Platform
{
    protected Dictionary<Transform, Transform> Famille;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(TestTag(collision.gameObject))
        {
            Famille.Add(collision.gameObject.transform, collision.gameObject.transform.parent);
            collision.gameObject.transform.SetParent(transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (TestTag(collision.gameObject))
        {
            Transform AncienParent;
            Famille.TryGetValue(collision.gameObject.transform, out AncienParent);
            collision.gameObject.transform.SetParent(AncienParent, true);
            Famille.Remove(collision.gameObject.transform);
        }
       
    }

    private bool TestTag(GameObject go)
    {
        return (go.gameObject.CompareTag("Player") || go.gameObject.CompareTag("Enemy")) ;
    }
}
