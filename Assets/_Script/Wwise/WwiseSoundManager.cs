using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum WwiseEventType
{
    Player_SuccessMove,
    Player_CannotMove,
    Player_FuckWall,
    UI_Click,
    UI_Choose,
    UI_Mouse,
    UI_Esc,
    BGM_Start,
    BGM_Stop,
    IngameBGM_Start,
    IngameBGM_Stop,
    UI_UP,
    UI_Down,
    Title_Enter,
    R,
    Z,
    H,
    Level_Complete,
    Cube_Enter,
    Cube_Mix,
    Cube_Trans,
    Cube_Leave,
    Player_Enter,
    Player_Leave,
    Enter_Nomouse,
    LC,
    FUCKSTOP,
    BURST,
    LOAD,
    PAUSE,
    RESUME
}
[Serializable]
public struct WwiseEventTypeStruct
{
    public WwiseEventType Type;
    public AK.Wwise.Event Event;
}

public class WwiseSoundManager : MonoBehaviour
{
    private static WwiseSoundManager instance;
    public static WwiseSoundManager Instance
    {
        get => instance;
    }
    private void Awake()
    {
        //经典单例
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (var item in WwiseEventTypeStructs)
        {

            WwiseEventTypeDictionary[item.Type] = item.Event;
        }

    }
    private void Start()
    {
        PostEvent(gameObject, WwiseEventType.BGM_Start);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// 在编辑器中配置
    /// </summary>
    public List<WwiseEventTypeStruct> WwiseEventTypeStructs;

    private Dictionary<WwiseEventType, AK.Wwise.Event> WwiseEventTypeDictionary = new Dictionary<WwiseEventType, AK.Wwise.Event>();
    /// <summary>
    /// 发出某个Wwise事件
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="eventType"></param>
    public void PostEvent(GameObject gameObject, WwiseEventType eventType)
    {
        AK.Wwise.Event targetEvent = WwiseEventTypeDictionary[eventType];
        if (targetEvent != null && gameObject)
        {
            targetEvent.Post(gameObject);
        }
    }
    /// <summary>
    /// 停止某个Wwise事件
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="eventType"></param>
    public void StopEvent(GameObject gameObject, WwiseEventType eventType)
    {
        AK.Wwise.Event targetEvent = WwiseEventTypeDictionary[eventType];
        if (targetEvent != null && gameObject)
        {
            targetEvent.Stop(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name.ToLower().Contains("game"))
        {
            //游戏关卡内
            PostEvent(gameObject, WwiseEventType.BGM_Stop);
            PostEvent(gameObject, WwiseEventType.IngameBGM_Start);
        }
        else
        { //主界面及选关
            PostEvent(gameObject, WwiseEventType.IngameBGM_Stop);
            PostEvent(gameObject, WwiseEventType.BGM_Start);
        }
    }



}
