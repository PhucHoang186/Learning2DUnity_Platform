using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class playerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;
    private Collider2D coll;
    private float hurt_force =10f;
    private float climb_speed =5f;
    private float naturalGravity;

    [HideInInspector] public bool CanClimb = false;
    [HideInInspector] public bool bottomLad = false;
    [HideInInspector] public bool topLad = false;
    public Ladder ladder;
  
    private enum State_player {idle,run,jump,fall,hurt,climb};
    private State_player state = State_player.idle; 
    
    [SerializeField] private float jump_force = 12f;
    [SerializeField] private float player_speed = 10f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource cherry;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        footstep = GetComponent<AudioSource>();
        naturalGravity = rb.gravityScale;
        //PermanentUI.perma.health_amount.text = PermanentUI.perma.Health.ToString();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            PermanentUI.perma.cherries += 1;
            PermanentUI.perma.cherries_text.text = PermanentUI.perma.cherries.ToString();
        }
        if (collision.tag == "PowerUp")
        {
            Debug.Log("Hit");
            Destroy(collision.gameObject);
            jump_force = 16;
            GetComponent<SpriteRenderer>().color = Color.blue;
            StartCoroutine(ResetPower());
        }
    }
   
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyScript Enemy = other.gameObject.GetComponent<EnemyScript>();
            if (state == State_player.fall)
            {
                Enemy.JumpOn();
                Jump();
            }
            else
            {
                state = State_player.hurt;
               
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurt_force, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurt_force, rb.velocity.y);
                }
                Health_function();
            }
        }
    }

    private void Health_function()
    {
        PermanentUI.perma.Health -= 1;
        PermanentUI.perma.health_amount.text = PermanentUI.perma.Health.ToString();
        if (PermanentUI.perma.Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void Update()
    { if (state == State_player.climb)
        {   
            Climb();
        }
        else if (state != State_player.hurt)
        {
            Movement();
        }
        StateSwitch();
        ani.SetInteger("state", (int)state);
    }

    private void Movement()
    {
        float H_input = Input.GetAxis("Horizontal");
        if (CanClimb && Mathf.Abs(Input.GetAxis("Vertical"))> 0.1f)
        {
            state = State_player.climb;

            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector2(ladder.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;
        }
        if (H_input < 0)
        {

            rb.velocity = new Vector2(-player_speed, rb.velocity.y);

            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (H_input > 0)
        {
            rb.velocity = new Vector2(player_speed, rb.velocity.y);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
            state = State_player.jump;
        }
    }
    private void Climb()
    {
        float vClimb = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Jump"))
        {
            
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            CanClimb = false;
            rb.gravityScale = naturalGravity;
            Jump();
            ani.speed = 1f;
            return;
        }
        else if (vClimb> 0.1f && !topLad)
        {
            rb.velocity = new Vector2(0f, vClimb* climb_speed);
            ani.speed = 1f;
        }
        else if(vClimb <-0.1f && !bottomLad)
        {
            rb.velocity = new Vector2(0f, vClimb* climb_speed);
            ani.speed = 1f;
        }
        else
        {
            ani.speed = 0f;
            rb.velocity = Vector2.zero; 
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump_force);
        state = State_player.jump;
    }

    private void StateSwitch()
    {
     
        if(state ==State_player.climb)
        {
            
        }
        else if (state == State_player.jump)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State_player.fall;
            }
        }
        else if (state == State_player.fall)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State_player.idle;
            }
        }
        else if (state == State_player.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) <.1f)
            {
                state = State_player.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 1.5f)
        {
            state = State_player.run;
        }
        else
        {
            state = State_player.idle;
        }
    }
    private void Footstep()
    {
        footstep.Play();
    }
    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(10);
        jump_force = 12f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
