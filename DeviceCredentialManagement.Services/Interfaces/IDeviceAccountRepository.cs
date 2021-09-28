using DeviceCredentialManagement.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCredentialManagement.Services
{
    public interface IDeviceAccountRepository
    {
        IEnumerable<DeviceAccountCredential> GetAllAccountCredentials();
        DeviceAccountCredential GetAccountCredential(int id);
        DeviceAccountCredential UpdateDeviceAccount(DeviceAccountCredential candidAccount);
        DeviceAccountCredential AddDeviceAccount(DeviceAccountCredential candidAccount);
        DeviceAccountCredential DeleteDeviceAccount(int id);
        IEnumerable<DeviceAccountCredential> SearchAccountCredentials(string searchTerm);
        string GeneratePassword(string accountName, AccountType? accountFor, string accountReferenceNumber);
        IEnumerable<DeviceAccountCredential> GetAccountCredentialByAccountName(string accountName);

        int GetTotalAccountCredentials(string searchString);
        IEnumerable<DeviceAccountCredential> GetAccountCredentials(int pageIndex, int pageSize,string searchString);
    }
}
