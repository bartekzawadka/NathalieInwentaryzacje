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
using Microsoft.Reporting.WinForms;

namespace NathalieInwentaryzacje.Views.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy ReportPreview.xaml
    /// </summary>
    public partial class ReportPreview : UserControl
    {
        public ReportPreview()
        {
            InitializeComponent();
            Loaded += ReportPreviewControl_Loaded;
        }

        void ReportPreviewControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadControl();
        }

        private void LoadControl()
        {
            //Pobranie reportViewera
            var reportViewer = RptViewer;

            //Załadowanie danych
            //_printerManager.PreviewReport(reportViewer.LocalReport, DataToView, ReportSettings.ReportType);
            reportViewer.ProcessingMode = ProcessingMode.Local;

            //Tryb widoku
            reportViewer.SetDisplayMode(DisplayMode.PrintLayout);

            reportViewer.ZoomMode = ZoomMode.FullPage;

            //reportViewer.LocalReport.

            //Początkowe przybliżenie podglądu
//            reportViewer.ZoomMode = ReportSettings.ZoomMode;
//            reportViewer.ZoomPercent = ReportSettings.PercentZoom;
//
//            #region Widoczniość przycisków
//            if (!ReportSettings.IsVisiblePDF)
//                DisableUnwantedExportFormat(reportViewer, "PDF");
//
//            if (!ReportSettings.IsVisibleExcel)
//            {
//                DisableUnwantedExportFormat(reportViewer, "Excel");
//                DisableUnwantedExportFormat(reportViewer, "EXCELOPENXML");
//            }
//
//            //Word ma być zawsze niedostępny
//            DisableUnwantedExportFormat(reportViewer, "WORD");
//            DisableUnwantedExportFormat(reportViewer, "WORDOPENXML");
//
//            reportViewer.ShowRefreshButton = false;
//            reportViewer.ShowFindControls = false;
//            reportViewer.ShowStopButton = false;
//            reportViewer.ShowBackButton = false;
//
//            reportViewer.ShowPrintButton = false;

            //reportViewer.LocalReport.ExecuteReportInCurrentAppDomain(AppDomain.CurrentDomain.Evidence);

//            if (!ReportSettings.IsVisibleExcel && !ReportSettings.IsVisiblePDF)
//                reportViewer.ShowExportButton = false;
//
//            if (!ReportSettings.IsVisiblePrint)
//                reportViewer.ShowPrintButton = false;
//
//            if (!ReportSettings.IsVisiblePaging)
//                reportViewer.ShowPageNavigationControls = false;
//            #endregion

//            var printerSettings = reportViewer.PrinterSettings;
//            printerSettings.Copies = (short)ReportSettings.CopyCount;

            //Ustawienie marginesów (ustawienia strony)
//            RefreshPageSettings();
//
//            //Ewentualna zmiana drukarki jesli drukujemy wklejki
//            if (ReportSettings.IsBlank)
//            {
//                _printerManager.ChangePrinterNameAndPaperSource(Properties.Settings.Default.DefaultBlankPrinter, Properties.Settings.Default.DefaultBlankPaperSource);
//            }
//
//            if (!ReportSettings.IsVisibleButtons)
//                reportViewer.ShowToolBar = false;

            //reportViewer.RenderingBegin += ReportViewerOnRenderingBegin;

            //Załadowanie komunikatów
            //_reportViewer.Messages = new CCustomMessageClass(); TODO jak się opisze teksty w klasie ReportViewerCustomMessages
            
            
            
            
            
            
            
            
            
            
            
            
            
            reportViewer.RefreshReport();
        }
    }
}
