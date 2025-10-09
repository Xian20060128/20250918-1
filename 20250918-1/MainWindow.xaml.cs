using Microsoft.Win32;
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
using System.IO;

namespace _20250918_1
{
    public partial class MainWindow : Window
    {
        Dictionary<string, int> drinks = new Dictionary<string, int>();
       

        Dictionary<string, int> orders = new Dictionary<string, int>();
        string resultMessage = "";
        string typeMessage = "";
        public MainWindow()
        {
            InitializeComponent();
        
            AddDrinkItems(drinks);
            DisplyDrinkMenu(drinks);
        }

        private void DisplyDrinkMenu(Dictionary<string, int> drinks)
        {
            DrinkMenu_StackPanel.Children.Clear();

            foreach (var drink in drinks)
            {
                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(2),
                    Height = 35,
                    VerticalAlignment = VerticalAlignment.Center,
                    Background = Brushes.AliceBlue
                };

                CheckBox cb = new CheckBox
                {
                    Content = drink.Key,
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    Margin = new Thickness(10, 0, 20, 0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.DarkBlue,
                };

                Label lb_price = new Label
                {
                    Content = $"{drink.Value} 元",
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    Margin = new Thickness(10, 0, 20, 0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.DarkCyan
                };

                Slider sl = new Slider
                {
                    Width = 150,
                    Minimum = 0,
                    Maximum = 20,
                    Value = 0,
                    IsSnapToTickEnabled = true,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                Label lb_amount = new Label
                {
                    Content = "0",
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    Margin = new Thickness(10, 0, 20, 0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.DarkGreen
                };

                Binding binding = new Binding("Value")
                {
                    Source = sl,
                    Mode = BindingMode.OneWay
                };
                lb_amount.SetBinding(Label.ContentProperty, binding);

                sp.Children.Add(cb);
                sp.Children.Add(lb_price);
                sp.Children.Add(sl);
                sp.Children.Add(lb_amount);

                DrinkMenu_StackPanel.Children.Add(sp);
            }
        }

        private void AddDrinkItems(Dictionary<string, int> drinks)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "請選擇飲料品項檔案";
            openFileDialog.Filter = "CSV檔案(*.csv)|*.csv|所有檔案(*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                string[] lines = File.ReadAllLines(fileName);

                foreach (var line in lines)
                {
                    string[] tokens = line.Split(',');
                    string drinkName = tokens[0];
                    int price = int.Parse(tokens[1]);
                    drinks.Add(drinkName, price);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            orders.Clear();
            resultMessage = "";
            for (int i = 0; i < DrinkMenu_StackPanel.Children.Count; i++)
            {
                var sp = DrinkMenu_StackPanel.Children[i] as StackPanel;
                var cb = sp.Children[0] as CheckBox;
                var sl = sp.Children[2] as Slider;

                int quantity = (int)sl.Value;

                if (cb.IsChecked == true && quantity > 0)
                {
                    string drinkName = cb.Content.ToString();
                    int price = drinks[drinkName];
                    orders.Add(drinkName, quantity);
                }
            }

            double total = 0.0;
            double sellPrice = 0.0;
            int index = 1;
            string discountMessage = "沒有折扣";

            resultMessage += $"購買方式：{typeMessage}，訂購清單如下:\n";
            foreach (var item in orders)
            {
                string drinkName = item.Key;
                int quantity = item.Value;
                int price = drinks[drinkName];

                int subTotal = price * quantity;
                total += subTotal;
                resultMessage += $"{index}. {drinkName} ： {price}元 x {quantity}杯 = {subTotal}元\n";
                index++;
            }
            resultMessage += $"總計: {total}元\n";

            if (total >= 500)
            {
                discountMessage = "滿500元打8折";
                sellPrice = total * 0.8;
            }
            else if (total >= 300)
            {
                discountMessage = "滿300元打85折";
                sellPrice = total * 0.85;
            }
            else if (total >= 200)
            {
                discountMessage = "滿200元打9折";
                sellPrice = total * 0.9;
            }
            else
            {
                sellPrice = total;
            }
            resultMessage += $"折扣訊息：{discountMessage}，實付金額： {sellPrice}元。";

            Result_TextBlock.Text = resultMessage;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "儲存訂購明細";
            saveFileDialog.Filter = "文字檔案(*.txt)|*.txt|所有檔案(*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                File.WriteAllText(fileName, resultMessage);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            typeMessage = rb.Content.ToString();
        }
    }
}
