using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace StudentManagement.Models
{
    public static class StatisticsManager
    {
        public static decimal CalculateAverageFromDataTable(DataTable dt, string columnName)
        {
            if (dt == null || dt.Rows.Count == 0 || !dt.Columns.Contains(columnName)) return 0.0m;

            List<decimal> values = new List<decimal>();
            foreach (DataRow row in dt.Rows)
            {
                if (row[columnName] != DBNull.Value && decimal.TryParse(row[columnName].ToString(), out decimal val))
                {
                    values.Add(val);
                }
            }
            return values.Any() ? values.Average() : 0.0m;
        }

        // You can add more complex statistical methods: Median, Mode, Standard Deviation etc.
        // For example, to calculate standard deviation:
        public static double CalculateStandardDeviation(List<decimal> values)
        {
            if (values == null || values.Count < 2) return 0.0;

            double avg = (double)values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => ((double)val - avg) * ((double)val - avg)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / (values.Count - 1)); // Sample StDev
        }
    }
}