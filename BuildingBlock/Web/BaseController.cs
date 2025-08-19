using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Web
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected ISender _sender;
        public BaseController(ISender sender)
        {
            _sender = sender;
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BaseAdminController : ControllerBase
    {
        protected ISender _sender;
        public BaseAdminController(ISender sender)
        {
            _sender = sender;
        }
    }
    [AllowAnonymous]
    public class BaseProviderController : BaseController
    {
        public BaseProviderController(ISender sender) : base(sender)
        {
        }
    }
}
