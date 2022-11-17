using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    
    [SerializeField]
    private float movementSpeed = 7f;
    [SerializeField]
    private float jumpForce = 7f;
    [SerializeField]
    private LayerMask jumpableGround;

    private BoxCollider2D boxCollider2D;
    private PlayerInput playerInput;
    private GameController gameController;

    private void Start()
    {
        SetRigidBody(GetComponent<Rigidbody2D>());
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerInput = PlayerInput.Instance;
        gameController = GameController.instance;
    }

    private void Awake()
    {
       
    }

    private void Update()
    {
        ExecutePlayerInputs(playerInput.Actions);
    }

    private void ExecutePlayerInputs(List<PlayerAction> actions)
    {
        if(actions.Contains(PlayerAction.Jump))
        {
            Jump();
            actions.Remove(PlayerAction.Jump);
        }
        else if(actions.Contains(PlayerAction.MoveHorizontal))
        {
            Move(playerInput.HorizontalInput);
            actions.Remove(PlayerAction.MoveHorizontal);
        }
        else if(actions.Contains(PlayerAction.Menu))
        {
            //openmenu
            gameController.SetGameSpeedForMenu();
            actions.Remove(PlayerAction.Menu);
        }
    }

    public void Move(float directionInput)
    {
        if (directionInput != 0)
        {
            var dirX = directionInput > 0 ? 0.5f : -0.5f;
            rigidBody.velocity = new Vector2(dirX * 7f, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }

    public void Jump()
    {
        if(IsGrounded)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, jumpForce, 0);
        }
    }

    public void SetRigidBody(Rigidbody2D rigidbody)
    {
        rigidBody = rigidbody;
    }

    public void SetCollider(BoxCollider2D boxCollider2D)
    {
        this.boxCollider2D = boxCollider2D;
    }

    public bool IsGrounded
    {
        get
        {
            return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        }        
    }
}
