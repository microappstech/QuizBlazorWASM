using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Radzen;

namespace Langua.WebUI.Pages.TeacherDashboard
{
    public partial class TeacherDashboardComponent:BasePage
    {
        [Inject] public IRepositoryCrudBase<GroupCandidates>? GroupCandidateService { get; set; }

        [Inject] public IRepositoryCrudBase<Models.GroupTeacher>? GroupTeacherService { get; set; }
        public IEnumerable<GroupCandidates>? groupCandidats { get; set; }
        public IEnumerable<Models.GroupTeacher>? groupTeachers { get; set; }
        public Teacher? Teacher { get; set; }
        public bool DataReady {  get; set; }
        public int NbRessource, NbGroups, NbCandidates;

        protected override async Task OnInitializedAsync()
        {
            await Security.IsAuthenticatedWidthRedirect();
            await Security.InitializeAsync();
            var teacher = await baseService.GetEntiteByUserId<Teacher>(Security.User.Id, t => t.UserId == Security.User.Id);
            var resultToCandidates = GroupCandidateService.GetAll();
            var resultGroups = GroupTeacherService.GetByExpression(x=>x.TeacherId == teacher.Id);
            if(resultToCandidates.Succeeded)
            {
                groupCandidats = (IEnumerable<GroupCandidates>)baseService.Apply(resultToCandidates.Value, new QueryCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> { { "include", new StringValues("Subject,Group") } }));
            }
        }
    }
}