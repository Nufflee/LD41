using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private GameObject player;

    private float enemyHover = 0.75f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

        // Set the agent's position to the player's position.
        GetComponent<NavMeshAgent>().SetDestination(player.transform.position);

        GetComponent<NavMeshAgent>().baseOffset = enemyHover + (Mathf.Sin(Time.time * 6f)) / 26f;

        // Look at the player.
        transform.LookAt(player.transform);
        transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
        
    }
}
