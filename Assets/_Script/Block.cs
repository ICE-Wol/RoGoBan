using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Block : MonoBehaviour {
    public static List<Block> blockList = new List<Block>();
    
    public BlockType type;
    public SpriteRenderer spriteRenderer;
    public bool isTemplate;
    public Color color;
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
        if(!isTemplate)
            spriteRenderer.color = color;
    }

    public void SetColor(Color c) {
        color = c;
        spriteRenderer.color = c;
    }

    protected void OnDestroy() {
        blockList.Remove(this);
    }
}
