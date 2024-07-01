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
    public int tarSceneIndex = 1;
    
    public void GeneratePillars() {
        pillars = new PillarCtrl[pillarNum];
        for (int i = 0; i < pillarNum; i++) {
            var pillar = Instantiate(pillarPrefab, transform);
            pillar.initX = startX + pillarWidth * i;
            pillars[i] = pillar;
        }
    }
    
    public IEnumerator ActivatePillars() {
        for (int i = 0; i < pillarNum; i++) {
            pillars[i].isActivated = true;
            yield return new WaitForSeconds(0.1f);
        }  
    }
    
    public IEnumerator LeavePillars(int sceneIndex)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        for (int i = 0; i < pillarNum; i++) {
            pillars[i].isLeaving = true;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    public void Awake() {
        DontDestroyOnLoad(gameObject);
        GeneratePillars();
    }
    
    public void Update() {
        if (Input.anyKeyDown) {
            StartCoroutine(ActivatePillars());
        }
        
        if(pillars[pillarNum - 1].isArrived && curSceneIndex != tarSceneIndex) {
            
            StartCoroutine( LeavePillars(tarSceneIndex));
            curSceneIndex = tarSceneIndex;
        }
    }
}
