//stealth player controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stealthcontroller : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool isonscreen = false;

    public float speed;
    
    private float moveInput;

    private bool facingRight = true;
    
    public LayerMask whatIsGround;


    public float stopQtime;
    private bool canpressQ = true;

    public LayerMask whatIsLadder;
    public float distance;
    private bool isClimbing;
    public float climbspeed;
    private float inputVertical;


    public bool ishidden = false;
    private bool canhide = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        moveInput = Input.GetAxisRaw("Horizontal");

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        if (isonscreen)
        {
            if (!ishidden)
            {
                rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            }
            
            if (canhide)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    ishidden = true;
                }
            }
            
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);

        if (hitInfo.collider != null)
        {
            if (Input.GetKey(KeyCode.W))
            {
                isClimbing = true;
            }

        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * climbspeed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = (float)2.4;
        }


    }
    void Update()
    {
        if(rb.velocity.y >= 2.7)
        {
            rb.gravityScale = (float)2.7;
        }
        
        
        if (Input.GetKeyDown(KeyCode.Q) && canpressQ)
        {
            isonscreen = !isonscreen;
            canpressQ = false;
            StartCoroutine(Reset());
        }
        //Debug.Log("canhide" + canhide);
        //Debug.Log("ishidden: " + ishidden);
        //Debug.Log(isonscreen);

        if (ishidden && Input.GetKeyDown(KeyCode.Space))
        {

            ishidden = false;

        }
    }



    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(stopQtime);
        canpressQ = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Table"))
        {
            canhide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Table"))
        {
            canhide = false;
        }
    }
}
