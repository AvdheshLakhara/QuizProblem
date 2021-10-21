using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().SetText(SceneManager.sceneManager.Score.ToString());
    }

    public void HomeButton()
    {
        SceneManager.sceneManager.Restart();
    }
}
