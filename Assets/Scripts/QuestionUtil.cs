using System;
using System.IO;
using System.Net;
using UnityEngine;

public class QuestionUtil
{
    public static Question[] GetQuestions()
    {
        HttpWebRequest request =
            (HttpWebRequest) WebRequest.Create(
                "https://my-json-server.typicode.com/strshri/json-server/questionsAndAnswers");
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        
        Stream responseStream = response.GetResponseStream();
        if (responseStream == null)
        {
            return null;
        }
        
        StreamReader reader = new StreamReader(responseStream);
        string jsonResponse = reader.ReadToEnd();
        
        Wrapper<Question> wrapper = JsonUtility.FromJson<Wrapper<Question>>(FixJson(jsonResponse));
        return wrapper.items;
    }

    [Serializable]
    public class Question
    {
        public string question;
        public string[] options;
        public int correctOptionIndex;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }

    private static string FixJson(string value)
    {
        // Converts json array of the form [...] to 
        // a json object of the form {"items":[...]}
        return "{\"items\":" + value + "}";
    }
}