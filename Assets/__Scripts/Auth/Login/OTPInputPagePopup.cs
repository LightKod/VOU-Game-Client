using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VOU
{
    public class OTPInputPagePopup : Popup
    {
        [SerializeField] private TMP_InputField[] inputFields;
        [SerializeField] private GameObject clearBtn;
        [SerializeField] private GameObject resendCounter;
        [SerializeField] private TMP_Text resendTimeText;
        [SerializeField] private GameObject resendBtn;

        float countdown = 60f;

        protected override void OnEnableUI()
        {
            foreach (var inputField in inputFields)
            {
                inputField.onValueChanged.AddListener(delegate { OnValueChanged(inputField); });
                inputField.onSelect.AddListener(delegate { SelectAllText(inputField); });
            }
            base.OnEnableUI();
        }

        protected override void OnDisableUI()
        {
            foreach (var inputField in inputFields)
            {
                inputField.onValueChanged.RemoveAllListeners();
                inputField.onSelect.RemoveAllListeners();
            }
            base.OnDisableUI();
        }

        private void Update()
        {
            if (countdown >= 0)
            {
                countdown -= Time.deltaTime;
                resendTimeText.text = Mathf.Ceil(countdown).ToString();

                if (countdown < 0)
                {
                    resendCounter.SetActive(false);
                    resendBtn.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                GoBackToPreviousInputField();
            }
        }

        private void GoBackToPreviousInputField()
        {
            foreach (var inputField in inputFields)
            {
                if (inputField.isFocused && string.IsNullOrEmpty(inputField.text))
                {
                    int index = System.Array.IndexOf(inputFields, inputField);
                    if (index > 0)
                    {
                        inputFields[index - 1].Select();
                        inputFields[index - 1].ActivateInputField();
                    }
                }
            }
        }

        private void SelectAllText(TMP_InputField inputField)
        {
            inputField.selectionAnchorPosition = 0;
            inputField.selectionFocusPosition = inputField.text.Length;
        }

        private void OnValueChanged(TMP_InputField inputField)
        {
            if (inputField.text.Length > 1)
            {
                inputField.text = inputField.text.Substring(0, 1);
            }

            // Set the clear button to active if there exists text
            SetClearButton();

            // If the input field has one character, move to the next field
            if (inputField.text.Length == 1)
            {
                int index = System.Array.IndexOf(inputFields, inputField);
                if (index < inputFields.Length - 1)
                {
                    inputFields[index + 1].Select();
                    inputFields[index + 1].ActivateInputField();
                }
            }
        }

        private void SetClearButton()
        {
            bool shouldActive = false;
            foreach (var inputField in inputFields)
            {
                if (inputField.text.Length > 0)
                {
                    shouldActive = true;
                }
            }
            clearBtn.SetActive(shouldActive);
        }

        public void ClearFields()
        {
            foreach (var inputField in inputFields)
            {
                inputField.text = "";
            }
            inputFields[0].Select();
            inputFields[0].ActivateInputField();
        }

        public void ResendOTP()
        {
            resendCounter.SetActive(true);
            resendBtn.SetActive(false);
            countdown = 60f;
        }
    }
}
