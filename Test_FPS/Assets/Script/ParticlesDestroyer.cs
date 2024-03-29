using UnityEngine;

public class ParticlesDestroyer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _tenporaryParticle;

    private void Update()
    {
        if(!_tenporaryParticle.isPlaying)
        {
            Destroy(_tenporaryParticle.gameObject);
        }
    }
}

