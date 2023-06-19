using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LanguageManager : DontDestroySingleton<LanguageManager>
{
    private bool isLoad = false;

    public string filePath;
    public Language nowLanguage;
    
    private Dictionary<Language, Dictionary<string, string>> languageData 
        = new Dictionary<Language, Dictionary<string, string>>();

    public IEnumerator runLoadData()
    {
        if(isLoad)
        {
            //�̹� �ε尡 �Ϸ�Ǿ��ִ�.
            yield break;
        }

        //,�� �������� ����� csv������ �д´�.
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        if (textAsset == null)
            yield break;

        //���� ������.
        string[] rows = textAsset.text.Split('\n');
        List<string> rowList = new List<string>();
        for(int i = 0; i < rows.Length; i++)
        {
            if (string.IsNullOrEmpty(rows[i]))
            {
                //�ƹ��͵� ���� ��ü
                continue;
            }
            string row = rows[i].Replace("\r", string.Empty);
            row = row.Trim();
            rowList.Add(rows[i]);
        }

        //������
        string[] subjects = rowList[0].Split(',');

        for(int r = 1; r < rowList.Count; r++)
        {
            //�ش� �ٺ��� �����ʹ�.
            string[] values = rowList[r].Split(',');

            //Ű���� ��´�.
            string keyValue = values[0].Replace('\r', ' ').Trim();

            for (int c = 1; c < values.Length; c++)
            {
                //�ش�ĭ�� �� �����´�.
                subjects[c] = subjects[c].Replace('\r', ' ').Trim();
                Language language = (Language)Enum.Parse(typeof(Language), subjects[c]);
                if (languageData.ContainsKey(language) == false)
                    languageData.Add(language, new Dictionary<string, string>());

                //������ȯ�Ѵ�.
                values[c] = values[c].Replace('\r', ' ').Trim();

                //�����͸� �߰��Ѵ�.
                languageData[language].Add(keyValue, values[c]);
            }
        }

        isLoad = true;
    }

    public static string GetText(string pKey)
    {
        if(instance.languageData.ContainsKey(instance.nowLanguage))
        {
            if (instance.languageData[instance.nowLanguage].ContainsKey(pKey))
            {
                string str = instance.languageData[instance.nowLanguage][pKey];
                str = str.Replace("\\n", "\n");
                return str;
            }
        }
        return string.Empty;
    }
}
