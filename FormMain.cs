using System;
using System.Drawing;
using System.Windows.Forms;

namespace KPFU_2_sem_programming_PaintPlusPlus {
    public partial class FormMain : Form {
        public const string title = "ПеинтПлюсПлюс";
        private const string fileUnnamed = "Безымянный";
        private string fileName = fileUnnamed;

        private bool fileIsSaved = true;
        private bool fileIsNew = true;

        private OpenFileDialog ofd = new OpenFileDialog();
        private SaveFileDialog sfd = new SaveFileDialog();

        public const string pathIconBlue = "Resources/csharp_blue.ico";
        public const string pathIconRed = "Resources/csharp_red.ico";

        private bool mouseDown = false;
        private Point mouseDownPoint = Point.Empty;
        private Point mousePoint = Point.Empty;

        private ArrayPonits arrayPoints = new ArrayPonits(2);

        private Bitmap map = new Bitmap(800, 500);
        private Graphics graphics;
        private Pen pen;

        public FormMain() {
            InitializeComponent();

            ofd.Filter = "";
            sfd.Filter = "";

            pen = createTool("Карандаш");

            setSize();
            resizeForm();
            changeIcon();
            changeTitle();
            updateStatusBar();
        }

        public void resizeForm() {

        }

        private void changeIcon() {
            if (fileIsSaved == true) {
                this.Icon = new Icon(pathIconBlue);
            } else {
                this.Icon = new Icon(pathIconRed);
            }
        }

        private void changeTitle() {
            string new_title = "";

            if (fileIsSaved == false) {
                new_title += "* ";
            }

            if (fileIsNew == true) {
                new_title += fileUnnamed;
            } else {
                new_title += fileName;
            }

            new_title += " - " + title;

            this.Text = new_title;
        }

        public void updateStatusBar() {

        }

        private Pen createTool(string what) {
            Pen p = new Pen(Color.White, 1);
            p.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;

            switch (what) {
                case "Карандаш":
                    p.Color = Color.Black;
                    p.Width = 1;
                    break;

                case "Маркер":
                    p.Color = Color.DarkCyan;
                    p.Width = 4;
                    break;

                case "Фломастер":
                    p.Color = Color.Gainsboro;
                    p.Width = 8;
                    break;

                case "Кисточка":
                    p.Color = Color.Yellow;
                    p.Width = 12;
                    break;

                case "Ластик":
                    p.Color = Color.White;
                    p.Width = 30;
                    break;
            }

            return p;
        }

        private void formMainPictureBox_MouseDown(object sender, MouseEventArgs e) {
            mouseDown = true;
            mousePoint = mouseDownPoint = e.Location;
        }

        private void formMainPictureBox_MouseUp(object sender, MouseEventArgs e) {
            mouseDown = false;
            arrayPoints.resetPoints();

            fileIsSaved = false;

            changeIcon();
            changeTitle();
        }

        private void formMainPictureBox_MouseMove(object sender, MouseEventArgs e) {
            mousePoint = e.Location;

            if (mouseDown == false) {
                return;
            }

            arrayPoints.setPoint(e.X, e.Y);

            if (arrayPoints.getCountPoints() >= 2) {
                graphics.DrawLines(pen, arrayPoints.getPoints());
                formMainPictureBox.Image = map;
                arrayPoints.setPoint(e.X, e.Y);
            }
        }

        private void setSize() {
            Rectangle rectangle = Screen.PrimaryScreen.Bounds;
            map = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(map);

            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void formMainGroupColorSetColor(object sender, EventArgs e) {
            pen.Color = ((Button) sender).BackColor;
        }

        private void formMainGroupColorChoose_Click(object sender, EventArgs e) {
            if (formMainColorDialog.ShowDialog() == DialogResult.OK) {
                pen.Color = formMainColorDialog.Color;
                ((Button) sender).BackColor = formMainColorDialog.Color;
            }
        }

        private void button20_Click(object sender, EventArgs e) {
            if (mouseDown == false) {
                return;
            }

            Region region = new Region(this.ClientRectangle);
            Rectangle rectangle = new Rectangle(
                Math.Min(mouseDownPoint.X, mousePoint.X),
                Math.Min(mouseDownPoint.Y, mousePoint.Y),
                Math.Abs(mouseDownPoint.X - mousePoint.X),
                Math.Abs(mouseDownPoint.Y - mousePoint.Y)
            );

            region.Xor(rectangle);
            graphics.FillRectangle(Brushes.White, rectangle);
        }

        private void formMainSelectTool(object sender, EventArgs e) {
            pen = createTool(((Button) sender).Tag.ToString());
        }

        private void newFile() {
            fileName = fileUnnamed;
            fileIsNew = true;
            fileIsSaved = true;

            changeIcon();
            changeTitle();
        }

        private void formMainFileCreate_Click(object sender, EventArgs e) {
            if (fileIsSaved == false) {
                if (showFormSave() == false) {
                    return;
                }
            }

            newFile();
        }

        private void formMainFileOpen_Click(object sender, EventArgs e) {
            if (fileIsSaved == false) {
                if (showFormSave() == false) {
                    return;
                }
            }

            newFile();

            if (ofd.ShowDialog() == DialogResult.OK) {
                fileName = ofd.FileName;

                if (fileName.EndsWith(".jpg")) {

                }

                fileIsNew = false;
                fileIsSaved = true;
            } else {
                return;
            }
        }

        private void formMainFileSave_Click(object sender, EventArgs e) {

        }

        private void formMainFileSaveAs_Click(object sender, EventArgs e) {

        }

        private void formMainFilePrint_Click(object sender, EventArgs e) {

        }

        private void formMainFileExit_Click(object sender, EventArgs e) {

        }

        private bool showFormSave() {
            FormSave formSave = new FormSave(fileName);
            DialogResult dialogResult = formSave.ShowDialog();

            return handlerFormSave(dialogResult);
        }

        private bool handlerFormSave(DialogResult dialogResult) {
            if (dialogResult == DialogResult.Yes) {
                if (fileIsNew == true) {
                    if (sfd.ShowDialog() == DialogResult.OK) {

                        return saveFile();
                    } else {
                        return false;
                    }
                } else {
                    return saveFile();
                }
            } else if (dialogResult == DialogResult.No) {
                this.Close();
                return false;
            } else if (dialogResult == DialogResult.Cancel) {
                return false;
            } else {
                return false;
            }
        }

        private bool saveFile() {
            fileName = sfd.FileName;
            fileIsSaved = true;
            changeTitle();
            changeIcon();

            return true;
        }
    }

    public class ArrayPonits {
        private int index = 0;
        private Point[] points;

        public ArrayPonits(int size) {
            if (size <= 0) {
                size = 2;
            }

            points = new Point[size];
        }

        public void setPoint(int x, int y) {
            if (index >= points.Length) {
                index = 0;
            }

            points[index] = new Point(x, y);
            ++index;
        }

        public void resetPoints() {
            index = 0;
        }

        public int getCountPoints() {
            return index;
        }

        public Point[] getPoints() {
            return points;
        }
    }
}
