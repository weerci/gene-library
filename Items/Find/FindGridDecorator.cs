using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace GeneLibrary.Items.Find
{
    class FindGridDecorator : FindDecorator
    {
        public FindGridDecorator(FindCoincidence findCoincidence, DataGridView dataGridView) : base(findCoincidence)
        {
            this.dataGridView = dataGridView;
        }

        public override void FindChildByParents()
        {
            base.FindChildByParents();
            FillGrid();
        }
        public override void FindOneParent()
        {
            base.FindOneParent();
            FillGrid();
        }
        public override void FindOnHalf()
        {
            base.FindOnHalf();
            FillGrid();
        }
        public override void FindOnMinimum()
        {
            base.FindOnMinimum();
            FillGrid();
        }
        public override void FindIklForBlend()
        {
            base.FindIklForBlend();
            FillGrid();
        }
        public override void FindTwoIklForBlend()
        {
            base.FindTwoIklForBlend();
            FillGrid();
        }

        public override System.Data.DataTable FindResult
        {
            get
            {
                return base.FindResult;
            }
        }
        public override FindCondition FoundCondition
        {
            get
            {
                return base.FoundCondition;
            }
        }

        // Private methods
        private void FillGrid()
        {
            dataGridView.DataSource = base.FindResult;
            foreach (DataColumn dc in base.FindResult.Columns)
                dataGridView.Columns[dc.ColumnName].HeaderText = dc.Caption;
            dataGridView.Columns["card_type"].Visible = false;

            dataGridView.Columns[0].HeaderText += String.Format(resDataNames.formFindCountOnHeader,
                base.FindResult.Rows.Count);
        }

        // Fields
        DataGridView dataGridView;
    }
}
