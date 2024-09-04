using Newtonsoft.Json;
using Owlet.Systems.SceneTransistions;
using Owlet.UI.Popups;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class LoginPopup : Popup
    {
        [SerializeField] Button btnConfirm;
        [SerializeField] InputFieldValidation inputEmail;
        [SerializeField] InputFieldValidation inputPassword;


        private void Awake()
        {
            btnConfirm.onClick.AddListener(SendLoginRequest);
        }

        protected override void OnEnableUI()
        {
            ToggleInteraction(true);
            base.OnEnableUI();
        }

        async void SendLoginRequest()
        {
            string email = inputEmail.GetValue();
            string password = inputPassword.GetValue();

            if (email.IsNullOrWhitespace())
            {
                ToastHandler.instance.Show("Please enter your email");
                return;
            }
            if (password.IsNullOrWhitespace())
            {
                ToastHandler.instance.Show("Please enter your password");
                return;
            }

            ToggleInteraction(false);

            Dictionary<string, string> form = new()
            {
                { "email", email },
                { "password", password },
            };

            await HttpClient.PostRequest(HttpClient.GetURL(Env.Routes.Auth.Login), form, OnLoginSuccess, OnLoginFail);
        }

        void OnLoginSuccess(string msg)
        {
            Dictionary<string, string> msgResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg);
            Debug.Log(msgResult["token"]);
            ToastHandler.instance.Show("Login Successful", ToastState.Success);
            PlayerPrefs.SetString(Keys.PlayerPrefs.User.Token, msgResult["token"]);
            SceneTransistion.instance.ChangeScene(Keys.Scene.HomeScene);
            //SelfClosing();
        }

        void OnLoginFail(string msg)
        {
            Debug.Log(msg);
            ToggleInteraction(true);
        }


        void ToggleInteraction(bool active)
        {
            btnConfirm.enabled = active;
        }
    }
}
