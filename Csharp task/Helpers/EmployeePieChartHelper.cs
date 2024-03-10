using Csharp_task.Models;
using SkiaSharp;

namespace Csharp_task.Helpers
{
    public static class EmployeePieChartHelper
    {

        private static HashSet<SKColor> usedColors = new HashSet<SKColor>();

        public static byte[] GenerateEmployeePieChart(List<EmployeeDto> employees)
        {
            double totalHours = employees.Sum(emp => emp.HoursWorked);
            Dictionary<string, double> employeePercentages = employees.ToDictionary(emp => emp.EmployeeName, emp => (emp.HoursWorked / totalHours) * 100);
            int height = employees.Count() * 40;
            using (var surface = SKSurface.Create(new SKImageInfo(700, height)))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                SKRect rectangle = new SKRect(10, 10, 390, 390);
                float startAngle = 0;
                float legendTextSize = 20;
                float legendSpacing = 5;

                float centerX = rectangle.MidX;
                float centerY = rectangle.MidY;

                SKRect legendRect = new SKRect(420, 10, 590, 390);
                foreach (var kvp in employeePercentages)
                {
                    float sweepAngle = (float)(360 * kvp.Value / 100);
                    using (SKPaint paint = new SKPaint())
                    {
                        string percentageLabel = $"{Math.Round(kvp.Value)}%";

                        paint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

                        paint.Color = GetRandomColor(usedColors);
                        paint.Style = SKPaintStyle.Fill;
                        canvas.DrawArc(rectangle, startAngle, sweepAngle, true, paint);

                        SKRect legendEntryRect = new SKRect(legendRect.Left, legendRect.Top, legendRect.Left + legendTextSize, legendRect.Top + legendTextSize);
                        canvas.DrawRect(legendEntryRect, paint);
                        paint.Color = SKColors.Black;
                        paint.TextSize = legendTextSize;
                        canvas.DrawText($"{kvp.Key} {percentageLabel}", legendEntryRect.Right + legendSpacing, legendEntryRect.MidY + legendTextSize / 2, paint);


                        float midAngle = startAngle + sweepAngle / 2;
                        float sliceCenterX = centerX + centerX * 0.5f * (float)Math.Cos(Math.PI * midAngle / 180);
                        float sliceCenterY = centerY + centerY * 0.5f * (float)Math.Sin(Math.PI * midAngle / 180);
                        float x = sliceCenterX - paint.MeasureText($"{Math.Round(kvp.Value)}%") / 2;
                        float y = sliceCenterY + legendTextSize / 2;

                        canvas.DrawText(percentageLabel, x, y, paint);
                    }
                    startAngle += sweepAngle;
                    legendRect.Top += 40;
                    legendRect.Bottom += 40;
                }

                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var memoryStream = new MemoryStream())
                {
                    data.SaveTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
        private static SKColor GetRandomColor(HashSet<SKColor> usedColors)
        {
            Random random = new Random();

            SKColor newColor = SKColors.Transparent;
            while (true)
            {
                int h = random.Next(360);
                int s = random.Next(80, 100);
                int l = random.Next(60, 80);
                newColor = SKColor.FromHsl(h, s, l);
                if (usedColors.Add(newColor))
                {
                    break;
                }
            }

            return newColor;      
        }
    }
}
