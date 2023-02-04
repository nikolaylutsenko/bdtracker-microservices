using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BdTracker.Back.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly IMapper _mapper;

        public BaseController(ILogger<BaseController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
    }
}