using System;
using System.Drawing;
using System.Windows.Forms;

namespace FloorTrace
{
    public class FloorTraceForm : Form
    {
        private PictureBox pictureBox;
        private Button loadImageButton;
        private Button setHorizontalScaleButton;
        private Button setVerticalScaleButton;
        private Button traceButton;
        private Point horizontalScalePoint1;
        private Point horizontalScalePoint2;
        private Point verticalScalePoint1;
        private Point verticalScalePoint2;
        private float horizontalScale;
        private float verticalScale;

        private MouseEventHandler horizontalHandler;
        private MouseEventHandler verticalHandler;


        public FloorTraceForm()
        {
            // Set up the form
            Text = "FloorTrace";
            Size = new Size(800, 600);

            // Create a FlowLayoutPanel to hold the buttons at the top-left corner
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Top,
                AutoSize = true,
            };

            // Create a PictureBox to display the image
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Create a "Load Image" button
            loadImageButton = new Button
            {
                Text = "Load Image"
            };
            loadImageButton.Click += LoadImageButton_Click;

            // Create a "Set Horizontal Scale" button
            setHorizontalScaleButton = new Button
            {
                Text = "Set Horizontal Scale"
            };
            setHorizontalScaleButton.Click += SetHorizontalScaleButton_Click;
            setHorizontalScaleButton.Width = 150;

            // Create a "Set Vertical Scale" button
            setVerticalScaleButton = new Button
            {
                Text = "Set Vertical Scale"
            };
            setVerticalScaleButton.Click += SetVerticalScaleButton_Click;
            setVerticalScaleButton.Width = 150;

            // Create a "Trace" button
            traceButton = new Button
            {
                Text = "Trace"
            };
            traceButton.Click += TraceButton_Click;
            traceButton.Width = 100;

            // Add the buttons to the FlowLayoutPanel
            buttonPanel.Controls.Add(loadImageButton);
            buttonPanel.Controls.Add(setHorizontalScaleButton);
            buttonPanel.Controls.Add(setVerticalScaleButton);
            buttonPanel.Controls.Add(traceButton);

            // Add the FlowLayoutPanel, PictureBox, and buttons to the form
            Controls.Add(buttonPanel);
            Controls.Add(pictureBox);

            // Initialize scale points
            horizontalScalePoint1 = Point.Empty;
            horizontalScalePoint2 = Point.Empty;
            verticalScalePoint1 = Point.Empty;
            verticalScalePoint2 = Point.Empty;

            // Show the form
            Show();
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog to let the user select an image
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Load the selected image and display it
                    Image image = Image.FromFile(openFileDialog.FileName);
                    pictureBox.Image = image;
                }
            }
        }

        private void SetHorizontalScaleButton_Click(object sender, EventArgs e)
        {
            // Remove any previous handler
            pictureBox.MouseDown -= verticalHandler;

            // Set new handler
            horizontalHandler = (s, args) => {
                if (horizontalScalePoint1 == Point.Empty)
                {
                    horizontalScalePoint1 = args.Location;
                }
                else
                {
                    horizontalScalePoint2 = args.Location;

                    // Prompt user for distance
                    using (Form inputForm = new Form())
                    {
                        TextBox textBox = new TextBox();
                        textBox.Left = 10;
                        textBox.Top = 10;
                        Button button = new Button();
                        button.Text = "OK";
                        button.Left = 10;
                        button.Top = 40;
                        button.Click += (se, ev) => {
                            float distance = float.Parse(textBox.Text);
                            horizontalScale = Distance(horizontalScalePoint1, horizontalScalePoint2) / distance;
                            inputForm.Close();
                        };

                        inputForm.Controls.Add(textBox);
                        inputForm.Controls.Add(button);
                        inputForm.ShowDialog();
                    }

                    // Reset points
                    horizontalScalePoint1 = Point.Empty;
                    horizontalScalePoint2 = Point.Empty;

                    // Reset handler
                    pictureBox.MouseDown -= horizontalHandler;
                    horizontalHandler = null;
                }
            };

            // Register handler
            pictureBox.MouseDown += horizontalHandler;
        }

        private float Distance(Point p1, Point p2)
        {
            return MathF.Sqrt((p1.X - p2.X) * (p1.X - p2.X) +
                              (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private void SetVerticalScaleButton_Click(object sender, EventArgs e)
        {
            // Remove any previous handler
            pictureBox.MouseDown -= horizontalHandler;

            // Set new handler
            verticalHandler = (s, args) => {
                if (verticalScalePoint1 == Point.Empty)
                {
                    verticalScalePoint1 = args.Location;
                }
                else
                {
                    verticalScalePoint2 = args.Location;

                    // Prompt user for distance
                    using (Form inputForm = new Form())
                    {
                        TextBox textBox = new TextBox();
                        textBox.Left = 10;
                        textBox.Top = 10;
                        Button button = new Button();
                        button.Text = "OK";
                        button.Left = 10;
                        button.Top = 40;
                        button.Click += (se, ev) => {
                            float distance = float.Parse(textBox.Text);
                            verticalScale = Distance(verticalScalePoint1, verticalScalePoint2) / distance;
                            inputForm.Close();
                        };

                        inputForm.Controls.Add(textBox);
                        inputForm.Controls.Add(button);
                        inputForm.ShowDialog();
                    }

                    // Reset points
                    verticalScalePoint1 = Point.Empty;
                    verticalScalePoint2 = Point.Empty;

                    // Reset handler  
                    pictureBox.MouseDown -= verticalHandler;
                    verticalHandler = null;
                }
            };

            // Register handler
            pictureBox.MouseDown += verticalHandler;
        }

        private void TraceButton_Click(object sender, EventArgs e)
        {
           //add later
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.V))
            {
                // Check if the clipboard contains an image
                if (Clipboard.ContainsImage())
                {
                    // Get the image from the clipboard and display it
                    Image image = Clipboard.GetImage();
                    pictureBox.Image = image;
                }
                return true; // Handle Ctrl+V
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FloorTraceForm());
        }
    }
}
