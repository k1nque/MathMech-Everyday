using System.Threading.Tasks;
using Bot.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot
{
    [ApiController]
    [Route("api/message")]
    public class BotController : ControllerBase
    {
        private readonly TelegramBotClient _bot;
        private readonly DataContext _context;

        public BotController(TelegramBot bot, DataContext context)
        {
            _context = context;
            _bot = bot.GetBot().Result;
        }

        [HttpPost("update")]
        //public async Task<IActionResult> Update(Update update)
        public  async Task<IActionResult> Update([FromBody]object update)   //костыль, надо пофиксить
        {
            //Здесь мы добавляем пользователя в бд
            var upd = JsonConvert.DeserializeObject<Update>(update.ToString()); //тот же костыль
            var chat = upd.Message?.Chat;
            //var chat = update.Message?.Chat;
            if (chat == null)
            {
                return Ok();
            }
            var appUser = new AppUsers
            {
                ChatId = chat.Id
            };

            await _context.Users.AddAsync(appUser);
            await _context.SaveChangesAsync();

            await _bot.SendTextMessageAsync(chat.Id, "Ура-ура! Теперь ты есть у нас в базе данных!", ParseMode.Markdown);
            return Ok();
        }
    }
}