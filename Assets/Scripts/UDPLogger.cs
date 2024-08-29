using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class UDPLogger : MonoBehaviour
{
    // Start is called before the first frame update
    private string filePath;

    void Start()
    {
        DateTime currentTime = DateTime.Now;
        string log = currentTime.ToString("yyyy.MM.dd_HH.mm.ss");
        log = "UDPlog_" + log + ".txt";
        filePath = Path.Combine(Application.persistentDataPath, log);
        // Example usage: appending some text to the file

        LogToFile("UDP log file:" + filePath);
        log = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
        LogToFile(log);
        Debug.Log($"Log file path: {filePath}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LogToFile(string inputString)
    {
        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine(inputString);
        }
        Debug.Log($"Appended text to {filePath}");
    }

    void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            // Delete the file
            File.Delete(path);
            Debug.Log($"File deleted: {path}");
        }
        else
        {
            Debug.LogWarning($"File not found: {path}");
        }
    }

    private void OnApplicationQuit()
    {
        using (StreamWriter sw = File.AppendText(filePath))
        {
            DateTime currentTime = DateTime.Now;
            string log = currentTime.ToString("yyyy.MM.dd_HH.mm.ss");
            sw.WriteLine(log);
            sw.Close();
        }
        Debug.Log($"OnApplicationQuit text to {filePath}");

    }
}

