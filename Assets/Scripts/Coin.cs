using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Coin : Selectable
{
    float floatingMultiplier;
    public float theta = 0;
    public float theta2 = 0;
    public float speed;
    float maxSpeed;
    Vector3 randAxis;
    float randStart;

    Vector3 posOffset;

    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
        randAxis = Vector3.Normalize(Random.insideUnitSphere);
        randStart = 2 * Mathf.PI * Random.value;
        theta2 = randStart;
    }

    // Update is called once per frame
    void Update()
    {
        float tempspeed = speed;
        float floatingMultiplier = 1;
        //float amplitude = 1;
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
        theta2 += Time.deltaTime * Mathf.PI * .05f * speed;
        this.transform.position = posOffset + Vector3.up * .03f * Mathf.Sin(theta2);
        mouseOver = false;

    }
}
