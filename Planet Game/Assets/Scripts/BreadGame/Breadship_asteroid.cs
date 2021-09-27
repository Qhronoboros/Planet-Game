using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breadship_asteroid : MonoBehaviour
{   public Transform camera;
    public Sprite[] sprites;
    public float a_size = 1.0f;
    public float a_min_size = 0.75f;
    public float a_max_size = 2.5f;
    public float a_speed = 9.0f;
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
        // this.transform.eulerAngles = new Vector3(0.0f,0.0f,Random.value *360);
        //var to randomize scale
        this.transform.localScale = Vector3.one * this.a_size;
        //var to randomize size
        rigid_body2D.mass = this.a_size;
        camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    public void OnHit()
    {
        GetComponentInParent<AudioSource>().Play();
        //if can split in 2 then split
        if ((this.a_size * 0.5) > this.a_min_size)
        {
            create_split();
            create_split();
        }
        Destroy(this.gameObject);
    }

    //collision with objects
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<BreadshipController>().OnHit();
            GetComponentInParent<AudioSource>().Play();
            Destroy(this.gameObject);
        }
    }
    
    private void create_split(){
        //get current position for new asteroid
        Vector2 new_a_pos = this.transform.position;
        //offset a bit to make it more natural to spawn
        new_a_pos += Random.insideUnitCircle * 0.5f;
        //create the asteroid 
        Breadship_asteroid a_half = Instantiate(this , new_a_pos , this.transform.rotation);

    }

    void Update(){

        transform.Translate(new Vector2(-1f,0f) * a_speed * Time.deltaTime);
        if(transform.position.x < camera.position.x - 15f ){
            Destroy(this.gameObject);
        }

    }
}