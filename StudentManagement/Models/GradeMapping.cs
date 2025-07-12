using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace StudentManagement.Models
{
    public static class GradeMapping
    {
        private static readonly Dictionary<string, decimal> Points;
        private static readonly List<string> ValidGradesForEntry;
        private static readonly string XmlFilePath = HttpContext.Current.Server.MapPath("~/App_Data/GradeMappings.xml");

        static GradeMapping()
        {
            Points = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            ValidGradesForEntry = new List<string>();
            LoadGradeMappingsFromXml();
        }

        private static void LoadGradeMappingsFromXml()
        {
            try
            {
                XDocument doc = XDocument.Load(XmlFilePath);
                foreach (XElement gradeElement in doc.Root.Elements("grade"))
                {
                    string letter = gradeElement.Attribute("letter")?.Value;
                    string pointsStr = gradeElement.Attribute("points")?.Value;
                    bool isPlaceholder = Convert.ToBoolean(gradeElement.Attribute("isPlaceholder")?.Value ?? "false");

                    if (!string.IsNullOrEmpty(letter) && decimal.TryParse(pointsStr, out decimal pointValue))
                    {
                        Points[letter] = pointValue;
                        if (!isPlaceholder)
                        {
                            ValidGradesForEntry.Add(letter);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading GradeMappings.xml: " + ex.Message + ". Falling back to defaults.");
                LoadDefaultMappings();
            }
            if (!Points.Any()) LoadDefaultMappings();
        }

        private static void LoadDefaultMappings()
        {
            Points.Clear();
            ValidGradesForEntry.Clear();
            var defaultMap = new Dictionary<string, decimal> {
                {"A+", 4.00m}, {"A", 4.00m}, {"A-", 3.67m},
                {"B+", 3.33m}, {"B", 3.00m}, {"B-", 2.67m},
                {"C+", 2.33m}, {"C", 2.00m}, {"C-", 1.67m},
                {"D+", 1.33m}, {"D", 1.00m}, {"F", 0.00m}
            };
            foreach (var entry in defaultMap)
            {
                Points.Add(entry.Key, entry.Value);
                ValidGradesForEntry.Add(entry.Key);
            }
        }

        public static decimal GetGradePoint(string letterGrade)
        {
            if (string.IsNullOrWhiteSpace(letterGrade)) return 0.0m;
            return Points.TryGetValue(letterGrade.ToUpper(), out decimal point) ? point : 0.0m;
        }

        public static List<string> GetValidLetterGradesForEntry()
        {
            return new List<string>(ValidGradesForEntry);
        }
    }
}