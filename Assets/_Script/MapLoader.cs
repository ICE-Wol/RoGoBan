using System;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MapLoader : MonoBehaviour {
    public bool isPlayMode;
    public int currentLevelNum;

    public string currentLevelName;
    public TMP_Text levelNameText;

    public MapCtrl mapTemplate;
    public MapCtrl map;

    public TextAsset mapData;
    public TextAsset[] mapList;

    public WallCtrl wallPrefab;
    public BoxCtrl boxPrefab;
    public PlayerCtrl playerPrefab;
    public LCBannerCtrl bannerCtrl;

    void Awake() {
        if (isPlayMode) {
            LoadCurrentLevel();
        }
    }

    public void LoadCurrentLevel() {
        mapData = mapList[currentLevelNum];
        ParseMapData(mapData.text);

        currentLevelName = mapList[currentLevelNum].name;
        levelNameText.text = currentLevelName;
    }

    public void LoadNextLevel() {
        currentLevelNum++;
        if (currentLevelNum >= mapList.Length) {
            currentLevelNum = 0;
        }

        LoadCurrentLevel();
    }

    public void LoadPrevLevel() {
        currentLevelNum--;
        if (currentLevelNum < 0) {
            currentLevelNum = mapList.Length - 1;
        }

        LoadCurrentLevel();
    }

    public bool CheckLevelComplete() {
        for (int i = 0; i < map.mapSize.x; i++) {
            for (int j = 0; j < map.mapSize.y; j++) {
                if (map.ColorTags[i, j] != Color.clear) {
                    if (map.Objects[i, j] == null || map.ColorTags[i, j] != map.Objects[i, j].color) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private void Update() {
        if (isPlayMode) {
            if (CheckLevelComplete()) {
                StartCoroutine(bannerCtrl.ShowBanner());
                if (Input.anyKeyDown) {
                    LoadNextLevel();
                    StartCoroutine(bannerCtrl.HideBanner());
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
                LoadPrevLevel();
            if (Input.GetKeyDown(KeyCode.R)) {
                map.MemGrids();
                map.SetBlocksFromInitHistory();
            }

            if (Input.GetKeyDown(KeyCode.N))
                LoadNextLevel();
        }
    }

    void ParseMapData(string data) {
        string[] lines = data.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // 读取地图大小
        string[] size = lines[0].Split(' ');
        int width = int.Parse(size[0]);
        int height = int.Parse(size[1]);

        //Debug.Log(width + " " + height);
        if (map != null) {
            Destroy(map.gameObject);
            Debug.Log("Destroy map");
        }

        map = Instantiate(mapTemplate, transform);
        map.InitWithSize(width, height);

        Camera.main.transform.position
            = new Vector3((width + height) / 4f, (width + height) / 4f, -10);
        //Debug.Log("Map size: " + width + "x" + height);

        // 读取地图数据
        for (int i = 1; i < lines.Length; i++) {
            string[] lineData = lines[i].Trim().Split(' ');

            int x = int.Parse(lineData[0]);
            int y = int.Parse(lineData[1]);

            if (i > width * height) {
                int color = int.Parse(lineData[2]);
                //Debug.Log($"Color Tag ({x},{y}) - Color Type: {color}");
                MapCtrl.mapCtrl.SetColorTagToGrid(new Vector2Int(x, y),
                    MapCtrl.mapCtrl.EnumToColor((ColorType)color));

                continue;
            }

            int blockTypeInt = int.Parse(lineData[2]);

            int colorTypeInt;
            if (lineData.Length > 3) {
                colorTypeInt = int.Parse(lineData[3]);
            }
            else {
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
            newBlock.transform.SetParent(map.transform);
            MapCtrl.mapCtrl.SetObjectToGrid(new Vector2Int(x, y), newBlock);

        }
        map.MemGrids();

    }
}
