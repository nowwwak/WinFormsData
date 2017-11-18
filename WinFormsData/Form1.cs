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

namespace WinFormsData
{
    public partial class Form1 : Form
    {
        private ObjectSource _source = new ObjectSource();
        private BindingSource _categoriesBindingSource = new BindingSource();
        private BindingSource _productsBindingSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _categoriesBindingSource.DataSource = _source.GetCategories();
            
            CategoryToolStripComboBox.ComboBox.DisplayMember = "CategoryName";
            CategoryToolStripComboBox.ComboBox.ValueMember = "CategoryID";
            //!!!! Warning if you put this before setting .ValueMember than .ValueMember will not work properly and will return whole object and not just property                                  
            CategoryToolStripComboBox.ComboBox.DataSource = _categoriesBindingSource;


            ProductsListBox.DataSource = _productsBindingSource;
            ProductsListBox.DisplayMember = "ProductName";

            ProductsDataGridView.DataSource = _productsBindingSource;
            ProductsListBox.DataSource = _productsBindingSource;
            ProductsListBox.DisplayMember = "ProductName";

            
            NameTextBox.DataBindings.Add("Text", _productsBindingSource, "ProductName");
            QuantityTextBox.DataBindings.Add("Text", _productsBindingSource, "QuantityPerUnit");
            PriceTextBox.DataBindings.Add("Text", _productsBindingSource, "UnitPrice");
            StockTextBox.DataBindings.Add("Text", _productsBindingSource, "UnitsInStock");
            OrderTextBox.DataBindings.Add("Text", _productsBindingSource, "UnitsOnOrder");
            DiscontinuedCheckBox.DataBindings.Add("Checked", _productsBindingSource, "Discontinued");
        }        

        private void CategoryToolStripComboBox_Click(object sender, EventArgs e)
        {

        }

        private void CategoryToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int catId = (int)CategoryToolStripComboBox.ComboBox.SelectedValue;
            _productsBindingSource.DataSource = _source.GetProducts(catId);
            
        }

        private void ProductsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Product selectedProduct =(Product) ProductsListBox.SelectedValue;
            if (selectedProduct != null)
            {
                SingleNameTextBox.DataBindings.Clear();
                SingleNameTextBox.DataBindings.Add("Text", selectedProduct, "ProductName");
                SingleQuantityTextBox.DataBindings.Clear();
                SingleQuantityTextBox.DataBindings.Add("Text", selectedProduct, "QuantityPerUnit");
            }
        }

        private void AddToolStripButton_Click(object sender, EventArgs e)
        {
            Category category = (Category) CategoryToolStripComboBox.SelectedItem;

            AddProductForm form = new AddProductForm(category);
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.OK)
                //_source.AddProduct(form.Product);
                _productsBindingSource.Add(form.Product);
        }

        private void DeleteToolStripButton_Click(object sender, EventArgs e)
        {
            Product product = (Product)ProductsListBox.SelectedItem;

            //_source.DeleteProduct(product);
            _productsBindingSource.Remove(product);
        }

        private void BackToolStripButton_Click(object sender, EventArgs e)
        {
            _productsBindingSource.MovePrevious();
        }

        private void ForwardToolStripButton_Click(object sender, EventArgs e)
        {
            _productsBindingSource.MoveNext();
        }
    }
}
