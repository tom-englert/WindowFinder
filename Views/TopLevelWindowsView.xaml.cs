using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WindowFinder
{
    /// <summary>
    /// Interaction logic for TopLevelWindowsView.xaml
    /// </summary>
    public partial class TopLevelWindowsView : UserControl
    {
        public TopLevelWindowsView()
        {
            InitializeComponent();
            Application.Current.MainWindow.Activated += MainWindow_Activated;
        }

        /// <summary>
        /// The associated view model.
        /// </summary>
        private TopLevelWindowsViewModel ViewModel
        {
            get
            {
                return DataContext as TopLevelWindowsViewModel;
            }
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            // Refresh the view every time  the main window is activated - other windows positions might have changed.
            ViewModel.Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Provide a keyboard shortcut to manually refresh the view.
            if (e.Key == Key.F5)
            {
                ViewModel.Refresh();
            }
        }

        #region Simple drag handler

        private Point dragStartMousePos;
        private Rect dragWindowRect;

        private void caption_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var caption = (FrameworkElement)sender;
            var window = (WindowItemViewModel)caption.DataContext;

            if (window.Handle == this.GetHandle())
            {
                // Moving our own window would cause strange flickering, don't allow this.
                return;
            }

            // Capture the mouse and connect to the elements mouse move & button up events.
            caption.CaptureMouse();
            caption.MouseLeftButtonUp += caption_MouseLeftButtonUp;
            caption.MouseMove += caption_MouseMove;

            // Remember the current mouse position and window rectangle.
            this.dragStartMousePos = e.GetPosition(caption);
            this.dragWindowRect = window.Rect;
        }

        private void caption_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var caption = (FrameworkElement)sender;

            // Stop dragging - release capture, disconnect events and refresh the view.
            caption.ReleaseMouseCapture();
            caption.MouseLeftButtonUp -= caption_MouseLeftButtonUp;
            caption.MouseMove -= caption_MouseMove;

            ViewModel.Refresh();
        }

        private void caption_MouseMove(object sender, MouseEventArgs e)
        {
            var caption = (FrameworkElement)sender;

            // Move the dragged window:
            var mousePos = e.GetPosition(caption);
            var delta = mousePos - this.dragStartMousePos;
            dragWindowRect.Offset(delta);

            // Apply changes to view model.
            var window = (WindowItemViewModel)caption.DataContext;
            window.Rect = dragWindowRect;
        }

        #endregion
    }
}
