using System.Linq;
namespace TelegramBot
{
    public class OtherMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }
        public string commandDescription { get; set; }
        private string text;

        public OtherMessage(string text)
        {
            this.text = text;
        }

        public OtherMessage()
        {
            textToCall = new[] {""};
        }

        public string GetMessage(long chatId, string groupNumber = "")
        {
            text.ToUpper();
            var status = UserState.GetChatStatus(chatId);
            var check = Group.AllGroupNumbers.Contains(text);
            if (status == UserState.Status.WaitingGroupNumber
                && check)
            {
                return SetGroupNumber(chatId, text);
            }
            else if (text.Split().Length == 2 && text.Split()[0] == "Р" &&
                     Group.AllGroupNumbers.Contains(text.Split()[1]))
            {
                return (new ScheduleMessage()).GetMessage(chatId, text.Split()[1]);
            }
            else
            {
                return "Я пока не знаю такой команды, проверь правильно ли введены данные";
            }
        }

        public static string SetGroupNumber(long chatId, string groupNumber)
        {
            UserState.SetChatStatus(chatId, UserState.Status.Registered);
            UserState.SetChatGroupNumber(chatId, groupNumber);
            return "Прекрасно, теперь ты можешь получать расписание своей группы просто" +
                   "написав слово \"расписание\" или вызвать команду /ds";
        }
    }
}