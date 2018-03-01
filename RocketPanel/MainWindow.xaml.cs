using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SWF = System.Windows.Forms;

namespace RocketPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static Button currentbutton = null;
        private static WrapPanel btnpnl;
        private static Flyout fl1;
        private static TextBox btnID;
        private static TextBox btnName;
        private static ComboBox btnType;
        private static TextBox btnScript;


        public MainWindow()
        {
            InitializeComponent();
            toolkit.CreateTable();
            btnpnl = btnPnl1;
            fl1 = thisFlyEdit;
            btnID = curBtnIDbox;
            btnName = btnNameBox;
            btnType = btnTypeBox;
            btnScript = btnFileBox;
            toolkit.GetImportedButtonList();
            btnType.Items.Add("Application");
            btnType.Items.Add("File");
            btnType.Items.Add("Script");
        }


        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            int prevChildren = btnPnl1.Children.Count;
            while (btnPnl1.Children.Count == prevChildren)
            {
                generateAndAddBtn();
            }
        }



        public static void generateAndAddBtn(string btnID ="",string btnName= "" , string btnType = "", string btnScript = "")
        {

            try
            {
                Button btn;
                if (btnID == "")
                {
                    btn = new Button
                    {

                        ContextMenu = getBtnMnu(),
                        Content = "New Button",
                        Name = toolkit.RandomString(10),
                        Margin = new System.Windows.Thickness(5, 5, 0, 0),
                        
                    };
                    btn.Click += Btn_Click;
                }
                else
                {
                    btn = new Button
                    {
                        ContextMenu = getBtnMnu(),
                        Content = btnName,
                        Name = btnID,
                        Margin = new System.Windows.Thickness(5, 5, 0, 0)
                    };
                    btn.Click += Btn_Click;
                }
                

                btnpnl.Children.Add(btn);
                
            }
            catch (Exception)
            {

            }
        }

        private static void Btn_Click(object sender, RoutedEventArgs e)
        {
            string holder = toolkit.getScript(((Button) sender).Name);
            try
            {
                Process.Start(holder);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private static ContextMenu getBtnMnu()
        {
            ContextMenu mnu = new ContextMenu();
            mnu.Items.Add(getBtnMI("Delete Button", button_HandlerCT));
            mnu.Items.Add(getBtnMI("Rename Button", button_HandlerCT));
            mnu.Items.Add(getBtnMI("Program Button", button_HandlerCT));
            return mnu;
        }

        private static MenuItem getBtnMI(string header, RoutedEventHandler routedEventHandlerName)
        {
            MenuItem item1 = new MenuItem();
            item1.Header = header;
            item1.Click += new RoutedEventHandler(routedEventHandlerName);
            return item1;
        }


        private static void showFlyout(Button theButton)
        {
            fl1.IsOpen = true;
            currentbutton = theButton;
            btnID.Text = theButton.Name;
            btnName.Text = theButton.Content.ToString();
            btnScript.Text = toolkit.getScript(theButton.Name);
            btnType.Text = toolkit.getType(theButton.Name);

        }


        public static void button_HandlerCT(object sender, RoutedEventArgs e)
        {
            // Find name of Button that contained this item
            System.Windows.Controls.MenuItem menuItem = (System.Windows.Controls.MenuItem)sender;
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)contextMenu.PlacementTarget;
            string actionVerb = menuItem.Header.ToString().Replace(" ", "");
            string buttonName = button.Name;
            switch (actionVerb.ToUpper())
            {
                case "DELETEBUTTON":
                    toolkit.DeleteButton(button.Name);
                    btnpnl.Children.Remove(button);
                    saveAllBtns();
                    break;

                case "RENAMEBUTTON":
                    string getNewName = "Please enter a new name for the button:";
                    HelperFunctions.ShowInputDialog(ref getNewName);
                    if (getNewName != "Please enter a new name for the button:" && getNewName.Length > 0 )
                    {
                        button.Content = getNewName;
                        saveAllBtns();
                    }
                    break;

                case "PROGRAMBUTTON":
                    showFlyout(button);
                    break;
            }
        }

        private void thisFlyEdit_ClosingFinished(object sender, RoutedEventArgs e)
        {
            if (currentbutton != null)
            {
                currentbutton.Content = btnNameBox.Text;
                saveThisButton(currentbutton,btnType.Text,btnScript.Text);
            }
            currentbutton = null;
        }
        
        
        
        public static void saveThisButton(Button btn, string btntype = "", string btnscript = "")
        {
            toolkit.CreateTable();
            using (var sConn = new SQLiteConnection("Data Source=buttons.sqlite;Version=3;"))
            {
                sConn.Open();
                if (btnscript != "")
                {
                    using (SQLiteCommand mCmd = new SQLiteCommand(
                        "replace into buttons ('buttonid' ,'buttonname','buttontype','buttonscript') VALUES ('" +
                        btn.Name +
                        "' ,'" + btn.Content + "','" + btnType.Text + "','" + btnScript.Text + "');", sConn))
                    {
                        mCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SQLiteCommand mCmd = new SQLiteCommand(
                        "replace into buttons ('buttonid' ,'buttonname') VALUES ('" +
                        btn.Name +
                        "' ,'" + btn.Content + "');", sConn))
                    {
                        mCmd.ExecuteNonQuery();
                    }
                }

                sConn.Close();
            }
        }

        public static void saveAllBtns()
        {
            toolkit.CreateTable();
            foreach (Button btn in btnpnl.Children)
            {
               saveThisButton(btn);
            }
        }

        private void btnFileBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.  
            SWF.OpenFileDialog openFileDialog1 = new SWF.OpenFileDialog();
            openFileDialog1.Filter = "All Files|*.*";
            openFileDialog1.Title = "Select a File";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .CUR file was selected, open it.  
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                btnFileBox.Text = openFileDialog1.FileName;
            }
        }
    }
}