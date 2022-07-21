using UnityEngine;

public class VFXDestroy : MonoBehaviour
{
    [SerializeField] private float _destroyTime;

    private void Awake()
    {
        _destroyTime = gameObject.GetComponent<ParticleSystem>().duration;
        Destroy(gameObject, _destroyTime);
    }
}
