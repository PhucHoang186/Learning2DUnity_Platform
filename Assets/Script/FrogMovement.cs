using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovement : EnemyScript
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    private bool facingLeft =true ;
    private float jumpHight = 5f;
    private float jumpLength = 3f;
    [SerializeField]private LayerMask ground;
    private Rigidbody2D frog_rb;
    private Collider2D coll;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        frog_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ani.GetBool("Jumping"))
        {
            if (frog_rb.velocity.y < 0.1f)
            {
                ani.SetBool("Falling", true);
                ani.SetBool("Jumping", false);
            }
        }
        if  (coll.IsTouchingLayers(ground) && ani.GetBool("Falling"))
        {
            ani.SetBool("Falling", false);
        }
    }
    private void Move()
    {
        if (facingLeft)
        {
            if (frog_rb.position.x > leftCap)
            {
                if (frog_rb.transform.localScale.x != 1)
                {
                    frog_rb.transform.localScale = new Vector3(1, 1, 1);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    frog_rb.velocity = new Vector2(-jumpLength, jumpHight);
                    ani.SetBool("Jumping", true);
                }
                else
                {

                }
            }
            else
            {

                facingLeft = false;
            }
        }
        else
        {
            if (frog_rb.position.x < rightCap)
            {
                if (frog_rb.transform.localScale.x != -1)
                {
                    frog_rb.transform.localScale = new Vector3(-1, 1, 1);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    frog_rb.velocity = new Vector2(jumpLength, jumpHight);
                    ani.SetBool("Jumping", true);
                }
            }
            else
            {

                facingLeft = true;
            }
        }
    }
 
}
