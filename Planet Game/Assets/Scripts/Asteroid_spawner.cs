
using UnityEngine;

public class Asteroid_spawner : MonoBehaviour
{   
    
    public Asteroid asteroid_prefab;
    // spawn rate of asteroid in seconds
    public float spawn_rate = 4.0f;
    // amount of spawns
    public int spawn_amount = 1;
    //offset of the trajectory towards the center in degrees
    public float trajectory_variance = 13.0f;
    //spawn distance of how far asteroid spawn from spawner
    // they will spawn 15 units away which will be a off screen
    public float spawn_distance = 60.0f;

    // Start is called before the first frame update
    void Start()
    {   
        //invoke a repeating fuction(string methodName, float time, float repeatRate);
        InvokeRepeating(nameof(spawn),this.spawn_rate,this.spawn_rate);
    }

    private void spawn(){
        if (!GameManager.playerDead && !GameManager.Instance.stageClear)
        {
            for (int i = 0; i < this.spawn_amount; i++)
            {
                //random direction. 
                //insideUnitcirkel which is the radius inside of the spawner
                // normalized will set magnitude to one which makes it be the edge of the circle
                Vector3 spawn_direction = Random.insideUnitCircle.normalized * this.spawn_distance;
                //spawn point of the asteroid (position of spawner object and spawn direction)
                Vector3 spawn_point = this.transform.position + spawn_direction;
                // randomize the variance
                float variance = Random.Range(-this.trajectory_variance, this.trajectory_variance);
                //rotate asteroid for visuals
                //vector3.forward means xyz = 001
                Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

                //create the asteriod
                Asteroid asteroid = Instantiate(this.asteroid_prefab, spawn_point, rotation);
                //Set parent
                asteroid.transform.parent = GameManager.Instance.asteroidParent.transform;

                // set the asteroid size by randomizing it
                asteroid.a_size = Random.Range(asteroid.a_min_size, asteroid.a_max_size);
                // set the asteroid trajectory
                //multiplied by negative spawn direction so the asteroid direction is going towards the spawnpoint
                asteroid.set_trajectory(rotation * -spawn_direction);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
