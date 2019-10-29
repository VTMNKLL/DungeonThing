using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Coin : Selectable
{
    float floatingMultiplier;
    float theta = 0;
    public float speed;
    float maxSpeed;
    public Vector3 randAxis;

    // Start is called before the first frame update
    void Start()
    {
        randAxis = Vector3.Normalize(Random.insideUnitSphere);
    }

    // Update is called once per frame
    void Update()
    {
        float tempspeed = speed;
        float floatingMultiplier = 1;
        if (mouseOver)
        {   theta += Time.deltaTime;
            theta = Mathf.Min(theta,1);
        }
        else
        {
            theta -= Time.deltaTime;
            theta = Mathf.Max(theta,0);
        }
        floatingMultiplier = Mathf.Sin(theta);
        tempspeed += 10 * speed * floatingMultiplier;
        //tempspeed = Mathf.Max(speed,tempspeed);

        this.transform.Rotate(Vector3.up * tempspeed * Time.deltaTime, Space.World);
        this.transform.Rotate(randAxis * tempspeed / 2 * Time.deltaTime, Space.World);
        mouseOver = false;

    }
}
