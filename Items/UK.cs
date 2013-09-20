using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WFDatabase;
using System.Data.OracleClient;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.ObjectModel;

namespace GeneLibrary.Items
{
    public class UKVocabulary : Vocabulary
    {
        public UKVocabulary() : base(){ }
        public override void Open()
        {
            CreateDataTableColumns();
            _gate.Open(DT);
        }
        public override void Open(DataGridView dataGridView)
        {
            CreateDataTableColumns(); 
            _gate.Open(base.DT);
            if (dataGridView != null)
                FillDataGrid(dataGridView);
        }
        public override void Open(DataGridView dataGridView, string filter)
        {
            CreateDataTableColumns();
            _gate.Open(base.DT, filter);
            if (dataGridView != null)
                FillDataGrid(dataGridView);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "ARTCL" };
        }
        public override bool Del(int[] ids)
        {
            return _gate.Del(ids);
        }
        public override bool IsEmpty()
        {
            return DT == null || DT.Rows.Count == 0;
        }
        public override void LoadItem()
        {
            Item = new UKItemInner();
        }
        public override void LoadItem(int id)
        {
            DataView dw = DT.DefaultView;
            dw.RowFilter = "id = " + id;
            if (dw.Count > 0)
                Item = new UKItemInner(id, dw[0]["artcl"].ToString() != "0");
        }
        public override string TableName()
        {
            return _gate.TableName();
        }

        public DataTable UKTable
        {
            get
            {
                return DT;
            }
        }

        private UKVocabularyGate _gate = GateFactory.UKDictionaryGate();
        public UKItem Item { get; set; }

        // Private methods
        private void CreateDataTableColumns()
        {
            //ID	NUMBER(10)				
            //ARTCL	NVARCHAR2(3)
            //NOTE	NVARCHAR2(512)	
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.ukTableID;
            DataColumn Artcl = new DataColumn("ARTCL", Type.GetType("System.String"));
            DataColumn Note = new DataColumn("NOTE", Type.GetType("System.String"));
            Note.Caption = resDataNames.ukTableName;

            DT.Columns.Add(Id);
            DT.Columns.Add(Artcl);
            DT.Columns.Add(Note);
        }
        private void FillDataGrid(DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();
            dataGridView.DataSource = base.DT;
            foreach (DataColumn dc in DT.Columns)
                dataGridView.Columns[dc.ColumnName].HeaderText = dc.Caption;
            dataGridView.Columns["ARTCL"].Visible = false;
        }

        class UKItemInner : UKItem
        {
            public UKItemInner() : base() { }
            public UKItemInner(int id, bool isArtcl) : base(id, isArtcl) { }
        }
    }
    public class UKItem : IDisposable
    {
        protected UKItem() { }
        protected UKItem(int id, bool isArticle)
        {
            this.Id = id;
            this.IsArticle = isArticle;
        }

        public void Open()
        {
            _gate.Open(this);
        }
        public int Save()
        {
            if (this.Id == 0)
            {
                return _gate.Insert(this);
            }
            else
            {
                _gate.Update(this);
                return this.Id;
            }
        }
        
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Article { get; set; }
        public string Note { get; set; }
        public string Text { get; set; }
        public bool IsArticle { get; set; }
        public DataTable UKDataTable
        {
            get { return _dt; }
        }

        private UKItemGate _gate = GateFactory.UKItemGate();
        private DataTable _dt = new DataTable() { Locale = CultureInfo.InvariantCulture };

        // Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                UKDataTable.Dispose();
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public abstract class UKVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract void Open(DataTable dataTable, string filter);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public abstract class UKItemGate
    {
        public abstract void Open(UKItem ukItem);
        public abstract int Insert(UKItem ukItem);
        public abstract void Update(UKItem ukItem);
    }
    public class UKVocabularyGateOracle : UKVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple(
                "select id, artcl, note from modern.v_ukstate order by artcl",
                dataTable, "id");
        }
        public override void Open(DataTable dataTable, string filter)
        {
            OracleWork.OracleSqlSimple(
                String.Format("select id, artcl, note from modern.v_ukstate {0} order by artcl", filter),
                dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.uk_item_del", ids, "a_id");
                WFOracle.DB.Commit();
                return true;
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override string TableName()
        {
            return "modern.v_ukstate";
        }
    }
    public class UKItemGateOracle : UKItemGate
    {
        public override void Open(UKItem ukItem)
        {
            String sql;
            if (ukItem.IsArticle)
                sql =
                    " select i.id, level, i.parent_id, a.artcl, a.note from modern.ukarticl a, modern.ukitem i" +
                    " where a.id = i.id connect by prior i.id = i.parent_id start with i.id = :id";
            else
                sql =
                    " select i.id, level, i.parent_id, '0' artcl, t.note from modern.uktext t, modern.ukitem i" +
                    " where t.id = i.id connect by prior i.id = i.parent_id start with i.id = :id";

            OracleWork.OracleSqlById(sql, ukItem.UKDataTable, "id", ukItem.Id);

            ukItem.UKDataTable.Columns["id"].AutoIncrement = true;
            ukItem.UKDataTable.Columns["id"].AutoIncrementSeed = -1100;
            ukItem.UKDataTable.Columns["id"].AutoIncrementStep = -1;

            DataView dw = new DataView(ukItem.UKDataTable);
            dw.RowFilter = "id=" + ukItem.Id;
            if (dw.Count > 0)
            {
                ukItem.Article = dw[0]["artcl"].ToString();
                ukItem.IsArticle = ukItem.Article == "0";
                ukItem.Note = dw[0]["note"].ToString();
                ukItem.Text = dw[0]["note"].ToString();
                ukItem.ParentId = Convert.ToInt32(dw[0]["parent_id"], CultureInfo.InvariantCulture);
            }

        }
        public override int Insert(UKItem ukItem)
        {
            int res;
            string sql;
            if (ukItem.IsArticle)
                sql = "begin :res := modern.prk_tab.uk_artcl_ins(:a_artcl, :a_note, :a_parent_id); end;";
            else
                sql = "begin :res := modern.prk_tab.uk_text_ins(:a_note, :a_parent_id); end;";

            OracleParameter prmRes;
            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("a_artcl", OracleType.NVarChar);
                cmd.Parameters.Add("a_note", OracleType.NVarChar);
                cmd.Parameters.Add("a_parent_id", OracleType.Number);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);

                // Добавляем первую запись (root)
                DataView dw = new DataView(ukItem.UKDataTable);
                dw.RowFilter = "LEVEL = 1";
                cmd.Parameters["a_artcl"].Value = ukItem.Article;
                if (ukItem.IsArticle)
                    cmd.Parameters["a_note"].Value = ukItem.Note;
                else
                    cmd.Parameters["a_note"].Value = ukItem.Text;
                cmd.Parameters["a_parent_id"].Value = 0;
                cmd.ExecuteNonQuery();

                res = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);

                if (ukItem.IsArticle)
                {
                    dw.RowFilter = "LEVEL = 2";
                    for (int i = 0; i < dw.Count; i++)
                    {
                        cmd.Parameters["a_artcl"].Value = dw[i]["artcl"].ToString();
                        cmd.Parameters["a_note"].Value = dw[i]["note"].ToString();
                        cmd.Parameters["a_parent_id"].Value = Convert.ToInt32(dw[i]["a_parent_id"], CultureInfo.InvariantCulture);
                        cmd.ExecuteNonQuery();
                    }

                    dw.RowFilter = "LEVEL = 3";
                    for (int i = 0; i < dw.Count; i++)
                    {
                        cmd.Parameters["a_artcl"].Value = dw[i]["artcl"].ToString();
                        cmd.Parameters["a_note"].Value = dw[i]["note"].ToString();
                        cmd.Parameters["a_parent_id"].Value = Convert.ToInt32(dw[i]["a_parent_id"], CultureInfo.InvariantCulture);
                        cmd.ExecuteNonQuery();
                    }
                }

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return res;
        }
        public override void Update(UKItem ukItem)
        {
            string sql;
            if (ukItem.IsArticle)
                sql = "modern.prk_tab.uk_artcl_upd";
            else
                sql = "modern.prk_tab.uk_text_upd";

            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("a_id", OracleType.Number);
                if (ukItem.IsArticle)
                    cmd.Parameters.Add("a_artcl", OracleType.NVarChar);
                cmd.Parameters.Add("a_note", OracleType.NVarChar);
                cmd.Parameters.Add("a_parent_id", OracleType.Number);


                // Обновляем первую запись (root)
                DataView dw = new DataView(ukItem.UKDataTable);
                dw.RowFilter = "LEVEL = 1";
                if (ukItem.IsArticle)
                {
                    cmd.Parameters["a_artcl"].Value = ukItem.Article;
                    cmd.Parameters["a_note"].Value = ukItem.Note;
                }
                else
                    cmd.Parameters["a_note"].Value = ukItem.Text;
                cmd.Parameters["a_id"].Value = ukItem.Id;
                cmd.Parameters["a_parent_id"].Value = 0;
                cmd.ExecuteNonQuery();

                // Обновляем последующие записи
                if (ukItem.IsArticle)
                {
                    dw.RowFilter = "LEVEL = 2";
                    for (int i = 0; i < dw.Count; i++)
                    {
                        cmd.Parameters["a_id"].Value = Convert.ToInt32(dw[i]["id"], CultureInfo.InvariantCulture);
                        cmd.Parameters["a_artcl"].Value = dw[i]["artcl"].ToString();
                        cmd.Parameters["a_note"].Value = dw[i]["note"].ToString();
                        cmd.Parameters["a_parent_id"].Value = Convert.ToInt32(dw[i]["parent_id"], CultureInfo.InvariantCulture);
                        cmd.ExecuteNonQuery();
                    }

                    dw.RowFilter = "LEVEL = 3";
                    for (int i = 0; i < dw.Count; i++)
                    {
                        cmd.Parameters["a_id"].Value = Convert.ToInt32(dw[i]["id"].ToString(), CultureInfo.InvariantCulture);
                        cmd.Parameters["a_artcl"].Value = dw[i]["artcl"].ToString();
                        cmd.Parameters["a_note"].Value = dw[i]["note"].ToString();
                        cmd.Parameters["a_parent_id"].Value = Convert.ToInt32(dw[i]["parent_id"], CultureInfo.InvariantCulture);
                        cmd.ExecuteNonQuery();
                    }
                }

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
    }
}
