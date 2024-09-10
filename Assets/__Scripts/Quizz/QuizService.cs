using Cysharp.Threading.Tasks;
using Owlet;
using Owlet.UI;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VOU.Env.Routes;

namespace VOU
{
    public class QuizService : SocketService
    {

        public const string EVENT_DEBUG = "debug";
        public const string EVENT_JOIN_ROOM = "joinRoom";
        public const string EVENT_LEAVE_ROOM = "leaveRoom";

        public const string EVENT_SEND_QUESTION = "sendQuestion";
        public const string EVENT_SEND_ANSWER = "sendAnswer";
        public const string EVENT_QUIZ_AUDIO = "audio";
        public const string EVENT_RESULT = "answerResult";
        public const string EVENT_END_QUIZ = "endQuiz";
        public const string EVENT_CHAT = "chat";

        int gameID;

        protected override string GetURL()
        {
            return HttpClient.GetURL(Socket.Quiz);
        }

        public void JoinRoom(int gameID)
        {
            socket.Emit(EVENT_JOIN_ROOM, gameID);
            this.gameID = gameID;
        }

        public void AnswerQuestion(string answer)
        {
            socket.Emit(EVENT_SEND_ANSWER, gameID, answer);
        }


        public void SendChatMessage(string message)
        {
            socket.Emit(EVENT_CHAT, gameID, message);
        }
    }
}
