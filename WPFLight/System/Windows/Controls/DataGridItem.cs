using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Controls {
    public class DataGridItem {
        public DataGridItem ( params string[] cells ) {
            this.Cells = cells;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public string[] Cells { get; private set; }
    }
}
