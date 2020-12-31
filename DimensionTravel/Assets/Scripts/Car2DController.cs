using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Car2DController : MonoBehaviour
{
    //car atribute
    public float speedForce = 10f;
    public float torqueForce = -200f;
    public float driftFactorSticky = 0.9f;
    public float driftFactorSlippy = 1f;
    public float maxStickyVelocity = 2.5f;
    public float minSlippyVelocity = 1.5f;
    public float boostFactor = 1.5f;
    public float carHealth = 100f;
    public AudioSource carSound;

    //boosted atribute
    public float velocityBoostedReq = 23f;
    public bool boosted = false;

    //game over panel
    public GameObject gameOverPanel;

    //dimensional button
    public Button boostButton;
    public Button switchDimensionBtn;

    //collectible items status
    private ItemsStatus itemsStatus;
    public int nextLevel = 0;
    private int numberItem;

    //text UI
    public Text timerDisplay;
    public Text nextLevelNotif;

    //dimensional terms condition
    public float dimensionTimer = 3f;
    public float timeLeft;
    private bool velocityAccepted = false;

    //attendance on map
    private bool map1WasAttended = true;
    private bool map2WasAttended = false;
    private bool map3WasAttended = false;

    private void Start()
    {
        //set time left equal dimension timer that was set
        timeLeft = dimensionTimer;
    }

    private void Update()
    {
        //setting car sound pitch
        carSound.pitch = GetComponent<Rigidbody2D>().velocity.magnitude / 20;

        //define items status variable
        itemsStatus = FindObjectOfType<ItemsStatus>();

        //activate game object of dimension button (boost button) when minimum a item collected
        if (itemsStatus.currentLevel == 1 && (itemsStatus.items1_1 || itemsStatus.items1_2))
        {
            boostButton.gameObject.SetActive(true);

            //add switching dimension button
            if (itemsStatus.items1_1 && itemsStatus.items1_2)
            {
                switchDimensionBtn.gameObject.SetActive(true);
                nextLevelNotif.gameObject.SetActive(true);
            }
        }
        else if (itemsStatus.currentLevel == 2 && (itemsStatus.items2_1 || itemsStatus.items2_2))
        {
            boostButton.gameObject.SetActive(true);

            //add switching dimension button
            if (itemsStatus.items2_1 && itemsStatus.items2_2)
            {
                switchDimensionBtn.gameObject.SetActive(true);
                nextLevelNotif.gameObject.SetActive(true);
            }
        }
        else if (itemsStatus.currentLevel == 3 && (itemsStatus.items3_1 || itemsStatus.items3_2))
        {
            boostButton.gameObject.SetActive(true);

            //add switching dimension button
            if (itemsStatus.items3_1 && itemsStatus.items3_2)
            {
                switchDimensionBtn.gameObject.SetActive(true);
                nextLevelNotif.gameObject.SetActive(true);
            }
        }

        //display count down timer to move to next map when minimum velocity requirement reached
        //if not, disactive the display.
        if (velocityAccepted)
        {
            timeLeft -= Time.deltaTime;
            timerDisplay.gameObject.SetActive(true);
            timerDisplay.text = timeLeft.ToString();
        }
        else
        {
            timeLeft = dimensionTimer;
            timerDisplay.gameObject.SetActive(false);
        }

        //do dimension travel if time left reached 0
        if(timeLeft < 0)
        {
            //when a map was not attended
            if (nextLevel == 2 && !map2WasAttended)
            {
                SceneManager.LoadScene("IntroMap" + nextLevel);
                FindObjectOfType<ItemsStatus>().currentLevel = nextLevel;
                map2WasAttended = true;
            }
            else if (nextLevel == 3 && !map3WasAttended)
            {
                SceneManager.LoadScene("IntroMap" + nextLevel);
                FindObjectOfType<ItemsStatus>().currentLevel = nextLevel;
                map3WasAttended = true;
            }
            else
            {
                SceneManager.LoadScene("Map" + nextLevel);
                FindObjectOfType<ItemsStatus>().currentLevel = nextLevel;
            }
        }
    }

    void FixedUpdate()
    {
        //getting rigidbody component
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //set drift factor
        float driftFactor = driftFactorSticky;
        if(RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }
        
        //velocity when drifting
        rb.velocity = ForwardVelocity() + RightVelocity()*driftFactor;

        //accelerate
        if (CrossPlatformInputManager.GetButton("Accelerate"))
        {
            if(boosted)
            {
                rb.AddForce(transform.up * speedForce * boostFactor);
            }
            else
            {
                rb.AddForce(transform.up * speedForce);
            }
        }

        if (CrossPlatformInputManager.GetButton("Break"))
        {
            rb.AddForce(transform.up * -speedForce/2f);
        }

        //turning
        float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 3f);
        rb.angularVelocity = CrossPlatformInputManager.GetAxis("Horizontal") * torqueForce ;

        //setting requirement to doing dimension travel
        if (rb.velocity.magnitude > velocityBoostedReq)
        {
            velocityAccepted = true;
        }
        else
        {
            velocityAccepted = false;
        }

        //set when game over
        if (carHealth <= 0)
        {
            gameOverPanel.SetActive(true);
        }
    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
    }

    Vector2 RightVelocity()
    {
        return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //take point damages if hit monster (tagged as enemy)
        if(collision.gameObject.tag == "Enemy")
        {
            carHealth -= 10f;
        }

        //take point damages if hit monster's bullet
        if (collision.gameObject.tag == "Bullet")
        {
            carHealth -= 5f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //take point damages when enter to the lava once
        if (collision.gameObject.tag == "Lava")
        {
            carHealth -= 5f;
            Debug.Log("ngidak lava");
        }
        
        //when collect collectible item
        if (collision.gameObject.tag == "Collective")
        {
            nextLevel = collision.gameObject.GetComponent<CollectiveItem>().nextLevel;
            numberItem = collision.gameObject.GetComponent<CollectiveItem>().numberItem;

            //set active items status to collected
            switch (numberItem)
            {
                case 1:
                    FindObjectOfType<ItemsStatus>().items1_1 = true;
                    break;
                case 2:
                    FindObjectOfType<ItemsStatus>().items1_2 = true;
                    break;
                case 3:
                    FindObjectOfType<ItemsStatus>().items2_1 = true;
                    break;
                case 4:
                    FindObjectOfType<ItemsStatus>().items2_2 = true;
                    break;
                case 5:
                    FindObjectOfType<ItemsStatus>().items3_1 = true;
                    break;
                case 6:
                    FindObjectOfType<ItemsStatus>().items3_2 = true;
                    break;
            }
        }

        //jump into epilog
        if (collision.gameObject.tag == "LastItem")
        {
            SceneManager.LoadScene("Epilog");
            FindObjectOfType<ItemsStatus>().items1_1 = false;
            FindObjectOfType<ItemsStatus>().items1_2 = false;
            FindObjectOfType<ItemsStatus>().items2_1 = false;
            FindObjectOfType<ItemsStatus>().items2_2 = false;
            FindObjectOfType<ItemsStatus>().items3_1 = false;
            FindObjectOfType<ItemsStatus>().items3_2 = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //take point damages when on lava
        if (collision.gameObject.tag == "Lava")
        {
            carHealth -= 0.5f;
            Debug.Log("ngidak lava");
        }


        if (collision.gameObject.tag == "Ice")
        {
            GetComponent<Rigidbody2D>().angularDrag = 0.2f;
            GetComponent<Rigidbody2D>().drag = 0.7f;
            Debug.Log("dalan lunyu");
        }
        else
        {
            GetComponent<Rigidbody2D>().angularDrag = 1f;
            GetComponent<Rigidbody2D>().drag = 1f;
        }
    }
}
