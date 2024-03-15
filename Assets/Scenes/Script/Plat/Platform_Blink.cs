using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Blink : Platform
{
    private SpriteRenderer Sr;
    private Color OriginalColor;
    private Color GhostColor;
    private bool IsSolid = true;

    [SerializeField] private float GhostAlpha = 0.5f;
    [SerializeField] private float SwitchDelay = 2f;
    [SerializeField] private float SwitchInterval = 4f;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        Sr = GetComponent<SpriteRenderer>();
        OriginalColor = Sr.color;
        GhostColor = Sr.color;
        GhostColor.a = GhostAlpha;
        StartCoroutine(SwitchState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SwitchState()
    {
        while (true)
        {
            Sr.color = OriginalColor;
            ColPlatform.enabled = true;
            yield return new WaitForSeconds(SwitchInterval);

            Sr.color = GhostColor;
            ColPlatform.enabled = false;
            yield return new WaitForSeconds(SwitchInterval);
        }
    }
}
