using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] sprites;
    public float a_size = 1.0f;
    public float a_min_size = 0.75f;
    public float a_max_size = 2.5f;
    public float a_speed = 15.0f;
    public float max_lifetime = 15.0f;
    public float a_half_speed_multiplier =  15;
    private SpriteRenderer sprite_renderer;
    private Rigidbody2D rigid_body2D;
    int stage = 1;

    // Start is called before the first frame update
    private void Awake(){
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigid_body2D =  GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        //randomize the sprites
        sprite_renderer.sprite = sprites[stage];
        //
        this.transform.eulerAngles = new Vector3(0.0f,0.0f,Random.value *360);
        //var to randomize scale
        this.transform.localScale = Vector3.one * this.a_size;
        //var to randomize size
        rigid_body2D.mass = this.a_size;
    }

    // add force to the ateroid and destroy it at a certain time
    public void set_trajectory(Vector2 direction){
        rigid_body2D.AddForce(direction * this.a_speed);
        Destroy(this.gameObject, this.max_lifetime);
    }

    public void set_trajectory_a_half(Vector2 direction){
        rigid_body2D.AddForce(direction * this.a_speed *this.a_half_speed_multiplier);
        Destroy(this.gameObject, this.max_lifetime);
    }

    public void OnHit()
    {
        GetComponentInParent<AudioSource>().Play();
        //if can split in 2 then split
        // Destroy(this.gameObject);
    }
}
