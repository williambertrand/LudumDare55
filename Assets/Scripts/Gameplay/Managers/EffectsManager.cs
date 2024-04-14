using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    SPARKLE
}

public class EffectsManager: MonoSingleton<EffectsManager>
{
    // Start is called before the first frame update

    [SerializeField] private GameObject sparkle;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject effectForType(EffectType e)
    {
        switch(e)
        {
            case EffectType.SPARKLE:
                return sparkle;
            default:
                return null;
        }
    }

    public void SpawnEffectAtPosition(EffectType effect, Vector3 pos)
    {
        GameObject efx = effectForType(effect);
        if (efx == null) return;

        Instantiate(efx, pos, Quaternion.identity);
    }


}
