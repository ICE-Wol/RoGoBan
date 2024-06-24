using System;
using System.Collections.Generic;
using _Scripts.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            curLevelSetIndex--;
            if (curLevelSetIndex < 0)
                curLevelSetIndex = levelSets.Length - 1;
            for(int i = 0; i < levelTags.Length; i++)
                Destroy(levelTags[i].gameObject);
            isLevelGenerated = false;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            curLevelSetIndex++;
            if (curLevelSetIndex >= levelSets.Length)
                curLevelSetIndex = 0;
            for(int i = 0; i < levelTags.Length; i++)
                Destroy(levelTags[i].gameObject);
            isLevelGenerated = false;
        }
        
    }

    public void SetLevelIndex() {
        if (Input.GetKeyDown(KeyCode.A)) {
            curLevelIndex--;
            if (curLevelIndex < 0)
                curLevelIndex = levelSets[curLevelSetIndex].LevelNum - 1;
        } 
        if (Input.GetKeyDown(KeyCode.D)) {
            curLevelIndex++;
            if (curLevelIndex >= levelSets[curLevelSetIndex].LevelNum)
                curLevelIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            if(curLevelIndex - numPerRow >= 0)
                curLevelIndex -= numPerRow;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            if(curLevelIndex + numPerRow < levelSets[curLevelSetIndex].LevelNum)
                curLevelIndex += numPerRow;
        }

        selectFrameCtrl.transform.position = 
            selectFrameCtrl.transform.position.ApproachValue
            (levelTags[curLevelIndex].transform.position,
            8f);
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
            PlayerPrefs.SetInt("LevelIndex", GetTotLevelIndex());
            SceneManager.LoadScene(2);
        }
    }
    
    

    private void Update()
    {
        levelNameText.text = levelSets[curLevelSetIndex].LevelName;
        
        if(!isLevelGenerated)
            GenerateLevelTags();
        
        SetCurLevelSetIndex();
        SetLevelIndex();
        EnterLevel();
        
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(1))
        {
            // 发射射线
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // 检测是否击中了物体
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                // 在此处可以执行其他逻辑，例如与物体交互
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
