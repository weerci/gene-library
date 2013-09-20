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
    /// <summary>
    /// Список кодов МВД
    /// </summary>
    public class MvdVocabulary : Vocabulary
    {
        public MvdVocabulary() : base(){}
        public override void Open()
        {
            //ID         NUMBER(10) not null,
            //NAME       NVARCHAR2(100) not null,
            //SHORT_NAME NVARCHAR2(100) not null,
            //CODE       NVARCHAR2(4) not null		
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.mvdTableID;
            DataColumn Name = new DataColumn("NAME", Type.GetType("System.String"));
            Name.Caption = resDataNames.mvdTableName;
            DataColumn ShortName = new DataColumn("SHORT_NAME", Type.GetType("System.String"));
            ShortName.Caption = resDataNames.mvdTableShortName;
            DataColumn Code = new DataColumn("CODE", Type.GetType("System.String"));
            Code.Caption = resDataNames.mvdTableCode;

            DT.Columns.Add(Id);
            DT.Columns.Add(Name);
            DT.Columns.Add(ShortName);
            DT.Columns.Add(Code);

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
            Item = new MvdItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new MvdItemInner(id);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "SHORT_NAME" };
        }
        public override string TableName()
        {
            return _gate.TableName();
        }

        public DataTable MvdTable 
        { 
            get 
            { 
                return DT; 
            } 
        }
        public MvdItem Item { get; set; }

        class MvdItemInner : MvdItem
        {
            public MvdItemInner() : base() { }
            public MvdItemInner(int id) : base(id) { }
        }
        private MvdVocabularyGate _gate = GateFactory.MvdDictionaryGate();

    }

    /// <summary>
    /// Элемент списка кодов
    /// </summary>
    public class MvdItem 
    { 
        protected MvdItem() {}
        protected MvdItem(int id)
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
        public string Code { get; set; }

        private MvdItemGate _gate = GateFactory.MvdItemGate();
    }

    /// <summary>
    /// Интерфейс списка кодов МВД
    /// </summary>
    public abstract class MvdVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    /// <summary>
    /// Интерфейс элемента кода МВД
    /// </summary>
    public abstract class MvdItemGate
    {
        public abstract void Open(MvdItem mvdItem);
        public abstract int Insert(MvdItem mvdItem);
        public abstract void Update(MvdItem mvdItem);
    }

    /// <summary>
    /// Реализация списка кодов для Oracle
    /// </summary>
    public class MvdVocabularyGateOracle : MvdVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple(
                "select id, name, short_name, code, id||code||short_name||name fhash from modern.code_mvd order by code",
                dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.mvd_del", ids, "a_id");
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
            return "modern.code_mvd";
        }
    }
    /// <summary>
    /// Реализация элемента кода МВД
    /// </summary>
    public class MvdItemGateOracle : MvdItemGate
    {
        public override void Open(MvdItem mvdItem)
        {
            String sql = "select id, code, short_name, name from modern.code_mvd where id = :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, mvdItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    mvdItem.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    mvdItem.Code = rdr["code"].ToString();
                    mvdItem.ShortName = rdr["short_name"].ToString();
                    mvdItem.Name = rdr["name"].ToString();
                }
            }
        }
        public override int Insert(MvdItem mvdItem)
        {
            string sql = "begin :res := modern.prk_tab.mvd_ins(:a_name, :a_short_name, :a_code); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_name", OracleType.NVarChar, mvdItem.Name, cmd, false);
                WFOracle.AddInParameter("a_short_name", OracleType.NVarChar, mvdItem.ShortName, cmd, false);
                WFOracle.AddInParameter("a_code", OracleType.NVarChar, mvdItem.Code, cmd, false);
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
        public override void Update(MvdItem mvdItem)
        {
            string sql = "modern.prk_tab.mvd_upd";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                WFOracle.AddInParameter("a_id", OracleType.Number, mvdItem.Id, cmd, false);
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, mvdItem.Name, cmd, false);
                WFOracle.AddInParameter("a_short_name", OracleType.NVarChar, mvdItem.ShortName, cmd, false);
                WFOracle.AddInParameter("a_code", OracleType.NVarChar, mvdItem.Code, cmd, false);

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
