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
    public class IklVocabulary : Vocabulary
    {
        // Constructors
        public IklVocabulary() : base() { }

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
            return gate.Del(ids);
        }
        public override bool IsEmpty()
        {
            return DT == null || DT.Rows.Count == 0;
        }
        public override void LoadItem()
        {
            Item = new IklItemInner();
        }
        public override void LoadItem(int id)
        {
            Item =  new IklItemInner(id);
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
        public IklItem Item { get; set; }

        // Types
        class IklItemInner : IklItem
        {
            public IklItemInner() : base() { }
            public IklItemInner(int id) : base(id) { }
        }

        // Private methods
        private void CreateDataTableColumns()
        {
            //ID	NUMBER(10)				
            //CARD_NUM	NVARCHAR2(100)				            
            //PERSON	NVARCHAR2(100)				
            //CRIM_NUM	NVARCHAR2(100)				
            //DATE_INS	TIMESTAMP(6)				

            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.iklTableID;
            DataColumn CardNum = new DataColumn("CARD_NUM", Type.GetType("System.String"));
            CardNum.Caption = resDataNames.iklTableCardNum;
            DataColumn Person = new DataColumn("PERSON", Type.GetType("System.String"));
            Person.Caption = resDataNames.iklTablePerson;
            DataColumn CrimNum = new DataColumn("CRIM_NUM", Type.GetType("System.String"));
            CrimNum.Caption = resDataNames.iklTableCrimNum;
            DataColumn DateIns = new DataColumn("DATE_INS", Type.GetType("System.DateTime"));
            DateIns.Caption = resDataNames.iklTableDateIns;

            DT.Columns.Add(Id);
            DT.Columns.Add(CardNum);
            DT.Columns.Add(Person);
            DT.Columns.Add(CrimNum);
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
        private IklVocabularyGate gate = GateFactory.IklVocabularyGate();

    }
    
    public class IklItem : CardItem
    {
        // Constructors
        protected IklItem() 
        {
            this.Profile.Load();
        }
        protected IklItem(int id)
        {
            this.Id = id;
            this.Profile.Load(id, GateFactory.GLDefault().DefaultMethod);
        }

        // Public methods
        public void Open()
        {
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
        public override CardItem CardCopy()
        {
            IklItem iklItem = new IklItem();
            iklItem.Ancillary = this.Ancillary;
            iklItem.ClassIklId = this.ClassIklId;
            iklItem.ClassIklVocabulary = this.ClassIklVocabulary;
            iklItem.CriminalNumber = this.CriminalNumber;
            iklItem.DateIns = this.DateIns;
            iklItem.ExamDate = this.ExamDate;
            iklItem.ExamNote = this.ExamNote;
            iklItem.ExpertId = this.ExpertId;
            iklItem.ExpertSign = this.ExpertSign;
            iklItem.ExpertVocabulary = this.ExpertVocabulary;
            iklItem.OrganizationId = this.OrganizationId;
            iklItem.OrganizationVocabulary = this.OrganizationVocabulary;
            iklItem.ParentId = this.ParentId;
            foreach (ComboBoxItem item in this.UkIds)
                iklItem.UkIds.Add(new ComboBoxItem(item.Id, item.Name, item.Short));
            iklItem.UKVocabulary = this.UKVocabulary;

            return iklItem;
        }

        // Properties
        /// <summary>
        /// Номер карты в базе данных
        /// </summary>
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string CriminalNumber { get; set; }
        public string Person { get; set; }
        public UKVocabulary UKVocabulary { get; set; }
        public int ExpertId { get; set; }
        public ExpertVocabulary ExpertVocabulary { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationVocabulary OrganizationVocabulary { get; set; }
        public ClassIklVocabulary ClassIklVocabulary { get; set; }
        public int ClassIklId { get; set; }
        public string ExamNumber { get; set; }
        public DateTime ExamDate { get; set; }
        public string ExamNote { get; set; }
        public DateTime DateIns { get; set; }
        public int ParentId { get; set; }
        public Profiles Profile 
        { 
            get 
            { 
                return this.profile; 
            }
            set
            {
                this.profile = value;
            }
        }
        public string Ancillary { get; set; }
        public string ExpertSign { get; set; }
        public int HistoryId { get; set; }
        public Collection<ComboBoxItem> UkIds
        { 
            get 
            {
                return new Collection<ComboBoxItem>(ukIds);
            }
        }

        // Fields
        private IklItemGate _gate = GateFactory.IklItemGate();
        private Profiles profile = new Profiles();
        private List<ComboBoxItem> ukIds = new List<ComboBoxItem>();
    }

    public abstract class IklVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract void Open(DataTable dataTable, string filter);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class IklVocabularyGateOracle : IklVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            throw new WFException(ErrType.Assert, ErrorsMsg.NotRealizeVirtualMethod);
        }
        public override void Open(DataTable dataTable, string filter)
        {
            OracleCommand cmd = new OracleCommand("", WFOracle.DB.OracleConnection);

            cmd.CommandText =
                " select c.id, c.card_num, c.crim_num, i.person, c.date_ins " +
                " from (select person, id mid from modern.ikl) i, modern.card c where c.id = i.mid";
            if (!String.IsNullOrEmpty(filter))
            {
                cmd.CommandText += " and c.hash like :filterString";
                WFOracle.AddInParameter("filterString", OracleType.NVarChar, String.Format("%{0}%", filter.ToUpper()), cmd, false);
            }
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            dataTable.Clear();
            da.Fill(dataTable);
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["id"] };
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                string sql = "begin modern.pkg_card.ikl_del(:a_id, :curr_user); end;";
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                foreach (int i in ids)
                {
                    cmd.Parameters.Clear();
                    WFOracle.AddInParameter("a_id", OracleType.Number, i, cmd, false);
                    WFOracle.AddInParameter("curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, false);
                    cmd.ExecuteNonQuery();
                }

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
            return "modern.card";
        }
    }

    public abstract class IklItemGate
    {
        public abstract void Open(IklItem iklItem);
        public abstract int Insert(IklItem iklItem);
        public abstract void Update(IklItem iklItem);
    }
    public class IklItemGateOracle : IklItemGate
    {
        // Private methods
        private void SaveUk(IklItem iklItem)
        {
            string sql = "begin modern.pkg_card.uk_del(:a_card_id); end;";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            cmd.CommandType = CommandType.Text;
            WFOracle.AddInParameter("a_card_id", OracleType.NVarChar, iklItem.Id, cmd, false);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "begin modern.pkg_card.uk_ins(:a_card_id, :a_uk_id); end;";
            WFOracle.AddInParameter("a_card_id", OracleType.NVarChar, iklItem.Id, cmd, false);
            cmd.Parameters.Add("a_uk_id", OracleType.Number);

            for (int i = 0; i < iklItem.UkIds.Count; i++)
            {
                cmd.Parameters["a_uk_id"].Value = iklItem.UkIds[i].Id;
                cmd.ExecuteNonQuery();
            }
        }

        // Public interface
        public override void Open(IklItem iklItem)
        {
            // Получаем список экспертов
            iklItem.ExpertVocabulary = new ExpertVocabulary();
            iklItem.ExpertVocabulary.Open();

            // Получаем список организаций
            iklItem.OrganizationVocabulary = new OrganizationVocabulary();
            iklItem.OrganizationVocabulary.Open();

            // Получаем список категорий
            iklItem.ClassIklVocabulary = new ClassIklVocabulary();
            iklItem.ClassIklVocabulary.Open(); 
        
            string  sql = 
                " select c.id, c.crim_num, c.org_id, c.expert_id, c.exam_num, c.exam_date, c.exam_note, class_id," +
                " c.date_ins, c.paren_id, c.history_id, c.card_num, i.person, i.ancillary, (select sign from modern.expert where id = c.expert_id) expert_sign "+
                " from modern.card c, modern.ikl i where i.id = c.id and c.id = :a_id";
            
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("a_id", OracleType.Number, iklItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    iklItem.CardNumber = rdr["CARD_NUM"].ToString();
                    iklItem.CriminalNumber = rdr["CRIM_NUM"].ToString();
                    iklItem.Person = rdr["PERSON"].ToString();
                    iklItem.ExpertId = Convert.ToInt32(rdr["EXPERT_ID"], CultureInfo.InvariantCulture);
                    iklItem.OrganizationId = Convert.ToInt32(rdr["ORG_ID"], CultureInfo.InvariantCulture);
                    iklItem.ExamNumber = rdr["EXAM_NUM"].ToString();        
                    iklItem.ExamDate = Convert.ToDateTime(rdr["EXAM_DATE"], CultureInfo.InvariantCulture);
                    iklItem.ExamNote = rdr["EXAM_NOTE"].ToString();
                    iklItem.DateIns = Convert.ToDateTime(rdr["DATE_INS"], CultureInfo.InvariantCulture);
                    iklItem.HistoryId = Convert.ToInt32(rdr["HISTORY_ID"], CultureInfo.InvariantCulture); 
                    iklItem.Ancillary = rdr["ANCILLARY"].ToString();
                    iklItem.ExpertSign = rdr["EXPERT_SIGN"].ToString();
                    iklItem.ClassIklId = Convert.ToInt32(rdr["CLASS_ID"], CultureInfo.InvariantCulture);
                }
            }

            // Получение списка статей
            cmd.Parameters.Clear();
            cmd.CommandText = 
                " select ukitem_id, (select count(*) from modern.ukarticl where id = ukitem_id) artcl,"+
                " modern.pkg_card.UkState(ukitem_id) state from modern.card_ukitem where card_id = :a_id";
            WFOracle.AddInParameter("a_id", OracleType.Number, iklItem.Id, cmd, false);
            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                    iklItem.UkIds.Add(new ComboBoxItem(
                            Convert.ToInt32(rdr["ukitem_id"], CultureInfo.InvariantCulture), 
                            rdr["state"].ToString(), 
                            rdr["artcl"].ToString()));
            }
        }
        public override int Insert(IklItem iklItem)
        {
            string sql =
                " begin :res := modern.pkg_card.ikl_ins(:a_card_num, :a_class_id, :a_crim_num," +
                " :a_org_id, :a_expert_id, :a_exam_num, :a_exam_date, :a_exam_note, :a_date_ins, :a_person," +
                " :a_ancillary, :curr_user); end;";

            OracleParameter prmRes;
            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_card_num", OracleType.NVarChar, iklItem.CardNumber, cmd, false);
                WFOracle.AddInParameter("a_class_id", OracleType.Number, iklItem.ClassIklId, cmd, false);
                WFOracle.AddInParameter("a_crim_num", OracleType.NVarChar, iklItem.CriminalNumber, cmd, false);
                WFOracle.AddInParameter("a_org_id", OracleType.Number, iklItem.OrganizationId, cmd, false);
                WFOracle.AddInParameter("a_expert_id", OracleType.Number, iklItem.ExpertId, cmd, false);
                WFOracle.AddInParameter("a_exam_num", OracleType.NVarChar, iklItem.ExamNumber, cmd, true);
                WFOracle.AddInParameter("a_exam_date", OracleType.DateTime, iklItem.ExamDate, cmd, true);
                WFOracle.AddInParameter("a_exam_note", OracleType.NVarChar, iklItem.ExamNote, cmd, true);
                WFOracle.AddInParameter("a_date_ins", OracleType.DateTime, iklItem.DateIns, cmd, false);
                WFOracle.AddInParameter("a_person", OracleType.NVarChar, iklItem.Person, cmd, false);
                WFOracle.AddInParameter("a_ancillary", OracleType.NVarChar, iklItem.Ancillary, cmd, true);
                WFOracle.AddInParameter("curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
                cmd.ExecuteNonQuery();
                iklItem.Id = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
                
                // Сохранение уголовных статей карточки
                SaveUk(iklItem);

                // Сохранение профиля карточки
                iklItem.Profile.Save(iklItem.Id);

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return iklItem.Id;
        }
        public override void Update(IklItem iklItem)
        {
            string sql =
                " begin modern.pkg_card.ikl_upd(:a_id, :a_class_id, :a_card_num, :a_crim_num," +
                " :a_org_id, :a_expert_id, :a_exam_num, :a_exam_date, :a_exam_note, :a_date_ins, :a_person," +
                " :a_ancillary, :curr_user); end;";

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_id", OracleType.Number, iklItem.Id, cmd, false);
                WFOracle.AddInParameter("a_class_id", OracleType.Number, iklItem.ClassIklId, cmd, false);
                WFOracle.AddInParameter("a_card_num", OracleType.NVarChar, iklItem.CardNumber, cmd, false);
                WFOracle.AddInParameter("a_crim_num", OracleType.NVarChar, iklItem.CriminalNumber, cmd, false);
                WFOracle.AddInParameter("a_org_id", OracleType.Number, iklItem.OrganizationId, cmd, false);
                WFOracle.AddInParameter("a_expert_id", OracleType.Number, iklItem.ExpertId, cmd, false);
                WFOracle.AddInParameter("a_exam_num", OracleType.NVarChar, iklItem.ExamNumber, cmd, true);
                WFOracle.AddInParameter("a_exam_date", OracleType.DateTime, iklItem.ExamDate, cmd, true);
                WFOracle.AddInParameter("a_exam_note", OracleType.NVarChar, iklItem.ExamNote, cmd, true);
                WFOracle.AddInParameter("a_date_ins", OracleType.DateTime, iklItem.DateIns, cmd, false);
                WFOracle.AddInParameter("a_person", OracleType.NVarChar, iklItem.Person, cmd, false);
                WFOracle.AddInParameter("a_ancillary", OracleType.NVarChar, iklItem.Ancillary, cmd, true);
                WFOracle.AddInParameter("curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                cmd.ExecuteNonQuery();

                // Сохранение уголовных статей карточки
                SaveUk(iklItem);

                // Сохранение профился
                iklItem.Profile.Save(iklItem.Id);

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
