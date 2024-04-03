using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Collections.Generic;

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

        private byte figure = 0; // see documentation
        private bool isPaintingFigure = false;
        private bool isPrintingText = false;
        private string printingText = string.Empty;
        private byte printingTextSize = 0;
        private Point paintingFigureFirstPoint;
        private Point paintingFigureSecondPoint;

        private ArrayPoints arrayPoints = new ArrayPoints(2);

        private List<object> allObjects = new List<object>();

        private int windowSizeX = Screen.PrimaryScreen.Bounds.Width;
        private int windowSizeY = Screen.PrimaryScreen.Bounds.Height;

        private Bitmap map;
        private Graphics graphics;
        private Pen pen;

        public FormMain() {
            InitializeComponent();

            ofd.Filter = "Bitmap file|*.bmp|PNG file|.png|JPG file|.jpg|TIFF file|.tiff|GIF file|.gif|Any format|*.*";
            sfd.Filter = "Bitmap file|*.bmp|PNG file|.png|JPG file|.jpg|TIFF file|.tiff|GIF file|.gif|Any format|*.*";

            map = new Bitmap(windowSizeX, windowSizeY);
            pen = createTool("Карандаш");

            setSize();
            resizeForm();
            changeIcon();
            changeTitle();
            updateStatusBar();
            resetPaintingFigure();
            resetPrintingText();
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
            formMainStatusBarSize.Text = string.Format("Размер окна: {0} x {1}", this.Width, this.Height);
            formMainStatusBarScale.Text = "Масштаб: 100%";
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
            figure = 0;

            return p;
        }

        private void formMainPictureBox_MouseDown(object sender, MouseEventArgs e) {
            mouseDown = true;
            mousePoint = mouseDownPoint = e.Location;

            if (isPaintingFigure == true) {
                paintingFigureFirstPoint = e.Location;
            } else if (isPrintingText == true) {
                graphics.DrawString(printingText, new Font("Arial", printingTextSize), pen.Brush, e.Location.X, e.Location.Y);
                Text text = new Text(printingText, new Font("Arial", printingTextSize), pen.Brush, e.Location.X, e.Location.Y);
                allObjects.Add(text);
                printingText = string.Empty;
                printingTextSize = 0;
            }

            changeIcon();
            changeTitle();
        }

        private void formMainPictureBox_MouseMove(object sender, MouseEventArgs e) {
            mousePoint = e.Location;

            if (mouseDown == false) {
                return;
            }

            if (isPaintingFigure == true) {
                formMainPictureBox.Invalidate();
                //formMainPictureBox.Invalidate();
                Rectangle rect = RectFrom2Points(paintingFigureFirstPoint, e.Location);
                graphics.DrawRectangle(pen, rect);
                formMainPictureBox.Image = map;
                //formMainPictureBox.Invalidate();
            } else if (isPrintingText == true) {

            } else {
                arrayPoints.setPoint(e.X, e.Y);

                if (arrayPoints.getCountPoints() >= 2) {
                    graphics.DrawLines(pen, arrayPoints.getPoints());
                    Point[] array = arrayPoints.getPoints();
                    allObjects.Add(array);
                    formMainPictureBox.Image = map;
                    arrayPoints.setPoint(e.X, e.Y);
                }
            }

            changeIcon();
            changeTitle();
        }

        private void formMainPictureBox_MouseUp(object sender, MouseEventArgs e) {
            mouseDown = false;
            arrayPoints.resetPoints();

            if (isPaintingFigure == true) {
                paintingFigureSecondPoint = e.Location;
                Rectangle rect = RectFrom2Points(paintingFigureFirstPoint, paintingFigureSecondPoint);
                allObjects.Add(rect);
                graphics.DrawRectangle(pen, rect);
                formMainPictureBox.Image = map;
                formMainPictureBox.Invalidate();

                resetPaintingFigure();
            } else if (isPrintingText == true) {
                isPrintingText = false;
            }

            this.formMainPictureBox.Cursor = Cursors.Default;

            fileIsSaved = false;

            changeIcon();
            changeTitle();
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

        private void formMainSelectTool(object sender, EventArgs e) {
            pen = createTool(((Button) sender).Tag.ToString());
        }

        private void newFile() {
            fileName = fileUnnamed;
            fileIsNew = true;
            fileIsSaved = true;

            graphics.Clear(Color.White);

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

                formMainPictureBox.Image = new Bitmap(ofd.FileName);

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
            ImageFormat imageFormat;
            if (sfd.FileName.EndsWith(".bmp")) {
                imageFormat = ImageFormat.Bmp;
            } else if (sfd.FileName.EndsWith(".jpg")) {
                imageFormat = ImageFormat.Jpeg;
            } else if (sfd.FileName.EndsWith(".png")) {
                imageFormat = ImageFormat.Png;
            } else if (sfd.FileName.EndsWith(".tiff")) {
                imageFormat = ImageFormat.Tiff;
            } else if (sfd.FileName.EndsWith(".gif")) {
                imageFormat = ImageFormat.Gif;
            } else {
                imageFormat = ImageFormat.Bmp;
            }

            formMainPictureBox.Image.Save(sfd.FileName, imageFormat);


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

        private void formMainGroupFiguresSquare_Click(object sender, EventArgs e) {
            isPaintingFigure = true;
            figure = 1;
            this.formMainPictureBox.Cursor = Cursors.Cross;
        }

        private void formMainGroupFiguresTriangle_Click(object sender, EventArgs e) {
            isPaintingFigure = true;
            figure = 2;
            this.formMainPictureBox.Cursor = Cursors.Cross;
        }

        private void formMainGroupFiguresEllipse_Click(object sender, EventArgs e) {
            isPaintingFigure = true;
            figure = 3;
            this.formMainPictureBox.Cursor = Cursors.Cross;
        }

        private void formMainGroupFiguresLine_Click(object sender, EventArgs e) {
            isPaintingFigure = true;
            figure = 4;
            this.formMainPictureBox.Cursor = Cursors.Cross;
        }

        private Rectangle RectFrom2Points(Point start, Point end) {
            Rectangle rect = new Rectangle(
                Math.Min(start.X, end.X),
                Math.Min(start.Y, end.Y),
                Math.Abs(end.X - start.X),
                Math.Abs(end.Y - start.Y)
            );
            return rect;
        }

        private void resetPaintingFigure() {
            figure = 0;
            isPaintingFigure = false;
            paintingFigureFirstPoint = Point.Empty;
            paintingFigureSecondPoint = Point.Empty;
        }

        private void resetPrintingText() {
            isPrintingText = false;
            printingText = string.Empty;
            printingTextSize = 0;
        }

        private void formMainText_Click(object sender, EventArgs e) {
            if (isPaintingFigure == false) {
                InputText inputText = new InputText();

                if (inputText.ShowDialog() == DialogResult.OK) {
                    printingText = inputText.formInputTextTextBox.Text;
                    printingTextSize = Convert.ToByte(inputText.formInputTextTextSize.Text);
                    isPrintingText = true;
                    this.formMainPictureBox.Cursor = Cursors.Cross;
                } else {
                    isPrintingText = false;
                }
            }
        }

        private void drawAllObjects() {

            foreach (object obj in allObjects) {
                if (obj is Rectangle) {
                    Rectangle rect = (Rectangle) obj;
                    graphics.DrawRectangle(pen, rect);
                } else if (obj is Text) {
                    Text text = (Text) obj;
                    
                    //graphics.DrawString(pen, text);
                } else if (obj is Point) {
                    //graphics.DrawLines(pen, array);
                }
            }
        }
    }
}
