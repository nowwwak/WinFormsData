using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsData;

namespace WinFormsData
{
    public partial class Form1 : Form
    {
        private ISource _currentSource;
        private ObjectSource _objectSource = new ObjectSource();
        private DataSetSource _dataSetSource = new DataSetSource();
        private BindingSource _categoriesBindingSource = new BindingSource();
        private BindingSource _productsBindingSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SourceToolStripComboBox.SelectedIndex = 0;
            SetSource();
            BindCategories();            
            
            CategoryToolStripComboBox.ComboBox.DisplayMember = "CategoryName";
            CategoryToolStripComboBox.ComboBox.ValueMember = "CategoryID";
            //!!!! Warning if you put this before setting .ValueMember than .ValueMember will not work properly and will return whole object and not just property                                  
            CategoryToolStripComboBox.ComboBox.DataSource = _categoriesBindingSource;


            ProductsDataGridView.DataSource = _productsBindingSource;            
            ProductsListBox.DisplayMember = "ProductName";
            ProductsListBox.ValueMember = "ProductID";
            ProductsListBox.DataSource = _productsBindingSource;
            
            NameTextBox.DataBindings.Add("Text", _productsBindingSource, "ProductName");
            QuantityTextBox.DataBindings.Add("Text", _productsBindingSource, "QuantityPerUnit");
            PriceTextBox.DataBindings.Add("Text", _productsBindingSource, "UnitPrice");
            StockTextBox.DataBindings.Add("Text", _productsBindingSource, "UnitsInStock");
            OrderTextBox.DataBindings.Add("Text", _productsBindingSource, "UnitsOnOrder");
            DiscontinuedCheckBox.DataBindings.Add("Checked", _productsBindingSource, "Discontinued");
        }

        private void BindCategories()
        {
            _categoriesBindingSource.DataSource = _currentSource.GetCategories();
        }

        private void BindProducts()
        {
            var catId = Convert.ToInt32(CategoryToolStripComboBox.ComboBox.SelectedValue);
            _productsBindingSource.DataSource = _currentSource.GetProducts(catId);
        }

        private void SetSource()
        {
            switch (SourceToolStripComboBox.SelectedIndex)
            {
                case 0:
                    if (_objectSource == null)
                    {
                        _objectSource = new ObjectSource();
                    }
                    _currentSource = _objectSource;
                    break;
                case 1:
                    if (_dataSetSource == null)
                    {
                        _dataSetSource = new DataSetSource();
                    }
                    _currentSource = _dataSetSource;
                    break;
            }
        }

        private void CategoryToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProducts();
            
        }

        private void ProductsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_currentSource == _objectSource)
            {
                //Product selectedProduct = (Product)ProductsListBox.SelectedValue;
                //if (selectedProduct != null)
                //{
                //    SingleNameTextBox.DataBindings.Clear();
                //    SingleNameTextBox.DataBindings.Add("Text", selectedProduct, "ProductName");
                //    SingleQuantityTextBox.DataBindings.Clear();
                //    SingleQuantityTextBox.DataBindings.Add("Text", selectedProduct, "QuantityPerUnit");
                //}
            }
        }

        private void AddToolStripButton_Click(object sender, EventArgs e)
        {
            int id =(int) CategoryToolStripComboBox.ComboBox.SelectedValue;
            var name = CategoryToolStripComboBox.Text;
            Category category = new Category(id, name);

            AddProductForm form = new AddProductForm(category);
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.OK)
                _currentSource.AddProduct(_productsBindingSource, form.Product);
        }

        private void DeleteToolStripButton_Click(object sender, EventArgs e)
        {
            int productId = (int) ProductsListBox.SelectedValue;
            _currentSource.DeleteProduct(_productsBindingSource, productId);        }

        private void BackToolStripButton_Click(object sender, EventArgs e)
        {
            _productsBindingSource.MovePrevious();
        }

        private void ForwardToolStripButton_Click(object sender, EventArgs e)
        {
            _productsBindingSource.MoveNext();
        }

        private void SourceToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSource();
            BindCategories();
            BindProducts();
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            _currentSource.Save();
        }
    }
}
