using OpenQA.Selenium;
using System;
using System.Threading;
using System.Collections.Generic;
using RankedDasEstrelas.Selenium.Interfaces;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Domain.Dto;
using RankedDasEstrelas.Selenium.WebDriver;

namespace RankedDasEstrelas.Selenium.Services
{
    public class SeleniumService : ISeleniumService
    {
        private readonly IPlayerRepository playerRepository;
        private readonly IWebDriver WebDriver = SeleniumWebDriver.WebDriver;

        public SeleniumService(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public (Match, List<MatchPlayerDto>) GetMatchData(string url)
        {
            int retryCount = 0;
            const int maxRetries = 3;
            try
            {
                SeleniumWebDriver.InitializeWebDriver();
                WebDriver.Navigate().GoToUrl(url);

                Thread.Sleep(1000);

                Console.WriteLine("Início do Selenium");
                Thread.Sleep(3000);

                CloseAdBlockNotification();

                Thread.Sleep(1000);

                Console.WriteLine("Site Aberto");

                var matchData = BuildMatchData(url);

                SeleniumWebDriver.QuitWebDriver();

                return matchData;
            }
            catch (WebDriverException ex)
            {
                Console.WriteLine(ex.ToString());
                SeleniumWebDriver.QuitWebDriver();

                retryCount++;
                if (retryCount <= maxRetries)
                {
                    Console.WriteLine("Tentando novamente...");
                    return GetMatchData(url);
                }
                else
                    throw new Exception("Erro ocorreu três vezes consecutivas. Encerrando execução.");
            }
        }

        private void CloseAdBlockNotification()
        {
            if (WebDriver.FindElement(By.ClassName(@"fc-close-icon")).Text.Contains("cancel"))
            {
                Console.WriteLine("AdBlock Detected!");
                WebDriver.FindElement(By.ClassName(@"fc-close-icon")).Click();
            }

            if (WebDriver.FindElement(By.XPath(@"//*[@id=""__next""]/div[2]/div/div[3]/button")).Displayed)
            {
                Console.WriteLine("AdBlock Detected!");
                WebDriver.FindElement(By.XPath(@"//*[@id=""__next""]/div[2]/div/div[3]/button")).Click();
            }
        }

        private List<MatchPlayerDto> GetMatchPlayers()
        {
            List<MatchPlayerDto> matchPlayers = new();

            for (int i = 1; i < 6; i++)
            {
                var tr = WebDriver.FindElement(By.XPath(@$"//*[@id='__next']/div[5]/li/div[2]/div[1]/table[1]/tbody/tr[{i}]"));
                var nick = tr.FindElement(By.XPath(@$"//*[@id='__next']/div[5]/li/div[2]/div[1]/table[1]/tbody/tr[{i}]/td[4]/a")).Text;
                var player = playerRepository.FindByNickNameAsync(nick).Result ?? throw new Exception($"O jogador {nick} não está cadastrado!");

                matchPlayers.Add(new MatchPlayerDto(player: player,
                    win: WebDriver.FindElement(By.XPath(@"//*[@id=""__next""]/div[5]/li/div[2]/div[1]/table[1]/thead/tr/th[1]/span")).Text == "Vitória", 
                    rank: tr.FindElement(By.XPath(@$"//*[@id='__next']/div[5]/li/div[2]/div[1]/table[1]/tbody/tr[{i}]/td[5]/div/div[2]/div")).Text, 
                    score: Convert.ToDouble(tr.FindElement(By.XPath(@$"//*[@id='__next']/div[5]/li/div[2]/div[1]/table[1]/tbody/tr[{i}]/td[5]/div/div[1]")).Text.Replace('.', ','))));
            }

            return matchPlayers;
        }

        private (Match, List<MatchPlayerDto>) BuildMatchData(string url)
        {
            var matchPlayers = GetMatchPlayers();

            return (new Match(url, matchPlayers[0].Win ? "WIN" : "LOSE",
                        matchPlayers[0].PlayerId,
                        matchPlayers[1].PlayerId,
                        matchPlayers[2].PlayerId,
                        matchPlayers[3].PlayerId,
                        matchPlayers[4].PlayerId), matchPlayers);
        }
    }
}