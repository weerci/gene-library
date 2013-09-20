using System;
using System.Collections.Generic;
using System.Linq;
using WFDatabase;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using WFExceptions;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.ObjectModel;

namespace GeneLibrary.Items
{
    public class SortObjectVocabulary : Vocabulary
    {
        public SortObjectVocabulary() : base() { }
        public override void Open()
        {
            //ID	NUMBER(10)			
            //NAME	NVARCHAR2(128)			
            //SHORT_NAME NVARCHAR2(64)
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.sortTableID;
            DataColumn Name = new DataColumn("NAME", Type.GetType("System.String"));
            Name.Caption = resDataNames.sortTableName;
            DataColumn ShortName = new DataColumn("SHORT_NAME", Type.GetType("System.String"));
            ShortName.Caption = resDataNames.sortTableShortName;

            DT.Columns.Add(Id);
            DT.Columns.Add(Name);
            DT.Columns.Add(ShortName);

            _gate.Open(base.DT);
        }
        public override void Open(DataGridView dataGridView)
        {
            this.Open();
            dataGridView.Columns.Clear();
            dataGridView.DataSource = base.DT;
            foreach (DataColumn dc in base.DT.Columns)
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
            Item = new SortObjectItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new SortObjectItemInner(id);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "SHORT_NAME" };
        }
        public override string TableName()
        {
            return _gate.TableName();
        }

        public DataTable DivTable
        { 
            get 
            { 
                return DT; 
            } 
        }
        public SortObjectItem Item { get; set; }

        class SortObjectItemInner : SortObjectItem
        {
            public SortObjectItemInner() : base() { }
            public SortObjectItemInner(int id) : base(id) { }
        }
        private SortObjectVocabularyGate _gate = GateFactory.SortObjectVocabularyGate();
    }
    public class SortObjectItem 
    { 
        protected SortObjectItem(){}
        protected SortObjectItem(int id) 
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

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        private SortObjectItemGate _gate = GateFactory.SortObjectItemGate();
    }
    public abstract class SortObjectVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class SortObjectVocabularyGateOracle : SortObjectVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple("select id, name, short_name, id || name || short_name fhash from modern.sort_object order by name",
                dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.sort_del", ids, "a_id");

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
            return "modern.sort_object";   
        }
    }
    public abstract class SortObjectItemGate
    {
        public abstract void Open(SortObjectItem divItem);
        public abstract int Insert(SortObjectItem divItem);
        public abstract void Update(SortObjectItem divItem);
    }
    public class SortObjectItemGateOracle : SortObjectItemGate
    {
        public override void Open(SortObjectItem sortObjectItem)
        {
            String sql = "select id, name, short_name from modern.sort_object where id = :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, sortObjectItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    sortObjectItem.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    sortObjectItem.Name = rdr["name"].ToString();
                    sortObjectItem.ShortName = rdr["short_name"].ToString();
                }
            }
        }
        public override int Insert(SortObjectItem sortObjectItem)
        {    
            string sql = "begin :res := modern.prk_tab.sort_ins(:a_name, :a_short_name); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_name", OracleType.NVarChar, sortObjectItem.Name, cmd, false);
                WFOracle.AddInParameter("a_short_name", OracleType.NVarChar, sortObjectItem.ShortName, cmd, false);
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
        public override void Update(SortObjectItem sortObjectItem)
        {
            string sql = "modern.prk_tab.sort_upd";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                WFOracle.AddInParameter("a_id", OracleType.Number, sortObjectItem.Id, cmd, false);
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, sortObjectItem.Name, cmd, false);
                WFOracle.AddInParameter("a_short_name", OracleType.NVarChar, sortObjectItem.ShortName, cmd, false);

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
