using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    public GameObject questionPrefab;
    public TextMeshProUGUI timeText;

    private QuestionUtil.Question[] _questions;
    private int[] _responses;
    private const int UnattemptedResponseCode = -1;

    private float _timer = 60f;
    private bool _isSubmitted;

    void Start()
    {
        PopulateQuestionUI();
    }

    void Update()
    {
        if (_timer >= 0)
        {
            _timer -= Time.deltaTime;
            timeText.text = ((int) _timer).ToString();
        }
        else
        {
            _isSubmitted = true;
        }

        if (_isSubmitted)
        {
            SceneManager.sceneManager.SubmitScore(CalculateScore());
        }
    }

    int CalculateScore()
    {
        int score = 0;
        for (int i = 0; i < _questions.Length; i++)
        {
            if (_responses[i] == _questions[i].correctOptionIndex) score += 10;
        }

        if (_timer > 0)
        {
            score += (int) _timer;
        }

        return score;
    }

    public void SubmitButton()
    {
        _isSubmitted = true;
    }

    void PopulateQuestionUI()
    {
        GameObject panel = GameObject.FindWithTag("QuestionPanel");
        _questions = SceneManager.sceneManager.GetQuestions();
        _responses = new int[_questions.Length];

        for (int i = 0; i < _questions.Length; i++)
        {
            QuestionUtil.Question question = _questions[i];
            _responses[i] = UnattemptedResponseCode;

            GameObject obj = Instantiate(questionPrefab, transform.position, Quaternion.identity);

            SetupQuestionUI(obj, question);
            SetupOptionsUI(obj, question, i);

            // obj.transform.SetParent(panel.transform);
            obj.GetComponent<RectTransform>().SetParent(panel.GetComponent<RectTransform>());
            // var rectTransform = panel.GetComponent<RectTransform>();
            // rectTransform.anchorMin = new Vector2(0, 1);
            // rectTransform.anchorMax = new Vector2(1, 1);
        }
    }

    void SetupQuestionUI(GameObject parentObj, QuestionUtil.Question question)
    {
        GameObject questionGameObject = GetChildrenByTag(parentObj.transform, "Question")[0];
        TextMeshProUGUI textMeshProUGUI = questionGameObject.GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.SetText(question.question);
    }

    void SetupOptionsUI(GameObject parentObj, QuestionUtil.Question question, int questionIndex)
    {
        GameObject[] optionGameObjects = GetChildrenByTag(parentObj.transform, "Option");
        for (int j = 0; j < question.options.Length; j++)
        {
            GameObject optionObj = optionGameObjects[j];
            Text optionTextMeshProUGUI = optionObj.GetComponentInChildren<Text>();
            optionTextMeshProUGUI.text = question.options[j];

            Toggle toggle = optionObj.GetComponent<Toggle>();

            var optionIndex = j;
            toggle.onValueChanged.AddListener((isSelected) =>
            {
                if (isSelected)
                {
                    _responses[questionIndex] = optionIndex;
                }
                else
                {
                    _responses[questionIndex] = UnattemptedResponseCode;
                }
            });
        }
    }

    private static GameObject[] GetChildrenByTag(Transform parent, string tag)
    {
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag.Equals(tag))
            {
                objects.Add(child.gameObject);
            }
        }

        return objects.ToArray();
    }
}