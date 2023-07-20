using System;

namespace RankDasEstrelas.Bot.Extensions
{
    public class InteractionResponseTimeOutException : TimeoutException
    {
        public InteractionResponseTimeOutException(string message) : base(message)
        {
        }
    }

    
}