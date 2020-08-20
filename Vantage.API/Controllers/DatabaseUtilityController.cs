using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vantage.Common.Models;
using Vantage.Data;

namespace Vantage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseUtilityController : ControllerBase
    {
        private const string AttachedDBFileNameParameter = "AttachDBFilename=";
        private const string InitialCatalogParameter = "Initial Catalog=";
        private readonly ApplicationDbContext _context;

        public DatabaseUtilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("BackUpDatabase/{filePath}")]
        public async Task<ActionResult<ApiResponse>> BackUpDatabase(string filePath)
        {
            try
            {
                string databaseName = GetDatabaseName();
                string backUpQuery = BackUpCommand(databaseName, filePath);
                var result = await _context.Database.ExecuteSqlRawAsync(backUpQuery);

                ApiResponse res = new ApiResponse { Result = true, Message = "Database backup successfully", };
                return res;
            }
            catch (Exception ex)
            {
                ApiResponse res = new ApiResponse { Result = false, Message = "Some error occurred while taking database backup, Please try again.", };
                return res;
            }
        }

        [HttpGet("RestoreDatabase/{filePath}")]
        public async Task<ActionResult<ApiResponse>> RestoreDatabase(string filePath)
        {
            try
            {
                string databaseName = GetDatabaseName();
                string restoreQuery = RestoreCommand(databaseName, filePath);
                var result = await _context.Database.ExecuteSqlRawAsync(restoreQuery);
                ApiResponse res = new ApiResponse { Result = true, Message = "Database Restored successfully", };
                return res;
            }
            catch (Exception ex)
            {
                ApiResponse res = new ApiResponse { Result = false, Message = "Some error occurred while restoring database, Please try again.", };
                return res;
            }
        }

        private string RestoreCommand(string databaseName, string fileAddress)
        {
            string command = @"use [master]
ALTER DATABASE  [" + databaseName + @"] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
RESTORE DATABASE [" + databaseName + @"] from DISK = N'" + fileAddress + @"' WITH REPLACE
ALTER DATABASE  [" + databaseName + @"] SET MULTI_USER WITH ROLLBACK IMMEDIATE";

            //string command = @"use [master]
            //            ALTER DATABASE  [" + databaseName + @"] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
            //            RESTORE DATABASE [" + databaseName + @"] FROM  DISK = N'" + fileAddress + "' WITH REPLACE, FILE = 3,  NOUNLOAD,  STATS = 5";

            return command;
        }

        private string BackUpCommand(string databaseName, string fileAddress)
        {
            string command = @"BACKUP DATABASE [" + databaseName + @"]
                           TO DISK = '" + fileAddress + "' WITH FORMAT";
            return command;
        }

        private string GetDatabaseName()
        {
            string connectionString = _context.Database.GetDbConnection().ConnectionString;
            string[] connectionParameter = connectionString.Split(";", System.StringSplitOptions.RemoveEmptyEntries);

            if (connectionParameter.Any(x => x.StartsWith(AttachedDBFileNameParameter)))
            {
                string attachedDbFile = connectionParameter.FirstOrDefault(x => x.StartsWith(AttachedDBFileNameParameter));
                return attachedDbFile.Replace(AttachedDBFileNameParameter, string.Empty);
            }
            else if (connectionParameter.Any(x => x.StartsWith(InitialCatalogParameter)))
            {
                string initialCatalogName = connectionParameter.FirstOrDefault(x => x.StartsWith(InitialCatalogParameter));
                return initialCatalogName.Replace(InitialCatalogParameter, string.Empty);
            }

            return string.Empty;
        }
    }
}