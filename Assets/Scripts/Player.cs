using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    //variable pour modifier la vitesse du player
    [SerializeField] private float Speed;
    //variable pour modifier le délai de déplacement du player "lissage"
    [SerializeField] private float Smoothing;

    //Puissance de saut
    [SerializeField] private float JumpForce;
    //On multiplie la gravité (quand le joueur tombera)
    [SerializeField] private float FallGravityScaleMultiplier = 1f;
    //On diminue la gravité quand le joueur monte et qu'on reste appuyé sur "espace" (en multipliant par moins de 1)
    [SerializeField] private float JumpGravityScaleMultiplier = 0.8f;

    //paramètres de notre OverlapBox (width & height)
    [SerializeField] private float GroundCheckWidth, GroundCheckHeight, WallCheckWidth, WallCheckHeight;

    //On crée un float pour donner une valeur au "waitforseconds" de la coroutine
    [SerializeField] private float CoyoteTime;

    //récupère le rigidbody2D du player
    private Rigidbody2D RbPlayer;

    //fait bouger le player en x
    private int HorizontalInput;
    private Vector2 velocity = Vector2.zero, GroundCheckPosition, WallCheckPositionLeft, WallCheckPositionRight;
    private float OriginalGravityScale;

    //booléens qui servent à savoir si 1on saute ou non /2si on touche le sol ou non
    [SerializeField] private bool isJumping, isGrounded;

    //On utilise un Layermask pour que l'Overlap (boîte de collision) est en collision avec un élément/type d'élément en particulier.
    [SerializeField] private LayerMask JumpLayerMask, WallLayerMask;

    [SerializeField] private bool isTouchingWallLeft, isTouchingWallRight;

    // Start is called before the first frame update
    void Start()
    {
        //On récupère le Rigidbody2D du player
        RbPlayer = GetComponent<Rigidbody2D>();
        OriginalGravityScale = RbPlayer.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //On appelle la fonction qui check la position du player
        UpdateCheckPosition();
        //on force l'horizontal input à se référer à des "int" plutôt que des float pour le raw
        HorizontalInput = (int)Input.GetAxisRaw("Horizontal");

        //On crée une boîte de collision aux pieds du player pour détecter si il touche le sol ou pas
        Collider2D col = Physics2D.OverlapBox(GroundCheckPosition, new Vector2(GroundCheckWidth, GroundCheckHeight), 0, JumpLayerMask);

        //On regarde si l'overlap(collider placé sous les pieds du player) touche le sol ou non
        //isGrounded = (col!= null);
        if (col != null)
        {
            isGrounded = true;
        }
        else if (isGrounded) // on ajoute un if au else pour pas qu'il tourne en boucle (ou moins), donc si isGrounded est déjà false, on rappelle pas la fonction
        {
            //on appelle et lance la coroutine
            StartCoroutine(ChangeIsGroundedState(false));
        }


        if (Input.GetButton("Jump") && isGrounded)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        //On fait un overlap gauche pour tester si on touche un mur sur la gauche du player ou non
        Collider2D colLeft = Physics2D.OverlapBox(WallCheckPositionLeft, new Vector2(WallCheckWidth, WallCheckHeight), 0, WallLayerMask);
        isTouchingWallLeft = (colLeft != null);

        //On fait un overlap pour tester si on touche un mur sur la droite du player
        Collider2D colRight = Physics2D.OverlapBox(WallCheckPositionRight, new Vector2(WallCheckWidth, WallCheckHeight), 0, WallLayerMask);
        isTouchingWallRight = (colRight != null);

        if ( !(HorizontalInput > 0 && isTouchingWallRight) || (HorizontalInput < 0  && isTouchingWallLeft))
        {
            Move();
        }

        //Ce que j'avais tenté pour le IF du Move()
        /*if((isTouchingWallRight == false) && Input.GetKey("Right"))
        {
            Move();
        }
        if((isTouchingWallLeft == false) && Input.GetKey("Left"))
        {
            Move();
        }*/

        if(isJumping)
        {
            Jump();
        }

        if(RbPlayer.velocity.y < 0)
        {
            //On est en chute donc on demande à multiplier la gravité par le "multiplier" pour que le player descende plus vite qu'il ne monte
            RbPlayer.gravityScale = OriginalGravityScale * FallGravityScaleMultiplier;
        }
        else
        {
            if (Input.GetButton("Jump"))
            {
                //On ajuste la gravité appliquée au rigidbody du player = la gravité de base x notre multiplieur
                RbPlayer.gravityScale = OriginalGravityScale * JumpGravityScaleMultiplier;
            }
            else
            {
                //On monte ou on est au sol donc on a une gravité normale
                RbPlayer.gravityScale = OriginalGravityScale;
            }
            
        }

    }

    //Coroutine qui va créer un délai avant que lejoueur passe en "ne touche pas le sol" pour le laisser sauter sans tomber direct quand trop proche du bord
    private IEnumerator ChangeIsGroundedState(bool isGroundedstate)
    {
        //On dit au programme d'attendre un certain temps (définit par CoyoteTime)
        yield return new WaitForSeconds(CoyoteTime);
        //On donne à isGrounded la valeur de isGroundedState (false donc)
        isGrounded = isGroundedstate;
    }

    private void Move()
    {
        Vector2 targetVelocity = new Vector2(HorizontalInput * Speed, RbPlayer.velocity.y);
        //crée un délai au démarrage et à l'arrêt du déplacement
        RbPlayer.velocity = Vector2.SmoothDamp(RbPlayer.velocity, targetVelocity, ref velocity, Smoothing);
    }

    private void Jump()
    {
        RbPlayer.velocity = new Vector2(RbPlayer.velocity.x, JumpForce);
        //on met un grounded false pour pas pouvoir voler le temps du CoyoteTime
        isGrounded = false;
    }

    private void UpdateCheckPosition()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float hauteur = spriteRenderer.bounds.size.y;
        float largeur = spriteRenderer.bounds.size.x;
        GroundCheckPosition = new Vector2(transform.position.x, transform.position.y - hauteur / 2f);
        WallCheckPositionLeft = new Vector2(transform.position.x - largeur/2f, transform.position.y);
        WallCheckPositionRight = new Vector2(transform.position.x + largeur/2f, transform.position.y);
    }
    //On donne juste un visuel à notre Overlap pour voir sa taille et si il ets bien positionné
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(GroundCheckPosition, new Vector2(GroundCheckWidth, GroundCheckHeight));
        Gizmos.DrawCube(WallCheckPositionLeft, new Vector2(WallCheckWidth, WallCheckHeight));
        Gizmos.DrawCube(WallCheckPositionRight, new Vector2(WallCheckWidth, WallCheckHeight));
    }

}