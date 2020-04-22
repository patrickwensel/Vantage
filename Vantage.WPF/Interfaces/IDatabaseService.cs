using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vantage.Common.Models;

namespace Vantage.WPF.Interfaces
{
    public interface IDatabaseService
    {
        Task<ApiResponse> BackupDatabase(string filePath);

        Task<ApiResponse> RestoreDatabase(string filePath);
    }
}
