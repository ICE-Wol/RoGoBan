using _Scripts.Tools;
using UnityEngine;
using UnityEngine.Serialization;

public class PillarCtrl : MonoBehaviour { 
    public SpriteRenderer blockPrefab;
    public SpriteRenderer[,] Blocks;
    public int height;
    public int width;

    public float initY = 9f;
    public float finalY = -8f;

    public float initX;

    public bool isActivated;
    public bool isArrived;
    public bool isLeaving;
    
    private void Start() {
        isArrived = false;
        Blocks = new SpriteRenderer[width, height];
        transform.localPosition = new Vector3(initX, initY, 0);
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                var block = Instantiate(blockPrefab, transform);
                block.transform.localPosition = new Vector3(i, j, 0);
                Blocks[i, j] = block;
            }
        }
    }
    
    private void Update() {
        if (!isLeaving) {
            if(isActivated)
                transform.localPosition =
                    transform.localPosition.ApproachValue(new Vector3(initX, finalY, 0), 12f);
        }
        else {
            transform.localPosition = 
            transform.localPosition.ApproachValue(new Vector3(initX, finalY - 20f, 0), 12f);
        }
        if (transform.localPosition.y < finalY + 0.1f) {
            isArrived = true;
        }
    }
}
