using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public float a_size = 1.0f;
    public float a_min_size = 0.75f;
    public float a_max_size = 2.5f;
    public float a_speed = 25.0f;
    public float max_lifetime = 2.0f;
    private SpriteRenderer sprite_renderer;
    private Rigidbody2D rigid_body2D;

    // Start is called before the first frame update
    private void Awake(){
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigid_body2D =  GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        //randomize the sprites
        sprite_renderer.sprite = sprites[Random.Range(0,sprites.Length)];
        //
        this.transform.eulerAngles = new Vector3(0.0f,0.0f,Random.value *360);
        //var to randomize scale
        this.transform.localScale = Vector3.one * this.a_size;
        //var to randomize size
        rigid_body2D.mass = this.a_size;
    }
    public void set_trajectory(Vector2 direction){
        rigid_body2D.AddForce(direction * this.a_speed);
        Destroy(this.gameObject, this.max_lifetime);
    }
}
