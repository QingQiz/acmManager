using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using acmManager.Authorization;
using acmManager.Controllers;
using acmManager.Users;
using acmManager.Users.Dto;
using acmManager.Web.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.PagesUsers_Admin)]
    public class AdminController : acmManagerControllerBase
    {
        private readonly UserAppService _userAppService;

        public AdminController(UserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public virtual ActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [HttpPost]
        [UnitOfWork]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_GetAll)]
        public async Task<ActionResult> GetAllWithFilter(GetAllUserWithFilterViewModel input)
        {
            var filter = ObjectMapper.Map<UserInfoDto>(input);
            var filterResult = await _userAppService.GetAllUserAsync(new GetAllUserInput()
            {
                Filter = filter,
                MaxResultCount = input.MaxResultCount,
                SkipCount = input.SkipCount
            });
            return View("Index", new IndexViewModel()
            {
                Users = filterResult,
                CurrentUserFilter = input
            });
        }
    }
}