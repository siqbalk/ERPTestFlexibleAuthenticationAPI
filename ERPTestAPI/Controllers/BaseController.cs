using CommonLayer.DTOs;
using CommonLayer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UnitOfWork.Portal;
using static CommonLayer.Constants;

namespace ERPTestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class BaseController : ControllerBase
    {
        protected readonly IERPUnitOfWork eRPUnitOfWork;

        public BaseController(IERPUnitOfWork eRPUnitOfWork)
        {
            this.eRPUnitOfWork = eRPUnitOfWork;
        }

        protected BaseResponse constructResponse(object response)
        {
            return new BaseResponse()
            {
                dynamicResult = response,
                isSuccessfull = OtherConstants.isSuccessful,
                statusCode = response != null ? 200 : 500,
                messageType = OtherConstants.messageType,
                message = OtherConstants.responseMsg,
                errorMessage = OtherConstants.responseMsg,
                NewAccessToken = Utils.NewAccessToken ?? null
            };
        }
    }
}
