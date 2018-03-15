using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace RocketPanel
{
    /// <summary>
    /// Interaction logic for DotBtn.xaml
    /// </summary>
    public partial class DotBtn : Window
    {
        public DotBtn()
        {
            InitializeComponent();
            Image imageHolder = image;
            image.MouseUp += Image_MouseUp;
            this.MouseDown += Window_MouseDown;
            this.MouseDoubleClick += DotBtn_MouseDoubleClick;
            PlaceLowerRight();
            image.Opacity = 90;

        }

        private void DotBtn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouseState = System.Windows.Forms.Control.MousePosition;
            if (e.ChangedButton == MouseButton.Left)
            {
                System.Threading.Thread.Sleep(800);
                var currentMouseState = System.Windows.Forms.Control.MousePosition;
                
                if (e.ButtonState == MouseButtonState.Pressed && mouseState.X + mouseState.Y != currentMouseState.X+currentMouseState.Y)
                {
                    this.DragMove();
                }
                
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            System.Windows.Application.Current.MainWindow.Show();
        }
        private void PlaceLowerRight()
        {
            //Determine "rightmost" screen
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            this.Left = rightmost.WorkingArea.Right - this.Width;
            this.Top = rightmost.WorkingArea.Bottom - this.Height;
        }
    }
}
