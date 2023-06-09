﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace LokataAdministrative2.Authentication
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public TokenAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetTokenAsync(string token)
        {
            if (token == null)
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authToken");
            }
            else
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authToken", token);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task<string> GetTokenAsync()
            => await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync();
            var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(ParseClaimsFromJwt(token), "auth");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
