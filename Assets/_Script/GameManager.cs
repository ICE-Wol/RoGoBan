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

    public int levelSelectedIndex;
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
                SceneManager.LoadScene(i - 1);
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
