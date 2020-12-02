﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAI : MonoBehaviour
{
    private Transform _player;
    public float MoveSpeed;
    public float IdleTimer = 2f;
    public float MoveTimer;
    private Vector2 startPos;
    private Vector2 curPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        MoveTimer = 3;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tracking();
        MoveCountdown();
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag.Equals("Player") != false)
        {
            MoveTimer = 0;
        }
        
    }

    void MoveCountdown()
    {
        MoveTimer += Time.deltaTime;
    }
    void tracking()
    {
        if(MoveTimer>=IdleTimer)
        {
            MoveSpeed = 10f;
            //if(Vector2.Distance(transform.position, _player.position) >3)
            transform.position = Vector2.MoveTowards(transform.position, _player.position, MoveSpeed * Time.deltaTime);
            curPos = transform.position;
        }else if(MoveTimer<IdleTimer)
        {
            MoveSpeed = 3f;
            //if(Vector2.Distance(transform.position, _player.position) >3)
            transform.position = Vector2.MoveTowards(curPos, startPos, MoveSpeed * Time.deltaTime);
            curPos = transform.position;
        }
            
    }
}
