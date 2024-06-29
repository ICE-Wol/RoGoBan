using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionCtrl : MonoBehaviour
{
    public PillarCtrl pillarPrefab;
    public PillarCtrl[] pillars;

    public float startX;
    public int pillarNum;
    public float pillarWidth;

    public int curSceneIndex = 0;
    public void GeneratePillars() {
        pillars = new PillarCtrl[pillarNum];
        for (int i = 0; i < pillarNum; i++) {
            var pillar = Instantiate(pillarPrefab, transform);
            pillar.initX = startX + pillarWidth * i;
            pillars[i] = pillar;
        }
    }
    
    public IEnumerator ActivatePillar() {
        for (int i = 0; i < pillarNum; i++) {
            pillars[i].isActivated = true;
            yield return new WaitForSeconds(0.1f);
        }  
    }
    
    public IEnumerator LeavePillars() {
        for (int i = 0; i < pillarNum; i++) {
            pillars[i].isLeaving = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Start() {
        DontDestroyOnLoad(gameObject);
        GeneratePillars();
    }
    
    public void Update() {
        if (Input.anyKeyDown) {
            StartCoroutine(ActivatePillar());
        }
        
        if(pillars[pillarNum - 1].isArrived && curSceneIndex != 1) {
            SceneManager.LoadScene(1);
            StartCoroutine( LeavePillars());
            curSceneIndex = 1;
        }
    }
}
