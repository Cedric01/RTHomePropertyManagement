using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagementTests;

public static class PropertyEndpointTestHelpers
{
    public static Task<IResult> InvokeCreateProperty(IPropertyRepository repo, Property p)
        => PropertyEndpoints.CreateProperty(repo, p);

    public static Task<IResult> InvokeListProperties(IPropertyRepository repo)
        => PropertyEndpoints.ListProperties(repo);

    public static Task<IResult> InvokeUpdateProperty(IPropertyRepository repo, int id, Property p)
        => PropertyEndpoints.UpdateProperty(repo, id, p);

    public static Task<IResult> InvokeDeleteProperty(IPropertyRepository repo, int id)
        => PropertyEndpoints.DeleteProperty(repo, id);
}