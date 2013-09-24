using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFDatabase;
using System.Data;
using System.Data.OracleClient;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.ObjectModel;

namespace GeneLibrary.Items
{
    public class MethodVocabulary : Vocabulary
    {
        public MethodVocabulary() : base(){}
        public override void Open()
        {
            //ID	NUMBER(10)			
            //NAME	NVARCHAR2(100)			
            //DESCRIPTION	NVARCHAR2(1024)		
            //DEF_FREQ	NUMBER
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.freqTableID;
            DataColumn Name = new DataColumn("NAME", Type.GetType("System.String"));
            Name.Caption = resDataNames.freqTableName;
            DataColumn Desc = new DataColumn("DESCRIPTION", Type.GetType("System.String"));
            Desc.Caption = resDataNames.freqTableDesc;
            DataColumn DefFreq = new DataColumn("DEF_FREQ", Type.GetType("System.String"));
            DefFreq.Caption = resDataNames.freqTableDefFreq;

            DT.Columns.Add(Id);
            DT.Columns.Add(Name);
            DT.Columns.Add(Desc);
            DT.Columns.Add(DefFreq);

            _gate.Open(DT);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "NAME" };
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
            Item = new MethItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new MethItemInner(id);
        }
        public override string TableName()
        {
            return _gate.TableName();
        }
        
        public DataTable ExpTable
        {
            get
            {
                return DT;
            }
        }
        public MethodItem Item { get; set; }

        class MethItemInner : MethodItem
        {
            public MethItemInner() : base() { }
            public MethItemInner(int id) : base(id) { } 
        }
        private MethodVocabularyGate _gate = GateFactory.MethodDictionaryGate();

    }
    public class MethodItem : IDisposable 
    {
        protected MethodItem() { }
        protected MethodItem(int id) 
        {
            this.Id = id;
        }

        public void Open()
        {
            this.Init();
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

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DefFreq { get; set; }

        public DataTable DTLocus
        {
            get
            {
                return _dtLocus;
            }
        }
        public DataTable DTAllele
        {
            get
            {
                return _dtAllele;
            }
        }

        private void Init()
        {
            DataColumn LocusId = new DataColumn("ID", Type.GetType("System.Int32"));
            DataColumn LocusName = new DataColumn("NAME", Type.GetType("System.String"));
            LocusName.Caption = resDataNames.freqLocusTableName;
            _dtLocus.Columns.Add(LocusId);
            _dtLocus.Columns.Add(LocusName);

            DataColumn AlleleId = new DataColumn("ID", Type.GetType("System.Int32"));
            DataColumn AlleleLocusId = new DataColumn("LOCUS_ID", Type.GetType("System.Int32"));
            DataColumn AlleleName = new DataColumn("NAME", Type.GetType("System.String"));
            AlleleName.Caption = resDataNames.freqAlleleTableName;
            DataColumn AlleleVal = new DataColumn("VAL", Type.GetType("System.Double"));
            AlleleVal.Caption = resDataNames.freqAlleleTableVal;
            DataColumn AlleleFreq = new DataColumn("FREQ", Type.GetType("System.String"));
            AlleleFreq.Caption = resDataNames.freqAlleleTableFreq;
            _dtAllele.Columns.Add(AlleleId);
            _dtAllele.Columns.Add(AlleleLocusId);
            _dtAllele.Columns.Add(AlleleName);
            _dtAllele.Columns.Add(AlleleVal);
            _dtAllele.Columns.Add(AlleleFreq);
        }
        private DataTable _dtLocus = new DataTable() { Locale = CultureInfo.InvariantCulture };
        private DataTable _dtAllele = new DataTable() { Locale = CultureInfo.InvariantCulture };
        private MethodItemGate _gate = GateFactory.MethodItemGate();

        // Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DTAllele.Dispose();
                DTLocus.Dispose();
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public abstract class MethodVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class MethodVocabularyGateOracle : MethodVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple(
                "select id, name, description, def_freq, id||name||description||def_freq fhash from modern.method order by name",
                dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.method_del", ids, "a_id");
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
            return "modern.method";
        }
    }

    public abstract class MethodItemGate
    {
        public abstract void Open(MethodItem freqItem);
        public abstract int Insert(MethodItem freqItem);
        public abstract void Update(MethodItem freqItem);
    }
    public class MethodItemGateOracle : MethodItemGate
    {
        public override void Open(MethodItem freqItem)
        { 
            String sql = "select id, name, description, def_freq from modern.method where id = :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, freqItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    freqItem.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    freqItem.Name = rdr["name"].ToString();
                    freqItem.Description = rdr["description"].ToString();
                    freqItem.DefFreq = Convert.ToDecimal(rdr["def_freq"], CultureInfo.InvariantCulture);
                }
            }

            OracleWork.OracleSqlById(
                " select a.id, a.locus_id, a.name, a.val, af.freq" +
                " from modern.allele a left join modern.allele_freq af on a.id = af.allele_id and af.method_id = :id",
                freqItem.DTAllele, "id", freqItem.Id);

            OracleWork.OracleSqlSimple(" select id, name from modern.locus order by name", freqItem.DTLocus, "id");

        }
        public override int Insert(MethodItem freqItem)
        {
            string sql = "begin :res := modern.prk_tab.method_ins(:a_name, :a_def_freq, :a_desc); end;";
            OracleParameter prmRes;
            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_name", OracleType.NVarChar, freqItem.Name, cmd, false);
                WFOracle.AddInParameter("a_def_freq", OracleType.Number, freqItem.DefFreq, cmd, false);
                WFOracle.AddInParameter("a_desc", OracleType.NVarChar, freqItem.Description, cmd, false);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
                cmd.ExecuteNonQuery();

                string sqlFreq = "begin modern.prk_tab.freq_ins(:a_allele_id, :a_method_id, :a_freq); end;";
                OracleCommand cmdFreq = new OracleCommand(sqlFreq, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmdFreq.CommandType = CommandType.Text;

                cmdFreq.Parameters.Clear();
                cmdFreq.Parameters.Add("a_allele_id", OracleType.Number);
                cmdFreq.Parameters.Add("a_method_id", OracleType.Number);
                cmdFreq.Parameters.Add("a_freq", OracleType.Number);
                foreach (DataRow row in freqItem.DTAllele.Rows)
                {
                    decimal d;
                    if (Decimal.TryParse(row["freq"].ToString().Replace('.', _separator), out d))
                    {
                        cmdFreq.Parameters["a_allele_id"].Value = Convert.ToInt32(row["id"], CultureInfo.InvariantCulture);
                        cmdFreq.Parameters["a_method_id"].Value = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
                        cmdFreq.Parameters["a_freq"].Value = d;
                        cmdFreq.ExecuteNonQuery();
                    }
                }
                
                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
        }
        public override void Update(MethodItem freqItem)
        {
            //procedure method_upd(a_id in number, a_name in nvarchar2, a_def_freq in number, a_desc in nvarchar2) is
            string sql = "modern.prk_tab.method_upd";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                WFOracle.AddInParameter("a_id", OracleType.Number, freqItem.Id, cmd, false);
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, freqItem.Name, cmd, false);
                WFOracle.AddInParameter("a_def_freq", OracleType.Number, freqItem.DefFreq, cmd, false);
                WFOracle.AddInParameter("a_desc", OracleType.NVarChar, freqItem.Description, cmd, false);
                cmd.ExecuteNonQuery();


                OracleWork.OracleDeleteSimple("modern.prk_tab.methods_freq_del", new int[] {freqItem.Id}, "a_id");

                cmd.CommandText = "begin modern.prk_tab.freq_ins(:a_allele_id, :a_method_id, :a_freq); end;";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("a_allele_id", OracleType.Number);
                cmd.Parameters.Add("a_method_id", OracleType.Number);
                cmd.Parameters.Add("a_freq", OracleType.Number);

                foreach (DataRow row in freqItem.DTAllele.Rows)
                {
                    decimal d;
                    if (Decimal.TryParse(row["freq"].ToString().Replace('.', _separator), out d))
                    {
                        cmd.Parameters["a_allele_id"].Value = Convert.ToInt32(row["id"], CultureInfo.InvariantCulture);
                        cmd.Parameters["a_method_id"].Value = freqItem.Id;
                        cmd.Parameters["a_freq"].Value = d;
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

        #region

        private Char _separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];

        #endregion
    }
}
