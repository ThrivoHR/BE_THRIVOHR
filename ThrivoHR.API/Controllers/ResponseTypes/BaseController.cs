using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ThrivoHR.API.Controllers.ResponseTypes
{
    [Route("api/v{apiVersion:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController(ISender sender) : ControllerBase
    {
        protected readonly ISender _sender = sender;
    }
}
