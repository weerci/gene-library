using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLibrary.Common;
using System.Windows.Forms;

namespace GeneLibrary.Items.Research
{
    class ResearchExcelDecorator : ResearchDecorator
    {
        public ResearchExcelDecorator(Research research, string excelTemplatePath) : base(research)
        {
            this.excelTemplatePath = excelTemplatePath;
        }
        public override void DirectIdent(string cardId, string fildName)
        {
            this.DirectIdent(Tools.GetIntFromText(cardId, fildName));
        }
        public override void DirectIdent(int cardId)
        {
            base.DirectIdent(cardId);
            MessageBox.Show("fff");
        }
        private string excelTemplatePath;
    }
}
