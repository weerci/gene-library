using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFDatabase;
using System.Data;
using System.Data.OracleClient;
using WFExceptions;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.ObjectModel;

namespace GeneLibrary.Items
{
    public class LinVocabulary : Vocabulary
    {
        public LinVocabulary() : base() { }
        public override void Open()
        {
            //ID	NUMBER(10)				
            //CODE	NVARCHAR2(100)				
            //ORGAN	NVARCHAR2(500)				
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.linTableID;
            DataColumn Code = new DataColumn("CODE", Type.GetType("System.String"));
            Code.Caption = resDataNames.linTableCode;
            DataColumn Organ = new DataColumn("ORGAN", Type.GetType("System.String"));
            Organ.Caption = resDataNames.linTableOrgan;

            DT.Columns.Add(Id);
            DT.Columns.Add(Code);
            DT.Columns.Add(Organ);

            _gate.Open(DT);
        }
        public override void Open(DataGridView dataGridView)
        {
            this.Open();
            dataGridView.Columns.Clear();
            dataGridView.DataSource = base.DT;
            foreach (DataColumn dc in DT.Columns)
                dataGridView.Columns[dc.ColumnName].HeaderText = dc.Caption;
            dataGridView.Columns["fhash"].Visible = false;
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
            Item = new LinItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new LinItemInner(id);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "CODE" };
        }
        public override string TableName()
        {
            return _gate.TableName();
        }        

        public DataTable LinTable
        { 
            get 
            { 
                return DT; 
            } 
        }
        public LinItem Item { get; set; }

        class LinItemInner : LinItem
        {
            public LinItemInner() : base() { }
            public LinItemInner(int id) : base(id) { }
        }
        private LinVocabularyGate _gate = GateFactory.LinDictionaryGate();

    }
    public class LinItem 
    {
        protected LinItem() {}
        protected LinItem(int id) 
        {
            this.Id = id;
        }

        public void Open()
        {
            if (this.Id != 0)
                _gate.Open(this);
        }
        public int Save()
        {
            if (this.Id == 0)
                return _gate.Insert(this);
            else
            {
                _gate.Update(this);
                return this.Id;
            }
        }

        public string Code { get; set; }
        public string Organ { get; set; }
        public int Id { get; set; }

        private LinItemGate _gate = GateFactory.LinItemGate();
    }

    public abstract class LinVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class LinVocabularyGateOracle : LinVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple("select id, code, organ, id||code||organ fhash from modern.code_lin order by code",
                dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.lin_del", ids, "a_id");
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
            return "modern.code_lin";
        }
    }

    public abstract class LinItemGate
    {
        public abstract void Open(LinItem linItem);
        public abstract int Insert(LinItem linItem);
        public abstract void Update(LinItem linItem);
    }
    public class LinItemGateOracle : LinItemGate
    {
        public override void Open(LinItem linItem)
        {
            String sql = "select id, code, organ from modern.code_lin where id = :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, linItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    linItem.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    linItem.Code = rdr["code"].ToString();
                    linItem.Organ = rdr["organ"].ToString();
                }
            }
        }
        public override int Insert(LinItem linItem)
        {
            string sql = "begin :res := modern.prk_tab.lin_ins(:a_code, :a_organ); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_code", OracleType.NVarChar, linItem.Code, cmd, false);
                WFOracle.AddInParameter("a_organ", OracleType.NVarChar, linItem.Organ, cmd, false);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);

                cmd.ExecuteNonQuery();
                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
        }
        public override void Update(LinItem linItem)
        {
            string sql = "modern.prk_tab.lin_upd";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                WFOracle.AddInParameter("a_id", OracleType.Number, linItem.Id, cmd, false);
                WFOracle.AddInParameter("a_code", OracleType.NVarChar, linItem.Code, cmd, false);
                WFOracle.AddInParameter("a_organ", OracleType.NVarChar, linItem.Organ, cmd, false);

                cmd.ExecuteNonQuery();
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
