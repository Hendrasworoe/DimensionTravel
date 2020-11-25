using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car2DController : MonoBehaviour
{
    public float speedForce = 10f;
    public float torqueForce = -200f;
    public float driftFactorSticky = 0.9f;
    public float driftFactorSlippy = 1f;
    public float maxStickyVelocity = 2.5f;
    public float minStickyVelocity = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float driftFactor = driftFactorSticky;
        if(RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }
        
        rb.velocity = ForwardVelocity() + RightVelocity()*driftFactor;

        if (Input.GetButton("Accelerate"))
        {
            rb.AddForce(transform.up * speedForce);
        }

        float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 2.5f);


        rb.angularVelocity = Input.GetAxis("Horizontal") * torqueForce ;



    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
    }

    Vector2 RightVelocity()
    {
        return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
    }
}
