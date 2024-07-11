using Newtonsoft.Json;
using Owlet.Systems.SaveLoad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Owlet.Systems.Currency
{
    /// <summary>
    /// This is the main resource controller of the entire project
    /// Resource include
    /// PlayerDataController should not need to save resources any more
    /// </summary>
    public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
    {
        [SerializeField]
        List<CurrencyType> currencyTypes;
        CurrencyData data;

        public static Action<CurrencyType ,int, string> onResourceGained;
        public static Action<CurrencyType, int, string> onResourceUsed;

        protected override void Init()
        {
            base.Init();
            SaveManager.saveableObjects.Add(this);
        }
        private void OnDestroy()
        {
            SaveManager.saveableObjects.Remove(this);
        }

        public bool CheckEnough(string currencyID, int amount)
        {
            if (data == null || !data.currency.ContainsKey(currencyID)) return false;
            return data.currency.ContainsKey(currencyID) && data.currency[currencyID] >= amount;

        }

        public int GetResource(string currencyID)
        {
            if (data == null || data.currency == null) return 0;
            if (!data.currency.ContainsKey(currencyID)) return 0;
            return data.currency[currencyID];
        }

        public bool UseResource(string currencyID, int amount, string source = "")
        {
            if (!data.currency.ContainsKey(currencyID)) return false;
            if(data.currency[currencyID] >= amount)
            {
                CurrencyType currency = currencyTypes.Find(x => x.currencyName == currencyID);
                data.currency[currencyID] -= amount;
                onResourceUsed?.Invoke(currency, data.currency[currencyID], source);
                return true;
            }
            return false;
        }

        public bool GainResource(string currencyID, int amount, string source = "")
        {
            if (!data.currency.ContainsKey(currencyID)) return false;
            CurrencyType currency = currencyTypes.Find(x => x.currencyName == currencyID);
            data.currency[currencyID] += amount;
            onResourceGained?.Invoke(currency, data.currency[currencyID], source);
            return true;
        }

        public void SetCurrency(string currencyID, int amount, string source = "")
        {
            if (!data.currency.ContainsKey(currencyID)) return;
            CurrencyType currency = currencyTypes.Find(x => x.currencyName == currencyID);
            data.currency[currencyID] = amount;
            onResourceGained?.Invoke(currency, data.currency[currencyID], source);
        }

        public string Save()
        {
            return JsonConvert.SerializeObject(data);
        }

        public bool Load(string json)
        {
            try
            {
                data = JsonConvert.DeserializeObject<CurrencyData>(json);
                data.UpdateDictionary(currencyTypes);
                if (data == null) return false;
                return true;
            }
            catch
            {
                Helper.Log("Load Player Data failed");
                return false;
            }
        }

        public void CreateNewSaveFile()
        {
            data = new(currencyTypes);
        }

        public string ID()
        {
            return "player_currency";
        }
    }
}
