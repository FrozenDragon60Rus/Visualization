using System;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace Visualization
{
    internal class ExcelReader
    {
        Application excel;
        Workbook book;
        Worksheet sheet;
        Range range, writeCells;
        public Range cells;
        public Point size = new Point();
        string fileName = "field" + ".xlsx";
        Color startColor = Color.FromArgb(0, 176, 240),
              endColor = Color.FromArgb(220, 220, 220),
              baseColor = Color.White,
              routeColor = Color.Red;

        public ExcelReader()
        {
            excel = new Application();
            Init();
        }

        private void Init()
        {
            if (File.Exists(fileName))
                book = excel.Workbooks.Open(Directory.GetCurrentDirectory() + @"\" + fileName);
            else
                book = excel.Workbooks.Add(Type.Missing);
            book.AfterSave += new WorkbookEvents_AfterSaveEventHandler(AfterSave);
            //Console.WriteLine(Directory.GetCurrentDirectory() + @"\" + fileName);
            sheet = book.Sheets["Лист1"];
            range = sheet.UsedRange;
            writeCells = book.Sheets["Лист2"].UsedRange.Cells;
            cells = range.Cells;
            size.x = cells.Rows.Count;
            size.y = cells.Columns.Count;
        }
        
        public float Read(int x, int y) =>
            Convert.ToSingle(cells[x, y].Value2);
        public void Write(Vector2 point, float value) => 
            writeCells[point.x, point.y].Value2 = value;

        public Color GetColor(int x, int y) => 
            ColorTranslator.FromOle((int)cells[x, y].Interior.Color);
        public bool IsSameColor(Color color1, Color color2) =>
            color1.R == color2.R &&
            color1.G == color2.G &&
            color1.B == color2.B;
        public void FindKeyPoint(out Vector2 startPoint, out Vector2 endPoint)
        {
            startPoint = Vector2.zero;
            endPoint = Vector2.zero;
            Color color;

            //Console.WriteLine("col^" + cells.Columns.Count + " row:" + cells.Rows.Count);
            foreach (Range cell in cells)
            {
                color = GetColor(cell.Row, cell.Column);
                //Console.WriteLine("color (" + cell.Column + "," + cell.Row + ") is " + color);
                if (IsSameColor(color, startColor))
                {
                    startPoint = new Vector2(cell.Row, cell.Column);
                    Console.WriteLine("start position " + startPoint.x + " " + startPoint.y);
                    continue;
                }
                if (IsSameColor(color, endColor))
                {
                    endPoint = new Vector2(cell.Row, cell.Column);
                    Console.WriteLine("end position " + endPoint.x + " " + endPoint.y);
                    continue;
                }
            }
        }
        public void Close() 
        {
            try
            {
                book.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
            
        private void AfterSave(bool success)
        {
            book.Close(false);
            excel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
        }
        public void Open()
        {
            excel.Run();
        }
        public void WriteRoute(Vector2[] point, Vector2 startPoint, Vector2 endPoint)
        {
            for (int i = 1; i < point.Length; i++)
                writeCells[point[i].x, point[i].y].Interior.Color = ColorTranslator.ToOle(routeColor);
            writeCells.Cells[startPoint.x, startPoint.y].Interior.Color = ColorTranslator.ToOle(startColor);
            writeCells.Cells[endPoint.x, endPoint.y].Interior.Color = ColorTranslator.ToOle(endColor);
        }
            
        public void Clear() =>
            writeCells.Clear();
    }
    internal struct Point
    {
        public float x, y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
