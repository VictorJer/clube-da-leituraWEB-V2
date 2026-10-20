using FluentResults;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public static class TempDataExtensions
{
    public static void AddErrorMessagem(this ITempDataDictionary tempData, ResultBase result)
    {
        tempData["MensagemErro"] = result.Errors.First().Message;
    }
}