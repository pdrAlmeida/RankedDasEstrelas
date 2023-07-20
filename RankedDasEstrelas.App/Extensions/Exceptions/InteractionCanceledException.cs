using System;

namespace RankDasEstrelas.Bot.Extensions
{
    public class InteractionCanceledException : OperationCanceledException
    {
        public InteractionCanceledException(string message) : base(message)
        {
        }
    }

    
}