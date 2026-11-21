using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame(string source)
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}