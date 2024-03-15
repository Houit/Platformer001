using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerTest : MonoBehaviour
{
    //Player Data
    [SerializeField] private float PlayerSpeed; //vitesse du joueur
    [SerializeField] private float Smoothing; //adoucie la courbe de déplacement (le perso ne passe pas de 0 à 100 en 1sec)
    private int HorizontalMove; //permet de récup l'axe de déplacement horizontal
    Rigidbody2D RbPlayer; //permet de récup le rigidbody du joueur
    private Vector2 Velocity = Vector2.zero;

    //Input System
    private Platformer2024 MyInputActions;


    //dimension pour le gizmo (à écrire à ecrire dans l'inspector)
    //jump
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask JumpLayerMask;
    private float OriginalGravityScale; //valeur de gravité de base pour le joueur
    [SerializeField] private float GravityMultiplier = 2;
    [SerializeField] private float FallGravityScaleMultiplier = 1.2f; //rend le joueur plus lourd -> chute plus rapide
    [SerializeField] private float JumpingGravityScaleMultiplier = 0.5f; //rend le joueur plus léger -> saute plus haut
    [SerializeField] private float CoyoteTime; //donne au joueur un délai supplémentaire pour sauter

    //dimension pour le gizmo (à écrire à ecrire dans l'inspector)
    [SerializeField] private float GroundCheckWidth;
    [SerializeField] private float GroundCheckHeight;
    [SerializeField] private float WallCheckWidth;
    [SerializeField] private float WallCheckHeight;


    //bools
    [SerializeField] private bool IsJumping;
    [SerializeField] private bool IsGrounded = false;
    private bool Ground;
    [SerializeField] private bool IsTouchingWallLeft;
    [SerializeField] private bool IsTouchingWallRight;


    //checks
    private Vector2 GroundCheckPosition;
    private Vector2 WallCheckPositionLeft;
    private Vector2 WallCheckPositionRight;




    // Start is called before the first frame update
    void Start()
    {
        MyInputActions = new Platformer2024();
        MyInputActions.Gameplay.Enable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        HorizontalMove = (int)MyInputActions.Gameplay.HorizontalMouvement.ReadValue<float>();
        float moveX = HorizontalMove * Time.deltaTime * PlayerSpeed;
        transform.Translate(moveX, 0, 0);
    }

}
