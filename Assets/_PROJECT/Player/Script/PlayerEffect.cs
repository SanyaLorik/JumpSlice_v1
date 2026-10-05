using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _jump;
    [SerializeField] private ParticleSystem _fall;

    public void Jump()
    {
        _jump.Play();
    }

    public void Fall()
    {
        _fall.Play();
    }
}