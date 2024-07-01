using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;
    
    private void Awake() {
        if (Manager == null) {
            Manager = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public SceneTransitionCtrl trTemplate;
    public SceneTransitionCtrl tr;
    public Color playerColor;
    public int levelSelectedIndex;
    public bool isLevelComplete;
    public bool prevLevelComplete;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            var i = SceneManager.GetActiveScene().buildIndex;
            if (i != 0) {
                if (i == 1) {
                    tr = Instantiate(trTemplate);
                    tr.curSceneIndex = 1;
                    tr.tarSceneIndex = 0;
                    StartCoroutine(tr.ActivatePillars());
                }else if (i == 2) {
                    SceneManager.LoadScene(1);
                }

            }
            else {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
            }
        }
    }
}
