using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipMovement : MonoBehaviour
{
    public float startingFuel;
    public float startSpeed;
    public float maxSpeed;
    public float acceleration;
    public float deacceleration;
    public float allowedSpeedUponImpact;
    public float allowedDistanceBeyondPlanetCenterPoint;
    public float allowedDistanceFromPlanetCenter;
    public float rotationalSpeed;
    public float fuelDrainagePerSecond;
    float currentSpeed;
    float currentFuel;
    Vector3 startingPosition;
    Vector3 startingRotation;

    public Text speedText;
    public Text fuelText;
    public Transform planet;

    bool landed;


    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.eulerAngles;



        currentFuel = startingFuel;
        currentSpeed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!landed)
        {
            Movement();
        }

        MissCheck();


        speedText.text = "Speed: " + (int)currentSpeed;
        fuelText.text = "Fuel: " + (int)currentFuel;


    }

    void MissCheck()
    {
        if (transform.position.y > planet.position.y + allowedDistanceBeyondPlanetCenterPoint)
        {
            Restart();
        }
        else if (Vector3.Distance(transform.position, planet.position) > allowedDistanceFromPlanetCenter)
        {
            Restart();
        }



    }

    void Movement()
    {

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotationalSpeed , 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotationalSpeed, 0);
        }




        if (Input.GetKey(KeyCode.Space) && currentFuel > 0)
        {
            currentFuel -= fuelDrainagePerSecond * Time.deltaTime;
            currentSpeed -= deacceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime;

            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }



        
        
        
        
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    void Restart()
    {
        transform.position = startingPosition;
        transform.eulerAngles = startingRotation;



        currentFuel = startingFuel;
        currentSpeed = startSpeed;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (currentSpeed > allowedSpeedUponImpact)
        {
            //Explode and Restart
            Restart();


        }
        else
        {
            landed = true;
            currentSpeed = 0;
        }
           


    }
}
