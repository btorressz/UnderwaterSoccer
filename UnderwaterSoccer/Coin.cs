using UnityEngine;

public class Coin : MonoBehaviour
{
    // The value of the coin
    public int value = 1;

    // The sound to play when the coin is collected
    public AudioClip collectSound;

    // The particle system to play when the coin is collected
    public ParticleSystem collectParticles;

    // The renderer for the coin
    private Renderer coinRenderer;

    // Whether or not the coin has been collected
    private bool isCollected = false;

    void Start()
    {
        // Get the renderer for the coin
        coinRenderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        // If the coin has already been collected, return
        if (isCollected)
        {
            return;
        }

        // If the collider is not a player, return
        Player player = other.GetComponent<Player>();
        if (player == null)
        {
            return;
        }

        // Add the value of the coin to the player's score
        player.AddScore(value);

        // Play the collect sound
        AudioSource.PlayClipAtPoint(collectSound, transform.position);

        // Play the collect particles
        collectParticles.Play();

        // Disable the renderer for the coin
        coinRenderer.enabled = false;

        // Set the coin as collected
        isCollected = true;

        // Destroy the coin after a short delay
        Destroy(gameObject, 0.5f);
    }
}