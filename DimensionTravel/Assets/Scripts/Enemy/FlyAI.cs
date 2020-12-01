using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAI : MonoBehaviour
{
    private Transform _player;
    public float MoveSpeed = 5f;
    public float stoppingDistance=10f;
    public float retreatDistance=20f;
    private float shotInterval;
    private float startShot = 2;

    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        shotInterval = startShot;
    }

    // Update is called once per frame
    void Update()
    {
        tracking();
        shot();
    }

    void tracking()
    {
        MoveSpeed = 5f;
        if(Vector2.Distance(transform.position, _player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.position, MoveSpeed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, _player.position) < stoppingDistance && Vector2.Distance(transform.position, _player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }else if(Vector2.Distance(transform.position, _player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.position, - MoveSpeed * Time.deltaTime);
        }
            
    }
    
    void shot()
    {
        if(shotInterval<=0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            shotInterval = startShot;
        }
        else
        {
            shotInterval -= Time.deltaTime;
        }
    }

}
