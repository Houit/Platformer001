using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected Collider2D ColPlatform;
    [SerializeField] private float ReEnableColliderDelay = 1f;

    private Platformer2024 MyInputActions;

    // Start is called before the first frame update
    public void Start()
    {
        ColPlatform = GetComponent<Collider2D>();

        MyInputActions = new Platformer2024();
        MyInputActions.Gameplay.Enable();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && MyInputActions.Gameplay.Crouch.IsPressed())
        {
            StartCoroutine(ChangeColliderState());
        }

    }

    private IEnumerator ChangeColliderState()
    {
        ColPlatform.enabled = false;
        yield return new WaitForSeconds(ReEnableColliderDelay);
        ColPlatform.enabled = true;
    }
}
