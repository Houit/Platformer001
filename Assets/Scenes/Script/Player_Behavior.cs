using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behavior : MonoBehaviour
{
    //Player Data
    [SerializeField] private float PlayerSpeed; //vitesse du joueur
    [SerializeField] private float Smoothing; //adoucie la courbe de déplacement (le perso ne passe pas de 0 à 100 en 1sec)
    private int HorizontalMove; //permet de récup l'axe de déplacement horizontal
    Rigidbody2D RbPlayer; //permet de récup le rigidbody du joueur
    private Vector2 Velocity = Vector2.zero;

    //Imput System
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
        RbPlayer = GetComponent<Rigidbody2D>(); //récup du rigidbody2D du joueur
        OriginalGravityScale = RbPlayer.gravityScale * GravityMultiplier; //donne une valeur de base pour la gravité du joueur

        MyInputActions = new Platformer2024();
        MyInputActions.Gameplay.Enable();

      //  MyInputActions.Gameplay.Jump.started += ctx => StartJump();
      //  MyInputActions.Gameplay.Jump.canceled += ctx => StopJump();



    }

    private void StartJump()
    {
        if (IsGrounded)
        {
            IsJumping = true;
        }
    }

    private void StopJump()
    {
        IsJumping = false;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateCheckPosition();

        if(!IsGrounded)
        {
            IsJumping = false;
        }

        // Old imput 

        //HorizontalMove = MyInputActions.Gameplay.HorizontalMouvement.ReadValue<int>();

        Collider2D col = Physics2D.OverlapBox(GroundCheckPosition, new Vector2(GroundCheckWidth, GroundCheckHeight),0, JumpLayerMask);

        Collider2D colLeft = Physics2D.OverlapBox(WallCheckPositionLeft, new Vector2(WallCheckWidth, WallCheckHeight), 0, JumpLayerMask);
        IsTouchingWallLeft = false;
        Collider2D colRight = Physics2D.OverlapBox(WallCheckPositionRight, new Vector2(WallCheckWidth, WallCheckHeight), 0, JumpLayerMask);
        IsTouchingWallRight = false;

        //Checker si le player est au sol
        if (col != null)
        {
            IsGrounded = true; 
        }
        else 
        {
            StartCoroutine(ChangeIsGroundedState(false));
        }



        //checker si le player peut sauter
        //MyInputActions.Gameplay.Jump.IsPressed()
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            print("jump");
            IsJumping = true;
        }
       

        

    }

    private void FixedUpdate()
    {
        HorizontalMove = (int)Input.GetAxisRaw("Horizontal");


        if (!((HorizontalMove > 0 && IsTouchingWallRight) || (HorizontalMove < 0 && IsTouchingWallLeft)))
        {
            Move();
        }


        if (IsJumping && IsGrounded)
        {
            Jump();
        }

        if (RbPlayer.velocity.y < 0)
        {
            //on est en chute
            RbPlayer.gravityScale = OriginalGravityScale * FallGravityScaleMultiplier;
        }
        else
        {
            if (MyInputActions.Gameplay.Jump.IsPressed())
            {
                RbPlayer.gravityScale = OriginalGravityScale * JumpingGravityScaleMultiplier;
            }
            else
            {
                RbPlayer.gravityScale = OriginalGravityScale;
            }
        }
    }

    private IEnumerator ChangeIsGroundedState(bool IsGroundedState)
    {
        yield return new WaitForSeconds(CoyoteTime);
        IsGrounded = IsGroundedState;
    }


    private void Move()
    {
        HorizontalMove = (int)Input.GetAxisRaw("Horizontal");
        //HorizontalMove = (int)MyInputActions.Gameplay.HorizontalMouvement.ReadValue<float>();
        float moveX = HorizontalMove * Time.deltaTime * PlayerSpeed;
        transform.Translate(moveX, 0, 0);

        //Jump = Input.GetAxis("Jump");

        Vector2 targetVelocity = new Vector2(HorizontalMove * PlayerSpeed, RbPlayer.velocity.y);
        RbPlayer.velocity = Vector2.SmoothDamp(RbPlayer.velocity, targetVelocity, ref Velocity, Smoothing);
    }

    private void Jump()
    {
        RbPlayer.velocity = new Vector2(RbPlayer.velocity.x, JumpForce);
        IsGrounded = false;
    }

   

    private void UpdateCheckPosition()
    {
        SpriteRenderer SpriteRenderer = GetComponent<SpriteRenderer>(); //récup le sprite du joueur
        float Hauteur = SpriteRenderer.bounds.size.y; //détermine la hauteur du sprite
        float Largeur = SpriteRenderer.bounds.size.x; //détermine la largeur du sprite
        GroundCheckPosition = new Vector2(transform.position.x, transform.position.y - Hauteur/2f);
        WallCheckPositionLeft = new Vector2(transform.position.x - Largeur/2f, transform.position.y);
        WallCheckPositionRight = new Vector2(transform.position.x + Largeur / 2f, transform.position.y);
    }

    private void OnDrawGizmos()
    {
        UpdateCheckPosition();
        Gizmos.color = Color.green;
        Gizmos.DrawCube(GroundCheckPosition, new Vector3(GroundCheckWidth, GroundCheckHeight, 0));
        Gizmos.DrawCube(WallCheckPositionLeft, new Vector3(WallCheckWidth, WallCheckHeight, 0));
        Gizmos.DrawCube(WallCheckPositionRight, new Vector3(WallCheckWidth, WallCheckHeight, 0));

    }
}
