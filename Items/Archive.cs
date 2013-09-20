using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OracleClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Common;
using WFDatabase;
using WFExceptions;

namespace GeneLibrary.Items
{
    class ArchiveVocabulary : Vocabulary
    {
        // Constructors
        public ArchiveVocabulary() : base() { }

        // Public methods
        public override void Open()
        {
            CreateDataTableColumns();
            gate.Open(DT);
        }
        public override void Open(DataGridView dgr)
        {
            this.Open();
            FillDataGridColumn(dgr);
        }
        public override void Open(DataGridView dgr, string filter)
        {
            CreateDataTableColumns();
            gate.Open(DT, filter);
            FillDataGridColumn(dgr);
        }
        public override bool Del(int[] ids)
        {
            return true;
        }
        public override bool IsEmpty()
        {
            return DT == null || DT.Rows.Count == 0;
        }
        public override void LoadItem()
        {
            throw new NotImplementedException();
        }       
        public override void LoadItem(int id)
        {
            Item =  new ArchiveItemInner(id);
        }
        public void RemoveToArchive(int[] ids)
        {
            gate.SetNote(this.Note);
            gate.RemoveToArchive(ids);
        }
        public void ExtractFromArchive(int[] ids)
        {
            gate.SetNote(this.Note);
            gate.ExtractFromARchive(ids);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "CARD_NUM" };
        }
        public override string TableName()
        {
            return gate.TableName();
        }

        // Properties
        public ArchiveItem Item { get; set; }
        public String Note { get; set; }

        // Types
        class ArchiveItemInner : ArchiveItem
        {
            public ArchiveItemInner() : base() { }
            public ArchiveItemInner(int id) : base(id) { }
        }

        // Private methods
        private void CreateDataTableColumns()
        {
            //ID	NUMBER(10)				
            //CARD_NUM	NVARCHAR2(100)				            
            //CRIM_NUM	NVARCHAR2(100)				
            //PERSON	NVARCHAR2(100)				
            //DATE_INS	TIMESTAMP(6)				

            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.iklTableID;

            DataColumn CardNum = new DataColumn("CARD_NUM", Type.GetType("System.String"));
            CardNum.Caption = resDataNames.iklOrik2TableCardNum;
            DataColumn CrimNum = new DataColumn("CRIM_NUM", Type.GetType("System.String"));
            CrimNum.Caption = resDataNames.iklTableCrimNum;
            DataColumn Person = new DataColumn("PERSON", Type.GetType("System.String"));
            Person.Caption = resDataNames.iklTablePerson;
            DataColumn DateIns = new DataColumn("DATE_INS", Type.GetType("System.DateTime"));
            DateIns.Caption = resDataNames.iklTableDateIns;

            DT.Columns.Add(Id);
            DT.Columns.Add(CardNum);
            DT.Columns.Add(CrimNum);
            DT.Columns.Add(Person);
            DT.Columns.Add(DateIns);
        }
        private void FillDataGridColumn(DataGridView dgr)
        {
            dgr.Columns.Clear();
            dgr.DataSource = base.DT;
            foreach (DataColumn dc in DT.Columns)
                dgr.Columns[dc.ColumnName].HeaderText = dc.Caption;
            dgr.Columns[0].HeaderText += String.Format(resDataNames.formFindCountOnHeader,
                base.DT.Rows.Count);
        }

        // Fields
        private ArchiveVocabularyGate gate = GateFactory.ArchiveVocabularyGate();

    }
    
    public class ArchiveItem
    {
        // Constructors
        public ArchiveItem() {}
        public ArchiveItem(int id) : this()
        {
            this.CardId = id;
        }
        
        // Properties
        public int CardId { get; set; }
        public CardKind CardKind { get; set; }
        public string CardNumber { get; set; }
        public CardItem Item 
        {
            get
            {
                switch (this.CardKind)
                {
                    case CardKind.ikl:
                        IklVocabulary iklVocabulary = new IklVocabulary();
                        iklVocabulary.LoadItem(this.CardId);
                        return iklVocabulary.Item;
                    case CardKind.ik2:
                        Ik2Vocabulary ik2Vocabulary = new Ik2Vocabulary();
                        ik2Vocabulary.LoadItem(this.CardId);
                        return ik2Vocabulary.Item;
                    default:
                        return null;
                }
            }
        }

    }

    public abstract class ArchiveVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract void Open(DataTable dataTable, string filter);
        public abstract bool Del(int[] ids);
        public abstract void SetNote(string note);
        public abstract void RemoveToArchive(int[] ids);
        public abstract void ExtractFromARchive(int[] ids);
        public abstract string TableName();
    }
    public class ArchiveVocabularyGateOracle : ArchiveVocabularyGate
    {
        //Constructors
        public ArchiveVocabularyGateOracle() { }

        public override void Open(DataTable dataTable)
        {
            throw new WFException(ErrType.Assert, ErrorsMsg.NotRealizeVirtualMethod);
        }
        public override void Open(DataTable dataTable, string filter)
        {
            OracleCommand cmd = new OracleCommand("", WFOracle.DB.OracleConnection);

            cmd.CommandText =
                " select c.id, c.card_num, c.crim_num, case when (select count(*) from modern.ikl_arch where id = c.id) = 0" +
                " then (select victim from modern.ik2_arch where id = c.id) else (select person from modern.ikl_arch where id = c.id)" +
                " end person, c.date_ins from modern.card_arch c"; 
            
            if (!String.IsNullOrEmpty(filter))
            {
                cmd.CommandText += " where c.hash like :filterString";
                WFOracle.AddInParameter("filterString", OracleType.NVarChar, String.Format("%{0}%", filter.ToUpper()), cmd, false);
            }
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            dataTable.Clear();
            da.Fill(dataTable);
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["id"] };
        }
        public override bool Del(int[] ids)
        {
            return true;
        }
        public override void SetNote(string note)
        {
            this.Note = note;
        }
        public override void RemoveToArchive(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                string sql = "begin modern.pkg_card.remove_to_archive(:a_id, :curr_user, :note); end;";

                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                foreach (int i in ids)
                {
                    cmd.Parameters.Clear();
                    WFOracle.AddInParameter("a_id", OracleType.Number, i, cmd, false);
                    WFOracle.AddInParameter("curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, false);
                    WFOracle.AddInParameter("note", OracleType.NClob, this.Note, cmd, false);
                    cmd.ExecuteNonQuery();
                }

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override void ExtractFromARchive(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                string sql = "begin modern.pkg_card.extract_from_archive(:a_id, :curr_user, :note); end;";

                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                foreach (int i in ids)
                {
                    cmd.Parameters.Clear();
                    WFOracle.AddInParameter("a_id", OracleType.Number, i, cmd, false);
                    WFOracle.AddInParameter("curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, false);
                    WFOracle.AddInParameter("note", OracleType.NClob, this.Note, cmd, false);
                    cmd.ExecuteNonQuery();
                }

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override string TableName()
        {
            return "modern.card_arch";
        }

        // Fields
        public string Note { get; set; }
    }

}
