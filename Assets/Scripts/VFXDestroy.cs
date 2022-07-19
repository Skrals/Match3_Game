using System.Collections;
using UnityEngine;

public class VFXDestroy : MonoBehaviour
{
    [SerializeField] private float _destroyTime;
    [SerializeField] private ParticleSystem _vFX;

    private void Awake()
    {
        _destroyTime = _vFX.duration;
        StartCoroutine(DestroyTimer(_destroyTime));
    }

    private IEnumerator DestroyTimer(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
