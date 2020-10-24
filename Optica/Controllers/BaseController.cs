using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Optica.Controllers
{
    public class BaseController : Controller
    {
       
        /*public ActionResult Index()
        {
            return View();
        }*/

        public string generateReport(string _url)
        {
            var url = Url.RouteUrl(_url);
            url = /*Request.Url.Authority*/"localhost:80" + url;
            //new Converter
            HtmlToPdf Converter = new HtmlToPdf();
            //document parameters
            var enteteUrl = Server.MapPath("~/Views/Shared/EntetePartial.html");
            var footerUrl = Server.MapPath("~/Views/Shared/FooterPartial.html");
            //var footerUrl = Server.MapPath("~/Datadir/FooterPartial.html");
            //Converter.Options.CssMediaType = 
            //header params
            Converter.Options.DisplayHeader = true;
            Converter.Header.DisplayOnFirstPage = true;
            Converter.Header.DisplayOnEvenPages = true;
            Converter.Header.DisplayOnOddPages = true;
            Converter.Header.Height = 95;
            PdfHtmlSection headerHtml = new PdfHtmlSection(enteteUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            Converter.Header.Add(headerHtml);

            //footer params
            Converter.Options.DisplayFooter = true;
            Converter.Footer.DisplayOnFirstPage = true;
            Converter.Footer.DisplayOnOddPages = true;
            Converter.Footer.DisplayOnEvenPages = true;
            Converter.Footer.Height = 20;
            PdfHtmlSection footerhtml = new PdfHtmlSection(footerUrl);
            footerhtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            Converter.Footer.Add(footerhtml);
            //add page number element to footer
            PdfTextSection text = new PdfTextSection(0, 4, "Page: {page_number} sur {total_pages} ", new System.Drawing.Font("Helvetica", 8));
            text.HorizontalAlign = PdfTextHorizontalAlign.Center;
            //PdfTextSection text2 = new PdfTextSection(0,4,"Lomé,"+DateTime.Now.ToString("dd/MM/yyyy"), new System.Drawing.Font("Helvetica",8));
            //text2.HorizontalAlign = PdfTextHorizontalAlign.Right;

            Converter.Footer.Add(text);
            //Converter.Footer.Add(text2);
            //conversion delay
            Converter.Options.MinPageLoadTime = 2;
            Converter.Options.MaxPageLoadTime = 90;
            Converter.Options.PdfPageSize = PdfPageSize.A4;
            Converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            Converter.Options.MarginTop = 30;
            Converter.Options.MarginBottom = 20;
            //Converter.Options.MarginRight = 50;
            //Converter.Options.MarginLeft = 50;
            //new document
            string statut = "ERROR";
            try
            {
                PdfDocument doc = Converter.ConvertUrl(url);
                string filename = _url == "_journalCaisseReport" ? "caisseReport.pdf" : "TraitementReport.pdf";
                string saveDir = Server.MapPath(System.IO.Path.Combine("~/Datadir/", filename));
                //saveDocument
                doc.Save(saveDir);
                doc.Close();
                statut = "OK";
            }
            catch (Exception e)
            {
                //
            }
            return statut;
        }
	}
}