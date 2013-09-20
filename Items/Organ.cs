using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WFDatabase;
using System.Data.OracleClient;
using System.Globalization;
using System.Collections.ObjectModel;

namespace GeneLibrary.Items
{
    public class OrganizationVocabulary : Vocabulary
    {
        // Constructors
        public OrganizationVocabulary() : base() { }
        
        public override void Open()
        {
            //ID	NUMBER(10)			
            //NOTE	NVARCHAR2(1024)				
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.orgTableID;
            DataColumn Note = new DataColumn("NOTE", Type.GetType("System.String"));
            Note.Caption = resDataNames.orgTableName;

            DT.Columns.Add(Id);
            DT.Columns.Add(Note);

            _gate.Open(base.DT);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "NOTE" };
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
            Item = new OrganizationItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new OrganizationItemInner(id);
        }
        public override string TableName()
        {
            return _gate.TableName();
        }

        public DataTable OrgTable
        {
            get
            {
                return DT;
            }
        }
        public OrganizationItem Item { get; set; }

        class OrganizationItemInner : OrganizationItem
        {
            public OrganizationItemInner() : base() { }
            public OrganizationItemInner(int id) : base(id) { }
        }
        private OrganizationVocabularyGate _gate = GateFactory.OrganizationDictionaryGate();
    }
    public class OrganizationItem
    {
        protected OrganizationItem() { }
        protected OrganizationItem(int id)
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
        public string Note { get; set; }

        private OrganizationItemGate _gate = GateFactory.OrganizationItemGate();
    }
    public abstract class OrganizationVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class OrganizationVocabularyGateOracle : OrganizationVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple("select id, note, id||note fhash from modern.organization", dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.org_del", ids, "a_id");

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
            return "modern.organization";
        }
    }
    
    public abstract class OrganizationItemGate
    {
        public abstract void Open(OrganizationItem organizationItem);
        public abstract int Insert(OrganizationItem organizationItem);
        public abstract void Update(OrganizationItem organizationItem);
    }
    public class OrganizationItemGateOracle : OrganizationItemGate
    {
        public override void Open(OrganizationItem organizationItem)
        {
            String sql = "select id, note, id||note fhash from modern.organization where id = :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, organizationItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    organizationItem.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    organizationItem.Note = rdr["note"].ToString();
                }
            }
        }
        public override int Insert(OrganizationItem organizationItem)
        {
            string sql = "begin :res := modern.prk_tab.org_ins(:a_note); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_note", OracleType.NVarChar, organizationItem.Note, cmd, false);
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
        public override void Update(OrganizationItem organizationItem)
        {
            string sql = "modern.prk_tab.org_upd";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                WFOracle.AddInParameter("a_id", OracleType.Number, organizationItem.Id, cmd, false);
                WFOracle.AddInParameter("a_note", OracleType.NVarChar, organizationItem.Note, cmd, false);

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
