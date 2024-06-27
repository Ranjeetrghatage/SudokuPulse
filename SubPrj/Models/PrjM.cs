using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubPrj.Models
{
    public class PrjM : BindableBase
    {

        private int rowNumber;
        private int columnNumber;
        private string textBoxText;

        public int RowNumber
        {
            get { return rowNumber; }
            set
            {
                rowNumber = value;
                RaisePropertyChanged(nameof(RowNumber));
            }
        }

        public int ColumnNumber
        {
            get { return columnNumber; }
            set
            {
                columnNumber = value;
                RaisePropertyChanged(nameof(ColumnNumber));
            }
        }

        public string TextBoxText
        {
            get { return textBoxText; }
            set
            {
                textBoxText = value;
                RaisePropertyChanged(nameof(TextBoxText));
            }
        }
    }
}
