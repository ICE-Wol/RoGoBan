using System;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    public ParticleCtrl particleTemplate;
    public int timer;

    public int genInterval;
    public int genCount;
    
    public Color genColor;
    
    public void GenerateParticle(Vector3 pos, Color color) {
        var particle = Instantiate(particleTemplate, pos, Quaternion.identity);
        particle.SetColor(color);
    }
    

    private void Update() {
        if (timer % genInterval == 0) {
            for (int i = 0; i < genCount; i++) {
                if(GameManager.Manager != null)
                    GenerateParticle(transform.position, GameManager.Manager.playerColor);
                else 
                    GenerateParticle(transform.position, Color.white);
            }
        }
        
        timer++;
    }
}
