using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private InputAgent[] agents;
    [SerializeField]
    private MovementController[] movementControllers;
    private Vector3 initialPosition;
    private Rigidbody body;
    private void Start()
    {
        initialPosition = transform.position;
        body = GetComponent<Rigidbody>();
        ResetAll();
    }
    public void ResetAll()
    {
        foreach (MovementController gm in movementControllers)
            gm.ResetObject();
        transform.rotation = Quaternion.identity;
        transform.position = initialPosition;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.AddForce(new Vector3(Random.Range(-2,2), Random.Range(-0.5f,4), Random.Range(-5,5)), ForceMode.VelocityChange);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player1"))
        {
            foreach(InputAgent inputAgent in agents)
            {
                if (inputAgent.gameObject.tag.Equals("P1"))
                {
                    inputAgent.SetReward(-1);
                    inputAgent.EndEpisode();
                }
                else
                { 
                    inputAgent.SetReward(1);
                    inputAgent.EndEpisode();
                }
            }
            Debug.Log("Player 2 wins!");
            ResetAll();
        }
        if(other.gameObject.tag.Equals("Player2"))
        {
            foreach (InputAgent inputAgent in agents)
            {
                if (inputAgent.gameObject.tag.Equals("P1"))
                {
                    inputAgent.SetReward(1);
                    inputAgent.EndEpisode();
                }
                else
                {
                    inputAgent.SetReward(-1);
                    inputAgent.EndEpisode();
                }
            }
            Debug.Log("Player 1 wins!");
            ResetAll();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag.Equals("P1") || collision.collider.gameObject.tag.Equals("P2"))
        {
            InputAgent input = collision.collider.GetComponent<InputAgent>();
            if (input != null)
                input.AddReward(0.1f);
        }
    }
}
