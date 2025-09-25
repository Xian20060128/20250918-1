using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250918_1
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var targetTextBox = sender as TextBox;
            var targetStaclkPanel = targetTextBox.Parent as  StackPanel;
            var targetNameLabel = targetStaclkPanel.Children[0] as Label;
            var targetPriceLabel= targetStaclkPanel.Children[1] as Label;

            int amount;
            bool succes = int.TryParse(targetTextBox.Text, out amount);
            if (!succes) MessageBox.Show("請輸入正確的數值", "輸入錯誤");
            else
            {
                string drankname=targetNameLabel.Content.ToString();
                int price=Convert.ToInt32(targetPriceLabel.Content.ToString().Substring(0,2));
                MessageBox.Show($"您輸入的飲料是{drankname}，單價是{price}，元總共是{amount}杯，"+$"總價{price*amount}元","輸入成功");
            }
        }
    }
}
