using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LSPGen : MonoBehaviour
{
    public LevelSelectParticle levelSelectPrefab;

    public int timer;
    
    public void GenerateParticle() {
        var levelSelectCtrl = Instantiate(levelSelectPrefab);
        levelSelectCtrl.transform.position = new Vector3(Random.Range(-15f,15f), 10f, 0);
    }
    private void Update() {
        timer++;
        if (timer % 30 == 0) {
            GenerateParticle();
        }
    }
}
