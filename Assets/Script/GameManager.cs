using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public GameObject player;

    public SceneChanger sceneChanger;
    public GameScene gameScene;

    #region Game status
    private Level currentLevelData;
    private bool isGameWin = false;
    private bool isGameLose = false;
    [SerializeField]
    private int _coin;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetTheLevelAtGivenIndex(LevelManager.instance.currentLevelIndex);
        GameObject map = Instantiate(currentLevelData.map);
        player = map.transform.GetChild(0).gameObject;
        Time.timeScale = 1;
        gameScene.UpdateCoin();
    }

    public void PlayerWinThisLevel()
    {
        if (isGameWin || isGameLose)
        {
            return;
        }
        LevelManager.instance.levelData.ReSetGivenLevelData(LevelManager.instance.currentLevelIndex, true, true);
        if (LevelManager.instance.levelData.GiveAllLevelAssigned().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetTheLevelAtGivenIndex(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.ReSetGivenLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }
        LevelManager.instance.levelData.SaveThisDataToJsonFile();

        isGameWin = true;
        gameScene.PopupWinPanelGameScene();
    }


    public void PlayerLoseThisLevelAndShowUI()
    {
        isGameLose = true;
        StartCoroutine(WaitToLose());
    }

    private IEnumerator WaitToLose()
    {
        yield return new WaitForSeconds(.5f);
        gameScene.PopupLosePanelGameScene();
    }

    public bool IsThisGameFinalOrWin()
    {
        return isGameWin;
    }

    public bool IsThisGameFinalOrLose()
    {
        return isGameLose;
    }

    public void AddCoin()
    {
        _coin++;
        gameScene.UpdateCoin();
    }

    public int GetCoin()
    {
        return _coin;
    }
}
