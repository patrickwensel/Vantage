using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Vantage.Common.Models;

namespace Vantage.WPF.Interfaces
{
    public interface IProductService
    {
        Task<ObservableCollection<Product>> GetAllProducts();
    }
}
