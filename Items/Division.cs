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
    public class DivisionVocabulary : Vocabulary
    {
        public DivisionVocabulary() : base() { }
        public override void Open()
        {
            //ID	NUMBER(10)			
            //NAME	NVARCHAR2(50)			
            //ADDRESS	NVARCHAR2(500)			
            //PHONE	NVARCHAR2(20)							
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.divTableID;
            DataColumn Name = new DataColumn("NAME", Type.GetType("System.String"));
            Name.Caption = resDataNames.divTableName;
            DataColumn Address = new DataColumn("ADDRESS", Type.GetType("System.String"));
            Address.Caption = resDataNames.divTableAddress;
            DataColumn Phone = new DataColumn("PHONE", Type.GetType("System.String"));
            Phone.Caption = resDataNames.divTablePhone;

            DT.Columns.Add(Id);
            DT.Columns.Add(Name);
            DT.Columns.Add(Address);
            DT.Columns.Add(Phone);

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
            Item = new DivisionItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new DivisionItemInner(id);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "NAME" };
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
        public DivisionItem Item { get; set; }

        class DivisionItemInner : DivisionItem
        {
            public DivisionItemInner() : base() { }
            public DivisionItemInner(int id) : base(id) { }
        }
        private DivisionVocabularyGate _gate = GateFactory.DivisionDictionaryGate();
    }
    public class DivisionItem 
    { 
        protected DivisionItem(){}
        protected DivisionItem(int id) 
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
        public string Address { get; set; }
        public string Phone { get; set; }

        private DivisionItemGate _gate = GateFactory.DivisionItemGate();
    }
    public abstract class DivisionVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class DivisionVocabularyGateOracle : DivisionVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple("select id, name, address, phone, id||name||address||phone fhash from modern.division order by name",
                dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.division_del", ids, "a_id");

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
            return "modern.division";
        }
    }
    public abstract class DivisionItemGate
    {
        public abstract void Open(DivisionItem divItem);
        public abstract int Insert(DivisionItem divItem);
        public abstract void Update(DivisionItem divItem);
    }
    public class DivisionItemGateOracle : DivisionItemGate
    {
        public override void Open(DivisionItem divItem)
        {
            String sql = "select id, name, address, phone from modern.division where id = :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, divItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    divItem.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    divItem.Name = rdr["name"].ToString();
                    divItem.Address = rdr["address"].ToString();
                    divItem.Phone = rdr["phone"].ToString();
                }
            }
        }
        public override int Insert(DivisionItem divItem)
        {    
            string sql = "begin :res := modern.prk_tab.division_ins(:a_name, :a_address, :a_phone); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_name", OracleType.NVarChar, divItem.Name, cmd, false);
                WFOracle.AddInParameter("a_address", OracleType.NVarChar, divItem.Address, cmd, false);
                WFOracle.AddInParameter("a_phone", OracleType.NVarChar, divItem.Phone, cmd, false);
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
        public override void Update(DivisionItem divItem)
        {
            string sql = "modern.prk_tab.division_upd";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                WFOracle.AddInParameter("a_id", OracleType.Number, divItem.Id, cmd, false);
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, divItem.Name, cmd, false);
                WFOracle.AddInParameter("a_address", OracleType.NVarChar, divItem.Address, cmd, false);
                WFOracle.AddInParameter("a_phone", OracleType.NVarChar, divItem.Phone, cmd, false);

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
