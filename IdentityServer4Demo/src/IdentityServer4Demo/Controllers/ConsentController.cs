// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Quickstart.UI.Models;

namespace IdentityServer4.Quickstart.UI.Controllers
{
    /// <summary>
    /// This controller implements the consent logic
    /// </summary>
    public class ConsentController : Controller
    {
        private readonly ILogger<ConsentController> _logger;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _scopeStore;
        private readonly IIdentityServerInteractionService _interaction;
        
        public ConsentController(
            ILogger<ConsentController> logger,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore scopeStore)
        {
            _logger = logger;
            _interaction = interaction;
            _clientStore = clientStore;
            _scopeStore = scopeStore;
        }

        /// <summary>
        /// Shows the consent screen
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await BuildViewModelAsync(returnUrl);
            if (vm != null)
            {
                return View("Index", vm);
            }

            return View("Error");
        }

        /// <summary>
        /// Handles the consent screen postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            // parse the return URL back to an AuthorizeRequest object
            var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            ConsentResponse response = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (model.Button == "no")
            {
                response = ConsentResponse.Denied;
            }
            // user clicked 'yes' - validate the data
            else if (model.Button == "yes" && model != null)
            {
                // if the user consented to some scope, build the response model
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    response = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = model.ScopesConsented
                    };
                }
                else
                {
                    ModelState.AddModelError("", "You must pick at least one permission.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Selection");
            }

            if (response != null)
            {
                // communicate outcome of consent back to identityserver
                await _interaction.GrantConsentAsync(request, response);

                // redirect back to authorization endpoint
                return Redirect(model.ReturnUrl);
            }

            var vm = await BuildViewModelAsync(model.ReturnUrl, model);
            if (vm != null)
            {
                return View("Index", vm);
            }

            return View("Error");
        }

        async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (request != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
                if (client != null)
                {
                    var scopes = await _scopeStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
                    if (scopes != null && scopes.IdentityResources.Any() || scopes.ApiResources.Any())
                    {
                        return CreateConsentViewModel(model, returnUrl, request, client, scopes);
                    }
                    else
                    {
                        _logger.LogError("No scopes matching: {0}", request.ScopesRequested.Aggregate((x, y) => x + ", " + y));
                    }
                }
                else
                {
                    _logger.LogError("Invalid client id: {0}", request.ClientId);
                }
            }
            else
            {
                _logger.LogError("No consent request matching request: {0}", returnUrl);
            }

            return null;
        }

        private ConsentViewModel CreateConsentViewModel(
           ConsentInputModel model, string returnUrl,
           AuthorizationRequest request,
           Client client, Resources resources)
        {
            var vm = new ConsentViewModel();
            vm.RememberConsent = model?.RememberConsent ?? true;
            vm.ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>();

            vm.ReturnUrl = returnUrl;

            vm.ClientName = client.ClientName;
            vm.ClientUrl = client.ClientUri;
            vm.ClientLogoUrl = client.LogoUri;
            vm.AllowRememberConsent = client.AllowRememberConsent;

            vm.IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            if (ConsentOptions.EnableOfflineAccess && resources.OfflineAccess)
            {
                vm.ResourceScopes = vm.ResourceScopes.Union(new ScopeViewModel[] {
                    GetOfflineAccessScope(vm.ScopesConsented.Contains(IdentityServerConstants.StandardScopes.OfflineAccess) || model == null)
                });
            }

            return vm;
        }

        public ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required,
            };
        }

        public ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required,
            };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = ConsentOptions.OfflineAccessDisplayName,
                Description = ConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
            };
        }

    }

    public class ConsentOptions
    {
        public static bool EnableOfflineAccess = true;
        public static string OfflineAccessDisplayName = "Offline Access";
        public static string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

        public static readonly string MuchChooseOneErrorMessage = "You must pick at least one permission";
        public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
    }

}