﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;

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

        private string printingText = string.Empty;
        private byte printingTextSize = 0;
        private Point paintingFigureFirstPoint;
        private Point paintingFigureSecondPoint;

        private Points points = new Points(2);

        private int windowSizeX = Screen.PrimaryScreen.Bounds.Width;
        private int windowSizeY = Screen.PrimaryScreen.Bounds.Height;

        private Bitmap mapDrawing;
        private Bitmap mapSaved;
        private Graphics graphics;
        private Pen pen;

        public FormMain() {
            InitializeComponent();

            this.DoubleBuffered = true;

            mapDrawing = new Bitmap(formMainPictureBox.Width, formMainPictureBox.Height);
            mapSaved = new Bitmap(formMainPictureBox.Width, formMainPictureBox.Height);
            graphics = Graphics.FromImage(mapDrawing);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            formMainPictureBox.Image = mapDrawing;

            ofd.Filter = "Bitmap file|*.bmp|PNG file|.png|JPG file|.jpg|TIFF file|.tiff|GIF file|.gif|Any format|*.*";
            sfd.Filter = "Bitmap file|*.bmp|PNG file|.png|JPG file|.jpg|TIFF file|.tiff|GIF file|.gif|Any format|*.*";

            pen = createTool(Color.Black);

            updateScreen();
            changeIcon();
            changeTitle();
            updateStatusBar();
        }

        private void updateScreen() {
            mapDrawing = new Bitmap(windowSizeX, windowSizeY);
            mapSaved = new Bitmap(windowSizeX, windowSizeY);
            graphics = Graphics.FromImage(mapDrawing);
            graphics.Clear(Color.White);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
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

        private Pen createTool(Color color) {
            Pen p = new Pen(color, formMainTrackBar.Value);

            p.Brush = Brushes.Black;
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;
            p.Alignment = PenAlignment.Center;
            p.DashStyle = DashStyle.Custom;
            p.LineJoin = LineJoin.Round;

            return p;
        }
        
        private void formMainPictureBox_MouseDown(object sender, MouseEventArgs e) {
            paintingFigureFirstPoint = e.Location;
            mouseDown = true;

            if (formMainMenuStripToolsText.Checked == true) {
                graphics.DrawString(printingText, new Font("Arial", printingTextSize), pen.Brush, e.Location.X, e.Location.Y);
                printingText = string.Empty;
                printingTextSize = 0;

                formMainMenuStripToolsText.Checked = false;
                formMainPictureBox.Invalidate();
            }
        }

        private void formMainPictureBox_MouseMove(object sender, MouseEventArgs e) {
            paintingFigureSecondPoint = e.Location;

            if (mouseDown == false) {
                return;
            }

            if (formMainMenuStripToolsRect.Checked == true || formMainMenuStripToolsEllipse.Checked == true) {
                graphics.Clear(Color.White);
                graphics.DrawImage(mapSaved, Point.Empty);

                Rectangle rect = RectFrom2Points(paintingFigureFirstPoint, paintingFigureSecondPoint);

                if (formMainMenuStripToolsRect.Checked == true) {
                    graphics.DrawRectangle(pen, rect);

                    if (formMainMenuStripToolsFill.Checked == true) {
                        graphics.FillRectangle(pen.Brush, rect);
                    }
                } else {
                    graphics.DrawEllipse(pen, rect);

                    if (formMainMenuStripToolsFill.Checked == true) {
                        graphics.FillEllipse(pen.Brush, rect);
                    }
                }

                formMainPictureBox.Invalidate();
            } else if (formMainMenuStripToolsLine.Checked == true) {
                graphics.Clear(Color.White);
                graphics.DrawImage(mapSaved, Point.Empty);

                graphics.DrawLine(pen, paintingFigureFirstPoint, paintingFigureSecondPoint);

                formMainPictureBox.Invalidate();
            } else if (formMainMenuStripToolsRubber.Checked == true) {
                graphics.Clear(Color.White);
                graphics.DrawImage(mapSaved, Point.Empty);

                Rectangle rect = RectFromPoint(paintingFigureSecondPoint, 30);

                graphics.FillEllipse(Brushes.White, rect);

                formMainPictureBox.Invalidate();
            } else {
                points.setPoint(e.X, e.Y);

                if (points.getCountPoints() >= 2) {
                    graphics.DrawLines(pen, points.getPoints());
                    formMainPictureBox.Image = mapDrawing;
                    points.setPoint(e.X, e.Y);
                }
            }
        }

        private void formMainPictureBox_MouseUp(object sender, MouseEventArgs e) {
            mapSaved = new Bitmap(mapDrawing);
            mouseDown = false;
            fileIsSaved = false;
            points.resetPoints();
            this.formMainPictureBox.Cursor = Cursors.Default;
        }

        private void formMainPictureBox_Paint(object sender, PaintEventArgs e) {
            e.Graphics.DrawImage(mapSaved, Point.Empty);
            e.Graphics.DrawImage(mapDrawing, Point.Empty);
        }

        private void FormMain_Resize(object sender, EventArgs e) {
            formMainPictureBox.Location = new Point(0, formMainMenuStrip.Location.Y + formMainMenuStripTools.Location.Y + formMainPanel.Location.Y);
            formMainPictureBox.Width = this.Width - 30;
            formMainPictureBox.Height = this.Height - 120;

            updateStatusBar();
        }

        private void formMainGroupColorSetColor(object sender, EventArgs e) {
            pen.Color = ((Button) sender).BackColor;
        }

        private void newFile() {
            fileName = fileUnnamed;
            fileIsNew = true;
            fileIsSaved = true;

            updateScreen();
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
                        saveFile();
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    saveFile();
                    return true;
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

        private void saveFile() {
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

        private Rectangle RectFromPoint(Point p, int radius) {
            Rectangle rect = new Rectangle(new Point(p.X - radius, p.Y - radius), new Size(radius * 2, radius * 2));
            return rect;
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

        private void formMainMenuStripToolsRect_Click(object sender, EventArgs e) {
            if (formMainMenuStripToolsRect.Checked == true) {
                formMainMenuStripToolsRect.Checked = false;
            } else {
                formMainMenuStripToolsSetOFF();
                formMainMenuStripToolsRect.Checked = true;
            }
        }

        private void formMainMenuStripToolsEllipse_Click(object sender, EventArgs e) {
            if (formMainMenuStripToolsEllipse.Checked == true) {
                formMainMenuStripToolsEllipse.Checked = false;
            } else {
                formMainMenuStripToolsSetOFF();
                formMainMenuStripToolsEllipse.Checked = true;
            }
        }

        private void formMainMenuStripToolsLine_Click(object sender, EventArgs e) {
            if (formMainMenuStripToolsLine.Checked == true) {
                formMainMenuStripToolsLine.Checked = false;
            } else {
                formMainMenuStripToolsSetOFF();
                formMainMenuStripToolsLine.Checked = true;
            }
        }

        private void formMainMenuStripToolsFill_Click(object sender, EventArgs e) {
            if (formMainMenuStripToolsFill.Checked == true) {
                formMainMenuStripToolsFill.Checked = false;
            } else {
                formMainMenuStripToolsFill.Checked = true;
            }
        }

        private void formMainMenuStripToolsRubber_Click(object sender, EventArgs e) {
            if (formMainMenuStripToolsRubber.Checked == true) {
                formMainMenuStripToolsRubber.Checked = false;
            } else {
                formMainMenuStripToolsSetOFF();
                formMainMenuStripToolsRubber.Checked = true;
            }
        }

        private void formMainMenuStripToolsText_Click(object sender, EventArgs e) {
            if (formMainMenuStripToolsText.Checked == true) {
                formMainMenuStripToolsText.Checked = false;
            } else {
                formMainMenuStripToolsSetOFF();
                formMainMenuStripToolsText.Checked = true;

                InputText inputText = new InputText();

                if (inputText.ShowDialog() == DialogResult.OK) {
                    printingText = inputText.formInputTextTextBox.Text;
                    printingTextSize = Convert.ToByte(inputText.formInputTextTextSize.Text);

                    this.formMainPictureBox.Cursor = Cursors.Cross;
                } else {
                    formMainMenuStripToolsText.Checked = false;
                }
            }
        }

        private void formMainMenuStripToolsSetOFF() {
            foreach (ToolStripMenuItem item in formMainMenuStripTools.Items) {
                item.Checked = false;
            }
        }
    }
}