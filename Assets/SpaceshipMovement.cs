using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Image rightArrow;
    public Image leftArrow;
    public Image speedBar;
    public Image fuelBar;
    public Image moonIcon;


    public Transform planet;

    bool landed;

    [Header("Distance to Planet")]

    float distance;
    float distanceplaycooldown;
    float distancetimer;

    public AudioSource DistanceAudioSource;
    public AudioClip Distancesound;
    [Range(0, 1)] public float DistanceVolume;

    public float DistanceX;

    public AudioSource EngineAudioSource;
    public AudioSource FuelAudioSource;
    public AudioSource AIAudioSource;

    public AudioClip AI90;
    public AudioClip AI80;
    public AudioClip AI70;
    public AudioClip AI60;
    public AudioClip AI50;
    public AudioClip AI40;
    public AudioClip AI30;
    public AudioClip AI20;
    public AudioClip AI10;
    public AudioClip AI00;
   

    private bool AS90;
    private bool AS80;
    private bool AS70;
    private bool AS60;
    private bool AS50;
    private bool AS40;
    private bool AS30;
    private bool AS20;
    private bool AS10;
    private bool AS00;

    private float FailTimer;
    public AudioSource EndAudioSource;
    public AudioClip miss;
    public AudioClip crash;
    public AudioClip win;

    public bool failed;

    private bool AS;

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
        if (!landed && !failed)
        {
            Movement();
        }

        MissCheck();


        //speedText.text = "Speed: " + (int)currentSpeed;
        //fuelText.text = "Fuel: " + (int)currentFuel;

        speedBar.fillAmount = currentSpeed / maxSpeed;
        fuelBar.fillAmount = currentFuel / startingFuel;

        //Vector3 angleHelp = transform.InverseTransformPoint(planet.position);
        //float angle = Vector3.Angle(transform.position, planet.position);
        // float angle = Vector3.Angle(Vector3.zero, someLocalVector);

        //Vector3 toPosition = (planet.position - transform.position);
        //float angle = Vector3.Angle(transform.forward, toPosition);

        float angle = Vector3.SignedAngle(planet.position - transform.position, transform.forward, Vector3.up);
        //float angle = Vector3.Angle(transform.forward, planet.position - transform.position);


        print(angle);
         

        distance = Vector3.Distance(transform.position, planet.position);
        distanceplaycooldown = (distance - 4000) / 10000;
        distancetimer += Time.deltaTime;

        if(distancetimer >= distanceplaycooldown)
        {
    
            DistanceX = (planet.position.x - transform.position.x);
            if(DistanceX > 6000)
            {
                DistanceAudioSource.panStereo = 1;
            }
            else if (DistanceX<=6000 && DistanceX > 5000)
            {
                DistanceAudioSource.panStereo = .85f;
            }
            else if (DistanceX<=5000 && DistanceX > 3500)
            {
                DistanceAudioSource.panStereo = .75f;
            }
            else if (DistanceX<=3500 && DistanceX > 2000)
            {
                DistanceAudioSource.panStereo = .50f;
            }
            else if (DistanceX<=2000 && DistanceX > 1000)
            {
                DistanceAudioSource.panStereo = .25f;
            }
            else if (DistanceX <= 1000 && DistanceX > -1000)
            {
                DistanceAudioSource.panStereo = 0;
            }
            else if (DistanceX <= -1000 && DistanceX > -2000)
            {
                DistanceAudioSource.panStereo = -0.25f;
            }
            else if (DistanceX <= -2000 && DistanceX > -3500)
            {
                DistanceAudioSource.panStereo = -0.50f;
            }
            else if (DistanceX <= -3500 && DistanceX > -5000)
            {
                DistanceAudioSource.panStereo = -0.75f;
            }
            else if (DistanceX <=5000)
            {
                DistanceAudioSource.panStereo = -0.50f;
            }

            DistanceAudioSource.PlayOneShot(Distancesound, DistanceVolume);
            distancetimer = 0;
        }

        EngineAudioSource.volume = currentSpeed / 1000;

        if (currentFuel <= 90 && AS90 == false)
        {
            AIAudioSource.PlayOneShot(AI90);
            AS90 = true;
        }
        if (currentFuel <= 80 && AS80 == false)
        {
            AIAudioSource.PlayOneShot(AI80);
            AS80 = true;
        }
        if (currentFuel <= 70 && AS70 == false)
        {
            AIAudioSource.PlayOneShot(AI70);
            AS70 = true;
        }
        if (currentFuel <= 60 && AS60 == false)
        {
            AIAudioSource.PlayOneShot(AI60);
            AS60 = true;
        }
        if (currentFuel <= 50 && AS50 == false)
        {
            AIAudioSource.PlayOneShot(AI50);
            AS50 = true;
        }
        if (currentFuel <= 40 && AS40 == false)
        {
            AIAudioSource.PlayOneShot(AI40);
            AS40 = true;
        }
        if (currentFuel <= 30 && AS30 == false)
        {
            AIAudioSource.PlayOneShot(AI30);
            AS30 = true;
        }
        if (currentFuel <= 20 && AS20 == false)
        {
            AIAudioSource.PlayOneShot(AI20);
            AS20 = true;
        }
        if (currentFuel <= 10 && AS10 == false)
        {
            AIAudioSource.PlayOneShot(AI10);
            AS10 = true;
        }
        if (currentFuel <= 0 && AS00 == false)
        {
            AIAudioSource.PlayOneShot(AI00);
            //if you comment the next line the game gets super spooky >:3c
            AS00 = true;
        }
       if (failed == true)
        {
            FailTimer += Time.deltaTime;
            if (FailTimer >= 3)
            {
                
                Restart();
            }
        }
    }

    void MissCheck()
    {
        if (transform.position.y > planet.position.y + allowedDistanceBeyondPlanetCenterPoint)
        {
            if (AS == false)
            {
                EndAudioSource.PlayOneShot(miss);
                AS = true;
            }
            failed = true;
            DistanceAudioSource.volume = 0;

            
        }
        else if (Vector3.Distance(transform.position, planet.position) > allowedDistanceFromPlanetCenter)
        {
            if (AS == false)
            {
                EndAudioSource.PlayOneShot(miss);
                AS = true;
            }
            failed = true;
            DistanceAudioSource.volume = 0;

        }



    }

    void Movement()
    {

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotationalSpeed , 0);
            //transform.Translate(-rotationalSpeed,0,0);
            leftArrow.enabled = true;
        }
        else
        {
            leftArrow.enabled = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotationalSpeed, 0);
            //transform.Translate(rotationalSpeed, 0, 0);
            rightArrow.enabled = true;
        }
        else
        {
            
            rightArrow.enabled = false;
        }




        if (Input.GetKey(KeyCode.Space) && currentFuel > 0)
        {
            currentFuel -= fuelDrainagePerSecond * Time.deltaTime;
            currentSpeed -= deacceleration * Time.deltaTime;
            FuelAudioSource.volume = 1;
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime;
            FuelAudioSource.volume = 0;
            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }



        
        
        
        
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    void Restart()
    {
        /*transform.position = startingPosition;
        transform.eulerAngles = startingRotation;



        currentFuel = startingFuel;
        currentSpeed = startSpeed;*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (currentSpeed > allowedSpeedUponImpact)
        {
            //Explode and Restart
            if (AS == false)
            {
                EndAudioSource.PlayOneShot(crash);
                AS = true;
            }
            failed = true;
            DistanceAudioSource.volume = 0;
            

        }
        else
        {
            landed = true;
            currentSpeed = 0;
        }
           


    }
}
