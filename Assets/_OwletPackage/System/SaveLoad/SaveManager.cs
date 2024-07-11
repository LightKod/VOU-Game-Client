using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;
namespace Owlet.Systems.SaveLoad
{
    public class SaveManager : Singleton<SaveManager>
    {

        public static Action onDataLoaded;
        public static Action onDataSaved;
        public static List<ISaveable> saveableObjects = new();
        public static bool isLoaded;
        protected override void Init()
        {
            isLoaded = false;
            base.Init();
        }

        private void Start()
        {
            Load();
        }


        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Backslash))
            {
                Save();
            }
        }

        /// <summary>
        /// Save all ISaveable
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            Debug.Log("SAVED");
            List<string> saveJson = new List<string>();
            foreach (ISaveable saveObject in saveableObjects)
            {
                string metadata = $"{{\"identifier\":\"{saveObject.ID()}\"}}";
                string json = metadata + saveObject.Save();
                saveJson.Add(json);
            }
            string finalData = JsonConvert.SerializeObject(saveJson);
            Helper.Save(finalData, Application.persistentDataPath + "/save.data");
            return true;
        }

        /// <summary>
        /// Load all ISaveable
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string loadedData = Helper.Load(Application.persistentDataPath + "/save.data");
            if (loadedData == "\0")
            {
                //Create new data object
                Helper.Load("Create new save file");
                CreateNewSaveFile();
                onDataLoaded?.Invoke();
                isLoaded = true;
                return false;
            }
            List<ISaveable> loadedObjects = new();
            //Debug.Log(loadedData);
            List<string> jsonList = JsonConvert.DeserializeObject<List<string>>(loadedData);
            foreach (var jsonString in jsonList)
            {
                //Debug.Log(jsonString);
                // Extract identifier from metadata to identify the corresponding ISaveable
                string metadata = jsonString.Substring(0, jsonString.IndexOf('}') + 1);
                var identifier = JsonUtility.FromJson<Metadata>(metadata).identifier;

                // Find the corresponding ISaveable using the identifier
                ISaveable saveable = FindSaveObject(identifier);
                loadedObjects.Add(saveable);

                // Load the data into the ISaveable object
                string jsonData = jsonString.Substring(metadata.Length);
                bool saveSucess = saveable.Load(jsonData);
                if (!saveSucess)
                {
                    Helper.Log($"Create new save file for {saveable.ID()}");
                    saveable.CreateNewSaveFile();
                }
                else
                {
                    Helper.Log($"Load successful {saveable.ID()}");
                }
            }

            List<ISaveable> unloadedObjects = saveableObjects.Except(loadedObjects).ToList();

            foreach (var unloadedObject in unloadedObjects)
            {
                Helper.Log($"Create new save file for {unloadedObject.ID()}");
                unloadedObject.CreateNewSaveFile();
            }

            onDataLoaded?.Invoke();
            isLoaded = true;
            return true;
        }

        void CreateNewSaveFile()
        {
            foreach (ISaveable saveObject in saveableObjects)
            {
                saveObject.CreateNewSaveFile();
                Helper.Log($"Create new save file for {saveObject.ID()}");
            }

            Save();
            onDataLoaded?.Invoke();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) Save();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        ISaveable FindSaveObject(string id)
        {
            return saveableObjects.Find(x => x.ID() == id);
        }
    }


    class Metadata
    {
        public string identifier;
    }
}
