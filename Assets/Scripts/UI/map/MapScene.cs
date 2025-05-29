using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _gameSceneName = "TheFirstShop";
    [SerializeField] private string _mapSceneName = "MapScene";
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _mainUI;
    [SerializeField] private Scene _scene;

    private bool _isMapLoaded = false;

    public void ToggleMap()
    {
        if (_isMapLoaded)
        {
            UnloadMap();
        }
        else
        {
            LoadMap();
        }
    }

    private void LoadMap()
    {
        SceneManager.LoadScene(_mapSceneName, LoadSceneMode.Additive);
        _isMapLoaded = true;

        // Можно приостановить игровые процессы, если нужно
        Time.timeScale = 0f;
        _mainCamera.SetActive(false);
        _mainUI.SetActive(false);
    }

    private void UnloadMap()
    {
        SceneManager.UnloadSceneAsync(_mapSceneName);
        _isMapLoaded = false;

        // Возобновляем игру
        Time.timeScale = 1f;
        _mainCamera.SetActive(true);
        _mainUI.SetActive(true);

    }
}