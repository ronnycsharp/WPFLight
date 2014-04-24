using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Controls {
    public class DataGridItemColleciton : List<DataGridItem> {
        public void Add ( params string[] cells ) {
            this.Add ( new DataGridItem ( cells ) );
        }
    }
}
