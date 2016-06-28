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
using System.Windows.Threading;
using System.Xml;
using CiteProc.v10;
using CiteProc.Formatting;
using Acoose.WpfToolkit.Menu;
using Microsoft.Win32;

namespace CiteProc.WpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _IsDirty = false;
        private string _StylePath = null;

        private bool _StyleChanged = false;
        private bool _JsonChanged = false;
        private bool _CultureChanged = false;

        private CiteProc.Processor _Processor = null;
        private Exception _ProcessorException = null;

        private CiteProc.Test.Fixtures.Input[] _Json = null;
        private Exception _JsonException = null;

        private TimeSpan _CompileDuration = TimeSpan.Zero;
        private TimeSpan _GenerateDuration = TimeSpan.Zero;

        private ComposedRun[] _Bibliography = new ComposedRun[] { };
        private ComposedRun[] _Citations = new ComposedRun[] { };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // initialize menu's
            CommandFactory.Current.AutoBind(this);

            // reset
            this.UpdateResults();
            this.UpdateTitle();

            // start background worker
            var action = new Action(this.BackgroundThread);
            action.BeginInvoke(null, null);

            // load initial values
            this.txtStyle.Text = this.GetManifestResource("CiteProc.WpfDemo.Examples.ExampleStyle.xml");
            this.txtJson.Text = this.GetManifestResource("CiteProc.WpfDemo.Examples.Items.json");

            // available cultures
            this.cboCulture.ItemsSource = v10.LocaleFile.Defaults
                .Select(x => x.XmlLang)
                .ToArray();

            // done
            this.IsDirty = false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // cancel?
            e.Cancel = !this.CloseStyle();
        }

        private void BackgroundThread()
        {
            // loop
            while (this.IsVisible)
            {
                // init
                var updateResults = false;

                // compile?
                if (this._StyleChanged)
                {
                    // reset
                    this._StyleChanged = false;
                    updateResults = true;

                    // timer
                    var start = DateTime.Now;

                    // try/catch
                    try
                    {
                        // load style
                        var style = this.Dispatcher.Invoke<File>(this.LoadStyle);

                        // recompile
                        this._Processor = Processor.Compile(style);
                        this._ProcessorException = null;
                    }
                    catch (Exception ex)
                    {
                        // error
                        this._Processor = null;
                        this._ProcessorException = ex;
                    }
                    finally
                    {
                        // timing
                        this._CompileDuration = DateTime.Now.Subtract(start);
                    }
                }

                // parse json
                if (this._JsonChanged)
                {
                    // reset
                    this._JsonChanged = false;
                    updateResults = true;

                    // try/catch
                    try
                    {
                        // load
                        this._Json = Test.Fixtures.Input.LoadJson(this.Dispatcher.Invoke(() => this.txtJson.Text));
                        this._JsonException = null;
                    }
                    catch (Exception ex)
                    {
                        this._Json = null;
                        this._JsonException = ex;
                    }
                }

                // regenerate
                if (updateResults || this._CultureChanged)
                {
                    // reset
                    this._CultureChanged = false;

                    // init
                    this._Bibliography = new ComposedRun[] { };
                    this._Citations = new ComposedRun[] { };

                    // locale
                    var locale = this.Dispatcher.Invoke(() => this.cboCulture.SelectedItem) as string;
                    var force = !string.IsNullOrEmpty(locale);

                    // regenerate
                    if (this._Processor != null && this._Json != null)
                    {
                        // init
                        var start = DateTime.Now;

                        // try/catch
                        try
                        {
                            // generate
                            this._Bibliography = this._Processor.GenerateBibliography(this._Json, locale, force);
                            this._Citations = this._Json.Select(x => this._Processor.GenerateCitation(new IDataProvider[] { x }, locale, force)).ToArray();
                        }
                        catch (Exception ex)
                        {
                            // error
                            this._Processor = null;
                            this._ProcessorException = ex;
                        }
                        finally
                        {
                            // done
                            this._GenerateDuration = DateTime.Now.Subtract(start);
                        }
                    }

                    // update results
                    this.Dispatcher.Invoke(this.UpdateResults);
                }

                // sleep
                System.Threading.Thread.Sleep(10);
            }
        }
        private File LoadStyle()
        {
            // load
            using (var sr = new System.IO.StringReader(this.txtStyle.Text))
            {
                using (var xr = XmlReader.Create(sr))
                {
                    return File.Load(xr);
                }
            }
        }
        private void UpdateResults()
        {
            // bibliography
            this.txtBibliography.Document.Blocks.Clear();
            this.txtBibliography.Document.Blocks.AddRange(this._Bibliography.Select(x => this.CreateParagraph(x)));
            this.txtBibliographyHtml.Text = (this._Bibliography.Length > 0 ? this._Bibliography.First().ToHtml() : "");

            // citation
            this.txtCitation.Document.Blocks.Clear();
            this.txtCitation.Document.Blocks.AddRange(this._Citations.Select(x => this.CreateParagraph(x)));
            this.txtCitationHtml.Text = (this._Citations.Length > 0 ? this._Citations.First().ToHtml() : "");

            // style
            this.rowStyleException.MaxHeight = (this._ProcessorException == null ? 0d : double.MaxValue);
            this.rowStyleException.MinHeight = (this._ProcessorException == null ? 0d : 28d);
            this.rowStyleSplitter.Height = new GridLength(this._ProcessorException == null ? 0d : 3d);
            this.SetErrorMessage(this.txtStyleException, this.txtStyle, this._ProcessorException);

            // json
            this.rowJsonException.MaxHeight = (this._JsonException == null ? 0d : double.MaxValue);
            this.rowJsonException.MinHeight = (this._JsonException == null ? 0d : 28d);
            this.rowJsonSplitter.Height = new GridLength(this._JsonException == null ? 0d : 3d);
            this.SetErrorMessage(this.txtJsonException, this.txtJson, this._JsonException);

            // title
            this.UpdateTitle();

            // durations
            this.lblCompile.Content = string.Format("Compile: {0:0} ms", this._CompileDuration.TotalMilliseconds);
            this.lblGenerate.Content = string.Format("Generate: {0:0} ms", this._GenerateDuration.TotalMilliseconds);
        }
        private void SetErrorMessage(TextBlock target, TextBox xml, Exception exception)
        {
            // init
            target.Inlines.Clear();
            var position = (xml != null);

            // show
            if (exception != null)
            {
                // loop
                for (var ex = exception; ex != null; ex = ex.InnerException)
                {
                    // init
                    Inline inline = new System.Windows.Documents.Run(ex.Message);

                    // xml exception?
                    if (ex is XmlException)
                    {
                        // cast
                        var xx = (XmlException)ex;

                        // create hyperlink
                        inline = new Hyperlink(inline)
                        {
                            NavigateUri = new Uri("http://citationstyles.org"),
                            Tag = xx
                        };

                        // event
                        ((Hyperlink)inline).RequestNavigate += XmlDetails_RequestNavigate;
                    }

                    // add inline
                    target.Inlines.Add(inline);
                    target.Inlines.Add(new LineBreak());
                }
            }
        }

        private void txtStyle_TextChanged(object sender, TextChangedEventArgs e)
        {
            // changed
            this._StyleChanged = true;
            this.IsDirty = true;

            // size
            this.lblSize.Content = string.Format("{0} bytes", this.txtStyle.Text.Length);
        }
        private void txtJson_TextChanged(object sender, TextChangedEventArgs e)
        {
            this._JsonChanged = true;
        }

        private void XmlDetails_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // init
            var hyperlink = (Hyperlink)e.Source;
            var xx = (XmlException)hyperlink.Tag;

            // set caret position
            var position = this.txtStyle.GetCharacterIndexFromLineIndex(xx.LineNumber - 1) + xx.LinePosition - 1;
            this.txtStyle.ScrollToLine(xx.LineNumber > 2 ? xx.LineNumber - 2 : xx.LineNumber - 1);
            this.txtStyle.Select(position, 0);
            this.txtStyle.Focus();
        }

        private Paragraph CreateParagraph(CiteProc.Formatting.Run run)
        {
            // init
            var result = new Paragraph()
            {
                TextAlignment = TextAlignment.Left,
                TextIndent = -30,
                Margin = new Thickness(30, 20, 0, 0),
            };

            // flatten
            var inlines = run.Flatten()
                .Select(txt =>
                {
                    // init
                    var r = new System.Windows.Documents.Run(txt.Text);

                    // font style
                    switch (txt.FontStyle)
                    {
                        case CiteProc.Formatting.FontStyle.Italic:
                            r.FontStyle = FontStyles.Italic;
                            break;
                        case CiteProc.Formatting.FontStyle.Oblique:
                            r.FontStyle = FontStyles.Oblique;
                            break;
                    }

                    // font weight
                    switch (txt.FontVariant)
                    {
                        case CiteProc.Formatting.FontVariant.SmallCaps:
                            r.Typography.Capitals = FontCapitals.SmallCaps;
                            break;
                    }

                    // font weight
                    switch (txt.FontWeight)
                    {
                        case CiteProc.Formatting.FontWeight.Bold:
                            r.FontWeight = FontWeights.Bold;
                            break;
                        case CiteProc.Formatting.FontWeight.Light:
                            r.FontWeight = FontWeights.ExtraLight;
                            break;
                    }

                    // text decoration
                    switch (txt.TextDecoration)
                    {
                        case CiteProc.Formatting.TextDecoration.Underline:
                            r.TextDecorations = TextDecorations.Underline;
                            break;
                    }

                    // vertical align
                    switch (txt.VerticalAlign)
                    {
                        case CiteProc.Formatting.VerticalAlign.Subscript:
                            r.BaselineAlignment = BaselineAlignment.Subscript;
                            r.FontSize -= 3;
                            break;
                        case CiteProc.Formatting.VerticalAlign.Superscript:
                            r.BaselineAlignment = BaselineAlignment.TextTop;
                            r.FontSize -= 3;
                            break;
                    }

                    // done
                    return r;
                })
                .ToArray();

            // add
            result.Inlines.AddRange(inlines);

            // done
            return result;
        }

        private bool IsDirty
        {
            get
            {
                return this._IsDirty;
            }
            set
            {
                // set
                this._IsDirty = value;

                // update
                this.UpdateTitle();
            }
        }
        private void UpdateTitle()
        {
            // update
            this.Title = string.Format("CiteProc.NET - WPF Demo{0}{1}{2}",
                (this._Processor == null ? null : string.Format(" - {0}", this._Processor.Title)),
                (string.IsNullOrEmpty(this._StylePath) ? null : string.Format(" - [{0}]", this._StylePath)),
                (this._IsDirty ? "*" : ""));
        }

        [Command(Name = "New")]
        public void NewStyle()
        {
            // close?
            if (this.CloseStyle())
            {
                // new
                this.txtStyle.Text = this.GetManifestResource("CiteProc.WpfDemo.Examples.NewStyle.xml");

                // done
                this._StylePath = null;
                this.IsDirty = false;
            }
        }
        [Command(Name = "Open")]
        public void OpenStyle()
        {
            // dialog
            var dialog = new OpenFileDialog()
            {
                Filter = "Citation Style Language (.csl)|*.csl"
            };
            if (dialog.ShowDialog(this) ?? false)
            {
                // close current style
                if (this.CloseStyle())
                {
                    // open file
                    try
                    {
                        // load
                        this.txtStyle.Text = System.IO.File.ReadAllText(dialog.FileName);

                        // success
                        this._StylePath = dialog.FileName;
                        this.IsDirty = false;
                    }
                    catch (Exception ex)
                    {
                        // show message
                        MessageBox.Show(this, ex.Message, "Error while saving", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private bool CloseStyle()
        {
            // init
            var result = !this.IsDirty;

            // save?
            if (!result)
            {
                // dialog
                var dialog = MessageBox.Show(this, "Do you want to save the changes?", "Save Changes?", MessageBoxButton.YesNoCancel);
                switch (dialog)
                {
                    case MessageBoxResult.Yes:
                        result = this.SaveStyle(this._StylePath);
                        break;
                    case MessageBoxResult.No:
                        result = true;
                        break;
                    case MessageBoxResult.Cancel:
                        result = false;
                        break;
                }
            }

            // done
            return result;
        }
        [Command(Name = "Save")]
        public void SaveStyle()
        {
            this.SaveStyle(this._StylePath);
        }
        [Command(Name = "SaveAs")]
        public void SaveStyleAs()
        {
            this.SaveStyle(null);
        }
        private bool SaveStyle(string path)
        {
            // init
            var result = false;

            // dialog?
            if (string.IsNullOrEmpty(path))
            {
                var dialog = new SaveFileDialog()
                {
                    Filter = "Citation Style Language (.csl)|*.csl",
                    FileName = (this._Processor == null ? null : this._Processor.Title)
                };
                if (dialog.ShowDialog(this) ?? false)
                {
                    // fill path
                    path = dialog.FileName;
                }
            }

            // save?
            if (!string.IsNullOrEmpty(path))
            {
                // save
                try
                {
                    // save
                    System.IO.File.WriteAllText(path, this.txtStyle.Text);

                    // success
                    result = true;
                    this._StylePath = path;
                    this.IsDirty = false;
                }
                catch (Exception ex)
                {
                    // failure
                    result = false;

                    // show message
                    MessageBox.Show(this, ex.Message, "Error while saving", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // done
            return result;
        }
        [Command(Name = "Close")]
        private void Quit()
        {
            this.Close();
        }

        [Command]
        private void About()
        {
            // init
            var about = new AboutWindow();
            about.Owner = this;
            about.ShowDialog();
        }

        private void txtStyle_SelectionChanged(object sender, RoutedEventArgs e)
        {
            this.SetCaretLabel(this.txtStyle);
        }
        private void txtJson_SelectionChanged(object sender, RoutedEventArgs e)
        {
            this.SetCaretLabel(this.txtJson);
        }
        private void SetCaretLabel(TextBox owner)
        {
            // init
            var line = owner.GetLineIndexFromCharacterIndex(owner.CaretIndex);
            var position = owner.CaretIndex - owner.GetCharacterIndexFromLineIndex(line);

            // done
            this.lblCaret.Content = string.Format("({0}, {1})", line + 1, position + 1);
        }

        private void txtStyle_LostFocus(object sender, RoutedEventArgs e)
        {
            this.lblCaret.Content = "";
        }
        private void txtJson_LostFocus(object sender, RoutedEventArgs e)
        {
            this.lblCaret.Content = "";
        }

        private void cboCulture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // culture changed
            this._CultureChanged = true;
        }

        private string GetManifestResource(string resourceName)
        {
            using (var stream = typeof(MainWindow).Assembly.GetManifestResourceStream(resourceName))
            {
                using (var sr = new System.IO.StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}