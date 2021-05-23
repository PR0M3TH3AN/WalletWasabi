using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WalletWasabi.WabiSabi;
using WalletWasabi.WabiSabi.Backend.Models;
using WalletWasabi.WabiSabi.Models;

namespace WalletWasabi.Backend.Filters
{
	public class ExceptionTranslateAttribute : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			var exception = context.Exception.InnerException ?? context.Exception;

			context.Result = exception switch
			{
				WabiSabiProtocolException e => new JsonResult(new Error(
					Type: ProtocolConstants.ProtocolViolationType,
					ErrorCode: e.ErrorCode.ToString(),
					Description: e.Message))
				{
					StatusCode = (int)HttpStatusCode.InternalServerError
				},
				_ => new StatusCodeResult((int)HttpStatusCode.InternalServerError)
			};
		}
	}
}
