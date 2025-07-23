using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsPool : MonoBehaviour
{
    public static EffectsPool Instance;

    [System.Serializable]
    public class EffectInfo
    {
        public string effectName;
        public GameObject effectPrefab;
        public int initialPoolSize = 10;
    }

    public List<EffectInfo> effectInfos;

    private Dictionary<string, Queue<ParticleSystem>> effectPools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        InitializePools();
    }

    private void InitializePools()
    {
        effectPools = new Dictionary<string, Queue<ParticleSystem>>();
        foreach (var effectInfo in effectInfos)
        {
            Queue<ParticleSystem> pool = new Queue<ParticleSystem>();

            for (int i = 0; i < effectInfo.initialPoolSize; i++)
            {
                GameObject obj = Instantiate(effectInfo.effectPrefab, transform);
                ParticleSystem effect = obj.GetComponent<ParticleSystem>();
                obj.SetActive(false);
                pool.Enqueue(effect);
            }

            effectPools[effectInfo.effectName] = pool;
        }
    }

    public ParticleSystem GetEffect(string effectName)
    {
        if (!effectPools.ContainsKey(effectName))
        {
            Debug.LogError($"Effect {effectName} not found in the pool!");
            return null;
        }

        if (effectPools[effectName].Count == 0)
        {
            EffectInfo info = effectInfos.Find(e => e.effectName == effectName);
            GameObject newObj = Instantiate(info.effectPrefab, transform);
            ParticleSystem newEffect = newObj.GetComponent<ParticleSystem>();
            return newEffect;
        }

        ParticleSystem effect = effectPools[effectName].Dequeue();
        effect.gameObject.SetActive(true);
        return effect;
    }

    public void ReturnEffect(string effectName, ParticleSystem effect)
    {
        effect.gameObject.SetActive(false);
        effectPools[effectName].Enqueue(effect);
    }

    public void PlayEffect(string effectName, Vector3 position, float duration = 0f)
    {
        ParticleSystem effect = GetEffect(effectName);
        if (effect != null)
        {
            effect.transform.position = position;
            effect.Play();

            if (duration <= 0)
            {
                duration = effect.main.duration;
            }
            
            StartCoroutine(ReturnEffectAfterDuration(effectName, effect, duration));
        }
    }

    private IEnumerator ReturnEffectAfterDuration(string effectName, ParticleSystem effect, float duration)
    {
        yield return new WaitForSeconds(duration);        
        ReturnEffect(effectName, effect);
    }
}
