using MediatR;
using System.Reflection;
using ThrivoHR.Application.Common.Exceptions;
using ThrivoHR.Application.Common.Interfaces;
using ThrivoHR.Application.Common.Security;

namespace ThrivoHR.Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse>(
        ICurrentUserService currentUserService) : IPipelineBehavior<TRequest, TResponse>
       where TRequest : notnull
    {
        //public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        //{
        //    var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        //    if (authorizeAttributes.Any())
        //    {
        //        // Must be authenticated user
        //        if (currentUserService.UserId == null)
        //        {
        //            throw new UnauthorizedAccessException();
        //        }

        //        // Role-based authorization
        //        var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

        //        if (authorizeAttributesWithRoles.Any())
        //        {
        //            foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
        //            {
        //                var authorized = false;
        //                foreach (var role in roles)
        //                {
        //                    var isInRole = await currentUserService.IsInRoleAsync(role.Trim());
        //                    if (isInRole)
        //                    {
        //                        authorized = true;
        //                        break;
        //                    }
        //                }

        //                // Must be a member of at least one role in roles
        //                if (!authorized)
        //                {
        //                    throw new ForbiddenAccessException();
        //                }
        //            }
        //        }

        //        // Policy-based authorization
        //        var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
        //        if (authorizeAttributesWithPolicies.Any())
        //        {
        //            foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
        //            {
        //                var authorized = await currentUserService.AuthorizeAsync(policy);

        //                if (!authorized)
        //                {
        //                    throw new ForbiddenAccessException();
        //                }
        //            }
        //        }
        //    }

        //    // User is authorized / authorization not required
        //    return await next();
        //}
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Extract authorization attributes and early exit if none
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();
            if (!authorizeAttributes.Any())
            {
                return await next(); // No authorization needed
            }

            // Authentication check
            if (currentUserService.UserId == null)
            {
                throw new UnauthorizedAccessException();
            }

            // Role-based authorization
            if (!await HandleRoleBasedAuthorization(authorizeAttributes))
            {
                throw new ForbiddenAccessException();
            }

            // Policy-based authorization
            if (!await HandlePolicyBasedAuthorization(authorizeAttributes))
            {
                throw new ForbiddenAccessException();
            }

            return await next(); // All checks passed
        }

        // Helper method for role-based authorization
        private async Task<bool> HandleRoleBasedAuthorization(IEnumerable<AuthorizeAttribute> attributes)
        {
            var roleTasks = attributes
                .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
                .Select(a => a.Roles.Split(','))
                .SelectMany(roles => roles.Select(role => currentUserService.IsInRoleAsync(role.Trim())))
                .ToList(); // Create a list to hold the tasks

            while (roleTasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(roleTasks);
                roleTasks.Remove(completedTask);

                if (await completedTask)
                {
                    return true; // User is in at least one role
                }
            }

            return false; // User is not in any required roles
        }


        // Helper method for policy-based authorization
        private async Task<bool> HandlePolicyBasedAuthorization(IEnumerable<AuthorizeAttribute> attributes)
        {
            var authorizeAttributesWithPolicies = attributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
            foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
            {
                if (!await currentUserService.AuthorizeAsync(policy))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
