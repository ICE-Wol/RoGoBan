using System;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
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

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(!isTemplate)
            spriteRenderer.color = color;
    }

    public void SetColor(Color c) {
        color = c;
        spriteRenderer.color = c;
    }

    private void OnDestroy() {
        //Debug.Log("Destroy Block type " + type + " at " + pos);
    }
}
