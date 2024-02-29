using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

public sealed class PaycheckIntegrationTests : IntegrationTest
{
    // GET
    [Fact]
    public async Task Get_Should_ReturnPaycheck()
    {
        var checkId = 26;
        var employeeId = 1;

        var response = await HttpClient.GetAsync($"/api/v1/paychecks/{checkId}/employee/{employeeId}");

        await response.ShouldReturn(HttpStatusCode.OK);
    }
}

