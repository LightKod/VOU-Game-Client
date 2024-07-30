using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class InputFieldValidation : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button extraButton;
        [SerializeField] private TMP_Text validationText;

        public bool shouldValidate = true;
        public bool isPasswordField = false;
        [SerializeField] private Image peekButtonImage;
        [SerializeField] private Sprite eyeOpenSprite;
        [SerializeField] private Sprite eyeCloseSprite;
        public bool isNotEmpty = true;
        public bool shouldDisplayValidationText = true;
        public bool shouldShowExtraButton = true;
        public string regexToCheck = "";
        public string regexValidationTextContent = "";

        private Regex regex;

        private Color validNormalColor, validColor, invalidColor;

        private bool isPasswordVisible = false;

        // Start is called before the first frame update
        void Start()
        {
            if(inputField != null)
            {
                inputField.onValueChanged.AddListener(delegate { ValidateInputField(); });
            }

            regex = new Regex(regexToCheck);

            if(!ColorUtility.TryParseHtmlString(Keys.Color.Black_Text, out validNormalColor))
            {
                validNormalColor = Color.black;
            }
            if(!ColorUtility.TryParseHtmlString(Keys.Color.Purple_UI, out validColor))
            {
                validColor = Color.black;
            }
            if(!ColorUtility.TryParseHtmlString(Keys.Color.Red, out invalidColor))
            {
                invalidColor = Color.red;
            }
        }

        private void ValidateInputField()
        {
            string input = inputField.text;

            if(input.Length > 0)
            {
                if(!extraButton.IsActive() && shouldShowExtraButton)
                {
                    extraButton.gameObject.SetActive(true);
                }
            }
            else
            {
                extraButton.gameObject.SetActive(false);
            }


            input = input.Trim();

            if (!CheckEmptyString(input))
            {
                string validationTextContent = "This field cannot be empty";
                DisplayValidationResult(false, validationTextContent);
                return;
            }

            if(!CheckWithRegex(input))
            {
                string validationTextContent = regexValidationTextContent;
                DisplayValidationResult(false, validationTextContent);
                return;
            }

            DisplayValidationResult(true);
        }

        private bool CheckEmptyString(string input)
        {
            if(isNotEmpty && input.Length == 0)
                return false;
            return true;
        }

        private bool CheckWithRegex(string input)
        {
            return regex.IsMatch(input);
        }

        private void DisplayValidationResult(bool isValidField, string validationTextContent = null)
        {
            ColorBlock colors = inputField.colors;

            if (!isValidField)
            {
                colors.normalColor = invalidColor;
                colors.highlightedColor = invalidColor;
                colors.selectedColor = invalidColor;

                inputField.colors = colors;

                if (shouldDisplayValidationText)
                {
                    validationText.SetText(validationTextContent);
                    if (!validationText.IsActive())
                    {
                        validationText.gameObject.SetActive(true);
                    }
                    LayoutRebuilder.ForceRebuildLayoutImmediate(validationText.GetComponent<RectTransform>());
                }
            }
            else
            {
                colors.normalColor = validNormalColor;
                colors.highlightedColor = validColor;
                colors.selectedColor = validColor;

                inputField.colors = colors;

                if (validationText.IsActive())
                {
                    validationText.gameObject.SetActive(false);
                }
            }
        }

        public void ClearText()
        {
            inputField.text = "";
        }

        public void TogglePeekPassword()
        {
            isPasswordVisible = !isPasswordVisible;

            inputField.inputType = isPasswordVisible ? TMP_InputField.InputType.Standard : TMP_InputField.InputType.Password;

            peekButtonImage.sprite = isPasswordVisible ? eyeCloseSprite : eyeOpenSprite;

            inputField.ForceLabelUpdate();
        }

        public bool IsValidField()
        {
            string input = inputField.text.Trim();
            return CheckEmptyString(input) && CheckWithRegex(input);
        }
    }
}
