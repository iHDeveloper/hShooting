using UnityEngine;
using System;
public class PlayerWeapon : MonoBehaviour{

    public string name = "Weapon Name";

    public int minDamge = 0;
    public int maxDamge = 10;
    public float range = 100f;
    public float reload = 1f;

    public ParticleSystem particle;

    private float RELOAD_GUN;

    public void Setup()
    {
        RELOAD_GUN = reload;
    }

    public void ResetReload()
    {
        reload = RELOAD_GUN;
    }

    public int GetNewDamge()
    {
        System.Random random = new System.Random();
        int damge = random.Next(minDamge, maxDamge);
        return damge;
    }

    public void TrueOnParticle()
    {
        particle.Simulate(0, false, true);
        Debug.Log("Starting");
    }

    public void TrueOffParticle()
    {
        particle.Stop();
        Debug.Log("Stoping");
    }

}
