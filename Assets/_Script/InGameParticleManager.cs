using UnityEngine;

public class InGameParticleManager : MonoBehaviour
{
    public InGameParticleCtrl particle;
    
    public void CreateParticle(Vector3 pos, int num) {
        for (int i = 0; i < num; i++) {
            var p = Instantiate(particle, pos, Quaternion.identity);
            p.centerPos = pos;
        }
    }
    int timer = 0;
    
    private void Update() {
        timer++;
        if (timer % 120 == 0) {
            CreateParticle(new Vector3(0, 0, 0), 5);
        }
    }
}
