﻿using Data;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CategoryToolStripComboBox.ComboBox.DisplayMember = "CategoryName";
            CategoryToolStripComboBox.ComboBox.ValueMember = "CategoryID";
            //!!!! Warning if you put this before setting .ValueMember than .ValueMember will not work properly and will return whole object and not just property                                  
            CategoryToolStripComboBox.ComboBox.DataSource = _source.GetCategories();
        }        

        private void CategoryToolStripComboBox_Click(object sender, EventArgs e)
        {

        }

        private void CategoryToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int catId = (int)CategoryToolStripComboBox.ComboBox.SelectedValue;
            IList<Product> products = _source.GetProducts(catId);
            ProductsListBox.DataSource = products;
            ProductsListBox.DisplayMember = "ProductName";

            ProductsDataGridView.DataSource = products;
            ProductsListBox.DataSource = products;
            ProductsListBox.DisplayMember = "ProductName";

            NameTextBox.DataBindings.Clear();
            NameTextBox.DataBindings.Add("Text", products, "ProductName");
            QuantityTextBox.DataBindings.Clear();
            QuantityTextBox.DataBindings.Add("Text", products, "QuantityPerUnit");
            PriceTextBox.DataBindings.Clear();
            PriceTextBox.DataBindings.Add("Text", products, "UnitPrice");
            StockTextBox.DataBindings.Clear();
            StockTextBox.DataBindings.Add("Text", products, "UnitsInStock");
            OrderTextBox.DataBindings.Clear();
            OrderTextBox.DataBindings.Add("Text", products, "UnitsOnOrder");
            DiscontinuedCheckBox.DataBindings.Clear();
            DiscontinuedCheckBox.DataBindings.Add("Checked", products, "Discontinued");
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
                _source.AddProduct(form.Product);
        }

        private void DeleteToolStripButton_Click(object sender, EventArgs e)
        {
            Product product = (Product)ProductsListBox.SelectedItem;

            _source.DeleteProduct(product);
        }
    }
}
