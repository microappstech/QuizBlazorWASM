using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Components;
using Langua.WebUI.Pages.Teachers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Langua.WebUI.Pages.Manager
{
    public partial class CUManager
    {
        [Parameter] public bool IsEdit { get; set; }
        [Inject] IRepositoryCrudBase<Models.Manager>? crudRepository { get; set; }
        public Models.Manager Manager { get; set; }
        [Parameter] public string? Id { get; set; }

        public List<string> Errors { get; set; } = new();
        protected override Task OnInitializedAsync()
        {
            Errors = new List<string>();
            if(IsEdit)
            {
                var resultM = crudRepository.GetById(int.Parse(Id));
                if(resultM.Succeeded)
                {
                    Manager = resultM.Value;
                }
            }
            else
            {
                Manager = new Models.Manager()
                {
                    FullName = "",
                    UserId ="",
                    Photo ="",
                    Password="",
                    Phone ="",
                    ConfirmPassword="",
                    Email=""
                };
            }
            return base.OnInitializedAsync();
        }
        public async Task Delete(Models.Manager args)
        {
            if(await dialogService.Confirm("Confirmation suppression","Are you sure you want to delete this manager") == true)
            {
                var result = crudRepository.Delete(args);
                if (result.Succeeded)
                {
                    Notify("success", "Suppression successful completed");
                }
            }
        }

        public async void loadImage(InputFileChangeEventArgs args)
        {
            MemoryStream memoryStream = new MemoryStream();
            await args.File.OpenReadStream().CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            string base64 = Convert.ToBase64String(bytes);

            Manager.Photo = "data:image/png;base64," + base64;
        }
        public async Task HandleValidSubmit()
        {
            Errors = new();
            if (Manager.Id == 0 && !IsEdit)
            {
                ApplicationUser _user = new ApplicationUser()
                {
                    Email = Manager.Email,
                    UserName = Manager.Email,
                    Password = Manager.Password,
                    NormalizedUserName = Manager.FullName,
                    PhoneNumber = Manager.Phone
                };
                //using (var scope = new TransactionScope(Options))
                //{

                var TaskUser = await Security.RegisterUser(_user);
                if (TaskUser.Succeeded)
                {
                    var result = crudRepository.Add(Manager);
                    if (result.Succeeded)
                    {
                         Notify("Success", "Item created successfully", Radzen.NotificationSeverity.Success);
                         dialogService.Close();
                    }
                    else
                    {
                        Errors.Add(result.Error);
                    }
                }
            }
            else
            {
                var result = crudRepository.Update(Manager);
                if (result.Succeeded)
                {
                    Notify("Success", "Item updated successfully", Radzen.NotificationSeverity.Success);
                    dialogService.Close();
                }
                else
                {
                    Errors.Add(result.Error);
                }
            }
        }
    }
}