using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    //Input System
    private Platformer2024 MyInputActions;
    private float HorizontalMove; //permet de récup l'axe de déplacement horizontal

    [Header("PlayerValues")]
    [SerializeField] private float PlayerSpeed; //vitesse du joueur
    [SerializeField] private float Smoothing; //adoucie la courbe de déplacement (le perso ne passe pas de 0 à 100 en 1sec)
    Rigidbody2D RbPlayer; //permet de récup le rigidbody du joueur


    [Header("Jump")]
    [SerializeField] private LayerMask JumpLayerMask;
    [SerializeField] private float JumpForce;
    private Vector2 Velocity = Vector2.zero;
    private float OriginalGravityScale; //valeur de gravité de base pour le joueur
    [SerializeField] private float GravityMultiplier;
    [SerializeField] private float FallGravityScaleMultiplier = 1.2f; //rend le joueur plus lourd -> chute plus rapide
    [SerializeField] private float JumpingGravityScaleMultiplier = 0.5f; //rend le joueur plus léger -> saute plus haut
    [SerializeField] private float CoyoteTime; //donne au joueur un délai supplémentaire pour sauter
    [SerializeField] private float JumpBufferTime;
    private float JumpBufferCounter;

    [Header("Dash")]
    [SerializeField] private float DashForce;
    [SerializeField] private float DashCooldown;

    //checks
    private Vector2 GroundCheckPosition;
    private Vector2 WallCheckPositionLeft;
    private Vector2 WallCheckPositionRight;

    [Header("Bools")]
    [SerializeField] private bool IsJumping;
    [SerializeField] private bool IsGrounded = false;
    private bool Ground;
    [SerializeField] private bool IsTouchingWallLeft = false;
    [SerializeField] private bool IsTouchingWallRight = false;
    [SerializeField] private bool CanDash;

    [Header("Gizoms")]
    [SerializeField] private float GroundCheckWidth;
    [SerializeField] private float GroundCheckHeight;
    [SerializeField] private float WallCheckWidth;
    [SerializeField] private float WallCheckHeight;




    // Start is called before the first frame update
    void Start()
    {
        RbPlayer = GetComponent<Rigidbody2D>(); //récup du rigidbody2D du joueur
        OriginalGravityScale = RbPlayer.gravityScale * GravityMultiplier; //donne une valeur de base pour la gravité du joueur


        MyInputActions = new Platformer2024();
        MyInputActions.Gameplay.Enable();

        MyInputActions.Gameplay.Jump.started += ctx => StartJump();
        MyInputActions.Gameplay.Jump.canceled += ctx => StopJump();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCheckPosition();

        Collider2D ColGround = Physics2D.OverlapBox(GroundCheckPosition, new Vector2(GroundCheckWidth, GroundCheckHeight), 0, JumpLayerMask);
        
        
        if (ColGround != null)
        {
            IsGrounded = true;
        }
        else
        {
            StartCoroutine(ChangeIsGroundedState(false));
        }

        if (MyInputActions.Gameplay.Jump.IsPressed() && IsGrounded)
        {
            IsJumping = true;
        }

        if(!IsGrounded)
        {
            IsJumping = false;
        }

        if(RbPlayer.velocity.y < 0)
        {
            //on est en chute
            RbPlayer.gravityScale = OriginalGravityScale * FallGravityScaleMultiplier;
        }
        else
        {
            if(MyInputActions.Gameplay.Jump.IsPressed())
            {
                RbPlayer.gravityScale = OriginalGravityScale * JumpingGravityScaleMultiplier;
            }
            else
            {
                RbPlayer.gravityScale = OriginalGravityScale;
            }
        }

        Collider2D ColLeft = Physics2D.OverlapBox(WallCheckPositionLeft, new Vector2(WallCheckWidth, WallCheckHeight), 0, JumpLayerMask);
        Collider2D ColRight = Physics2D.OverlapBox(WallCheckPositionRight, new Vector2(WallCheckWidth, WallCheckHeight), 0, JumpLayerMask);

        if(ColLeft != null)
        {
            IsTouchingWallLeft = true;
            print("left touched");
        }
        else
        {
            IsTouchingWallLeft = false;
        }

        if(ColRight != null)
        {
            IsTouchingWallRight = true;
            print("right touched");
        }
        else 
        { 
            IsTouchingWallRight = false; 
        }


    }

    private void FixedUpdate()
    {
        HorizontalMove = MyInputActions.Gameplay.HorizontalMouvement.ReadValue<float>();

        if (!((HorizontalMove > 0 && IsTouchingWallRight == true) || (HorizontalMove < 0 && IsTouchingWallLeft == true)))
        {
            Move();
        }
        else
        {
            if (IsTouchingWallRight == true && IsTouchingWallLeft == true)
            { 
                Move();
            }
        }

        if (IsJumping && IsGrounded)
        {
            Jump();
        }
        
    }

    


    private void Move()
    {
        HorizontalMove = MyInputActions.Gameplay.HorizontalMouvement.ReadValue<float>();

        Vector2 TargetVelocity = new Vector2(HorizontalMove * PlayerSpeed, RbPlayer.velocity.y);
        RbPlayer.velocity = Vector2.SmoothDamp(RbPlayer.velocity, TargetVelocity, ref Velocity, Smoothing);
    }




    private void StartJump()
    {
        IsJumping = true;
    }

    private void StopJump()
    {
        IsJumping = false;
    }

    private void Jump()
    {
        RbPlayer.velocity = new Vector2(RbPlayer.velocity.x, JumpForce);
        IsGrounded = false;
    }
    
    private IEnumerator ChangeIsGroundedState(bool IsGroundedState)
    {
        yield return new WaitForSeconds(CoyoteTime);
        IsGrounded = IsGroundedState;
    }



    private void UpdateCheckPosition()
    {
        SpriteRenderer SpriteRenderer = GetComponent<SpriteRenderer>(); //récup le sprite du joueur
        float Hauteur = SpriteRenderer.bounds.size.y; //détermine la hauteur du sprite
        float Largeur = SpriteRenderer.bounds.size.x; //détermine la largeur du sprite
        GroundCheckPosition = new Vector2(transform.position.x, transform.position.y - Hauteur / 2f);
        WallCheckPositionLeft = new Vector2(transform.position.x - Largeur / 2f, transform.position.y);
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
