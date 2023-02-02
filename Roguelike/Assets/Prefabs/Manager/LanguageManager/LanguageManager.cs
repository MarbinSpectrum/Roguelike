using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LanguageManager : FieldObjectSingleton<LanguageManager>
{
    public string filePath;
    public Language nowLanguage;

    private Dictionary<Language, Dictionary<string, string>> languageData 
        = new Dictionary<Language, Dictionary<string, string>>();

    public IEnumerator runLoadData()
    {
        //,자 형식으로 저장된 csv파일을 읽는다.
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        if (textAsset == null)
            yield break;

        //줄을 나눈다.
        string[] rows = textAsset.text.Split('\n');
        List<string> rowList = new List<string>();
        for(int i = 0; i < rows.Length; i++)
        {
            if (string.IsNullOrEmpty(rows[i]))
            {
                //아무것도 없는 객체
                continue;
            }
            string row = rows[i].Replace("\r", string.Empty);
            row = row.Trim();
            rowList.Add(rows[i]);
        }

        //제목줄
        string[] subjects = rowList[0].Split(',');

        for(int r = 1; r < rowList.Count; r++)
        {
            //해당 줄부터 데이터다.
            string[] values = rowList[r].Split(',');

            //키값을 얻는다.
            string keyValue = values[0].Replace('\r', ' ').Trim();

            for (int c = 1; c < values.Length; c++)
            {
                //해당칸의 언어를 가져온다.
                subjects[c] = subjects[c].Replace('\r', ' ').Trim();
                Language language = (Language)Enum.Parse(typeof(Language), subjects[c]);
                if (languageData.ContainsKey(language) == false)
                    languageData.Add(language, new Dictionary<string, string>());

                //값을반환한다.
                values[c] = values[c].Replace('\r', ' ').Trim();

                //언어데이터를 추가한다.
                languageData[language].Add(keyValue, values[c]);
            }
        }
    }

    public static string GetText(string pKey)
    {
        if(instance.languageData[instance.nowLanguage].ContainsKey(pKey))
            return instance.languageData[instance.nowLanguage][pKey];
        return string.Empty;
    }
}
