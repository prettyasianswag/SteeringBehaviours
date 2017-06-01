using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehaviour
{
    public Transform target;
    public float stoppingDistance = 5f;

    public override Vector3 GetForce()
    {
        // SET force to zero
        Vector3 force = Vector3.zero;

        // IF target is null
        if (target == null)
        {
            // RETURN force
            return force;
        }

        // SET desiredForce to direction from target to owner's position;
        Vector3 desiredForce = target.position - owner.transform.position;
        // SET desiredForce y to zero
        desiredForce.y = 0f;

        // IF desiredForce is greater than stoppingDistance
        if (desiredForce.magnitude > stoppingDistance)
        {
            // SET desiredForce to desiredForce normalized x weighting
            desiredForce = desiredForce.normalized * weighting;
            // SET force to desiredForce - owner's velocity
            force = desiredForce - owner.velocity;
        }

        // RETURN force    
        return force;
    }
}
