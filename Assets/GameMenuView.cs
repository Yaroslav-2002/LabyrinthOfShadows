using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuView : View
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button exitButton;
    private const int MENU_SCENE_NUM = 0;  

    public override void Initialize()
    {
        resumeButton.onClick.AddListener(() => ViewManager.ShowLast());
        settingsButton.onClick.AddListener(() => ViewManager.Show<SettingsView>());
        saveButton.onClick.AddListener(() => DataPersistenceManager.Instance.SaveGameAsync());
        exitButton.onClick.AddListener(() => OnExitButton());
    }
    private void OnExitButton()
    {
        DataPersistenceManager.Instance.SaveGameAsync();
        SceneManager.LoadScene(MENU_SCENE_NUM);
    }
}