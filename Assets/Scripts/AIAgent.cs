using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public Vector3 force;
    public Vector3 velocity;
    public float maxVelocity = 100f;

    private SteeringBehaviour[] behaviours;

    void Start()
    {
        behaviours = GetComponents<SteeringBehaviour>();
    }

    void Update()
    {
        ComputeForces();
        ApplyVelocity();
    }

    void ComputeForces()
    {
        // SET force to zero
        force = Vector3.zero;
        // FOR i = 0 to behaviours length
        for (int i = 0; i < behaviours.Length; i++)
        {
            SteeringBehaviour behaviour = behaviours[i];
            // IF behaviour is not enabled
            if (!behaviour.enabled)
            {
                // CONTINUE
                continue;
            }
            // SET force to force + behaviour's force
            force += behaviour.GetForce(); // += is like saying force = force + behaviour.GetForce();

            // IF force is greater than maxVelocity    
            if (force.magnitude > maxVelocity)
            {
                // SET force to force normalized x maxVelocity
                force = force.normalized * maxVelocity;
                // BREAK
                break;
            }
        }
    }

    void ApplyVelocity()
    {
        // SET velocity to velocity + force x delta time
        velocity += force * Time.deltaTime;
        // IF velocity is greater than maxVelocity 
        if (velocity.magnitude > maxVelocity)
        {
            // SET velocity to velocity normalized x maxVelocity
            velocity = velocity.normalized * maxVelocity;
        }

        // IF velocity is greater than zero
        if (velocity != Vector3.zero)
        {
            // SET position to position + velocity x delta time
            transform.position += velocity * Time.deltaTime;
            // SET rotation to Quaternion.LookRotation velocity
            transform.rotation = Quaternion.LookRotation(velocity); 
        }
    }
}
