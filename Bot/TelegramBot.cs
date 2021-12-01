using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace Bot
{
    public class TelegramBot
    {
        private readonly IConfiguration _configuration;
        private TelegramBotClient _bot;

        public TelegramBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TelegramBotClient> GetBot()
        {
            if (_bot != null)
            {
                return _bot;
            }
            _bot = new TelegramBotClient(_configuration["Token"]);

            var hook = $"{_configuration["Url"]}api/message/update";
            await _bot.SetWebhookAsync(hook);
            return _bot;
        }
    }
}