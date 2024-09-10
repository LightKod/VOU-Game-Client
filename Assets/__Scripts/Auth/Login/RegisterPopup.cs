using Owlet.UI.Popups;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace VOU
{
    public class RegisterPopup : Popup
    {
        [SerializeField] Button btnConfirm;
        [SerializeField] InputFieldValidation inputEmail;
        [SerializeField] InputFieldValidation inputPhone;
        [SerializeField] InputFieldValidation inputPassword;
        [SerializeField] InputFieldValidation inputConfirmPassword;

        string phoneNumber;

        private void Awake()
        {
            btnConfirm.onClick.AddListener(SendRegisterRequest);
        }

        protected override void OnEnableUI()
        {
            ToggleInteraction(true);
            base.OnEnableUI();
        }

        async void SendRegisterRequest()
        {
            phoneNumber = "";
            string email = inputEmail.GetValue();
            string password = inputPassword.GetValue();
            string confirmPassword = inputConfirmPassword.GetValue();
            phoneNumber = inputPhone.GetValue();

            if (email.IsNullOrWhitespace())
            {
                ToastHandler.instance.Show("Please enter your email");
                return;
            }
            if(password.IsNullOrWhitespace())
            {
                ToastHandler.instance.Show("Please enter your password");
                return;
            }
            if(password != confirmPassword)
            {
                ToastHandler.instance.Show("Confirm password is not identical");
                return;
            }
            if (phoneNumber.IsNullOrWhitespace())
            {
                ToastHandler.instance.Show("Please enter your phone number");
                return;
            }

            ToggleInteraction(false);


            Dictionary<string, string> form = new()
            {
                { "name", "AAA"},
                { "email", email },
                { "password", password },
            };

            await HttpClient.PostRequest(HttpClient.GetURL(Env.Routes.Auth.Register), form,false ,OnRegisterSuccess, OnRegisterFailed);
        }

        void OnRegisterSuccess(string msg)
        {
            Debug.Log(msg);
            //TODO: a verification step?

            ToastHandler.instance.Show("Register Successful", ToastState.Success);
            /*FirebaseAuthManager.instance.SendPhoneVerificationRequest(ModifyPhoneNumber(phoneNumber), () =>
            {
                SelfClosing();
            }, () =>
            {
                ToggleInteraction(true);
            });*/
            SelfClosing();
        }

        void OnRegisterFailed(string msg)
        {
            Debug.Log(msg);
            ToggleInteraction(true);
        }

        void ToggleInteraction(bool active)
        {
            btnConfirm.enabled = active;
        }


        public string ModifyPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be null or empty", nameof(phoneNumber));
            }

            if (phoneNumber.StartsWith("0"))
            {
                return "+84" + phoneNumber.Substring(1);
            }
            else
            {
                return phoneNumber;
            }
        }
    }
}
