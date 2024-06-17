using System;
using System.Collections.Generic;
using _Scripts.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum BlockType{
    Empty,
    Wall,
    Box,
    Player,
}

public class MapCtrl : MonoBehaviour {
    public static MapCtrl mapCtrl;

    private void Awake() {
        if (mapCtrl == null) {
            mapCtrl = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public Vector2Int mapSize;

    public SpriteRenderer gridTemplate;
    public SpriteRenderer[,] Grids;
    public Color[,] ColorTags;
    public Block[,] Objects;

    public PlayerCtrl playerCtrl;

    public List<Block[,]> HistoryList;
    private void Start() {
        //玩家、箱子、墙等对象
        Objects = new Block[mapSize.x, mapSize.y];
        
        //地板，用于射线检测
        Grids = new SpriteRenderer[mapSize.x, mapSize.y];
        
        //地板标记，用于通关
        ColorTags = new Color[mapSize.x, mapSize.y];
        
        //历史记录，用于撤销
        HistoryList = new List<Block[,]>();

        // (1) 场景、prefab、gameobject里没有挂载对象
        // 用数据生成出来
        // (2) 场景、prefab里已经有配置好的关卡 gameobject
        // 直接 Instantiate 一整个关卡 prefab
        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                GameObject grid = Instantiate(gridTemplate.gameObject, new Vector3(x, y, 0), Quaternion.identity);
                grid.transform.SetParent(transform);
                Grids[x, y] = grid.GetComponent<SpriteRenderer>();
            }
        }

        //playerCtrl = Instantiate(playerCtrl, new Vector3(0, 0, 0), Quaternion.identity);
        SetObjectToGrid(new Vector2Int(0, 0), playerCtrl);
    }

    public void MemGrids() {
        var tempGrid = new Block[mapSize.x, mapSize.y];
        for (int i = 0; i < mapSize.x; i++) {
            for (int j = 0; j < mapSize.y; j++) {
                tempGrid[i, j] = Objects[i, j];
            }
        }
        HistoryList.Add(tempGrid);
    }
    
    public void SetBlocksFromHistory() {
        if (Input.GetMouseButtonDown(1)) {
            if (HistoryList.Count == 0) return;
            Objects = HistoryList[^1];
            HistoryList.RemoveAt(HistoryList.Count - 1);
            for (int i = 0; i < mapSize.x; i++) {
                for (int j = 0; j < mapSize.y; j++) {
                    if (Objects[i, j] != null) {
                        Objects[i, j].pos = new Vector2Int(i, j);
                        Objects[i, j].tarPos = new Vector3(i, j, 0);
                    }
                }
            }
        }
    }
    

    
    
    private void Update() {
        SetBlocksColorInGrid();
        SetBlocksFromHistory();
    }

    public bool IsPosValid(Vector2Int pos) {
        return pos.x >= 0 && pos.x < mapSize.x && pos.y >= 0 && pos.y < mapSize.y;
    }

    public void SetObjectToGrid(Vector2Int pos, Block obj) {
        if (IsPosValid(pos)) {
            Objects[pos.x, pos.y] = obj;
            //Debug.Log("Set Object To Grid: " + obj + " At " + pos);
        }
        //else Debug.LogError($"Invalid Position {pos}");
    }

    public void SetColorTagToGrid(Vector2Int pos, Color color) {
        if (IsPosValid(pos)) ColorTags[pos.x, pos.y] = color;
    }

    public Block GetObjectFromGrid(Vector2 pos) {
        return Objects[(int)pos.x, (int)pos.y];
    }

    public BlockType GetObjectTypeFromGrid(Vector2 pos) {
        var obj = Objects[(int)pos.x, (int)pos.y];
        if (obj == null) return BlockType.Empty;
        return obj.type;
    }

    public Color GetColorTagFromGrid(Vector2 pos) {
        return ColorTags[(int)pos.x, (int)pos.y];
    }

    private void SetBlocksColorInGrid() {
        for (int i = 0; i < mapSize.x; i++) {
            for (int j = 0; j < mapSize.y; j++) {
                var m = Objects[i, j];
                if (m == null) {
                    Grids[i, j].color = Color.white;
                    continue;
                }

                switch (Objects[i, j].type) {
                    case BlockType.Box:
                        Grids[i, j].color = Color.yellow;
                        break;
                    case BlockType.Wall:
                        Grids[i, j].color = Color.red;
                        break;
                    case BlockType.Player:
                        Grids[i, j].color = Color.blue;
                        break;
                }
            }
        }
    }

}

//     public void ChangeBlockType() {
//         if (Input.GetMouseButtonDown(0)) {
//             var pos = Input.mousePosition;
//             pos = Camera.main.ScreenToWorldPoint(pos);
//             var x = (int)(pos.x + 0.5f);
//             var y = (int)(pos.y + 0.5f);
//             //if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y) {
//             if (IsPosValid(new Vector2Int(x, y))) {
//                 int blockType;
//                 if(Objects[x, y] == null) {
//                     blockType = 0;
//                 }
//                 else {
//                     blockType = (int)Objects[x, y].type;
//                 }
//                 if (blockType != 3) {
//                     blockType += 1;
//                     if(blockType == 3) blockType = 0;
//                 }
//                 else {
//                     print("Can't Change Player Type!");
//                     return;
//                 }
//
//                 Debug.Log("Change Grid Type To: " + (BlockType)blockType);
//                 Block newBlock = null;
//                 if (blockType == 1) {
//                     newBlock = Instantiate(wallCtrl, new Vector3(x, y, 0), Quaternion.identity);
//                 }
//                 else if (blockType == 2) {
//                     newBlock = Instantiate(boxCtrl, new Vector3(x, y, 0), Quaternion.identity);
//                     newBlock.tarPos = new Vector3(x, y, 0);
//                     newBlock.pos = new Vector2Int(x, y);
//                     newBlock.HistoryPos.Add((newBlock.pos, newBlock.color));
//                     
//                 }
//                 if(Objects[x,y] != null)
//                     Destroy(Objects[x, y].gameObject);
//                 Objects[x, y] = newBlock;
//
//             }
//         }
//     }
