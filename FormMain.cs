using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
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

            ofd.Filter = "Bitmap file|*.bmp|Any format|*.*";
            sfd.Filter = "Bitmap file|*.bmp|Any format|*.*";

            pen = createTool("Карандаш");

            setSize();
            resizeForm();
            changeIcon();
            changeTitle();
            updateStatusBar();
        }

        private void resizeForm() {
            formMainPictureBox.Width = this.Width - 30;
            formMainPictureBox.Height = this.Height - 120;

            updateStatusBar();
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
            formMainStatusBarSize.Text = string.Format("{0} x {1}", this.Width, this.Height);
            formMainStatusBar.Update();
            formMainStatusBar.Refresh();
        }

        private Pen createTool(string what) {
            Pen p = new Pen(Color.White, 1);
            p.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;

            switch (what) {
                case "Карандаш":
                p.Color = Color.Black;
                p.Width = 3;
                break;

                case "Маркер":
                p.Color = Color.DarkCyan;
                p.Width = 3;
                break;

                case "Фломастер":
                p.Color = Color.Gainsboro;
                p.Width = 3;
                break;

                case "Кисточка":
                p.Color = Color.Yellow;
                p.Width = 3;
                break;

                case "Ластик":
                p.Color = Color.White;
                p.Width = 3;
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

                if (fileName.EndsWith(".bmp")) {
                    formMainPictureBox.Image = new Bitmap(ofd.FileName);
                } else {
                    formMainPictureBox.Image = new Bitmap(ofd.FileName);
                }

                fileIsNew = false;
                fileIsSaved = true;
            } else {
                return;
            }

            changeTitle();
            changeIcon();
        }

        private void formMainFileSave_Click(object sender, EventArgs e) {
            if (fileIsNew == true) {
                if (sfd.ShowDialog() == DialogResult.OK) {
                    saveFile();

                    fileIsNew = false;
                } else {
                    fileIsSaved = false;
                }
            } else {
                saveFile();
            }
        }

        private void formMainFileSaveAs_Click(object sender, EventArgs e) {
            if (sfd.ShowDialog() == DialogResult.OK) {
                saveFile();
            }
        }

        private void formMainFilePrint_Click(object sender, EventArgs e) {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPageHandler;

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK) {
                printDialog.Document.Print();
            }
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e) {
            e.Graphics.DrawImageUnscaled(formMainPictureBox.Image, 0, 0);
        }

        private void formMainFileExit_Click(object sender, EventArgs e) {
            if (fileIsSaved == false) {
                if (showFormSave() == false) {
                    return;
                }
            }

            this.Close();
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
            if (sfd.FileName.EndsWith(".bmp")) {
                formMainPictureBox.Image.Save(sfd.FileName, ImageFormat.Bmp);
            } else if (sfd.FileName.EndsWith(".jpg")) {
                formMainPictureBox.Image.Save(sfd.FileName, ImageFormat.Jpeg);
            } else if (sfd.FileName.EndsWith(".png")) {
                formMainPictureBox.Image.Save(sfd.FileName, ImageFormat.Png);
            } else if (sfd.FileName.EndsWith(".tiff")) {
                formMainPictureBox.Image.Save(sfd.FileName, ImageFormat.Tiff);
            }
            
            fileName = sfd.FileName;
            fileIsSaved = true;
            changeTitle();
            changeIcon();

            return true;
        }

        private void FormMain_Resize(object sender, EventArgs e) {
            resizeForm();
        }

        private void formMainTrackBar_Scroll(object sender, EventArgs e) {
            pen.Width = formMainTrackBar.Value;
        }

        private void formMainChooseColor_Click(object sender, EventArgs e) {
            if (formMainColorDialog.ShowDialog() == DialogResult.OK) {
                pen.Color = formMainColorDialog.Color;
                ((Button) sender).BackColor = formMainColorDialog.Color;
            }
        }

        private void formMainMenuStripReferenceAbout_Click(object sender, EventArgs e) {
            new FormAbout().Show();
        }

        private void formMainMenuStripViewStatusBar_Click(object sender, EventArgs e) {
            if (formMainMenuStripViewStatusBar.CheckState == CheckState.Checked) {
                formMainMenuStripViewStatusBar.CheckState = CheckState.Unchecked;
                formMainStatusBar.Visible = false;
                formMainPictureBox.Size = new Size(formMainMenuStripViewStatusBar.Size.Width, formMainPictureBox.Size.Height + formMainMenuStripViewStatusBar.Size.Height);
            } else {
                formMainMenuStripViewStatusBar.CheckState = CheckState.Checked;
                formMainStatusBar.Visible = true;
                formMainPictureBox.Size = new Size(formMainMenuStripViewStatusBar.Size.Width, formMainPictureBox.Size.Height - formMainMenuStripViewStatusBar.Size.Height);
            }
        }

        private void formMainMenuStripViewScaleUp_Click(object sender, EventArgs e) {
            changeZoom("up");
        }

        private void formMainMenuStripViewScaleDown_Click(object sender, EventArgs e) {
            changeZoom("down");
        }

        private void formMainMenuStripViewScaleRecover_Click(object sender, EventArgs e) {
            changeZoom("recover");
        }

        private void changeZoom(string value) {

            updateStatusBar();
        }

        private void button5_Click(object sender, EventArgs e) {
            /*MouseEventArgs mouse = e as MouseEventArgs;
            if (mouse != null) {
                int x = mouse.X;
                int y = mouse.Y;
            }
            graphics = Graphics.FromImage(formMainPictureBox.Image);
            Rectangle rect = new Rectangle();
            graphics.DrawRectangle(pen, rect);*/
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
