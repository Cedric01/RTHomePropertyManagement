using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Models;

namespace RTHomePropertManagementTests;

public static class PropertyEndpointTestHelpers
{
    public static Task<IResult> InvokeCreateProperty(AppDbContext db, Property p)
        => (Task<IResult>)typeof(PropertyEndpoints)
            .GetMethod("CreateProperty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .Invoke(null, new object[] { db, p });

    public static Task<IResult> InvokeListProperties(AppDbContext db)
        => (Task<IResult>)typeof(PropertyEndpoints)
            .GetMethod("ListProperties", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .Invoke(null, new object[] { db });

    public static Task<IResult> InvokeUpdateProperty(AppDbContext db, int id, Property p)
        => (Task<IResult>)typeof(PropertyEndpoints)
            .GetMethod("UpdateProperty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .Invoke(null, new object[] { db, id, p });

    public static Task<IResult> InvokeDeleteProperty(AppDbContext db, int id)
        => (Task<IResult>)typeof(PropertyEndpoints)
            .GetMethod("DeleteProperty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .Invoke(null, new object[] { db, id });
}