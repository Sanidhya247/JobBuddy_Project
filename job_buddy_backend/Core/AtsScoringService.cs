using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging; // For parsing Word documents
using Microsoft.Extensions.Logging;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System.Text;

namespace job_buddy_backend.Core
{
    public class AtsScoringService
    {
        private readonly MLContext _mlContext = new MLContext();
        private readonly ILogger<AtsScoringService> _logger;

        public AtsScoringService(ILogger<AtsScoringService> logger)
        {
            _logger = logger;
        }

        public double CalculateAtsScore(string resumePath, string jobDescription)
        {
            try
            {
                _logger.LogInformation("Starting ATS score calculation.");

                // Extract resume text based on the file format
                string resumeText = ExtractResumeText(resumePath);

                if (string.IsNullOrEmpty(resumeText))
                {
                    _logger.LogWarning("Extracted resume text is empty.");
                    throw new InvalidOperationException("Unable to extract text from the provided resume.");
                }

                _logger.LogInformation("Resume and Job Description text extracted successfully.");

                // Define the input data as a list of strings
                var data = new List<TextData>
                {
                    new TextData { Text = resumeText },
                    new TextData { Text = jobDescription }
                };

                // Load data into an ML.NET data view
                var dataView = _mlContext.Data.LoadFromEnumerable(data);

                // Define text processing pipeline: TF-IDF vectorization
                var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(TextData.Text));

                // Fit the pipeline to the data and transform it to get vectorized features
                var transformer = pipeline.Fit(dataView);
                var transformedData = transformer.Transform(dataView);

                // Extract the feature vectors for resume and job description
                var features = _mlContext.Data.CreateEnumerable<TransformedTextData>(transformedData, reuseRowObject: false).ToArray();

                if (features.Length < 2 || features[0].Features.Length == 0 || features[1].Features.Length == 0)
                {
                    _logger.LogWarning("Feature extraction resulted in empty vectors.");
                    throw new InvalidOperationException("Failed to extract meaningful features from the resume or job description.");
                }

                // Calculate cosine similarity between the two feature vectors
                var similarityScore = CosineSimilarity(features[0].Features, features[1].Features) * 100;  // Convert to percentage

                _logger.LogInformation($"Calculated ATS Score: {similarityScore}%");
                return Math.Round(similarityScore, 2);  // Round to 2 decimal places for readability
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during ATS score calculation.");
                throw;
            }
        }

        private string ExtractResumeText(string resumePath)
        {
            string extension = System.IO.Path.GetExtension(resumePath).ToLower();
            _logger.LogInformation($"Extracting text from resume with extension: {extension}");

            switch (extension)
            {
                case ".pdf":
                    return ExtractTextFromPdf(resumePath);
                case ".docx":
                    return ExtractTextFromDocx(resumePath);
                case ".txt":
                    return File.ReadAllText(resumePath);
                default:
                    throw new NotSupportedException("Unsupported file format. Please upload a PDF, DOCX, or TXT file.");
            }
        }

        private string ExtractTextFromPdf(string pdfPath)
        {
            _logger.LogInformation("Starting PDF text extraction with iText7.");
            try
            {
                using (PdfReader pdfReader = new PdfReader(pdfPath))
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                {
                    StringBuilder text = new StringBuilder();
                    for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                    {
                        var page = pdfDocument.GetPage(i);
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        text.AppendLine(PdfTextExtractor.GetTextFromPage(page, strategy));
                    }
                    return text.ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting text from PDF.");
                return string.Empty;
            }
        }


        private string ExtractTextFromDocx(string docxPath)
        {
            _logger.LogInformation("Starting DOCX text extraction.");
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(docxPath, false))
                {
                    return wordDoc.MainDocumentPart.Document.Body.InnerText;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting text from DOCX.");
                return string.Empty;
            }
        }

        private static double CosineSimilarity(VBuffer<float> vectorA, VBuffer<float> vectorB)
        {
            try
            {
                // Extract dense representations of the vectors
                var spanA = vectorA.GetValues();
                var spanB = vectorB.GetValues();

                // Ensure vectors are of equal length
                int length = Math.Min(spanA.Length, spanB.Length);

                // Calculate dot product and magnitudes
                double dotProduct = 0.0;
                double magnitudeA = 0.0;
                double magnitudeB = 0.0;

                for (int i = 0; i < length; i++)
                {
                    dotProduct += spanA[i] * spanB[i];
                    magnitudeA += spanA[i] * spanA[i];
                    magnitudeB += spanB[i] * spanB[i];
                }

                if (magnitudeA == 0 || magnitudeB == 0)
                {
                    return 0.0; // Handle cases where magnitude is zero to avoid division by zero
                }

                // Calculate cosine similarity
                return dotProduct / (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
            }
            catch (Exception ex)
            {
                // Log the exception and return a similarity of 0
                Console.WriteLine($"Error calculating cosine similarity: {ex.Message}");
                return 0.0;
            }
        }

        public class TextData
        {
            public string Text { get; set; }
        }

        public class TransformedTextData : TextData
        {
            public VBuffer<float> Features { get; set; }
        }
    }
}