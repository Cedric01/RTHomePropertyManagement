using System.ComponentModel.DataAnnotations;

namespace RTHomePropertyManagement.Extensions;

// Generic minimal-API endpoint filter that runs DataAnnotations validation
// (including IValidatableObject) against the first argument assignable to T
// before the handler runs. Wire it up per-route with
// .AddEndpointFilter<ValidationFilter<TDto>>() on any endpoint that binds a
// TDto from the request body.
public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var target = context.Arguments.OfType<T>().FirstOrDefault();
        if (target is not null)
        {
            var validationContext = new ValidationContext(target);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(target, validationContext, validationResults, validateAllProperties: true))
            {
                var errors = validationResults
                    .SelectMany(r => (r.MemberNames.Any() ? r.MemberNames : new[] { string.Empty })
                        .Select(member => (Member: LowerFirst(member), r.ErrorMessage)))
                    .GroupBy(x => x.Member)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage ?? "Invalid value.").ToArray());

                return Results.ValidationProblem(errors);
            }
        }

        return await next(context);
    }

    private static string LowerFirst(string value) =>
        value.Length > 0 ? char.ToLowerInvariant(value[0]) + value[1..] : value;
}
