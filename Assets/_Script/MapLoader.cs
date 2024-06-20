using System;
using Unity.VisualScripting;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public TextAsset mapData;
    public TextAsset[] mapList;
    
    public MapCtrl blankMap;
    
    public WallCtrl wallPrefab;
    public BoxCtrl boxPrefab;
    public PlayerCtrl playerPrefab;

    void Start()
    {
        if (mapData != null)
        {
            ParseMapData(mapData.text);
        }
        else
        {
            Debug.LogError("Map data is not assigned.");
        }
    }

    void ParseMapData(string data)
    {
        string[] lines = data.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // 读取地图大小
        string[] size = lines[0].Split(' ');
        int width = int.Parse(size[0]);
        int height = int.Parse(size[1]);
        //Debug.Log("Map size: " + width + "x" + height);
        

        // 读取地图数据
        for (int i = 1; i < lines.Length; i++) {
            string[] lineData = lines[i].Trim().Split(' ');

            int x = int.Parse(lineData[0]);
            int y = int.Parse(lineData[1]);
            
            if(i > width * height) {
                int color = int.Parse(lineData[2]);
                //Debug.Log($"Color Tag ({x},{y}) - Color Type: {color}");
                MapCtrl.mapCtrl.SetColorTagToGrid(new Vector2Int(x,y), 
                    MapCtrl.mapCtrl.EnumToColor((ColorType)color));

                continue;
            }
            int blockTypeInt = int.Parse(lineData[2]);
            
            int colorTypeInt;
            if(lineData.Length > 3) {
                colorTypeInt = int.Parse(lineData[3]);
            } else {
                colorTypeInt = 0;
            }

            BlockType blockType = (BlockType)blockTypeInt;
            ColorType colorType = (ColorType)colorTypeInt;

            
            ////Debug.Log($"Cell ({i - 1}) - Block Type: {blockType}, Color Type: {colorType}");
            
            Block newBlock = null;
            switch (blockType) {
                case BlockType.Wall:
                    newBlock = Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    break;
                case BlockType.Box: 
                    newBlock = Instantiate(boxPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    newBlock.pos = new Vector2Int(x, y);
                    newBlock.tarPos = new Vector3(x, y, 0);
                    newBlock.color = MapCtrl.mapCtrl.EnumToColor(colorType);
                    break;
                case BlockType.Player:
                    newBlock = Instantiate(playerPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    newBlock.pos = new Vector2Int(x, y);
                    newBlock.tarPos = new Vector3(x, y, 0);
                    newBlock.color = MapCtrl.mapCtrl.EnumToColor(colorType);
                    break;
            }
            //Debug.Log($"Block {newBlock} created at ({x}, {y})");
            if (blockType == BlockType.Empty) continue;
            MapCtrl.mapCtrl.SetObjectToGrid(new Vector2Int(x, y),newBlock);
            
                
        }
    }
}
