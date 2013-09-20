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
    public class PostVocabulary : Vocabulary
    {
        public PostVocabulary() : base(){}
        public override void Open()
        {
            //ID	NUMBER(10)				
            //NAME	NVARCHAR2(50)						
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.postTableID;
            DataColumn Name = new DataColumn("NAME", Type.GetType("System.String"));
            Name.Caption = resDataNames.postTableName;

            DT.Columns.Add(Id);
            DT.Columns.Add(Name);

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
            Item = new PostItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new PostItemInner(id);
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
        
        public PostItem Item { get; set; }

        class PostItemInner : PostItem
        {
            public PostItemInner() : base() { }
            public PostItemInner(int id) : base(id) { }
        }
        private PostVocabularyGate _gate = GateFactory.PostDictionaryGate();

    }
    public class PostItem 
    { 
        protected PostItem() {}
        protected PostItem(int id)
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

        private PostItemGate _gate = GateFactory.PostItemGate();
    }

    public abstract class PostVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class PostVocabularyGateOracle : PostVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple("select id, name, id||name fhash from modern.post order by name",
                dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.post_del", ids, "a_id");
                WFOracle.DB.Commit();
            }
            catch 
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return true;
        }
        public override string TableName()
        {
            return "modern.post";
        }
    }

    public abstract class PostItemGate
    {
        public abstract void Open(PostItem postItem);
        public abstract int Insert(PostItem postItem);
        public abstract void Update(PostItem postItem);
    }
    public class PostItemGateOracle : PostItemGate
    {
        public override void Open(PostItem postItem)
        {
            String sql = "select id, name from modern.post  where id = :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, postItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    postItem.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    postItem.Name = rdr["name"].ToString();
                }
            }
        }
        public override int Insert(PostItem postItem)
        {
            string sql = "begin :res := modern.prk_tab.post_ins(:a_name); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_name", OracleType.NVarChar, postItem.Name, cmd, false);
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
        public override void Update(PostItem postItem)
        {
            string sql = "modern.prk_tab.post_upd";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                WFOracle.AddInParameter("a_id", OracleType.Number, postItem.Id, cmd, false);
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, postItem.Name, cmd, false);

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
