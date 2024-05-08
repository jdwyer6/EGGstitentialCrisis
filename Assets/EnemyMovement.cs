using UnityEngine;
using UnityEngine.AI; // Include the AI namespace for NavMesh components

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    public float chaseDistance = 15f;

    private NavMeshAgent agent;
    private float distanceToPlayer;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(player.position, transform.position);

            if (distanceToPlayer <= chaseDistance)
            {
                ChasePlayer();
            }
            else
            {
                Idle();
            }
        }
    }

    private void ChasePlayer()
    {
        anim.SetBool("Run", true);
        agent.SetDestination(player.position);
    }

    private void Idle()
    {
        anim.SetBool("Run", false);
        // Here you can define what the enemy does when idle. For now, we'll just stop the agent.
        agent.ResetPath();
    }
}
