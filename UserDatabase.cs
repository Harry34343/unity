using System.IO;
using UnityEngine;

public class UserDatabase : MonoBehaviour
{
    private string filePath;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "users.txt");
        Debug.Log("User file path: " + filePath);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            Debug.Log("Created user file.");
        }
    }


    private void EnsureFilePath()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = Path.Combine(Application.persistentDataPath, "users.txt");
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
        }
    }

    public void Register(string username, string password)
    {
        EnsureFilePath();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.Log("Username or password is empty!");
            return;
        }

        File.AppendAllText(filePath, username + ":" + password + "\n");
        Debug.Log("User registered!");
    }

    public bool Login(string username, string password)
    {
        EnsureFilePath();

        foreach (string line in File.ReadAllLines(filePath))
        {
            string[] parts = line.Split(':');
            if (parts[0] == username && parts[1] == password)
            {
                Debug.Log("Login successful!");
                return true;
            }
        }

        Debug.Log("Login failed!");
        return false;
    }

    public bool UserExists(string username)
    {
        EnsureFilePath(); // <-- Make sure path is valid

        foreach (string line in File.ReadAllLines(filePath))
        {
            if (line.StartsWith(username + ":"))
                return true;
        }
        return false;
    }
}