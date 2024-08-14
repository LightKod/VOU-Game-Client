using Cysharp.Threading.Tasks.Triggers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class SearchBarManager : MonoBehaviour
    {
        [SerializeField] private Image searchIcon;
        [SerializeField] private Button clearButton;
        [SerializeField] private Image clearButtonImage;

        private Color focusColor, nonFocusColor;

        private TMP_InputField searchField;

        // Start is called before the first frame update
        void Start()
        {
            searchField = GetComponent<TMP_InputField>();
            searchField.onSelect.AddListener(delegate { OnSelect(); });
            searchField.onDeselect.AddListener(delegate { OnDeselect(); });
            searchField.onEndEdit.AddListener(delegate { OnEndEdit(); });
            searchField.onValueChanged.AddListener(delegate { OnValueChanged(); });

            if (!ColorUtility.TryParseHtmlString(Keys.Color.Purple_UI, out focusColor))
            {
                focusColor = Color.black;
            }
            if (!ColorUtility.TryParseHtmlString(Keys.Color.Grey_Placeholder, out nonFocusColor))
            {
                focusColor = Color.black;
            }
        }

        private void OnValueChanged()
        {
            string input = searchField.text;

            if (input.Length > 0)
            {
                if (!clearButton.IsActive())
                {
                    clearButton.gameObject.SetActive(true);
                }
            }
            else
            {
                clearButton.gameObject.SetActive(false);
            }
        }

        private void OnEndEdit()
        {
            Debug.Log("OnEndEdit");
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Debug.Log(searchField.text);
            }
        }

        private void OnDeselect()
        {
            Debug.Log("OnDeselect");
            searchIcon.color = nonFocusColor;
            clearButtonImage.color = nonFocusColor;
        }

        private void OnSelect()
        {
            Debug.Log("OnSelect");
            searchIcon.color = focusColor;
            clearButtonImage.color = focusColor;
        }

        public void ClearText()
        {
            searchField.text = "";
        }
    }
}
