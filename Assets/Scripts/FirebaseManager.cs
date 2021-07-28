using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Newtonsoft.Json;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance { get; protected set; }

    private DatabaseReference database;
    private DatabaseReference usersDB;
    private DatabaseReference usernamesDB;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            Debug.Log("Singleton for FirebaseManager is ready");
            instance = this;
        }
    }

    void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
        usersDB = database.Child("users");
        usernamesDB = database.Child("usernames");
    }

    public void UpdateUserData(string userId, KnightsMobile.IPlayer player)
    {
        UpdateUserMainData(userId, player);
        UpdatePlayerStatistics(userId, player);
        UpdatePlayerItems(userId, player);
    }

    public void UpdateUserMainData(string userId, KnightsMobile.IPlayer player)
    {
        usersDB.Child(userId).Child("mainData").SetRawJsonValueAsync(JsonConvert.SerializeObject(player.GetPlayerMainData())).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // some error
                Debug.LogError(task.Exception.ToString());
            }
            if (task.IsCompleted)
            {
                // good
                Debug.Log("Successfully updated user main data");
            }
        });
    }

    public void UpdatePlayerStatistics(string userId, KnightsMobile.IPlayer player)
    {
        usersDB.Child(userId).Child("statistics").SetRawJsonValueAsync(JsonConvert.SerializeObject(player.GetPlayerStatistics())).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // some error
                Debug.LogError(task.Exception.ToString());
            }
            if (task.IsCompleted)
            {
                // good
                Debug.Log("Successfully updated user statistics");
            }
        });
    }

    public void UpdatePlayerItems(string userId, KnightsMobile.IPlayer player)
    {
        usersDB.Child(userId).Child("items").SetRawJsonValueAsync(JsonConvert.SerializeObject(player.GetPlayerItems())).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // some error
                Debug.LogError(task.Exception.ToString());
            }
            if (task.IsCompleted)
            {
                // good
                Debug.Log("Successfully updated user items");
            }
        });
    }

    public void ReadUserData(string userId, KnightsMobile.IPlayer player)
    {
        ReadUserMainData(userId, player);
        ReadPlayerStatistics(userId, player);
        ReadPlayerItems(userId, player);
    }

    public void ReadUserMainData(string userId, KnightsMobile.IPlayer player)
    {
        usersDB.Child(userId).Child("mainData").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // some error
                Debug.LogError(task.Exception.ToString());
            }
            if (task.IsCompleted)
            {
                // good
                if (task.Result.Exists)
                {
                    Dictionary<KnightsMobile.MainData, int> data = JsonConvert.DeserializeObject<Dictionary<KnightsMobile.MainData, int>>(task.Result.GetRawJsonValue());
                    player.SetPlayerMainData(data);
                }
            }
        });
    }

    public void ReadPlayerStatistics(string userId, KnightsMobile.IPlayer player)
    {
        usersDB.Child(userId).Child("statistics").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // some error
                Debug.LogError(task.Exception.ToString());
            }
            if (task.IsCompleted)
            {
                // good
                if (task.Result.Exists)
                {
                    Dictionary<KnightsMobile.Statistic, int> data = JsonConvert.DeserializeObject<Dictionary<KnightsMobile.Statistic, int>>(task.Result.GetRawJsonValue());
                    player.SetPlayerStatistics(data);
                }
            }
        });
    }

    public void ReadPlayerItems(string userId, KnightsMobile.IPlayer player)
    {
        usersDB.Child(userId).Child("items").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // some error
                Debug.LogError(task.Exception.ToString());
            }
            if (task.IsCompleted)
            {
                // good
                if (task.Result.Exists)
                {
                    Dictionary<KnightsMobile.ItemType, KnightsMobile.Item> data = JsonConvert.DeserializeObject<Dictionary<KnightsMobile.ItemType, KnightsMobile.Item>>(task.Result.GetRawJsonValue());
                    player.SetPlayerItems(data);
                }
            }
        });
    }

    public void AddUsername(string userId, string username)
    {
        usernamesDB.Child(username).SetRawJsonValueAsync(JsonConvert.SerializeObject(userId)).ContinueWith(task => 
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // some error
                Debug.LogError(task.Exception.ToString());
            }
            if (task.IsCompleted)
            {
                // good
                Debug.Log("Successfully added username");
            }
        });
    }

    public void IsUsernameAvailable(string username, System.Action<bool> callback)
    {
        usernamesDB.Child(username).GetValueAsync().ContinueWith(task => 
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // some error
                Debug.Log("Error checking username availability");
                callback(false);
            }
            else if (task.IsCompleted)
            {
                callback(!task.Result.Exists);
            }
        });
    }
}
