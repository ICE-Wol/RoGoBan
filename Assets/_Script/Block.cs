using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Tools;
using Unity.Collections;
using UnityEngine;

public class Block : MonoBehaviour {
    public static List<Block> blockList = new List<Block>();
    
    public BlockType type;
    public SpriteRenderer spriteRenderer;
    public bool isTemplate;
    public bool isPlayer;
    public Color color;

    public Vector2Int oldPos;
    /// <summary>
    /// 逻辑位置
    /// </summary>
    public Vector2Int pos;
    
    /// <summary>
    /// 动画目标位置
    /// </summary>
    public Vector3 tarPos;

    protected void Start() {
        if(type != BlockType.Empty) 
            blockList.Add(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color c) {
        color = c;
        //spriteRenderer.color = c;
    }

    public IEnumerator DisableAfterDelayCoroutine()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(1f);

        // 禁用游戏对象
        gameObject.SetActive(false);
    }
    protected void Update() {
        if(pos != oldPos) {
            var tagColor = MapCtrl.mapCtrl.ColorTags[pos.x, pos.y];
            if (tagColor == color) {
                if (isPlayer) {
                    /*play player sound*/
                }
                else {
                    /*play box sound*/
                }
            }
            oldPos = pos;
        }
        
        if(!isTemplate) spriteRenderer.color = 
            spriteRenderer.color.ApproachValue(color, 16f);
    }

    protected void OnDestroy() {
        blockList.Remove(this);
    }
}
