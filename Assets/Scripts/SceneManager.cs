using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private QuestionUtil.Question[] _questions;
    private int _score;

    public int Score => _score;

    public static SceneManager sceneManager;

    void Start()
    {
        if (sceneManager == null)
        {
            DontDestroyOnLoad(gameObject);
            sceneManager = this;
        }
        else if (sceneManager != this)
        {
            Destroy(gameObject);
        }
    }


    public QuestionUtil.Question[] GetQuestions()
    {
        return _questions;
    }


    public void StartButton()
    {
        QuestionUtil.Question[] questions = QuestionUtil.GetQuestions();

        if (questions != null)
        {
            _questions = questions;
            UnityEngine.SceneManagement.SceneManager.LoadScene("QuizScreen");
        }
    }

    public void SubmitScore(int score)
    {
        _score = score;
        UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScreen");
    }

    public void Restart()
    {
        Destroy(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");
    }
}