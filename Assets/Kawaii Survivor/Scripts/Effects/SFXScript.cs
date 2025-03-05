using UnityEngine;

public class SFXScript : MonoBehaviour
{
    public AudioSource buttonSound;

    public void PlaySFX()
    {
        buttonSound.Play();
        Debug.LogWarning("SFX Played");
    }
}
