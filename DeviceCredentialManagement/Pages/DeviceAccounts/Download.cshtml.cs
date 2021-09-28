using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using DeviceCredentialManagement.Model;
using DeviceCredentialManagement.Services;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DeviceCredentialManagement.Pages.DeviceAccounts
{
    public class DownloadModel : PageModel
    {
        private readonly ILogger _logger;
        private IDeviceAccountRepository deviceAccountRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        public string ErrorMessage { get; set; }
        [BindProperty]
        public DeviceAccountCredential DeviceAccount { get; set; }

        public DownloadModel(IDeviceAccountRepository deviceAccountRepository,
            IWebHostEnvironment webHostEnvironment,
            ILogger<DownloadModel> logger)
        {
            this.deviceAccountRepository = deviceAccountRepository;
            this.webHostEnvironment = webHostEnvironment;
            this._logger = logger;
        }

        public IActionResult OnPost()
        {
            string fullPathForZipFile = GenerateZipFileForAccount(false);
            if (!string.IsNullOrEmpty(fullPathForZipFile))
            {
                string zipfilename = Path.GetFileName(fullPathForZipFile);
                string virtualPathForZip = fullPathForZipFile.Replace(webHostEnvironment.WebRootPath, "");
                _logger.LogInformation("Single File downloaded from system for account id"+DeviceAccount.Id);
                return File(virtualPathForZip, "application/zip",
                  zipfilename);
            }
            else
            {
                ErrorMessage = "File could not be downloaded at this moement";
                return Page();
            }
        }


        public IActionResult OnPostDownloadAllFiles()
        {
            string fullPathForZipFile = GenerateZipFileForAccount(true);
            if (!string.IsNullOrEmpty(fullPathForZipFile))
            {
                string zipfilename = Path.GetFileName(fullPathForZipFile);
                string virtualPathForZip = fullPathForZipFile.Replace(webHostEnvironment.WebRootPath, "");
                _logger.LogInformation("All File downloaded from system for account id" + DeviceAccount.Id);
                return File(virtualPathForZip, "application/zip",
                  zipfilename);
            }
            else
            {
                ErrorMessage = "File could not be downloaded at this moement";
                return Page();
            }

        }

        public IActionResult OnGet(int id)
        {
            
            DeviceAccount = deviceAccountRepository.GetAccountCredential(id);

            if (DeviceAccount != null)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("/NotFound");
            }

        }

        private string GenerateZipFileForAccount(bool includeAllfiles = false)
        {
            string zipfileName = "";
            try
            {
                List<DeviceAccountCredential> listOfAccToDwnload = new List<DeviceAccountCredential>();
                DeviceAccountCredential cndAccount = deviceAccountRepository.GetAccountCredential(DeviceAccount.Id);
                string uniqueFileName = "";
                if (includeAllfiles)
                {
                    var deviceaccounts = deviceAccountRepository.GetAccountCredentialByAccountName(cndAccount.AccountName);
                    listOfAccToDwnload = deviceaccounts != null ? deviceaccounts.ToList() : listOfAccToDwnload;
                    uniqueFileName = DeviceAccount.AccountName + "_all.txt";
                }
                else
                {
                    listOfAccToDwnload.Add(cndAccount);
                    uniqueFileName = cndAccount.AccountName + "_" + cndAccount.AccountReferenceNumber.Replace(":", "").Replace("-", "") + ".txt";
                }

                string directoryName = cndAccount.AccountName + DateTime.Now.ToString().Replace(":", "").Replace("/", "");
                string downloadFolder = Path.Combine(webHostEnvironment.WebRootPath, "downloads", directoryName);
                if (!Directory.Exists(downloadFolder))
                {
                    Directory.CreateDirectory(downloadFolder);
                }
                string filePath = Path.Combine(downloadFolder, uniqueFileName);

                List<string> alllines = new List<string>();
                alllines.Add("AccountName, AccountType, ReferenceNo, UserName, Password");//header

                foreach (var eachAccount in listOfAccToDwnload)
                {
                    alllines.Add(eachAccount.AccountName + ", " + eachAccount.AccountFor.ToString() + ", " + eachAccount.AccountReferenceNumber + ", " + eachAccount.Username + ", " + eachAccount.Password);
                }
                // WriteAllLines creates a file, writes a collection of strings to the file,
                // and then closes the file.  You do NOT need to call Flush() or Close().
                System.IO.File.WriteAllLines(filePath, alllines.ToArray());

                string zipfilepath = Path.Combine(webHostEnvironment.WebRootPath, "downloads", directoryName + ".zip");
                // System.IO.Compression.ZipFile.CreateFromDirectory(downloadFolder, zipfilepath);

                //create zip
                // Zip up the files - From SharpZipLib Demo Code
                using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(zipfilepath)))
                {
                    s.SetLevel(9); // 0-9, 9 being the highest compression
                    s.Password = "admin";
                    byte[] buffer = new byte[4096];


                    ZipEntry entry = new ZipEntry(Path.GetFileName(filePath));

                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(filePath))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0,
                            buffer.Length);

                            s.Write(buffer, 0, sourceBytes);

                        } while (sourceBytes > 0);
                    }

                    s.Finish();
                    s.Close();
                }
                zipfileName = zipfilepath;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error while generating zip file for account id" + DeviceAccount.Id+"Error information :"+ex.ToString());
            }

            return zipfileName;
        }

    }
}
