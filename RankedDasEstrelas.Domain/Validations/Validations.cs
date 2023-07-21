using RankedDasEstrelas.Domain.Dto;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using System.Collections.Generic;

namespace RankedDasEstrelas.Domain.Validations
{
    public class Validations : IValidations
    {
        private readonly IMatchRepository matchRepository;

        public Validations(IMatchRepository matchRepository) => this.matchRepository = matchRepository;

        public List<UrlValidationDto> ValidateSentMatch(string url)
            => new() { ValidateUrl(url), ValidateUnplayedMatch(url) };

        private UrlValidationDto ValidateUnplayedMatch(string url)
        {
            bool unplayedMatch = matchRepository.FindMatchOrDefaultAsync(url.Split('/')[^1]).Result is null;
            return new UrlValidationDto(unplayedMatch, unplayedMatch ? "Partida não contabilizada" : "Esta partida já foi contabilizada!");
        }

        private static UrlValidationDto ValidateUrl(string url)
        {
            bool validUrl = url.Contains("https://br.op.gg/summoners/br") || url.Contains("https://www.op.gg/summoners/br");
            return new UrlValidationDto(validUrl, validUrl ? "Url válida" : "Url inválida!");
        }
    }
}