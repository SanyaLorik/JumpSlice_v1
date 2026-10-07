using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] private Transform _center;
    [SerializeField] private ParticleSystem _jump;
    [SerializeField] private ParticleSystem _fall;

    public void Jump()
    {
        _jump.transform.position = _center.position;

        _jump.Play();
    }

    public void Fall()
    {
        _fall.transform.position = _center.position;

        _fall.Play();
    }
}