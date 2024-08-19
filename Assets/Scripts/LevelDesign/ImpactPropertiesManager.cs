using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ImpactPropertiesManager : MonoBehaviour
{
    [SerializeField] List<ImpactPropertyData> impactData = new List<ImpactPropertyData>();

    [SerializeField] private int maxParticles = 20;

    public static ImpactPropertiesManager instance;

    HashSet<ParticleSystem> activeParticles = new HashSet<ParticleSystem>();

    private void Awake()
    {
        instance = this;
    }

    public void PlayImpactPropertyAtPoint(BaseBlock.ImpactMaterial material, Vector2 position) 
    {
        ImpactPropertyData impact = impactData.Find(i => i.impact == material);

        if (impact == null) 
        {
            Debug.LogError($"MISSING IMPACT MATERIAL FOR: {material.ToString()}");
            return;
        }

        if (activeParticles.Count > maxParticles) 
        {
            Debug.LogWarning("Over maxparticles watch out!");
            return;
        }

        ParticleSystem impactParticle = GameObject.Instantiate(impact.particle);
        activeParticles.Add(impactParticle);
        impactParticle.transform.position = position;
        

    }

    private void Update()
    {
        HashSet<ParticleSystem> trashList = new HashSet<ParticleSystem>();
        foreach(ParticleSystem active in activeParticles)
        {
            if (!active.isPlaying) 
            {
                trashList.Add(active);   
            }
        }

        foreach(ParticleSystem particle in trashList) 
        {
            activeParticles.Remove(particle);
            Destroy(particle.gameObject);
        }

    }
}
