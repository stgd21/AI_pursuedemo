using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SteeringType
{
    Seek,
    Flee,
    Pursue,
    Evade,
    FollowPath,
    Seperation
}

public class AppliedSteering : MonoBehaviour
{
    public Vector3 linearVelocity;
    public float angularVelocity;
    public Kinematic target;
    public SteeringType type;

    //Kinematic holds data about our agent
    private Kinematic kinematic;

    //For Pathfollow
    public Kinematic[] pathOfObjects;

    //For Seperation
    public Kinematic[] seperateObstacles;

    private PathFollow followAI;
    private Pursue pursueAI;
    private Seek seekAI;
    private Flee fleeAI;
    private Evade evadeAI;
    private Seperation seperationAI;

    private void Start()
    {
        kinematic = GetComponent<Kinematic>();

        switch (type)
        {
            case SteeringType.Pursue:
                pursueAI = new Pursue();
                pursueAI.character = kinematic;
                pursueAI.target = target;
                break;
            case SteeringType.Evade:
                evadeAI = new Evade();
                evadeAI.character = kinematic;
                evadeAI.target = target;
                break;
            case SteeringType.FollowPath:
                followAI = new PathFollow();
                followAI.character = kinematic;
                followAI.path = pathOfObjects;
                break;
            case SteeringType.Seek:
                seekAI = new Seek();
                seekAI.character = kinematic;
                seekAI.target = target;
                break;
            case SteeringType.Flee:
                fleeAI = new Flee();
                fleeAI.character = kinematic;
                fleeAI.target = target;
                break;
            case SteeringType.Seperation:
                seperationAI = new Seperation();
                seperationAI.character = kinematic;
                seperationAI.targets = seperateObstacles;
                break;
        }
    }
    void Update()
    {
        SteeringOutput steering;
        //Update position and rotation
        transform.position += linearVelocity * Time.deltaTime;
        Vector3 angularIncrement = new Vector3(0, angularVelocity * Time.deltaTime, 0);
        transform.eulerAngles += angularIncrement;

        switch (type)
        {
            case SteeringType.Pursue:
                steering = pursueAI.GetSteering();
                break;
            case SteeringType.Evade:
                steering = evadeAI.GetSteering();
                break;
            case SteeringType.FollowPath:
                steering = followAI.GetSteering();
                break;
            case SteeringType.Seek:
                steering = seekAI.GetSteering();
                break;
            case SteeringType.Flee:
                steering = fleeAI.GetSteering();
                break;
            case SteeringType.Seperation:
                steering = seperationAI.GetSteering();
                break;
            default:
                steering = seekAI.GetSteering();
                break;
        }
  
        linearVelocity += steering.linear * Time.deltaTime;
        angularVelocity += steering.angular * Time.deltaTime;

        //Update kinematic reference with complex data it can't get by itself
        kinematic.GetData(steering);
    }
}

public class SteeringOutput
{
    public Vector3 linear;
    public float angular;
}
