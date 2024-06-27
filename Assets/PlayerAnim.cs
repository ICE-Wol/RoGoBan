using _Scripts.Tools;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnim : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public PlayerCtrl playerCtrl;
    
    public SpriteRenderer[] eyes;

    public int curDir;
    public float initEyeOffsetX;
    public float eyeFloatMag;
    public float eyeFloatSpeed;
    public float eyeFloatBase;

    public float eyeFlipMultiplier = 1f;
    public float eyeFlipTimer;
    public float eyeFlipTime;
    
    public float curEyeOffsetY;
    public float tarEyeOffsetY;
    public Vector2 tarEyeOffset;
    public Vector2 curEyeOffset;

    public float curScaleY;
    public float tarScaleY = 2f;

    public float eyeBackTimer;
    public int memDir;
    
    public Vector2Int[] dirVector = 
    {
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
    };
    
    private void Start() {
        eyeFlipTime = 0;
    }

    private void Update() {
        if(curDir != playerCtrl.curDir) {
            eyeBackTimer = 0;
        }
        else {
            eyeBackTimer += Time.deltaTime;
            if(eyeBackTimer > 3f) {
                curDir = -1;
                playerCtrl.curDir = -1;
            }
        }
        curDir = playerCtrl.curDir;
        
        
        
        

        eyeFlipTime += Time.deltaTime;
        if (eyeFlipTime > eyeFlipTimer) {
            eyeFlipTime = 0;
            eyeFlipTimer = Random.Range(4f, 4.5f);
            if (eyeFlipTimer < 4.1f) {
                eyeFlipTimer = 0.5f;
            }
            eyeFlipMultiplier *= -1f;
            
            tarScaleY *= -1f;
        }
        
        curScaleY.ApproachRef(tarScaleY, 16f);

        if (curDir == -1) {
            tarEyeOffset = Vector2.zero;
        }
        else {
            tarEyeOffset = 0.1f * (Vector2)dirVector[curDir];
        }
        curEyeOffset = curEyeOffset.ApproachValue(tarEyeOffset, 16f);
        
        tarEyeOffsetY = eyeFlipMultiplier * (eyeFloatMag * Mathf.Sin(Time.time * eyeFloatSpeed) + eyeFloatBase);
        curEyeOffsetY.ApproachRef(tarEyeOffsetY, 16f);
        
        eyes[0].transform.localPosition = new Vector3( initEyeOffsetX, curEyeOffsetY, 0) + (Vector3)curEyeOffset;
        eyes[1].transform.localPosition = new Vector3(-initEyeOffsetX, curEyeOffsetY, 0) + (Vector3)curEyeOffset;
        eyes[0].transform.localScale = new Vector3(1, curScaleY, 1);
        eyes[1].transform.localScale = new Vector3(1, curScaleY, 1);
    }
}
