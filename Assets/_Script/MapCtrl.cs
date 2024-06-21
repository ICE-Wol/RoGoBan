using System;
using System.Collections.Generic;
using System.IO;
using _Scripts.Tools;
using TMPro;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum BlockType{
    Empty,
    Wall,
    Box,
    Player,
}

public enum ColorType{
    None,
    Red,
    Green,
    Blue,
    Yellow,
    Cyan,
    Magenta,
    Black,
    YinYang,
    White,
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
        
        //玩家、箱子、墙等对象
        Objects = new Block[mapSize.x, mapSize.y];
        
        //地板，用于射线检测
        Grids = new SpriteRenderer[mapSize.x, mapSize.y];
        
        //地板标记，用于通关
        ColorTags = new Color[mapSize.x, mapSize.y];
        
        //历史记录，用于撤销
        HistoryList = new List<(Block[,],Color[,])>();

        InitGrids();
    }
    
    public Vector2Int mapSize;
    
    public SpriteRenderer gridTemplate;
    
    public SpriteRenderer[,] Grids;
    public Color[,] ColorTags;
    public Block[,] Objects;
    
    public List<(Block[,],Color[,])> HistoryList;

    private void InitGrids() {
        // (1) 场景、prefab、gameobject里没有挂载对象
        // 用数据生成出来
        // (2) 场景、prefab里已经有配置好的关卡 gameobject
        // 直接 Instantiate 一整个关卡 prefab
        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                GameObject grid = Instantiate(gridTemplate.gameObject, new Vector3(x, y, 0), Quaternion.identity);
                grid.transform.SetParent(transform);
                Grids[x, y] = grid.GetComponent<SpriteRenderer>();
                grid.GetComponent<Block>().pos = new Vector2Int(x, y);
            }
        }
    }

    public ColorType ColorToEnum(Color c) {
        if(c == Color.red) return ColorType.Red;
        if(c == Color.green) return ColorType.Green;
        if(c == Color.blue) return ColorType.Blue;
        if(c == new Color(1,1,0,1)) return ColorType.Yellow;
        if(c == Color.cyan) return ColorType.Cyan;
        if(c == Color.magenta) return ColorType.Magenta;
        if(c == Color.black) return ColorType.Black;
        if(c == Color.gray) return ColorType.YinYang;
        if(c == Color.white) return ColorType.White;
        return ColorType.White;
    }
    
    public Color EnumToColor(ColorType c) {
        switch (c) {
            case ColorType.Red: return Color.red;
            case ColorType.Green: return Color.green;
            case ColorType.Blue: return Color.blue;
            case ColorType.Yellow: return new Color(1, 1, 0, 1);
            case ColorType.Cyan: return Color.cyan;
            case ColorType.Magenta: return Color.magenta;
            case ColorType.Black: return Color.black;
            case ColorType.YinYang: return Color.gray;
            case ColorType.White: return Color.white;
            default: return Color.white;
        }
    }


    public void SaveMapData() {
        if (Input.GetKeyDown(KeyCode.C)) {
            string data = "";
            
            //记录方块
            data += mapSize.x + " " + mapSize.y + "\n";
            for (int i = 0; i < mapSize.x; i++) {
                for (int j = 0; j < mapSize.y; j++) {
                    data += i + " " + j + " ";
                    if (Objects[i, j] == null) {
                        data += "0\n";
                        continue;
                    }
                    data += (int)Objects[i, j].type + " " + (int)ColorToEnum(Objects[i, j].color) + "\n";
                }
                data += "\n";
            }
            
            //记录地板标记
            
            for (int i = 0; i < mapSize.x; i++) {
                for (int j = 0; j < mapSize.y; j++) {
                    if(ColorTags[i, j] == Color.clear) continue;
                    data += i + " " + j + " ";
                    data += (int)ColorToEnum(ColorTags[i, j]) + "\n";
                }
                data += "\n";
            }
            Debug.Log(data);
            var date = DateTime.Now.ToString("HH-mm-ss-MM-dd-yyyy");
            string path = "Assets/Resources/" + date + ".txt";
            File.WriteAllText(path,data);
            //AssetDatabase.Refresh();
        }
    }

    public void MemGrids() {
        var tempGrid = new Block[mapSize.x, mapSize.y];
        var tempColor = new Color[mapSize.x, mapSize.y];
        for (int i = 0; i < mapSize.x; i++) {
            for (int j = 0; j < mapSize.y; j++) {
                tempGrid[i, j] = Objects[i, j];
                if(Objects[i, j] != null)
                    tempColor[i, j] = Objects[i, j].color;
            }
        }
        HistoryList.Add((tempGrid, tempColor));
    }
    
    public void SetBlocksFromHistory() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (HistoryList.Count == 0) {
                print("No History!");
                return;
            }
            //print(HistoryList.Count);
            Objects = HistoryList[^1].Item1;
            var colors = HistoryList[^1].Item2;
            
            
            HistoryList.RemoveAt(HistoryList.Count - 1);
            
            for (int i = 0; i < mapSize.x; i++) {
                for (int j = 0; j < mapSize.y; j++) {
                    if (Objects[i, j] != null) {
                        if (!Objects[i, j].gameObject.activeSelf) {
                            Objects[i, j].gameObject.SetActive(true);
                        }
                        
                        Objects[i, j].pos = new Vector2Int(i, j);
                        Objects[i, j].tarPos = new Vector3(i, j, 0);
                        Objects[i, j].SetColor(colors[i, j]);
                        SetObjectToGrid(Objects[i,j].pos, Objects[i, j]);
                    }
                }
            }
            
        }
    }
    

    
    
    private void Update() {
        SetBlocksColorInGrid();
        SetBlocksFromHistory();
        
        SaveMapData();
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

    public void SetColorToObject(Vector2Int pos, Color color) {
        if (IsPosValid(pos)) GetObjectFromGrid(pos).color = color;
    }
    
    public Color GetColorFromObject(Vector2Int pos) {
        if (IsPosValid(pos)) {
            if (GetObjectFromGrid(pos) != null)
                return GetObjectFromGrid(pos).color;
        }
        return Color.clear;
    }

    public Block GetObjectFromGrid(Vector2 pos) {
        return Objects[(int)pos.x, (int)pos.y];
    }

    public BlockType GetObjectTypeFromGrid(Vector2 pos) {
        var obj = Objects[(int)pos.x, (int)pos.y];
        if (obj == null) return BlockType.Empty;
        return obj.type;
    }

    public void SetColorTagToGrid(Vector2Int pos, Color c) {
        if(ColorTags[pos.x, pos.y] == Color.clear)
            ColorTags[pos.x, pos.y]= c;
        else ColorTags[pos.x, pos.y] = Color.clear;
    }

    public Color GetColorTagFromGrid(Vector2Int pos) {
        return ColorTags[pos.x, pos.y];
    }

    private void SetBlocksColorInGrid() {
        for (int i = 0; i < mapSize.x; i++) {
            for (int j = 0; j < mapSize.y; j++) {
                Grids[i, j].color = new Color(1,25/255f,0,0.2f);//ColorTags[i, j];
                //todo
                SpriteRenderer[] sr = Grids[i, j].GetComponentsInChildren<SpriteRenderer>();
                sr[1].color = ColorTags[i, j];
                //Grids[i, j].color = Color.gray;
                // var m = Objects[i, j];
                // if (m == null) {
                //     Grids[i, j].color = Color.gray;
                //     continue;
                // }
                //
                // switch (Objects[i, j].type) {
                //     case BlockType.Box:
                //         Grids[i, j].color = Color.yellow;
                //         break;
                //     case BlockType.Wall:
                //         Grids[i, j].color = Color.red;
                //         break;
                //     case BlockType.Player:
                //         Grids[i, j].color = Color.blue;
                //         break;
                //}
            }
        }
    }
}
