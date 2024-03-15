using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformParkour : PlatformMobile
{
    private List<Vector2> CheckPointPositions= new List<Vector2>();
    private int NextCheckPoint = 0;
    [SerializeField] private float CheckPointDelay;
    [SerializeField] private float StartDelay;
    [SerializeField] private float EndDelay;
    [SerializeField] private float Speed = 5f;
    [SerializeField] private bool IsPingPong;
    [SerializeField] private Color TextColor = Color.white;
    [SerializeField] private Vector2 TextOffset;
    [SerializeField] private int TextSize = 20;
    private int Sens = -1;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        for (int i = 0; i < transform.childCount; i++)
        {
            CheckPointPositions.Add(transform.GetChild(i).position);
        }
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Move()
    {
        float Step = Speed * Time.deltaTime;
        Vector3 Target = CheckPointPositions[NextCheckPoint];

        transform.position = Vector2.MoveTowards(transform.position, Target, Step);
        if (transform.position == Target)
        {
            yield return new WaitForSeconds(CheckPointDelay);

            if(NextCheckPoint == 0)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            if ((NextCheckPoint == CheckPointPositions.Count -1) && !IsPingPong)
            {
                yield return new WaitForSeconds(EndDelay);
            }

            if ((Sens > 0 && NextCheckPoint < CheckPointPositions.Count - 1) || (Sens < 0 && NextCheckPoint > 0))
            {
                NextCheckPoint += Sens;
            }
            else
            {
                if (IsPingPong)
                {
                    Sens = -Sens;
                }
                else
                {
                    NextCheckPoint = 0;
                }

            }
        }
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(Move());
    }

    private void OnDrawGizmos()
    {
        GUI.color = TextColor;
        GUIStyle LabelStyle = GUI.skin.label;
        LabelStyle.fontStyle = FontStyle.Bold;
        LabelStyle.fontSize = TextSize;
        LabelStyle.alignment = TextAnchor.MiddleCenter;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Handles.Label((Vector2)transform.GetChild(i).position + TextOffset, i.ToString(), LabelStyle);
            if (i < transform.childCount - 1)
            {
                Handles.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position, 2f);
            }
            else if(!IsPingPong)
            {
                Handles.DrawLine(transform.GetChild(i).position, transform.GetChild(0).position, 2f);
            }
        }
    }
}
