using RankedDasEstrelas.Domain.Dto;
using System.Collections.Generic;

namespace RankedDasEstrelas.Domain.Validations
{
    public interface IValidations
    {
        List<UrlValidationDto> ValidateSentMatch(string url);
    }
}