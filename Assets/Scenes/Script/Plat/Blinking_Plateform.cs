using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ghosttohumanplatform : MonoBehaviour
{
    private Collider2D Col;
    private SpriteRenderer Sr; 
    private Color OriginalColor;
    private Color GhostColor;
    private bool IsSolid = true;

    [SerializeField] private float GhostAlpha = 0.5f;
    [SerializeField] private float SwitchDelay = 2f;
    [SerializeField] private float SwitchInterval = 4f;

    // Start is called before the first frame update
    void Start()
    {
        Col = GetComponent<Collider2D>();
        Sr = GetComponent<SpriteRenderer>();
        OriginalColor = Sr.color;
        GhostColor = Sr.color;
        GhostColor.a = GhostAlpha;
        StartCoroutine(SwitchState());
    }


   /*private void BeSolid()
    {
        Col.enabled = true;
        Sr.color = OriginalColor;
        Invoke("BeGhost", SwitchInterval);
    }

    private void BeGhost()
    {
        Col.enabled = false;
        Sr.color = new Color(Sr.color.r, Sr.color.g, Sr.color.b, 0.5f);
        Invoke("BeGhost", SwitchInterval);
    }*/


    private IEnumerator SwitchState()
    {
        while (true)
        {
            Sr.color = OriginalColor;
            Col.enabled = true;
            yield return new WaitForSeconds(SwitchInterval);

            Sr.color = GhostColor;
            Col.enabled = false;
            yield return new WaitForSeconds(SwitchInterval);
        }
    }
 

}
