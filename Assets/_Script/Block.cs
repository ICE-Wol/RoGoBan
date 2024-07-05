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

    protected virtual void Start()
    {
        if(type != BlockType.Empty) 
            blockList.Add(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
        oldPos = pos;
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
    protected void Update()
    {
        if (pos != oldPos)
        {
            var tagColor = MapCtrl.mapCtrl.ColorTags[pos.x, pos.y];
            if (tagColor == color)
            {
                if (isPlayer)
                {
                    /*play player sound*/
                    //玩家音效
                    WwiseSoundManager.Instance.PostEvent(gameObject, WwiseEventType.Player_Enter);

                }
                else
                {
                    /*play box sound*/
                    //BOX音效
                    Debug.Log("播放box in");
                    WwiseSoundManager.Instance.PostEvent(gameObject, WwiseEventType.Cube_Enter);
                }
            }
            var oldPosColor = MapCtrl.mapCtrl.ColorTags[oldPos.x, oldPos.y];
            if (oldPosColor == color)
            {
                if (isPlayer)
                {
                    WwiseSoundManager.Instance.PostEvent(gameObject, WwiseEventType.Player_Leave);
                }
                else
                {
                    WwiseSoundManager.Instance.PostEvent(gameObject, WwiseEventType.Cube_Leave);
                }
            }
            oldPos = pos;
        }

        if (!isTemplate) spriteRenderer.color =
            spriteRenderer.color.ApproachValue(color, 16f);
    }

    protected void OnDestroy()
    {
        blockList.Remove(this);
    }
}
