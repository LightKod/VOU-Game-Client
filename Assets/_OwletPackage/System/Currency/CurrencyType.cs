using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet.Systems.Currency
{
    [CreateAssetMenu(menuName ="Owlet/Currency")]
    public class CurrencyType : ScriptableObject
    {
        public string currencyName;
        public int initalAmount = 0;
        public Sprite icon;
    }
}
