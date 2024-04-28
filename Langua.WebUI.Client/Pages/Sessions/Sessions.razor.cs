using Langua.WebUI.Client;
//using Langua.WebUI.Client.Layout;
using Langua.WebUI.Client.Pages.Shared;
using Langua.WebUI.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Components.Web.RenderMode;

namespace Langua.WebUI.Client.Pages.Sessions
{
    public partial class SessionsComponent : BasePageClient
    {
        

        protected IEnumerable<Models.Session> sessions;
        protected RadzenDataGrid<Models.Session> grid0;
        protected int count;
        protected bool isEdit = true;
        

        
        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await LangClientService.GetSessions(filter: $"{args.Filter}", expand: "Group,Teacher", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null);
                sessions = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (Exception ex)
            {
                Notify("Error", "Unable to load Sessions", NotificationSeverity.Error);
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            //await DialogService.OpenAsync<AddSession>("Add Session", null);
            await grid0.Reload();
        }

        protected async Task EditRow(Models.Session args)
        {
            //await DialogService.OpenAsync<EditSession>("Edit Session", new Dictionary<string, object> { { "Id", args.Id } });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Models.Session session)
        {
            try
            {
                if (await dialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await LangClientService.DeleteSession(id: session.Id);//if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                Notify($"Error", $"Unable to delete Session",NotificationSeverity.Error);
            }
        }
        protected bool errorVisible;
        protected Models.Session session;

        protected IEnumerable<Models.Groups> groupsForGroupId;

        protected IEnumerable<Models.Teacher> teachersForTeacherId;


        protected int groupsForGroupIdCount;
        protected Models.Groups groupsForGroupIdValue;
        protected async Task groupsForGroupIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await LangClientService.GetGroups(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(Name, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                groupsForGroupId = result.Value.AsODataEnumerable();
                groupsForGroupIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                Notify($"Error", $"Unable to load Group" , NotificationSeverity.Error);
            }
        }

        protected int teachersForTeacherIdCount;
        protected Models.Teacher teachersForTeacherIdValue;
        protected async Task teachersForTeacherIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await LangClientService.GetTeachers(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(UserId, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                teachersForTeacherId = result.Value.AsODataEnumerable();
                teachersForTeacherIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                //NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Teacher" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await LangClientService.UpdateSession(id: session.Id, session) : await LangClientService.CreateSession(session);
                dialogService.Close(session);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            //dialogService.Close(null);
        }
    }
}
