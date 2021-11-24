# DeviceAccountManagement
This is a small application to create/update/manage/lookup credentials for different devices using Razor Pages.


It is a standard web application made with .Net core razor pages completed with Identity authentcation.
3 kind of roles are there: 
      administrator (can create user, assign roles along with permission for CRUD operations on the data)
      user (do not have access on user management, only CRUD operation permission for data)
      viewer (can view and download only)
The data is stored in a SQL server database. Done with EntityFramework
