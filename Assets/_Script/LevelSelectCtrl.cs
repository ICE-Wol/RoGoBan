using System;
using _Scripts.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[Serializable]
public struct levelSet
{
    public string LevelName;
    public int LevelNum;
}
public class LevelSelectCtrl : MonoBehaviour
{
    public SpriteRenderer levelTagTemplate;
    public SpriteRenderer[] levelTags;
    
    public TMP_Text[] levelNumTexts;
    
    [FormerlySerializedAs("LevelSets")]
    public levelSet[] levelSets;
    
    public int curLevelSetIndex = 0;
    public bool isLevelGenerated = false;
    public TMP_Text levelNameText;

    public Vector3 levelTagCenter;
    public int numPerRow = 6;
    public float distMultiplier = 2;
    
    public int curLevelIndex = 0;
    public SelectFrameCtrl selectFrameCtrl;
    
    public Transform levelTitle;
    public bool isOnTitle;

    public GameObject intoLevelTransition;
    private void GenerateLevelTags() {
        var maxLevelNum = levelSets[curLevelSetIndex].LevelNum;
        
        levelTags = new SpriteRenderer[maxLevelNum];
        levelNumTexts = new TMP_Text[maxLevelNum];
        for (int i = 0; i < maxLevelNum; i++) {
            levelTags[i] = Instantiate(levelTagTemplate, transform);
            levelTags[i].transform.localPosition =
                new Vector3(i % numPerRow * distMultiplier, -i / numPerRow * distMultiplier, 0);
            levelTags[i].transform.localPosition += levelTagCenter;
            levelNumTexts[i] = levelTags[i].GetComponentInChildren<TMP_Text>(false);
            levelNumTexts[i].text = (i + 1).ToString();
        }
        
        isLevelGenerated = true;
    }

    public void SetCurLevelSetIndex()
    {
        if (isOnTitle) {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                curLevelSetIndex--;
                if (curLevelSetIndex < 0)
                    curLevelSetIndex = levelSets.Length - 1;
                for (int i = 0; i < levelTags.Length; i++)
                    Destroy(levelTags[i].gameObject);
                isLevelGenerated = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)|| Input.GetKeyDown(KeyCode.D)) {
                curLevelSetIndex++;
                if (curLevelSetIndex >= levelSets.Length)
                    curLevelSetIndex = 0;
                for (int i = 0; i < levelTags.Length; i++)
                    Destroy(levelTags[i].gameObject);
                isLevelGenerated = false;
            }
        }

    }

    public void SetLevelIndex() {
        if (!isOnTitle) {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                curLevelIndex--;
                if (curLevelIndex < 0)
                    curLevelIndex = levelSets[curLevelSetIndex].LevelNum - 1;
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                curLevelIndex++;
                if (curLevelIndex >= levelSets[curLevelSetIndex].LevelNum)
                    curLevelIndex = 0;
            }

            if (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow)) {
                if (curLevelIndex - numPerRow >= 0)
                    curLevelIndex -= numPerRow;
            }

            if (Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow)) {
                if (curLevelIndex + numPerRow < levelSets[curLevelSetIndex].LevelNum)
                    curLevelIndex += numPerRow;
            }
        }

        if (curLevelIndex < 6 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) {
            isOnTitle = true;
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            isOnTitle = false;
            if(curLevelIndex >= levelSets[curLevelSetIndex].LevelNum)
                curLevelIndex = levelSets[curLevelSetIndex].LevelNum - 1;
        }

        if (isOnTitle) {
            selectFrameCtrl.transform.position =
                selectFrameCtrl.transform.position.ApproachValue
                    (levelTitle.position, 8f);
        }
        else {
            selectFrameCtrl.transform.position =
                selectFrameCtrl.transform.position.ApproachValue
                    (levelTags[curLevelIndex].transform.position, 8f);
        }
        
    }
    
    public int GetTotLevelIndex() {
        int tot = 0;
        for (int i = 0; i < curLevelSetIndex; i++)
            tot += levelSets[i].LevelNum;
        return tot + curLevelIndex;
    }

    public void EnterLevel() {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Manager.levelSelectedIndex = GetTotLevelIndex();
            //PlayerPrefs.SetInt("LevelIndex", GetTotLevelIndex());
            //SceneManager.LoadScene(2);
            intoLevelTransition.SetActive(true);
        }
    }
    
    

    private void Update()
    {
        levelNameText.text = levelSets[curLevelSetIndex].LevelName;
        
        if(!isLevelGenerated)
            GenerateLevelTags();
        
        SetCurLevelSetIndex();
        SetLevelIndex();
        if(!isOnTitle) EnterLevel();
        
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (mousePosition.y > 2f) {
                if (mousePosition.x < 0f) {
                    curLevelSetIndex--;
                    if (curLevelSetIndex < 0)
                        curLevelSetIndex = levelSets.Length - 1;
                    for(int i = 0; i < levelTags.Length; i++)
                        Destroy(levelTags[i].gameObject);
                    isLevelGenerated = false;
                }
                else {
                    curLevelSetIndex++;
                    if (curLevelSetIndex >= levelSets.Length)
                        curLevelSetIndex = 0;
                    for(int i = 0; i < levelTags.Length; i++)
                        Destroy(levelTags[i].gameObject);
                    isLevelGenerated = false;
                }
            }
            
            // 发射射线
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // 检测是否击中了物体
            if (hit.collider != null)
            {
                for(int i = 0; i < levelTags.Length; i++)
                {
                    if (hit.collider.gameObject == levelTags[i].gameObject)
                    {
                        curLevelIndex = i;
                        selectFrameCtrl.transform.position = 
                            selectFrameCtrl.transform.position.ApproachValue
                            (levelTags[curLevelIndex].transform.position,
                                8f);
                        //PlayerPrefs.SetInt("LevelIndex", GetTotLevelIndex());
                        GameManager.Manager.levelSelectedIndex = GetTotLevelIndex();
                        print(GetTotLevelIndex());
                        //SceneManager.LoadScene(2);
                        intoLevelTransition.SetActive(true);
                        
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < levelTags.Length; i++)
            {
                var dist = Mathf.Abs(levelTags[i].transform.position.x - mousePosition.x) +
                           Mathf.Abs(levelTags[i].transform.position.y - mousePosition.y);
                if (dist < 0.5f)
                    levelTags[i].transform.localScale =
                        levelTags[i].transform.localScale.ApproachValue(Vector3.one * 1.2f, 8f);
                else
                    levelTags[i].transform.localScale =
                        levelTags[i].transform.localScale.ApproachValue(Vector3.one, 8f);
            }
        }
    }
}
