using System;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool isAttacking = false;
    
    [SerializeField] ParticleSystem particle;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D bc;

    

    private void Update()
    {
        if (isAttacking)
        {
            if (!particle.isPlaying)
            {
                particle.Play();
            }

            RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, 100f, LayerMask.GetMask("Player"));

            float distance = ray.collider != null ? ray.distance : 100f;
            
            sr.size = new Vector2(ray.distance, 0.75f);
            bc.size = new Vector2(ray.distance, 0.5f);
            bc.offset = new Vector2(ray.distance / 2, 0);
            var main = particle.main;
            main.startLifetime = distance / particle.main.startSpeed.constant; 
            // Adjust divisor based on your particle speed (or hardcode if preferred)
        }
        else
        {
            if (particle.isPlaying)
            {
                particle.Stop();
            }
        }
    }
}
