using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet.Systems.Currency
{
    public class CurrencyData
    {
        public Dictionary<string, int> currency;

        public CurrencyData(List<CurrencyType> currencyTypes) 
        { 
            currency = new Dictionary<string, int>();
            foreach (CurrencyType c in currencyTypes)
            {
                currency.Add(c.currencyName, c.initalAmount);
            }
        }
        public void UpdateDictionary(List<CurrencyType> currencyTypes)
        {
            foreach (CurrencyType c in currencyTypes)
            {
                if (currency.ContainsKey(c.currencyName)) continue;
                currency.Add(c.currencyName, c.initalAmount);
            }
        }

        public CurrencyData()
        {
            currency = new Dictionary<string, int>();
        }
    }
}
