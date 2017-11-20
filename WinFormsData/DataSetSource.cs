using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Data;
using System.Data;
using System.Data.SqlClient;
using WinFormsData.Properties;

namespace WinFormsData
{
    public class DataSetSource : ISource
    {
        private DataSet _dataSet;
        private int _nextId = int.MaxValue;
        SqlDataAdapter _productsAdatapter;

        public DataSetSource()
        {
            _dataSet = new DataSet();
            SqlConnection cn = new SqlConnection(Settings.Default.NorthwindConnection);
            SqlDataAdapter cda = new SqlDataAdapter("Select CategoryId, CategoryName from Categories", cn);
            cda.Fill(_dataSet, "Categories");

            _productsAdatapter = new SqlDataAdapter("Select * From Products", cn);
            //I need to set insert, update and delete commands,
            //instead of writing it I can use sql command bulder 
            //that will set them in adapter based on select command
            var builder = new SqlCommandBuilder(_productsAdatapter);
            _productsAdatapter.Fill(_dataSet, "Products");
        }
        //for datatable we don't need to add to bindingSource
        //because there is proper notifications
        public void AddProduct(BindingSource bindingSource, Product product)
        {
            DataTable table = _dataSet.Tables["Products"];
            _nextId -= 1;
            DataRow row = table.NewRow(); 
            //for rows in memory we use temporary ID (we need this for delete)
            //when we push to database it will be replaced with
            //acutal increment value
            row["ProductId"] = _nextId;
            row["ProductName"] = product.ProductName;
            row["SupplierID"] = 1;
            row["CategoryID"] = product.CategoryId;
            row["QuantityPerUnit"] = product.QuantityPerUnit;
            row["UnitPrice"] = product.UnitPrice;
            row["UnitsInStock"] = product.UnitsInStock;
            row["UnitsOnOrder"] = product.UnitsOnOrder;
            row["ReorderLevel"] = 0;
            row["Discontinued"] = product.Discontinued;

            table.Rows.Add(row);
        }

        public void DeleteProduct(BindingSource bindingSource, int productId)
        {
            DataTable table = _dataSet.Tables["Products"];

            var query = from p in table.AsEnumerable()
                        where p.RowState != DataRowState.Deleted
                        && p.Field<int>("ProductID") == productId
                        select p;
            DataRow row = query.Single();

            row.Delete();
        }

        public object GetCategories()
        {
            DataTable table = _dataSet.Tables["Categories"];

            return table;
        }

        public object GetProducts(int categoryId)
        {
            DataTable table = _dataSet.Tables["Products"];
            table.DefaultView.RowFilter = "CategoryID = " + categoryId;
            return table;
        }

        public void Save()
        {
            SqlConnection cn = null;

            try
            {
                cn = new SqlConnection(Properties.Settings.Default.NorthwindConnection);
                cn.Open();
                _productsAdatapter.Update(_dataSet, "Products");
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                if (cn != null)
                {
                    cn.Dispose();
                }
            }
        }
    }
}
