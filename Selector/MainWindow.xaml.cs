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

namespace Selector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitEvents();
            selectorControl = new SelectorControl();
        }

        #region Поля
        SelectorControl selectorControl;
        #endregion

        private void InitEvents()
        {
            //ListView
            lstvwChanal.SelectionChanged += LstvwChanal_SelectionChanged;

            //Grid
            mainGrid.MouseLeftButtonDown += MainGrid_MouseLeftButtonDown;

            //Button
            btnMinimize.Click += BtnMinimize_Click;
            btnClose.Click += BtnClose_Click;

            //TagleButton
            tbComPortConnection.Checked += tbComPortConnection_Checked; 
            tbComPortConnection.Unchecked += tbComPortConnection_Unchecked;

            //ComboBox
            cbComPorts.SelectionChanged += CbComPorts_SelectionChanged;
            cbComPorts.DropDownOpened += CbComPorts_DropDownOpened;

            //checkBox
            checkBoxShdnPower.Click += CheckBoxShdnPower_Click;
        }

        private void CheckBoxShdnPower_Click(object sender, RoutedEventArgs e)
        {
            if (selectorControl.IsConnected)
            {
                try
                {
                    selectorControl.ActivationSHDN((bool)checkBoxShdnPower.IsChecked);
                }
                catch (Exception)
                {
                    selectorControl.Disconnect();
                    tbComPortConnection.IsChecked = false;
                }

            }
        }

        private void LstvwChanal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectorControl.IsConnected)
            {
                try
                {
                    selectorControl.ActivationChannels(lstvwChanal.SelectedIndex);
                }
                catch (Exception)
                {
                    selectorControl.Disconnect();
                    tbComPortConnection.IsChecked = false;
                }
                
            }
        }

        private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CbComPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbComPorts.SelectedItem != null)
            {
                selectorControl.NameComPort = Convert.ToString(cbComPorts.SelectedItem); //присваиваем новое имя COMPORT
            }
        }

        private void CbComPorts_DropDownOpened(object sender, EventArgs e)
        {
            cbComPorts.Items.Clear();
            string[] ports = selectorControl.SearchComPorts();
            foreach (var item in ports)
            {
                cbComPorts.Items.Add(item);
            }
        }

        private void tbComPortConnection_Checked(object sender, RoutedEventArgs e)
        {
            
            try
            {
                Cursor = Cursors.Wait;
                bool isConnect = selectorControl.Connect();
                if (isConnect)
                {
                    SetCheckIcon(statusConnection);
                    cbComPorts.IsEnabled = false;
                    //tbComPortConnection.Content = "Закрыть";
                }
                else
                {
                    SetCheckIcon(statusConnection);
                    tbComPortConnection.IsChecked = false;
                    cbComPorts.IsEnabled = true;
                }
                Cursor = default;
            }
            catch (Exception)
            {
                SetCanselIcon(statusConnection);
                tbComPortConnection.IsChecked = false;
                cbComPorts.IsEnabled = true;
                Cursor = default;

            }
        }

        private void tbComPortConnection_Unchecked(object sender, RoutedEventArgs e)
        {

            try
            {
                Cursor = Cursors.Wait;
                SetCanselIcon(statusConnection);
                cbComPorts.IsEnabled = true;
                selectorControl.Disconnect();
                //tbComPortConnection.Content = "Открыть";
                Cursor = default;
            }
            catch (Exception)
            {
                SetCheckIcon(statusConnection);
                cbComPorts.IsEnabled = false;
                tbComPortConnection.IsChecked = true;
                Cursor = default;
            }
        }


        private void SetCanselIcon(GeometryGroup geometryG)
        {
            GeometryGroup geometryGroup = geometryG;
            geometryGroup.Children.Remove((Geometry)Application.Current.Resources["check"]);
            geometryGroup.Children.Add((Geometry)Application.Current.Resources["cancel"]);
            pathStatusConnection.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x2E, 0x00));
        }

        private void SetCheckIcon(GeometryGroup geometryG)
        {
            GeometryGroup geometryGroup = geometryG;
            geometryGroup.Children.Remove((Geometry)Application.Current.Resources["cancel"]);
            geometryGroup.Children.Add((Geometry)Application.Current.Resources["check"]);
            pathStatusConnection.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xFF, 0x0C));
        }



    }
}
