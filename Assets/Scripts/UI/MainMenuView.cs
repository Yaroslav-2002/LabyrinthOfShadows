using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : View
{
    private const int MAIN_SCENE_NUM = 1;
    [SerializeField] Button exitButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button newGameButton;
    [SerializeField] Button loadButton;

    public override void Initialize()
    {
        exitButton.onClick.AddListener(OnExitButtonClick);
        settingsButton.onClick.AddListener(() => ViewManager.Show<SettingsView>());
        if(!DataPersistenceManager.Instance.HasData())
            loadButton.interactable = false;
        else
        {
            loadButton.interactable = true;
            loadButton.onClick.AddListener(() => OnLoadGameClicked());
        }
        newGameButton.onClick.AddListener(() => OnNewGameClicked());
    }

    private void OnLoadGameClicked()
    {
        SceneManager.LoadSceneAsync(MAIN_SCENE_NUM);
    }

    private void OnNewGameClicked()
    {
        SceneManager.LoadSceneAsync(MAIN_SCENE_NUM);
    }

    void OnExitButtonClick()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
