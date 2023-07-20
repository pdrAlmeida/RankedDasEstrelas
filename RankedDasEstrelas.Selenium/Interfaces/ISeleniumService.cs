using RankedDasEstrelas.Domain.Dto;
using RankedDasEstrelas.Domain.Entities;
using System.Collections.Generic;

namespace RankedDasEstrelas.Selenium.Interfaces
{
    public interface ISeleniumService
    {
        (Match, List<MatchPlayerDto>) GetMatchData(string url);
    }
}