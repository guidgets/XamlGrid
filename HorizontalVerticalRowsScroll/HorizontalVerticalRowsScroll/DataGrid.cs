using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HorizontalVerticalRowsScroll
{
   
    public class DataGrid : ItemsControl
    {
        private Panel panel;
        static DataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGrid), new FrameworkPropertyMetadata(typeof(DataGrid)));
        }

        public DataGrid()
        {
            this.Columns = new ColumnsCollection();
            double columnWidth = 100;
            for (int i = 0; i < 10; i++)
            {
                Column column = new Column();
                this.Columns.Add(column);
            }

            double columnOffset = 0;
            foreach (var column in this.Columns)
            {
                column.Width = columnWidth;
                column.Offset = columnOffset;
                columnOffset += columnWidth;
            }
        }

        public ColumnsCollection Columns { get; private set; }

        
    }
}
