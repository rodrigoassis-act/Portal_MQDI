using Aspose.Words;
using System;
using System.IO;

namespace ONS.PortalMQDI.Shared.Utils
{
    public static class DocumentGeneratorUtil
    {
        private static string ImageBase64;

        private static void LoadConfig()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = Path.Combine(basePath, "wwwroot", "imagem", "headerLogo.png");
                ImageBase64 = Convert.ToBase64String(File.ReadAllBytes(imagePath));

                License license = new License();
                string licensePath = Path.Combine(basePath, "License", "Aspose.Total.lic");
                license.SetLicense(licensePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static byte[] GeneratePdfFromHtml(string htmlContent, string title = null, string headerCustom = null)
        {
            return GeneratePdfFromHtml(htmlContent, title, headerCustom, false);
        }

        private static byte[] GeneratePdfFromHtml(string htmlContent, string title = null, string headerCustom = null, bool modoRetrato = true)
        {
            LoadConfig();
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            SetEstilos(builder, modoRetrato);
            builder.InsertHtml(htmlContent);
            SetHeader(builder, title, headerCustom);

            MemoryStream pdfStream = new MemoryStream();
            doc.Save(pdfStream, SaveFormat.Pdf);

            if (false)
            {
                string path = $@"D:\Templete\teste.pdf";
                File.WriteAllBytes(path, pdfStream.ToArray());
            }

            return pdfStream.ToArray();
        }

        public static void SetEstilos(DocumentBuilder builder, bool modoRetrato)
        {
            builder.PageSetup.PageStartingNumber = 1;
            builder.PageSetup.Orientation = modoRetrato ? Orientation.Portrait : Orientation.Landscape;
            builder.PageSetup.PaperSize = PaperSize.A4;
            builder.PageSetup.LeftMargin = 42.52;
            builder.PageSetup.RightMargin = 42.52;
            builder.PageSetup.TopMargin = 42.52;
            builder.PageSetup.BottomMargin = 42.52;
        }

        public static void SetHeader(DocumentBuilder builder, string title, string headerCustom)
        {
            builder.PageSetup.HeaderDistance = 20;

            if (!string.IsNullOrEmpty(title))
            {
                string formattedTitle = @$"<div style=""display: flex; justify-content: center; margin-bottom: 1rem;""> <h4 style=""flex: 0 0 66.666667%; max-width: 66.666667%;"">{title}</h4> </div>";
                builder.MoveToHeaderFooter(HeaderFooterType.HeaderPrimary);
                builder.InsertHtml(formattedTitle);
            }

            if (!string.IsNullOrEmpty(headerCustom))
            {
                builder.InsertHtml(headerCustom);
            }

            builder.MoveToHeaderFooter(HeaderFooterType.HeaderPrimary);
            byte[] imageBytes = Convert.FromBase64String(ImageBase64);
            using (MemoryStream imageStream = new MemoryStream(imageBytes))
            {
                builder.InsertImage(imageStream, 120, 40);
            }
        }
    }
}
