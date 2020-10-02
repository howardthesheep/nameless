using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject Prefab;
    public float Radius;
    public float Mass;
    public float RestDensity;
    public float Viscosity;
    public float Drag;
    public int Amount;

    private float   smoothingRadius = 1.0f;
    private Vector3 gravity = new Vector3(0.0f, -9.81f, 0.0f);
    private float   gravityMultiplier = 200.0f;
    private float   gas = 200.0f;
    private float   dampening = -0.5f;

    private Particle[]         particles;
    private ParticleCollider[] particleColliders;
    private bool               clearing;

    private void Start()
    {
        Initialize();   
    }

    private void Update()
    {
        CalculateForces();
        ParticleMovement();
        CalculateCollisions();
    }

    // Create a Rectangle of particles to simulate within Unity
    private void Initialize()
    {
        particles = new Particle[Amount];
        float perRow = Mathf.Sqrt((float)Amount);
        perRow = Mathf.RoundToInt(perRow);

        for (int i = 0; i < Amount; i++)
        {
            // Create Particle, add it to game object and particle array
            GameObject currentGo = Instantiate(Prefab);
            Particle currentParticle = currentGo.AddComponent<Particle>();
            particles[i] = currentParticle;

            // Calculate Random position within limited area to place each particle
            float x = (i % perRow) + Random.Range(-0.1f, 0.1f);
            float y = (2 * Radius) + (float)((i / perRow) / perRow) * 1.1f;
            float z = ((i / perRow) % perRow) + Random.Range(-0.1f, 0.1f);

            // Update Game object transform props
            currentGo.transform.localScale = Vector3.one * Radius;
            currentGo.transform.position = new Vector3(x,y,z);

            // Set current Particles game object equal to the GameObject we just created
            currentParticle.Go = currentGo;
            currentParticle.Position = currentGo.transform.position;
        }
    }

    // Caluclates the forces between the particles/particle colliders
    private void CalculateForces()
    {
     
        for(int i = 0; i < particles.Length; i++)
        {
            if (clearing)
            {
                return;
            }

            // Find the density of particles in a neighborhood for each particle
            for(int j = i; j < particles.Length; j++)
            {
                // Get distance between two particles
                Vector3 direction = particles[j].Position - particles[i].Position;
                float distance = direction.magnitude;

                particles[i].Density = ParticleDensity(particles[i], distance);
                particles[i].Pressure = gas * (particles[i].Density - RestDensity);
            }
        }
    }

    // Calculates the density around the particle given its distance from another particle
    private float ParticleDensity(Particle currentParticle, float distance)
    {
        // If particle is within smoothing Radius
        if(distance < smoothingRadius)
        {
            // Reacalculate our particle Density
            return currentParticle.Density += Mass * (315.0f / (64.0f * Mathf.PI * Mathf.Pow(smoothingRadius, 9.0f))) *
                Mathf.Pow(smoothingRadius - distance, 3.0f);
        }
        return currentParticle.Density;
    }

    // Caluclates the pressure force between two particles
    private Vector3 ParticlePressure(Particle a, Particle b, Vector3 direction, float distance)
    {
        if (distance < smoothingRadius)
        {
            return -1 * (direction.normalized) * Mass * (a.Pressure + b.Pressure) / (2.0f * b.Density) *
                (-45.0f / (Mathf.PI * Mathf.Pow(smoothingRadius, 6.0f))) * Mathf.Pow(smoothingRadius - distance, 2.0f);
        }

        return Vector3.zero;
    }


    // Calculates the viscosity force between two particles
    private Vector3 ParticleViscosity(Particle a, Particle b, float distance)
    {
        if (distance < smoothingRadius)
        {
            return Viscosity * Mass * (b.Velocity - a.Velocity) / b.Density * (45.0f / (Mathf.PI *
                Mathf.Pow(smoothingRadius, 6.0f))) * (smoothingRadius - distance);
        }

        return Vector3.zero;
    }

    // Actually applies a movement force to our particles
    private void ParticleMovement()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (clearing)
            {
                return;
            }

            // Initialize pressure and viscosity forces
            Vector3 forcePressure = Vector3.zero;
            Vector3 forceViscosity = Vector3.zero;

            for (int j = i; j < particles.Length; j++)
            {
                // ignore self
                if (i == j) continue;

                // get vector between two particles
                Vector3 direction = particles[j].Position - particles[i].Position;
                float distance = direction.magnitude;

                // Calculate the pressure and viscosity forces between the two particles
                forcePressure += ParticlePressure(particles[i], particles[j], direction, distance);
                forceViscosity += ParticleViscosity(particles[i], particles[j], distance);
            }

            // Apply gravity force to the particles
            Vector3 forceGravity = gravity * particles[i].Density * gravityMultiplier;

            // Update the state of each particle with new values
            particles[i].CombinedForce = forcePressure + forceViscosity + forceGravity;
            particles[i].Velocity += Time.deltaTime * (particles[i].CombinedForce) / particles[i].Density;
            particles[i].Position += Time.deltaTime * particles[i].Velocity;
            particles[i].Go.transform.position = particles[i].Position;
        }
    }


    // Calculates the collisions between particles after their movement
    private void CalculateCollisions()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            for (int j = 0; j < particleColliders.Length; j++)
            {
                if(clearing || particleColliders.Length == 0)
                {
                    return;
                }

                Vector3 penetrationNormal;
                Vector3 penetrationPosition;
                float penetrationLength;
                if (Collision(particleColliders[j], particles[i].Position, Radius, out penetrationNormal, out penetrationPosition, out penetrationLength))
                {
                    particles[i].Velocity = DampenVelocity(particleColliders[j], particles[i].Velocity, penetrationNormal, 1.0f - Drag);
                    particles[i].Position = penetrationPosition - penetrationNormal * Mathf.Abs(penetrationLength);
                }
            }
        }
    }

    // Calculates the collision between two particles and returns some data regarding the collision
    private static bool Collision(ParticleCollider collider, Vector3 position, float radius, out Vector3 penetrationNormal, out Vector3 penetrationPosition, out float penetrationLength)
    {
        Vector3 colliderProjection = collider.Position - position;

        penetrationNormal = Vector3.Cross(collider.Right, collider.Up);
        penetrationLength = Mathf.Abs(Vector3.Dot(colliderProjection, penetrationNormal)) - (radius / 2.0f);
        penetrationPosition = collider.Position - colliderProjection;

        return penetrationLength < 0.0f
            && Mathf.Abs(Vector3.Dot(colliderProjection, collider.Right)) < collider.Scale.x
            && Mathf.Abs(Vector3.Dot(colliderProjection, collider.Up)) < collider.Scale.y;
    }

    // Recalculates the velocity of particles after a collision has occured
    private Vector3 DampenVelocity(ParticleCollider collider, Vector3 velocity, Vector3 penetrationNormal, float drag)
    {
        Vector3 newVelocity = Vector3.Dot(velocity, penetrationNormal) * penetrationNormal * dampening + Vector3.Dot(velocity, collider.Right) *
            collider.Right * drag + Vector3.Dot(velocity, collider.Up) * collider.Up * drag;

        return Vector3.Dot(newVelocity, Vector3.forward) * Vector3.forward + Vector3.Dot(newVelocity, Vector3.right) * Vector3.right
            + Vector3.Dot(newVelocity, Vector3.up) * Vector3.up;
    }

}
