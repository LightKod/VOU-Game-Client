using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Owlet;
using Owlet.Systems.SceneTransistions;
using Owlet.UI;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace VOU
{
    //TODO: Upgrade this to use Socket
    public class QuizManager : Singleton<QuizManager>
    {
        [SerializeField] string tempToken;
        [SerializeField] int tempGameID;

        [SerializeField] QuizzWaitCountdown countdown;
        QuizService service;

        public Action<string> onAnswerSelected;
        public Action<QuestionObject> onQuestionReceive;
        public Action<AudioClip> onMCDataReceieve;
        public Action<string, string> onChatReceived;


        int gameID = 20;

        protected override void Init()
        {
            base.Init();
            //gameID = PlayerPrefs.GetInt(Keys.PlayerPrefs.GameID);
            gameID = tempGameID;
            PlayerPrefs.SetString(Keys.PlayerPrefs.User.Token, tempToken);
        }

        private void Start()
        {
            SetupConnection();

            QuizTimer.onCounterFinish += ClosePopup;
        }

        private void OnDestroy()
        {
            QuizTimer.onCounterFinish -= ClosePopup;

            service?.Dispose();
        }

        public async void SetupConnection()
        {
            service = new();
            await service.CreateConnection();
            service.JoinRoom(gameID);

            service.On(QuizService.EVENT_JOIN_ROOM, OnJoinRoomResultReceived);

            service.On(QuizService.EVENT_SEND_QUESTION, OnQuesionReceived);
            service.On(QuizService.EVENT_QUIZ_AUDIO, OnAudioReceived);
            service.On(QuizService.EVENT_SEND_ANSWER, OnAnswerReceived);
            service.On(QuizService.EVENT_CHAT, OnChatReceived);
            service.On(QuizService.EVENT_END_QUIZ, OnQuizEnd);
        }

        public void SelectAnswer(string answer)
        {
            service.AnswerQuestion(answer);
            onAnswerSelected.Invoke(answer);
        }

        public void SendChatMessage(string msg)
        {
            service.SendChatMessage(msg);
        }

        void OnJoinRoomResultReceived(SocketIOResponse res)
        {
            string resultString = res.GetValue<string>(0);
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(resultString);

            string roomState = result["roomState"] as string;

            string startTimeStr = result["startTime"] as string;
            long startTimeTimestamp = long.Parse(startTimeStr);

            DateTime startTime = DateTimeOffset.FromUnixTimeSeconds(startTimeTimestamp).LocalDateTime;
            Debug.Log("Converted DateTime: " + startTime);
            if (roomState == "Concluded")
            {
                MessagePopup.Open("Game has ended!", "The game has ended, you can still chat with other players", () =>
                {
                    //service.Dispose();
                    //SceneTransistion.instance.ChangeScene(Keys.Scene.HomeScene);
                });
            }

            if (roomState == "Waiting")
            {
                //TODO: Setup something here for the waiting state
                //      maybe a countdown?
                countdown.ShowUI(startTime);

            }

            if (roomState == "Playing")
            {
                //TODO: Setup the state to playing
            }

            SceneTransistion.instance.DisableLoadingScreen();
        }

        async void OnQuesionReceived(SocketIOResponse res)
        {
            StopAllCoroutines();

            string questionStr = res.GetValue<string>(0);
            QuestionObject questionObject = JsonConvert.DeserializeObject<QuestionObject>(questionStr);

            QuizzAnswerSelector answerSelector = await PopupManager.instance.OpenUI<QuizzAnswerSelector>(Keys.Popup.QuizzAnswerSelector, 1);
            answerSelector.SetupUI(questionObject);

            onQuestionReceive?.Invoke(questionObject);
        }

        async void OnAudioReceived(SocketIOResponse res)
        {
            string audioStr = res.GetValue<string>(0);
            AudioClip clip = await HandleAudio(audioStr);
            onMCDataReceieve?.Invoke(clip);
        }

        async void OnAnswerReceived(SocketIOResponse res)
        {
            StopAllCoroutines();

            string ansStr = res.GetValue<string>(1);
            AnswerObject answerObject = JsonConvert.DeserializeObject<AnswerObject>(ansStr);

            string qstStr = res.GetValue<string>(0);
            QuestionObject questionObject = JsonConvert.DeserializeObject<QuestionObject>(qstStr);

            QuizzAnswerSelector answerSelector = await PopupManager.instance.OpenUI<QuizzAnswerSelector>(Keys.Popup.QuizzAnswerSelector, 1);
            answerSelector.SetupUIResult(questionObject, answerObject);

            StartCoroutine(ClosePopupDelay());
        }

        async void OnQuizEnd(SocketIOResponse res)
        {
            string victoryPlayerStr = res.GetValue<string>(0);
            var voucherTemplateData = res.GetValue<string>(1);

            Debug.Log(victoryPlayerStr);
            Debug.Log(voucherTemplateData);

            VoucherTemplateModel voucherTemplateModel = JsonConvert.DeserializeObject<VoucherTemplateModel>(voucherTemplateData);
            List<VictoryPlayer> victoryPlayers = JsonConvert.DeserializeObject<List<VictoryPlayer>>(victoryPlayerStr);

            QuizResultPopup quizResultPopup = await PopupManager.instance.OpenUI<QuizResultPopup>(
                Keys.Popup.QuizResult, 1, false);

            quizResultPopup.SetData(victoryPlayers, voucherTemplateModel);
            quizResultPopup.EnableUI();
            StartCoroutine(ClosePopupDelay());
        }
        void OnChatReceived(SocketIOResponse res)
        {
            string username = res.GetValue<string>(0);
            string chatMessage = res.GetValue<string>(1);

            onChatReceived?.Invoke(username, chatMessage);
        }

        public async UniTask<AudioClip> HandleAudio(string audioStr)
        {
            var audioBytes = Convert.FromBase64String(audioStr);
            var tempPath = Path.Combine(Application.persistentDataPath, "tmpMP3Base64.mp3");

            // Write bytes to a temporary file
            await File.WriteAllBytesAsync(tempPath, audioBytes);

            // Use UnityWebRequest to load the audio file into an AudioClip
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + tempPath, AudioType.MPEG))
            {
                ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
                await www.SendWebRequest();

                DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)www.downloadHandler;

                if (dlHandler.isDone)
                {
                    AudioClip audioClip = dlHandler.audioClip;

                    if (audioClip != null)
                    {
                        var _audioClip = DownloadHandlerAudioClip.GetContent(www);
                        return _audioClip;

                    }
                    else
                    {
                        Debug.Log("Couldn't find a valid AudioClip :(");
                    }
                }
                else
                {
                    Debug.Log("The download process is not completely finished.");
                }
            }

            return null;
        }

        
        private IEnumerator ClosePopupDelay()
        {
            yield return new WaitForSeconds(6);
            ClosePopup();
        }

        private void ClosePopup()
        {
            PopupManager.instance.CloseUI(Keys.Popup.QuizzAnswerSelector);
        }
    }
}
