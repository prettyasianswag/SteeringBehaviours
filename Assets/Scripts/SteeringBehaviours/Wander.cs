using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GGL;

public class Wander : SteeringBehaviour
{
    public float offset = 1.0f;
    public float radius = 1.0f;
    public float jitter = 0.2f;

    private Vector3 targetDir;
    private Vector3 randomDir;
    private Vector3 circlePos;
    private Vector3 desiredForce;

    public override Vector3 GetForce()
    {
        // SET force to zero
        Vector3 force = Vector3.zero;

        // Generating a random number between -half max value to half max value
        // 0x7fff = 32767
        float randX = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);
        float randZ = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);

        #region Calculate RandomDir
        // SET randomDir to new Vector3 x = randX & z = randZ
        randomDir = new Vector3(randX, 0, randZ);
        // SET randomDir to normalized randomDir
        randomDir = randomDir.normalized;
        // SET randomDir to randomDir x jitter
        randomDir = randomDir * jitter;

        #endregion

        #region Calculate TargetDir
        // SET targetDir to targetDir + randomDir
        targetDir += randomDir;
        // SET targetDir to normalized targetDIr
        targetDir = targetDir.normalized;
        // SET targetDir to targetDir x radius
        targetDir = targetDir * radius;
        #endregion

        #region Calculate Force
        // SET seekPos to owner's position + targetDir
        // (owner.transform.position)
        Vector3 seekPos = owner.transform.position + targetDir; // not the answer
        // SET seekPos to seekPos + owner's forward x offset
        seekPos = seekPos + owner.transform.forward * offset;

        #region GIZMOS
        Vector3 offsetPos = transform.position + transform.forward.normalized * offset;
        GizmosGL.AddCircle(offsetPos + Vector3.up * 0.1f, radius, Quaternion.LookRotation(Vector3.down), 16, Color.red);
        Vector3 jitterPos = seekPos;
        GizmosGL.AddCircle(seekPos + Vector3.up * 0.15f, radius * 0.6f, Quaternion.LookRotation(Vector3.down), 16, Color.blue);
        #endregion

        // SET desiredForce to seekPos - position
        desiredForce = seekPos - transform.position;
        // IF desiredForce is not zero
        if (desiredForce.magnitude != 0)
        {
            // SET desiredForce to desiredForced normalized x weighting
            desiredForce = desiredForce.normalized * weighting;
            // SET force to desiredForce - owner's velocity
            force = desiredForce - owner.velocity;
        }
        #endregion

        // RETURN force
        return force;
    }

}
