using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    // The maximum speed of the boid
    public float maxSpeed = 5f;

    // The maximum steering force of the boid
    public float maxSteerForce = 0.5f;

    // The radius around the boid that determines which other boids it perceives
    public float perceptionRadius = 3f;

    // The weight of the cohesion behavior
    public float cohesionWeight = 1f;

    // The weight of the separation behavior
    public float separationWeight = 1f;

    // The weight of the alignment behavior
    public float alignmentWeight = 1f;

    // The weight of the target-seeking behavior
    public float targetWeight = 1f;

    // The weight of the random movement behavior
    public float randomWeight = 1f;

    // The target for the boid to seek
    public Transform target;

    // The rigidbody for the boid
    private Rigidbody rb;

    void Start()
    {
        // Get the rigidbody for the boid
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get the nearby boids
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, perceptionRadius);
        List<Boid> nearbyBoids = new List<Boid>();
        foreach (Collider collider in nearbyColliders)
        {
            Boid boid = collider.GetComponent<Boid>();
            if (boid != null && boid != this)
            {
                nearbyBoids.Add(boid);
            }
        }

        // Calculate the cohesion, separation, and alignment steering forces
        Vector3 cohesionSteer = Cohesion(nearbyBoids) * cohesionWeight;
        Vector3 separationSteer = Separation(nearbyBoids) * separationWeight;
        Vector3 alignmentSteer = Alignment(nearbyBoids) * alignmentWeight;

        // Calculate the target-seeking and random movement steering forces
        Vector3 targetSteer = Vector3.zero;
        if (target != null)
        {
            targetSteer = Seek(target.position) * targetWeight;
        }
        Vector3 randomSteer = RandomMovement() * randomWeight;

        // Combine the steering forces
        Vector3 steerForce = cohesionSteer + separationSteer + alignmentSteer + targetSteer + randomSteer;

        // Limit the steering force
        if (steerForce.magnitude > maxSteerForce)
        {
            steerForce = steerForce.normalized * maxSteerForce;
        }

        // Apply the steering force to the velocity of the boid
        Vector3 newVelocity = rb.velocity + steerForce * Time.fixedDeltaTime;

        // Limit the speed of the boid
        if (newVelocity.magnitude > maxSpeed)
        {
            newVelocity = newVelocity.normalized * maxSpeed;
        }

        // Set the velocity of the rigidbody
        rb.velocity = newVelocity;

        // Rotate the boid to face its direction of travel
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    // Calculates the cohesion steering force for the boid
    Vector3 Cohesion(List<Boid> nearbyBoids)
    {
        Vector3 centerOfMass = Vector3.zero;
        foreach (Boid boid in nearbyBoids)
        {
            centerOfMass += boid.transform.position;
        }
        if (nearbyBoids.Count > 0)
        {
            centerOfMass /= nearbyBoids.Count;
        }
    }