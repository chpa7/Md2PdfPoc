// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Markdig;
using SelectPdf;

// get the current directory
string? currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

// set an example markdown filepath
string mdFilepath = currentDir + "/../../../example_markdown_file.md";
string mdDocument = File.ReadAllText(mdFilepath);

// initialize the pipeline
var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

// convert markdown to html first
string htmlRes = Markdown.ToHtml(mdDocument, pipeline);

// instantiate the html to pdf converter
HtmlToPdf converter = new();

// set the page size
converter.Options.PdfPageSize = PdfPageSize.A4;

// set the header settings
converter.Options.DisplayHeader = true;
converter.Header.DisplayOnFirstPage = true;
converter.Header.DisplayOnOddPages = false;
converter.Header.DisplayOnEvenPages = false;
converter.Header.Height = 100;

// get the logo url
string logoUrl = currentDir + "/../../../P_logo_white.png";

PdfHtmlSection headerHtml = new(0, 0, logoUrl)
{
    AutoFitHeight = HtmlToPdfPageFitMode.ShrinkOnly,
    AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly
};

// add the logo to the header 
converter.Header.Add(headerHtml);

// convert the html string to pdf
PdfDocument doc = converter.ConvertHtmlString(htmlRes);

// set the pdf title
doc.DocumentInformation.Title = "PLAY! Test PDF title";

// save the pdf document
doc.Save(currentDir + "/../../../play_test.pdf");

// close the pdf document
doc.Close();

Console.WriteLine("Markdown to PDF conversion completed successfully");
