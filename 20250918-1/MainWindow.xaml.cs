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
        //Dictionary<string, int> drinks = new Dictionary<string, int>();
        Dictionary<string, int> drinks = new Dictionary<string, int>()
        {
            {"綠茶大杯", 50 },
            {"紅茶大杯", 55},
            {"清茶大杯", 45 },
            {"綠茶小杯", 30 },
            {"紅茶小杯", 35 }
        };

        Dictionary<string, int> orders = new Dictionary<string, int>();
        string resultMessage = "";
        string typeMessage = "";
        public MainWindow()
        {
            InitializeComponent();
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
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            typeMessage = rb.Content.ToString();
        }

      
       
    }
}
