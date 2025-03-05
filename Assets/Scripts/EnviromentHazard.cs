using UnityEngine;
using System.Collections;

public class EnvironmentHazard : MonoBehaviour
{
    public int damageAmount = 10;
    public float activationInterval = 2f;
    public Color activeColor = Color.red;
    public Color inactiveColor = Color.gray;

    private bool isActive = false;
    private Renderer hazardRenderer;

    private void Start()
    {
        hazardRenderer = GetComponent<Renderer>();
        StartCoroutine(ActivationCycle());
    }

    private IEnumerator ActivationCycle()
    {
        while (true)
        {
            isActive = !isActive;
            UpdateVisuals();
            PlayStateChangeSound();
            yield return new WaitForSeconds(activationInterval);
        }
    }

    private void UpdateVisuals()
    {
        hazardRenderer.material.color = isActive ? activeColor : inactiveColor;
    }

    private void PlayStateChangeSound()
    {
        // Play appropriate sound effect (implement this later)
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }
}
