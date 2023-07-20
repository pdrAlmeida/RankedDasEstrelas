using System;

namespace RankDasEstrelas.Bot.Extensions
{
    public class NullInteractionResponseException : Exception
    {
        public NullInteractionResponseException(string message) : base(message)
        {
        }
    }    
}