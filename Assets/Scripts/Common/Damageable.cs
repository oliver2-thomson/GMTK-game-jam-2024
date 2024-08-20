using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{

    public enum ImpactMaterial
    {
        Metal = 0,
        Wood = 1,
        Dirt = 2,
        Blood = 3,
        Stone = 4
    }

    [SerializeField] private ImpactMaterial material;
    [SerializeField] private float MaxHealth;
    public float _Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value > MaxHealth)
            {
                currentHealth = MaxHealth;
            }
            else if (value < 0)
            {
                currentHealth = 0;
                OnDeath();
            }
            else
            {
                currentHealth = value;
            }
        }
    }

    private float currentHealth;

    public virtual void Awake()
    {
        currentHealth = MaxHealth;
    }
    public virtual void DamageAtPoint(Vector2 point, float damage)
    {
        ImpactPropertiesManager.instance.PlayImpactPropertyAtPoint(material, point);
        DamageBlock(damage);
    }

    public virtual void DamageBlock(float damage)
    {
        _Health -= damage;
    }

    public abstract void OnDeath();
}
