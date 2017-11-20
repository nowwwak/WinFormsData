using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Data;

namespace WinFormsData
{
    public class EfSource : ISource
    {

        public object GetCategories()
        {
            using (NORTHWNDEntities context = new NORTHWNDEntities())
            {
                return context.Categories.ToList();
            }
        }

        public object GetProducts(int categoryId)
        {
            using (NORTHWNDEntities context = new NORTHWNDEntities())
            {
                return context.Products.Where(p => p.CategoryID == categoryId).ToList();
            }
        }

        public bool AddProduct(Product product)
        {
            using (NORTHWNDEntities context = new NORTHWNDEntities())
            {
                ProductsEF efEntity = new ProductsEF();

                //efEntity."ProductId"] = _nextId;
                efEntity.ProductName = product.ProductName;
                efEntity.SupplierID = 1;
                efEntity.CategoryID = product.CategoryId;
                efEntity.QuantityPerUnit = product.QuantityPerUnit;
                efEntity.UnitPrice = product.UnitPrice;
                efEntity.UnitsInStock = (short)product.UnitsInStock;
                efEntity.UnitsOnOrder = (short)product.UnitsOnOrder;
                efEntity.ReorderLevel = 0;
                efEntity.Discontinued = product.Discontinued;

                context.Products.Add(efEntity);
                Save(context);
            }

            return true;
        }

        public bool DeleteProduct(int productId)
        {
            using (NORTHWNDEntities context = new NORTHWNDEntities())
            {
                ProductsEF product = context.Products.FirstOrDefault(p => p.ProductID == productId);
                if (product != null)
                {
                    context.Products.Remove(product);
                    Save(context);
                }
            }

            return true;
        }
        public void Save(NORTHWNDEntities  context)
        {
            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void Save()
        {
            using (NORTHWNDEntities context = new NORTHWNDEntities())
            {
                Save(context);

            }
        }
    }
}

