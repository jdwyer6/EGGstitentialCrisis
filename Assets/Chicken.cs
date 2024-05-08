using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    private Animator anim;
    private AudioManager am;
    private bool chickenHoldIsPlaying = false;
    private float changeDirectionTime = 0;
    private float idleTime = 0;
    private Vector3 targetDirection;
    private float speed;

    public float minChangeTime = 2f;
    public float maxChangeTime = 5f;
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float minIdleTime = 3f;
    public float maxIdleTime = 6f;
    private bool isIdle = false;

    public float minEggLayTime = 5f;
    public float maxEggLayTime = 10f;
    public GameObject eggPrefab;

    private Collider chickenCollider; 
    private Collider eggCollider;

    public bool isInCoop = false;

    void Start()
    {
               chickenCollider = GetComponent<Collider>();
        eggCollider = eggPrefab.GetComponent<Collider>();
        Physics.IgnoreCollision(chickenCollider, eggCollider, true);
        anim = GetComponentInChildren<Animator>();
        am = FindObjectOfType<AudioManager>();

        SetNextAction();
        StartCoroutine(LayEggTimer());
    }

    void Update()
    {
        if (GetComponent<ItemData>().isBeingHeld)
        {
            SetBeingHeldState();
        }
        else
        {
            if (isInCoop) return;
            MoveOrIdle();
        }
    }

    private void SetBeingHeldState()
    {
        anim.SetBool("isBeingHeld", true);
        // anim.SetBool("isWalking", false);

        if (!chickenHoldIsPlaying)
        {
            am.Play("ChickenHold");
            chickenHoldIsPlaying = true;
        }
    }

    private void MoveOrIdle()
    {
        if (chickenHoldIsPlaying)
        {
            am.Stop("ChickenHold");
            chickenHoldIsPlaying = false;
        }

        anim.SetBool("isBeingHeld", false);

        if (isIdle)
        {
            idleTime -= Time.deltaTime;
            // anim.SetBool("isWalking", false);

            if (idleTime <= 0)
            {
                isIdle = false;
                SetNextAction();
            }
        }
        else
        {
            WalkAround();
        }
    }

    private void WalkAround()
    {
        changeDirectionTime -= Time.deltaTime;

        if (changeDirectionTime <= 0)
        {
            SetNextAction();
        }

        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);
        transform.LookAt(transform.position + targetDirection);

        // anim.SetBool("isWalking", true);
    }

    private void SetNextAction()
    {
        if (Random.value > 0.5f) // Randomly decide whether to walk or idle
        {
            targetDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            speed = Random.Range(minSpeed, maxSpeed);
            changeDirectionTime = Random.Range(minChangeTime, maxChangeTime);
            isIdle = false;
        }
        else
        {
            isIdle = true;
            idleTime = Random.Range(minIdleTime, maxIdleTime);
        }
    }

    public void LayEgg()
    {
        if (isInCoop) return;
        GameObject egg = Instantiate(eggPrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator LayEggTimer()
    {
        while(true) {
            yield return new WaitForSeconds(Random.Range(minEggLayTime, maxEggLayTime));
            LayEgg();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Egg")
        {
            Debug.Log("Egg collision");
        }
    }
}
