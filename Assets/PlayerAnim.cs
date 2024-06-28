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

    [FormerlySerializedAs("eyeFlipMultiplier")] public float eyeFlickMultiplier = 1f;
    [FormerlySerializedAs("eyeFlipTimer")] public float eyeFlickTimer;
    [FormerlySerializedAs("eyeFlipTime")] public float eyeFlickTime;
    
    //public float curEyeOffsetY;
    //public float tarEyeOffsetY;
    public Vector2 tarEyeOffset;
    public Vector2 curEyeOffset;

    public float curEyeScaleY;
    public float tarEyeScaleY = 2f;

    
    public float bodyWriggleTimer;
    public float bodyWriggleSpdMul = 20f;
    public float curBodyScaleY;
    public float tarBodyScaleY;
    public float curBodyScaleX;
    public float tarBodyScaleX;

    public float eyeBackTimer;
    
    public Vector2Int[] dirVector = 
    {
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
    };
    
    private void Start() {
        eyeFlickTime = 0;
    }

    private void Update() {
        
        if(!playerCtrl.isMoving) {
            tarBodyScaleX = 1f;
            tarBodyScaleY = 1f;
            bodyWriggleTimer = Mathf.PI / 2f / 20f;
        }
        else if(curDir == 0 || curDir == 2) {
            bodyWriggleTimer += Time.deltaTime;
            tarBodyScaleX = 1.2f - 0.3f * Mathf.Sin(bodyWriggleTimer * bodyWriggleSpdMul);
            tarBodyScaleY = 0.8f + 0.3f * Mathf.Sin(bodyWriggleTimer * bodyWriggleSpdMul);
        }
        else {
            bodyWriggleTimer += Time.deltaTime;
            tarBodyScaleX = 0.8f + 0.3f * Mathf.Sin(bodyWriggleTimer * bodyWriggleSpdMul);
            tarBodyScaleY = 1.2f - 0.3f * Mathf.Sin(bodyWriggleTimer * bodyWriggleSpdMul);
        }
        
        curBodyScaleX.ApproachRef(tarBodyScaleX, 16f);
        curBodyScaleY.ApproachRef(tarBodyScaleY, 16f);
        
        spriteRenderer.transform.localScale = 
            new Vector3(curBodyScaleX, curBodyScaleY, 1);
        
        //暂停3秒后眼睛回到中间
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
        
        //眨眼,小概率眨两次
        eyeFlickTime += Time.deltaTime;
        if (eyeFlickTime > eyeFlickTimer) {
            eyeFlickTime = 0;
            eyeFlickTimer = Random.Range(4f, 4.5f);
            if (eyeFlickTimer < 4.1f) {
                eyeFlickTimer = 0.5f;
            }
            eyeFlickMultiplier *= -1f;
            
            tarEyeScaleY *= -1f;
        }
        curEyeScaleY.ApproachRef(tarEyeScaleY, 16f);
        //tarEyeOffsetY = eyeFlickMultiplier * (eyeFloatMag * Mathf.Sin(Time.time * eyeFloatSpeed) + eyeFloatBase);
        //curEyeOffsetY.ApproachRef(tarEyeOffsetY, 16f);

        //眼睛移动
        if (curDir == -1) {
            if((eyeBackTimer % 3f).Equal(1.5f,0.01f)) {
                tarEyeOffset = 0.1f * Random.Range(0f, 360f).Deg2Dir();
                //print("Set Eye Offset Left");
            }
            if((eyeBackTimer % 3f) < 1.5f){
                tarEyeOffset = Vector2.zero;
            }
        }
        else {
            tarEyeOffset = 0.1f * (Vector2)dirVector[curDir];
        }
        curEyeOffset = curEyeOffset.ApproachValue(tarEyeOffset, 16f);
        
        eyes[0].transform.localPosition = new Vector3( initEyeOffsetX, 0.05f/*curEyeOffsetY*/, 0) + (Vector3)curEyeOffset;
        eyes[1].transform.localPosition = new Vector3(-initEyeOffsetX, 0.05f/*curEyeOffsetY*/, 0) + (Vector3)curEyeOffset;
        eyes[0].transform.localScale = new Vector3(1, curEyeScaleY, 1);
        eyes[1].transform.localScale = new Vector3(1, curEyeScaleY, 1);
    }
}
