using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    protected Animator ani;
    protected AudioSource explode;
    protected Rigidbody2D rb;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        ani = GetComponent<Animator>();
        explode = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    public void JumpOn()
    {
        ani.SetTrigger("Death");
        explode.Play();
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
    }
    private void Death()
    {
        
        //this.gameObject.transfo
        Destroy(this.gameObject);
    }
}
