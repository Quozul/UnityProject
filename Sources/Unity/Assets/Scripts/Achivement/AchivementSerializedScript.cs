using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public class AchivementSerializedScript
{
    [Serializable]
    public class AchievementClass
    {
        public int id;
        public String title;
        public String descr = null;
        public String date;

        public void init(int id, String title, [CanBeNull] String descr)
        {
            this.id = id;
            this.title = title;
            this.descr = descr;
        }
    }
    
    public AchievementClass achievement = new AchievementClass();
    
    private String fileName = "achivement";

    public void createFile()
    {
        
        String path =  $"{Application.dataPath}/{fileName}.txt";
        
        // Init time
        
        achievement.date = achivementTime();

        if (!File.Exists(path))
        {
            String json = JsonUtility.ToJson(achievement);
            File.WriteAllText(path,json+Environment.NewLine);
        }
        else
        {
            using (StreamWriter sw = new StreamWriter(path,append:true))
            {
                sw.BaseStream.Seek(0, SeekOrigin.End);
                String json = JsonUtility.ToJson(achievement);
                sw.WriteLine(json);
            }
            
        }

    }

    private String achivementTime()
    {
        DateTime localDate = DateTime.Today;
        return localDate.ToString("d");
    }
    
    [CanBeNull]
    public List<AchievementClass> readFile()
    {
        String path =  $"{Application.dataPath}/{fileName}.txt";
        if (File.Exists(path))
        {
            List<AchievementClass> tmp = new List<AchievementClass>();
            foreach (String line in File.ReadLines(path))
            {
                tmp.Add(JsonUtility.FromJson<AchievementClass>(line));
            }

            return tmp;

        }
        else
        {
            return null;
        }
    }
    
}
