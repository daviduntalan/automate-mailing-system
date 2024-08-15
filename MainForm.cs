using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace AutomateMailingOfBirForm {
    public partial class MainForm : Form {

        private int pageToExtract = 0; // initializes by search engine
        private string extractedPage;
        private Dictionary<String, String> persons;
        private bool enableSending = false;

        public MainForm() {
            InitializeComponent();
        }

        private void btnImportPdfFile_Click(object sender, EventArgs e) {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // openFileDialog1.InitialDirectory = "C:\\Users\\david\\source\\repos\\AutomateMailingOfBirForm\\assets";
            openFileDialog1.InitialDirectory = "\\";
            openFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtPdfSourceFile.Text = openFileDialog1.FileName.ToString();
                    lstFoundNames.Items.Add("Start processing...");
                    StartSearchProcess();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void StartSearchProcess() {

            if (txtPdfSourceFile.Text.Length == 0)
            {
                MessageBox.Show("Please provide the source PDF file to process. Try using Import File button. Thank you.",
                    "No PDF Source File Provided.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var person in persons)
            {
                lstFoundNames.Items.Add("searching... " + person.Value + " - " + person.Key);
                SearchNamesInPdf(person);
            }
        }

        // DoWork event handler: Executes the email sending on a separate thread
        private void EmailWorker_DoWork(object sender, DoWorkEventArgs e) {

            // Retrieve the parameters
            NameEmailParameters p = (NameEmailParameters)e.Argument;

            // Call the method to send the email
            SendEmailWithAttachment(p.SenderEmail, p.SenderPassword, p.RecipientEmail, p.ExtractedPage);
            e.Result = p.RecipientEmail; /* set identity for each worker - for later retrieval purposes */
        }

        // RunWorkerCompleted event handler: Called when the email sending is completed
        private void EmailWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {

            /* BackgroundWorker worker = sender as BackgroundWorker; */
            if (e.Error != null)
            {
                lstFoundNames.Items.Add("An error occurred: " + e.Error.Message);
            }
            else
            {
                lstFoundNames.Items.Add("Email was sent successfully to - " + e.Result);
            }
            pageToExtract = 0; // reset
        }

        private void SearchNamesInPdf(KeyValuePair<string, string> person) {

            string searchName = person.Value;
            // string recipientEmail = "daviduntalan@gmail.com"; 
            string recipientEmail = person.Key;
            string senderEmail = "aimglobal2013@gmail.com";
            string senderPassword = "rjbcxpoxkfjahzzh";
            string pdfSourceFile = txtPdfSourceFile.Text;

            if (pdfSourceFile.Length == 0)
            {
                lstFoundNames.Items.Add("No PDF files found in the current directory.");
                return;
            }

            bool foundInFile = SearchTextInPdf(pdfSourceFile, searchName);

            if (foundInFile && pageToExtract > 0)
            {
                lstFoundNames.Items.Add($"The name '{searchName}' was found in page: {pageToExtract}");
                ExtractPageFromPdf(pdfSourceFile, pageToExtract);

                if (enableSending)
                {
                    ProcessEmailThreadSending(senderEmail, senderPassword, recipientEmail);
                }
            }
            else
            {
                lstFoundNames.Items.Add($"The name '{searchName}' was NOT found.");
            }
        }

        private void ProcessEmailThreadSending(string senderEmail, string senderPassword, string recipientEmail) {
            // Initialize and configure BackgroundWorker
            BackgroundWorker emailWorker = new BackgroundWorker();
            emailWorker.DoWork += EmailWorker_DoWork;
            emailWorker.RunWorkerCompleted += EmailWorker_RunWorkerCompleted;

            // Create a parameters object to pass to the worker
            var nameEmailParameters = new NameEmailParameters
            {
                SenderEmail = senderEmail,
                SenderPassword = senderPassword,
                RecipientEmail = recipientEmail,
                ExtractedPage = extractedPage
            };

            // Run the BackgroundWorker - starts EmailWorker_DoWork() thread with the following arguments
            emailWorker.RunWorkerAsync(nameEmailParameters);

            // Your main thread can continue with other tasks
            lstFoundNames.Items.Add("Email is being sent in the background. You can perform other tasks.");

            /* Replaced with background worker -- EmailWorker_DoWork and EmailWorker_RunWorkerCompleted
            SendEmailWithAttachment(senderEmail, senderPassword, recipientEmail, extractedPage);                
            pageToExtract = 0; // reset */
        }

        // Send the PDF file as an attachment
        private void SendEmailWithAttachment(string senderEmail, string senderPassword,
            string recipientEmail, string extractedPage) {
            string attachmentPath = extractedPage;
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail);
                    mail.To.Add(recipientEmail);
                    mail.Subject = "Your BIR Form";
                    mail.Body = $"Your name was found in our import source PDF file.";
                    mail.Attachments.Add(new Attachment(attachmentPath));
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                    lstFoundNames.Items.Add($"Email sent to '{recipientEmail}' with " +
                        $"the attachment of: {System.IO.Path.GetFileName(attachmentPath)}");
                }
            }
            catch (Exception ex)
            {
                /* can't call from thread worker */
                Console.Error.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        // Extract the specific page
        private void ExtractPageFromPdf(string pdfSourceFile, int pageNumberToExtract) {
            try
            {
                using (PdfReader reader = new PdfReader(pdfSourceFile))
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document();
                    extractedPage = "BIR-Form-" + pageNumberToExtract + ".pdf"; /* modifies the global variable to use */
                    using (FileStream fs = new FileStream(extractedPage, FileMode.Create))
                    {
                        PdfCopy copy = new PdfCopy(document, fs);
                        document.Open();

                        // Ensure the page number is within the valid range
                        if (pageNumberToExtract > 0 && pageNumberToExtract <= reader.NumberOfPages)
                        {
                            // Add the specified page to the new PDF
                            copy.AddPage(copy.GetImportedPage(reader, pageNumberToExtract));
                            lstFoundNames.Items.Add($"Page {pageNumberToExtract} extracted to {extractedPage}");
                        }
                        else
                        {
                            lstFoundNames.Items.Add("Invalid page number.");
                        }

                        document.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                lstFoundNames.Items.Add($"Error extracting page: {ex.Message}");
            }
        }

        private bool SearchTextInPdf(string pdfSourceFile, string searchName) {
            try
            {
                using (PdfReader reader = new PdfReader(pdfSourceFile))
                {
                    for (int page = 1; page <= reader.NumberOfPages; page++)
                    {
                        string pageText = PdfTextExtractor.GetTextFromPage(reader, page);
                        if (pageText.ToLower().Contains(searchName.ToLower()))
                        {
                            pageToExtract = page;
                            return true; // The text was found in this PDF file
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstFoundNames.Items.Add(
                    $"Error reading PDF file '{System.IO.Path.GetFileName(pdfSourceFile)}': {ex.Message}"
                );
            }

            return false; // The text was not found in this PDF file
        }

        private void btnClose_Click(object sender, EventArgs e) {
            /*
            Choosing the Right Method
            Single Form Application: - this.Close() is usually sufficient if you're only dealing with one form.            
            Multi-Form Application:  - Use Application.Exit() if you want to ensure all forms are closed and the application exits.           
            Immediate Exit:          - Environment.Exit(0) is the most abrupt method, bypassing all cleanup events.            
            Prompting Before Exit:   - Handle the FormClosing event to prompt the user for confirmation or to save data before exiting.
            */
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            LoadNamesFromExcel();
        }

        private void LoadNamesFromExcel() {
            // Dictionary to store Name and Email Address
            persons = new Dictionary<string, string>();
            /* = new Dictionary<String, String>() {
                { "benju@riway.com", "CORPUZ, BENJU" },
                { "bnjcrps@gmail.com", "ESTEBAN SARAH JANE SO" },
                { "benjusokor@gmail.com", "MANZANO MERLYN BIENE" }
            }; */

            // Path to your Excel file
            // string filePath = @"path_to_your_file.xlsx";
            // string filePath = "C:\\Users\\david\\source\\repos\\AutomateMailingOfBirForm\\assets\\sample names and emails.xlsx";
            string filePath = "sample names and emails.xlsx";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "\\";
            openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Please select the Spreadsheet to use as basis for Name searches and Email.";

            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                try { filePath = openFileDialog1.FileName.ToString(); }
                catch (Exception ex) {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    Application.Exit();
                }
            } 
            else {
                MessageBox.Show("Error: Please provide your spreadsheet file to use.");
                Application.Exit();
            }

            // Load the Excel file
            FileInfo fileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                // Get the first worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Loop through the rows (assuming the first row contains headers)
                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++) // Start at row 2 to skip headers
                {
                    // Read Name (Column 1) and Email Address (Column 3)
                    string fullname = worksheet.Cells[row, 1].Text; // Column A - NAME
                    string memberId = worksheet.Cells[row, 2].Text; // Column B - MEMBER ID
                    string emailAdd = worksheet.Cells[row, 3].Text; // Column C - EMAIL ADDRESS
                    string position = worksheet.Cells[row, 4].Text; // Column D - POSITION

                    // Add to dictionary
                    if (!string.IsNullOrWhiteSpace(fullname) && !string.IsNullOrWhiteSpace(emailAdd))
                    {
                        persons[emailAdd] = fullname;
                    }
                }
            }

            // Display the dictionary contents
            foreach (var entry in persons)
            {
                lstFoundNames.Items.Add($"Name: {entry.Value} - Email: {entry.Key}");
            }
        }

        private void btnSendToAll_Click(object sender, EventArgs e) {
            enableSending = true;
            StartSearchProcess();
            enableSending = false;
        }
    }

    internal class NameEmailParameters {
        public string RecipientEmail { get; set; }
        public string SenderEmail { get; set; }
        public string ExtractedPage { get; set; }
        public string SenderPassword { get; set; }
    }
}
