using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    private List<Vector2> CheckpointPosition = new List<Vector2>();
    [SerializeField] private Color TextColor = Color.white;
    [SerializeField] private Vector2 TextOffset;
    [SerializeField] private float TextScale = 20;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++) //boucle pour identifier la position des enfants de la plateforme
        {
            CheckpointPosition.Add(transform.GetChild(i).position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() //numéroter les enfants
    {
        GUI.color = TextColor;
        GUIStyle LabelStyle = GUI.skin.label;
        LabelStyle.fontStyle = FontStyle.Bold;
        LabelStyle.fontSize = TextScale;
        LabelStyle.
        int i = 0;
        foreach(Vector2 point in CheckpointPosition)
        {
            Handles.Label((Vector2)transform.GetChild(i).position, i.ToString(), LabelStyle); //ToString = conversion en chaîne de caractères
        }
        
    }
}
