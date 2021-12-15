namespace TelegramBot
{
    public class RegisterMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }
        public string commandDescription { get; set; }
        public RegisterMessage()
        {
            textToCall = new[] { "/reg" };
            commandDescription = "помогу зарегестрироваться и запомню тебя";
        }
        public string GetMessage(long chatId, string groupNumber = "")
        {
            if (UserState.GetChatStatus(chatId) == UserState.Status.Registered)
            {
                return "Я всё видел, ты уже зарегистрировался! " +
                       "Теперь ты можешь воспользоваться функцией /ds";
            }
            UserState.SetChatStatus(chatId, UserState.Status.WaitingGroupNumber);
            return "Введи свой номер группы в формате \"МЕН-000000\" :)";
        }
    }
}