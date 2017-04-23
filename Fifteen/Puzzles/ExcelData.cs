using System.Data;

namespace Fifteen.Puzzles
{
    public class ExcelData
    {
        private int currow;
        public DataTable ReportData { get; set; }

        private int MaxRow
        {
            get
            {
                return this.ReportData.Rows.Count;
            }
        }

        public object this[string name]
        {
            get
            {
                if (this.currow > this.ReportData.Rows.Count)
                {
                    return string.Empty;
                }

                if (!this.ReportData.Columns.Contains(name))
                {
                    return string.Empty;
                }

                return this.ReportData.Rows[this.currow - 1][name] + " ";
            }
        }
        public bool Read()
        {
            if (this.ReportData == null)
            {
                return false;
            }

            if (this.currow <= this.MaxRow)
            {
                this.currow++;
            }

            return this.currow <= this.MaxRow;
        }
    }
}