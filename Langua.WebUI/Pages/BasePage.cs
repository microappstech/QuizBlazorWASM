﻿using Langua.Models;
using Langua.Repositories.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Langua.Repositories.Interfaces;
using Radzen;
using Langua.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Langua.WebUI.Client.Pages;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;


namespace Langua.WebUI.Pages
{
    public partial class BasePage : BasePageClient
    {
        [Inject] public NavigationManager Navigation { get; set; } = null!;
        [Inject] ProtectedSessionStorage LocalStorage { get;set; }

        public RadzenProgressBarCircular? loading;
        public bool IsStuck { get; set; } = true;
        [Inject] protected Langua.Account.SecurityService Security { get; set; } = null!;
        [Inject] public LanguaService LanguaService { get; set; } = null!;
        [Inject] public BaseService baseService { get; set; } = null!;
        [Inject] protected AuthenticationStateProvider? authenticationStateProvider { get; set; }
        [Inject] protected IMailService? mailService { get; set; }
        public async void setValue(string key,object value)
        {
            await LocalStorage.SetAsync(key, value);
        }
        public async Task<object> getValue(string key)
        {
            var r = await LocalStorage.GetAsync<object>(key);
            return r;
        }
        public bool ValidateMail(string mail = "")
        {
            if(string.IsNullOrEmpty(mail)) return false;
            if(!mail.Contains("@gmail.com")) return false;
            return true;
        }
        public bool MailNotTaken(string mail, string _olmail = null)
        {
            if(!string.IsNullOrEmpty(mail))
            {
                return mail != _olmail;
            }
            return !Security.MailTaken(mail);
        }
    }
}
