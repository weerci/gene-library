using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFDatabase;
using System.Data;
using System.Windows.Forms;
using System.Data.OracleClient;
using GeneLibrary.Common;
using WFExceptions;
using System.Collections.ObjectModel;
using System.Globalization;


namespace GeneLibrary.Items
{
    public class Ik2Vocabulary : Vocabulary
    {
        // Constructors
        public Ik2Vocabulary() : base() { }

        // Public methods
        public override void Open()
        {
            CreateDataTableColumns();
            _gate.Open(DT);
        }
        public override void Open(DataGridView dgr)
        {
            this.Open();
            FillDataGridColumn(dgr);
        }
        public override void Open(DataGridView dgr, string filter)
        {
            CreateDataTableColumns();
            _gate.Open(DT, filter);
            FillDataGridColumn(dgr);
        }
        public void OpenIdent(DataGridView dgr, string filter)
        {
            CreateDataTableColumns();
            _gate.OpenIdent(DT, filter);
            FillDataGridColumn(dgr);
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
            Item = new Ik2ItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new Ik2ItemInner(id);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "CARD_NUM" };
        }
        public override string TableName()
        {
            return _gate.TableName();
        }

        // Properties
        public IK2Item Item { get; set; }

        // Types
        class Ik2ItemInner : IK2Item
        {
            public Ik2ItemInner() : base() { }
            public Ik2ItemInner(int id) : base(id) { }
        }

        // Private methods
        private void CreateDataTableColumns()
        {
            //ID	NUMBER(10)				
            //CARD_NUM	NVARCHAR2(100)				            
            //VICTIM	NVARCHAR2(100)				
            //CRIM_NUM	NVARCHAR2(100)				
            //DATE_INS	TIMESTAMP(6)				

            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.ik2TableID;
            DataColumn CardNum = new DataColumn("CARD_NUM", Type.GetType("System.String"));
            CardNum.Caption = resDataNames.ik2TableCardNum;
            DataColumn Victim = new DataColumn("VICTIM", Type.GetType("System.String"));
            Victim.Caption = resDataNames.ik2TableVictim;
            DataColumn CrimNum = new DataColumn("CRIM_NUM", Type.GetType("System.String"));
            CrimNum.Caption = resDataNames.ik2TableCrimNum;
            DataColumn DateIns = new DataColumn("DATE_INS", Type.GetType("System.DateTime"));
            DateIns.Caption = resDataNames.ik2TableDateIns;

            DT.Columns.Add(Id);
            DT.Columns.Add(CardNum);
            DT.Columns.Add(Victim);
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
        private IK2VocabularyGate _gate = GateFactory.IK2DictionaryGate();

    }
    public class IK2Item : CardItem
    {
        // Constructors
        protected IK2Item() 
        {
            this.Profile.Load();
        }
        protected IK2Item(int id) 
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
        public void Ident(int cardId)
        { 
            if (this.Id > 0)
                _gate.Ident(this.Id, cardId);
        }
        public void UnIdent(int cardId)
        {
            if (this.Id > 0)
                _gate.UnIdent(this.Id, cardId);
        }
        public void UnIdentAll()
        {
            if (this.Id > 0)
                _gate.UnIdentAll(this);
        }
        public void RemoveToArchive()
        {
            if (this.Id > 0)
                _gate.RemoveToArchive(this);
        }
        public override CardItem CardCopy()
        {
            IK2Item ik2Item = new IK2Item();
            ik2Item.AddressCrime = this.AddressCrime;
            ik2Item.AmountDna = this.AmountDna;
            ik2Item.ArchNote = this.ArchNote;
            ik2Item.ClassId = this.ClassId;
            ik2Item.ClassObjectVocabulary = this.ClassObjectVocabulary;
            ik2Item.ConcentDna = this.ConcentDna;
            ik2Item.CriminalNumber = this.CriminalNumber;
            ik2Item.DateCrime = this.DateCrime;
            ik2Item.DateIns = this.DateIns;
            ik2Item.ExamDate = this.ExamDate;
            ik2Item.ExpertId = this.ExpertId;
            ik2Item.ExpertVocabulary = this.ExpertVocabulary;
            ik2Item.LinId = this.LinId;
            ik2Item.LinVocabulary = this.LinVocabulary;
            ik2Item.MvdId = this.MvdId;
            ik2Item.MvdVocabulary = this.MvdVocabulary;
            ik2Item.Object = this.Object;
            ik2Item.SortObjectVocabulary = this.SortObjectVocabulary;
            ik2Item.OrganizationId = this.OrganizationId;
            ik2Item.OrganizationVocabulary = this.OrganizationVocabulary;
            ik2Item.SeralNumberDNA = this.SeralNumberDNA;
            ik2Item.SortId = this.SortId;
            ik2Item.SpotSize = this.SpotSize;
            ik2Item.YearState = this.YearState;

            foreach (ComboBoxItem item in this.UkIds)
                ik2Item.UkIds.Add(new ComboBoxItem(item.Id, item.Name, item.Short));
            ik2Item.UKVocabulary = this.UKVocabulary;

            return ik2Item;
        }
        
        // Properties
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string CriminalNumber { get; set; }
        public int MvdId { get; set; }
        public MvdVocabulary MvdVocabulary { get; set; }
        public int LinId { get; set; }
        public LinVocabulary LinVocabulary { get; set; }
        public int UkId { get; set; }
        public UKVocabulary UKVocabulary { get; set; }
        public string AddressCrime { get; set; }
        public DateTime DateCrime { get; set; }
        public string Victim { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationVocabulary OrganizationVocabulary { get; set; }
        public int ExpertId { get; set; }
        public ExpertVocabulary ExpertVocabulary { get; set; }
        public int ClassId { get; set; }
        public ClassObjectVocabulary ClassObjectVocabulary { get; set; }
        public int SortId { get; set; }
        public SortObjectVocabulary SortObjectVocabulary { get; set; }
        public string ExamNumber { get; set; }
        public DateTime ExamDate { get; set; }
        public DateTime DateIns { get; set; }
        public string SeralNumberDNA { get; set; }
        public string Object { get; set; }
        public string SpotSize { get; set; }
        public string ConcentDna { get; set; }
        public string AmountDna { get; set; }
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
        public int HistoryId { get; set; }
        public Collection<CardIdent> CardIdent 
        {
            get { return new Collection<CardIdent>(iklCoincide); }
        }
        public Collection<CardIdent> CardUnIdent
        {
            get { return new Collection<CardIdent>(cardUnIdent); }
            set { this.cardUnIdent = new List<CardIdent>(value); }
        }
        public int YearState { get; set; }
        public Collection<ComboBoxItem> UkIds
        {
            get
            {
                return new Collection<ComboBoxItem>(ukIds);
            }
        }
        public string ArchNote { get; set; }

        // Fields
        private IK2ItemGate _gate = GateFactory.IK2ItemGate();
        private Profiles profile = new Profiles();
        private List<CardIdent> iklCoincide = new List<CardIdent>();
        private List<CardIdent> cardUnIdent = new List<CardIdent>();
        private List<ComboBoxItem> ukIds = new List<ComboBoxItem>();

    }

    public abstract class IK2VocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract void Open(DataTable dataTable, string filter);
        public abstract void OpenIdent(DataTable dataTable, string filter);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class IK2VocabularyGateOracle : IK2VocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            throw new WFException(ErrType.Assert, ErrorsMsg.NotRealizeVirtualMethod);
        }
        public override void Open(DataTable dataTable, string filter)
        {
            OracleCommand cmd = new OracleCommand("", WFOracle.DB.OracleConnection);

            cmd.CommandText =
                " select c.id, c.card_num, c.crim_num, i.victim, c.date_ins" +
                " from (select id mid, victim from modern.ik2) i, modern.card c where c.id = i.mid";
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
        public override void OpenIdent(DataTable dataTable, string filter)
        {
            OracleCommand cmd = new OracleCommand("", WFOracle.DB.OracleConnection);

            cmd.CommandText =
                " select c.id, c.card_num, c.crim_num, i.victim, c.date_ins" +
                " from (select id mid, victim from modern.ik2 where ident = 1) i, modern.card c where c.id = i.mid";
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
                string sql = "begin modern.pkg_card.ik2_del(:a_id, :curr_user); end;";
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

    public abstract class IK2ItemGate
    {
        public abstract void Open(IK2Item ik2Item);
        public abstract int Insert(IK2Item ik2Item);
        public abstract void Update(IK2Item ik2Item);
        public abstract void Ident(int ik2Id, int cardId);
        public abstract void UnIdent(int ik2Id, int cardId);
        public abstract void UnIdentAll(IK2Item ik2Item);
        public abstract void RemoveToArchive(IK2Item ik2Item);
    }
    public class IK2ItemGateOracle : IK2ItemGate
    {
        // Private methods
        private void SaveUk(IK2Item ik2Item)
        {
            string sql = "begin modern.pkg_card.uk_del(:a_card_id); end;";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            cmd.CommandType = CommandType.Text;
            WFOracle.AddInParameter("a_card_id", OracleType.NVarChar, ik2Item.Id, cmd, false);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "begin modern.pkg_card.uk_ins(:a_card_id, :a_uk_id); end;";
            WFOracle.AddInParameter("a_card_id", OracleType.NVarChar, ik2Item.Id, cmd, false);
            cmd.Parameters.Add("a_uk_id", OracleType.Number);

            for (int i = 0; i < ik2Item.UkIds.Count; i++)
            {
                cmd.Parameters["a_uk_id"].Value = ik2Item.UkIds[i].Id;
                cmd.ExecuteNonQuery();
            }

        }
        private void SaveCardIdent(IK2Item ik2Item)
        {
            string sql = "begin modern.pkg_card.un_ident(:a_ik2, :a_card, :a_curr_user); end;";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            cmd.Parameters.Clear();
            WFOracle.AddInParameter("a_ik2", OracleType.NVarChar, ik2Item.Id, cmd, false);
            WFOracle.AddInParameter("a_curr_user", OracleType.NVarChar, GateFactory.LogOnBase().Id, cmd, false);
            cmd.Parameters.Add("a_card", OracleType.Number);
            foreach (int i in (from CardIdent cardUnIdent in ik2Item.CardUnIdent select cardUnIdent.CardId))
            {
                cmd.Parameters["a_card"].Value = i;
                cmd.ExecuteNonQuery();
            }
        }
        
        // Public interface
        public override void Open(IK2Item ik2Item)
        {

            // Получаем список экспертов
            ik2Item.ExpertVocabulary = new ExpertVocabulary();
            ik2Item.ExpertVocabulary.Open();

            // Получаем список организаций
            ik2Item.OrganizationVocabulary = new OrganizationVocabulary();
            ik2Item.OrganizationVocabulary.Open();

            // Получаем список кодов МВД
            ik2Item.MvdVocabulary = new MvdVocabulary();
            ik2Item.MvdVocabulary.Open();

            // Получаем список кодов райлинорганов
            ik2Item.LinVocabulary = new LinVocabulary();
            ik2Item.LinVocabulary.Open();

            // Получаем список категорий
            ik2Item.ClassObjectVocabulary = new ClassObjectVocabulary();
            ik2Item.ClassObjectVocabulary.Open();

            // Получаем список видов
            ik2Item.SortObjectVocabulary = new SortObjectVocabulary();
            ik2Item.SortObjectVocabulary.Open();

            // Получаем список найденных соответствий
            CoincideList(ik2Item);

            string sql =
                " select c.id, i.sort_id, i.class_id, c.crim_num, c.org_id, c.expert_id, c.exam_num, c.exam_date," +
                " c.exam_note, c.date_ins, c.paren_id, c.history_id, c.card_num, i.mvd_id," +
                " i.lin_id, i.address_crime, i.victim, i.sn_dna, i.obj, i.spotsize, i.concent," +
                " i.amount, i.date_crime, (select sign from modern.expert where id = c.expert_id) expert_sign," +
                " i.year_state from modern.card c, modern.ik2 i where i.id = c.id and c.id = :a_id";

            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("a_id", OracleType.Number, ik2Item.Id, cmd, false);
            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    ik2Item.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    ik2Item.SortId = Convert.ToInt32(rdr["SORT_ID"], CultureInfo.InvariantCulture);
                    ik2Item.ClassId = Convert.ToInt32(rdr["CLASS_ID"], CultureInfo.InvariantCulture);
                    ik2Item.CardNumber = rdr["CARD_NUM"].ToString();
                    ik2Item.CriminalNumber = rdr["CRIM_NUM"].ToString();
                    ik2Item.OrganizationId = Convert.ToInt32(rdr["ORG_ID"], CultureInfo.InvariantCulture);
                    ik2Item.ExpertId = Convert.ToInt32(rdr["EXPERT_ID"], CultureInfo.InvariantCulture);
                    ik2Item.ExamNumber = rdr["EXAM_NUM"].ToString();
                    ik2Item.ExamDate = Convert.ToDateTime(rdr["EXAM_DATE"], CultureInfo.InvariantCulture);
                    ik2Item.DateIns = Convert.ToDateTime(rdr["DATE_INS"], CultureInfo.InvariantCulture);
                    ik2Item.HistoryId = Convert.ToInt32(rdr["HISTORY_ID"], CultureInfo.InvariantCulture);
                    ik2Item.MvdId = Convert.ToInt32(rdr["MVD_ID"], CultureInfo.InvariantCulture);
                    ik2Item.LinId = Convert.ToInt32(rdr["LIN_ID"], CultureInfo.InvariantCulture);
                    ik2Item.AddressCrime = rdr["ADDRESS_CRIME"].ToString();
                    ik2Item.Victim = rdr["VICTIM"].ToString();
                    ik2Item.SeralNumberDNA = rdr["SN_DNA"].ToString();
                    ik2Item.Object = rdr["OBJ"].ToString();
                    ik2Item.SpotSize = rdr["SPOTSIZE"].ToString();
                    ik2Item.ConcentDna = rdr["CONCENT"].ToString();
                    ik2Item.AmountDna = rdr["AMOUNT"].ToString();
                    ik2Item.DateCrime = Convert.ToDateTime(rdr["DATE_CRIME"].ToString());
                    ik2Item.YearState = Convert.ToInt32(rdr["YEAR_STATE"].ToString(), CultureInfo.InvariantCulture);
                }
            }

            // Получение списка статей
            cmd.Parameters.Clear();
            cmd.CommandText =
                " select ukitem_id, (select count(*) from modern.ukarticl where id = ukitem_id) artcl," +
                " modern.pkg_card.UkState(ukitem_id) state from modern.card_ukitem where card_id = :a_id";
            WFOracle.AddInParameter("a_id", OracleType.Number, ik2Item.Id, cmd, false);
            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                    ik2Item.UkIds.Add(new ComboBoxItem(
                            Convert.ToInt32(rdr["ukitem_id"], CultureInfo.InvariantCulture),
                            rdr["state"].ToString(),
                            rdr["artcl"].ToString()));
            }
        }
        public override int Insert(IK2Item ik2Item)
        {
            string sql =
                " begin :res := modern.pkg_card.ik2_ins(:a_card_num, :a_class_id, :a_sort_id, :a_crim_num, " +
                " :a_org_id, :a_expert_id, :a_exam_num, :a_exam_date, :a_date_ins, :a_mvd_id," +
                " :a_lin_id, :a_date_crime, :a_victim, :a_sn_dna, :a_obj, :a_spotsize, :a_concent, :a_amount,"+
                " :a_address_crime, :a_year_state, :curr_user); end;";

            OracleParameter prmRes;
            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_card_num", OracleType.NVarChar, ik2Item.CardNumber, cmd, false);
                WFOracle.AddInParameter("a_class_id", OracleType.Number, ik2Item.ClassId, cmd, false);
                WFOracle.AddInParameter("a_sort_id", OracleType.Number, ik2Item.SortId, cmd, false);
                WFOracle.AddInParameter("a_crim_num", OracleType.NVarChar, ik2Item.CriminalNumber, cmd, false);
                WFOracle.AddInParameter("a_org_id", OracleType.Number, ik2Item.OrganizationId, cmd, false);
                WFOracle.AddInParameter("a_expert_id", OracleType.Number, ik2Item.ExpertId, cmd, false);
                WFOracle.AddInParameter("a_exam_num", OracleType.NVarChar, ik2Item.ExamNumber, cmd, true);
                WFOracle.AddInParameter("a_exam_date", OracleType.DateTime, ik2Item.ExamDate, cmd, true);
                WFOracle.AddInParameter("a_date_ins", OracleType.DateTime, ik2Item.DateIns, cmd, false);
                WFOracle.AddInParameter("a_mvd_id", OracleType.Number, ik2Item.MvdId, cmd, false);
                WFOracle.AddInParameter("a_lin_id", OracleType.Number, ik2Item.LinId, cmd, false);
                WFOracle.AddInParameter("a_date_crime", OracleType.DateTime, ik2Item.DateCrime, cmd, false);
                WFOracle.AddInParameter("a_victim", OracleType.NVarChar, ik2Item.Victim, cmd, false);
                WFOracle.AddInParameter("a_sn_dna", OracleType.NVarChar, ik2Item.SeralNumberDNA, cmd, false);
                WFOracle.AddInParameter("a_obj", OracleType.NVarChar, ik2Item.Object, cmd, false);
                WFOracle.AddInParameter("a_spotsize", OracleType.NVarChar, ik2Item.SpotSize, cmd, false);
                WFOracle.AddInParameter("a_concent", OracleType.NVarChar, ik2Item.ConcentDna, cmd, false);
                WFOracle.AddInParameter("a_amount", OracleType.NVarChar, ik2Item.AmountDna, cmd, false);
                WFOracle.AddInParameter("a_address_crime", OracleType.NVarChar, ik2Item.AddressCrime, cmd, false);
                WFOracle.AddInParameter("a_year_state", OracleType.Number, ik2Item.YearState, cmd, false);
                WFOracle.AddInParameter("curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
                cmd.ExecuteNonQuery();
                ik2Item.Id = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);


                // Сохранение уголовных статей карточки
                SaveUk(ik2Item);
                
                // Сохранение профиля
                ik2Item.Profile.Save(Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture));

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
        }
        public override void Update(IK2Item ik2Item)
        {
            string sql =
                " begin modern.pkg_card.ik2_upd(:a_id, :a_class_id, :a_sort_id, :a_card_num, :a_crim_num, :a_org_id, :a_expert_id,"+
                " :a_exam_num, :a_exam_date, :a_date_ins, :a_mvd_id, :a_lin_id, :a_date_crime, :a_victim, :a_sn_dna,"+
                " :a_obj, :a_spotsize, :a_concent, :a_amount, :a_address_crime, :a_year_state, :curr_user); end;";

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;


                WFOracle.AddInParameter("a_id", OracleType.Number, ik2Item.Id, cmd, false);
                WFOracle.AddInParameter("a_class_id", OracleType.Number, ik2Item.ClassId, cmd, false);
                WFOracle.AddInParameter("a_sort_id", OracleType.Number, ik2Item.SortId, cmd, false);
                WFOracle.AddInParameter("a_card_num", OracleType.NVarChar, ik2Item.CardNumber, cmd, false);
                WFOracle.AddInParameter("a_crim_num", OracleType.NVarChar, ik2Item.CriminalNumber, cmd, false);
                WFOracle.AddInParameter("a_org_id", OracleType.Number, ik2Item.OrganizationId, cmd, false);
                WFOracle.AddInParameter("a_expert_id", OracleType.Number, ik2Item.ExpertId, cmd, false);
                WFOracle.AddInParameter("a_exam_num", OracleType.NVarChar, ik2Item.ExamNumber, cmd, true);
                WFOracle.AddInParameter("a_exam_date", OracleType.DateTime, ik2Item.ExamDate, cmd, true);
                WFOracle.AddInParameter("a_date_ins", OracleType.DateTime, ik2Item.DateIns, cmd, false);
                WFOracle.AddInParameter("a_mvd_id", OracleType.Number, ik2Item.MvdId, cmd, false);
                WFOracle.AddInParameter("a_lin_id", OracleType.Number, ik2Item.LinId, cmd, false);
                WFOracle.AddInParameter("a_date_crime", OracleType.DateTime, ik2Item.DateCrime, cmd, false);
                WFOracle.AddInParameter("a_victim", OracleType.NVarChar, ik2Item.Victim, cmd, false);
                WFOracle.AddInParameter("a_sn_dna", OracleType.NVarChar, ik2Item.SeralNumberDNA, cmd, true);
                WFOracle.AddInParameter("a_obj", OracleType.NVarChar, ik2Item.Object, cmd, true);
                WFOracle.AddInParameter("a_spotsize", OracleType.NVarChar, ik2Item.SpotSize, cmd, true);
                WFOracle.AddInParameter("a_concent", OracleType.NVarChar, ik2Item.ConcentDna, cmd, true);
                WFOracle.AddInParameter("a_amount", OracleType.NVarChar, ik2Item.AmountDna, cmd, true);
                WFOracle.AddInParameter("a_address_crime", OracleType.NVarChar, ik2Item.AddressCrime, cmd, false);
                WFOracle.AddInParameter("a_year_state", OracleType.Number, ik2Item.YearState, cmd, false);
                WFOracle.AddInParameter("curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                cmd.ExecuteNonQuery();

                // Сохранение уголовных статей карточки
                SaveUk(ik2Item);

                // Сохранение профился
                ik2Item.Profile.Save(ik2Item.Id);

                // Сохранение списка совпавших карточек
                SaveCardIdent(ik2Item);

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override void Ident(int ik2Id, int cardId)
        {
            string sql =
                " begin modern.pkg_card.ident(:a_ik2, :a_card, :a_curr_user); end;";

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;


                WFOracle.AddInParameter("a_ik2", OracleType.Number, ik2Id, cmd, false);
                WFOracle.AddInParameter("a_card", OracleType.Number, cardId, cmd, false);
                WFOracle.AddInParameter("a_curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                cmd.ExecuteNonQuery();

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override void UnIdent(int ik2Id, int cardId)
        {
            string sql =
                " begin modern.pkg_card.un_ident(:a_ik2, :a_card, :a_curr_user); end;";

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;


                WFOracle.AddInParameter("a_ik2", OracleType.Number, ik2Id, cmd, false);
                WFOracle.AddInParameter("a_card", OracleType.Number, cardId, cmd, false);
                WFOracle.AddInParameter("a_curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                cmd.ExecuteNonQuery();

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override void UnIdentAll(IK2Item ik2Item)
        {
            string sql = " begin modern.pkg_card.un_ident_all(:a_ik2, :a_curr_user); end;";

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_ik2", OracleType.Number, ik2Item.Id, cmd, false);
                WFOracle.AddInParameter("a_curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                cmd.ExecuteNonQuery();

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override void RemoveToArchive(IK2Item ik2Item)
        {
            string sql =
                " begin modern.pkg_card.remove_to_archive(:a_ik2, :curr_user, :note); end;";

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;


                WFOracle.AddInParameter("a_ik2", OracleType.Number, ik2Item.Id, cmd, false);
                WFOracle.AddInParameter("a_curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                WFOracle.AddInParameter("note", OracleType.NClob, ik2Item.ArchNote, cmd, true);
                cmd.ExecuteNonQuery();

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }

        // Private methods
        private void CoincideList(IK2Item ik2Item)
        {
            string sql =
                " select c.card_id, case when c.arch = 0 then  (select card_num from modern.card where id = c.card_id) else" +
                " (select card_num from modern.card_arch where id = c.card_id) end card_num, case" +
                " when (select count(*) from (select id from modern.ikl union all select id from modern.ikl_arch) where id = c.card_id) = 0 then 1 else 0 end card_type," +
                " arch from modern.v_card_ident c where c.id in (select id from modern.card_ident where card_id = :a_ik2_id)" +
                " and card_id <> :a_ik2_id";
                
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("a_ik2_id", OracleType.Number, ik2Item.Id, cmd, false);
            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ik2Item.CardIdent.Add(new CardIdent(
                        Convert.ToInt32(rdr["card_id"], CultureInfo.InvariantCulture),
                        rdr["card_num"].ToString(),
                        rdr["card_num"].ToString(),
                        Tools.CardKindType(Convert.ToInt32(rdr["card_type"], CultureInfo.InvariantCulture)),
                        Convert.ToInt32(rdr["arch"], CultureInfo.InvariantCulture)
                        ));
                }
            }

        }
    }
    public class CardIdent
    {
        // Constructors
        public CardIdent(int iklId, string iklNumber, string iklPersonInfo, CardKind cardKind, int arch)
        {
            this.CardId = iklId;
            this.CardNumber = iklNumber;
            this.CardKind = cardKind;
            this.CardPersonInfo = CardPersonInfo;
            this.Arch = arch;
        }
        
        // Properties
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public string CardPersonInfo { get; set; }
        public CardKind CardKind { get; set; }
        public int Arch { get; set; }
    }
}
