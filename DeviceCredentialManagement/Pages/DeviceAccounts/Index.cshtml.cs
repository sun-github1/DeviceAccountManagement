using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceCredentialManagement.Model;
using DeviceCredentialManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace DeviceCredentialManagement.Pages.DeviceAccounts
{
    public class IndexModel : PageModel
    {
        private IDeviceAccountRepository deviceAccountRepository;
        public IEnumerable<DeviceAccountCredential> DeviceAccountCredentials;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        private readonly IConfiguration _configuration;


        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } =1;
        public int Count { get; set; }
        public int PageSize { get; set; } 
        [BindProperty(SupportsGet = true)]
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));


        public IndexModel(IDeviceAccountRepository deviceAccountRepository, IConfiguration _configuration)
        {
            this.deviceAccountRepository = deviceAccountRepository;
            this._configuration = _configuration;

            PageSize = _configuration.GetValue<int>("PageSize");
        }
        //public void OnGet()
        //{
        //    if (string.IsNullOrEmpty(SearchTerm))
        //    {
        //        Count= deviceAccountRepository.GetTotalAccountCredentials(string.Empty);
        //        DeviceAccountCredentials = deviceAccountRepository.GetAccountCredentials(CurrentPage, PageSize,string.Empty);
        //    }
        //    else
        //    {
        //        Count = deviceAccountRepository.GetTotalAccountCredentials(SearchTerm);
        //        DeviceAccountCredentials = deviceAccountRepository.GetAccountCredentials(CurrentPage, PageSize, SearchTerm);
        //    }
        //}



        public void OnGet(string searchstring)
        {
            if (searchstring == null)
            {
                searchstring = SearchTerm;
            }

            SearchTerm = searchstring;
            if (string.IsNullOrEmpty(searchstring))
            {
                Count = deviceAccountRepository.GetTotalAccountCredentials(string.Empty);
                DeviceAccountCredentials = deviceAccountRepository.GetAccountCredentials(CurrentPage, PageSize, string.Empty);
            }
            else
            {
                Count = deviceAccountRepository.GetTotalAccountCredentials(searchstring);
                DeviceAccountCredentials = deviceAccountRepository.GetAccountCredentials(CurrentPage, PageSize, searchstring);
            }
        }

    }
}
