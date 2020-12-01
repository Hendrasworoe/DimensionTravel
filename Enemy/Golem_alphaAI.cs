using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_alphaAI : MonoBehaviour
{
    private float shotInterval;
    private float startShot = 2;

    public GameObject projectileGolem;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        onewayprojectile();
    }

    void onewayprojectile()
    {
        if (shotInterval <= 0)
        {
            Instantiate(projectileGolem, transform.position, Quaternion.identity);
            shotInterval = startShot;
        }
        else
        {
            shotInterval -= Time.deltaTime;
        }
    }
}
