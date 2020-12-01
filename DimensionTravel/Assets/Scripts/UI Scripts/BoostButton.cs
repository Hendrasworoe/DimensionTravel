using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostButton : MonoBehaviour
{
    public Car2DController car;
    private int counter;

    public void BoostChangeStts()
    {
        counter++;
        if (counter % 2 == 1)
        {
            car.boosted = true;
        }
        else
        {
            car.boosted = false;
        }
    }
}
