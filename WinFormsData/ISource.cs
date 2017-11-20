using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsData
{
    public interface ISource
    {
        object GetCategories();
        object GetProducts(int categoryId);
        /// <returns>Returns true if rebind is needed. For implementation with DataSet this will return false because
        /// rebind is done automatically and in other cases we needed to notify that rebind is needed</returns>
        bool DeleteProduct(int productId);

        /// <returns>Returns true if rebind is needed. For implementation with DataSet this will return false because
        /// rebind is done automatically and in other cases we needed to notify that rebind is needed</returns>
        bool AddProduct(Product product);
        void Save();
    }
}
