using System.Threading.Tasks;
using acmManager.Models.TokenAuth;
using acmManager.Web.Controllers;
using Shouldly;
using Xunit;

namespace acmManager.Web.Tests.Controllers
{
    public class HomeController_Tests: acmManagerWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}