using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.OracleClient;
using WFDatabase;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using GeneLibrary.Items;
using WFExceptions;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;


namespace GeneLibrary.Common
{
    public class FilterConrol : UserControl
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FilterConrol
            // 
            this.Name = "FilterConrol";
            this.ResumeLayout(false);

        }
        
        // Constructors
        public FilterConrol() : base() { }

        // Protected
        protected FilterField filterField;
    }

    /// <summary>
    /// Список фильтров
    /// </summary>
    public class FilterVocabulary : Vocabulary
    {
        // Constructors
        public FilterVocabulary() : base() { }
        
        // Public methods
        public override void Open()
        {
            CreateDataTableColumns();
            gate.Open(DT);
        }
        public override void Open(DataGridView dataGridView)
        {
            this.Open();
            FillDataGridColumn(dataGridView);
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
            Item = GateFactory.FilterItem(0);
        }
        public override void LoadItem(int id)
        {
            Item = GateFactory.FilterItem(id);
            Item.Load();
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "NAME" };
        }
        public override string TableName()
        {
            return gate.TableName();
        }

        // Private methods
        private void CreateDataTableColumns()
        {
            //ID	NUMBER(10)				
            //NAME  VARCHAR2(256)
            //TEXT  NCLOB

            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.filterID;
            DataColumn filterName = new DataColumn("NAME", Type.GetType("System.String"));
            filterName.Caption = resDataNames.filterName;
            DataColumn filterText = new DataColumn("TEXT", Type.GetType("System.String"));
            filterText.Caption = resDataNames.filterText;

            DT.Columns.Add(Id);
            DT.Columns.Add(filterName);
            DT.Columns.Add(filterText);
        }
        private void FillDataGridColumn(DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();
            dataGridView.DataSource = base.DT;
            foreach (DataColumn dc in DT.Columns)
                dataGridView.Columns[dc.ColumnName].HeaderText = dc.Caption;
            dataGridView.Columns[0].HeaderText += String.Format(resDataNames.formFindCountOnHeader,
                base.DT.Rows.Count);
        }

        // Properties
        public FilterItem Item { get; set; }

        // Fields
        private FilterVocabularyGate gate = GateFactory.FilterVocabularyGate();
    }
    public abstract class FilterVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class FilterVocabularyGateOracle : FilterVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleCommand oracelCommand = new OracleCommand("select id, name, text from modern.filters order by name",
                WFOracle.DB.OracleConnection);

            OracleDataAdapter da = new OracleDataAdapter(oracelCommand);
            dataTable.Clear();
            da.Fill(dataTable);
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["id"] };
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand("begin modern.prk_tab.filter_del(:a_id); end;", 
                    WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                foreach (int i in ids)
                {
                    cmd.Parameters.Clear();
                    WFOracle.AddInParameter("a_id", OracleType.Number, i, cmd, false);
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
            return "modern.filters";
        }
    }
    
    /// <summary>
    /// Фильтр применяемый к массиву карточек ИКЛ, ИК-2 и карточек находящихся в архиве.
    /// Фильтр представляет набор из отдельных условий (FilterFieldsCondition) налагаемых на поля фильтра (FilterFields).
    /// </summary>
    public abstract class FilterItem
    {
        // Constructors
        protected FilterItem() { }

        // Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool InIkl { get; set; }
        public bool InIk2 { get; set; }
        public bool InArchive { get; set; }
        public Collection<FilterField> IklFilter 
        { 
            get 
            {
                return new Collection<FilterField>(iklFilter);
            } 
        }
        public Collection<FilterField> Ik2Filter 
        {
            get
            {
                return new Collection<FilterField>(ik2Filter);
            }
        }
        public DataGridView DataGridView {get; set;}
        
        public static FilterFieldType CreateFilterTypeItem(string className, string name, string value)
        {
            switch (className)
            {
                case "FilterFieldRange":
                    return new FilterFieldRange(name);
                case "FilterFieldOne":
                    return new FilterFieldOne(name, value);
                case "FilterFieldList":
                    return new FilterFieldList(name, value);
                default:
                    return new FilterFieldAll();
            }
        }

        // Public method
        public abstract int Save();
        public abstract void Load();
        public abstract void Apply();

        #region Поля фильтрации
        private List<FilterField> iklFilter = new List<FilterField>() 
        {
            new FilterField("cardIklId", Properties.Resources.cardId, FilterValueType.valueInt),
            new FilterField("cardIklNumber", Properties.Resources.cardIklNumber, FilterValueType.valueString),
            new FilterField("cardIklCategory", Properties.Resources.cardIklCategory, FilterValueType.valueList),
            new FilterField("cardIklCrimNumber", Properties.Resources.cardCrimNumber, FilterValueType.valueString),
            new FilterField("cardIklSuspect", Properties.Resources.cardSuspect, FilterValueType.valueString),
            new FilterField("cardIklUkNumber", Properties.Resources.cardUkNumber, FilterValueType.valueList),
            new FilterField("cardIklOrgan", Properties.Resources.cardOrgan, FilterValueType.valueList),
            new FilterField("cardIklDepartment", Properties.Resources.cardDepartment, FilterValueType.valueList),
            new FilterField("cardIklExamNumber", Properties.Resources.cardExamNumber, FilterValueType.valueString),
            new FilterField("cardIklExamDate", Properties.Resources.cardExamDate, FilterValueType.valueDate),
            new FilterField("cardIklExpert", Properties.Resources.cardExpert, FilterValueType.valueList),
            new FilterField("cardIklExpertConclusion", Properties.Resources.cardExpertConclusion, FilterValueType.valueString),
            new FilterField("cardIklCreateData", Properties.Resources.cardCreateData, FilterValueType.valueDate),
            new FilterField("cardIklExpertSign", Properties.Resources.cardExpertSign, FilterValueType.valueInt),
            new FilterField("cardIklAncillary", Properties.Resources.cardAncillary, FilterValueType.valueString)
        };
        private List<FilterField> ik2Filter = new List<FilterField>() 
        {
            new FilterField("cardIk2Id", Properties.Resources.cardId, FilterValueType.valueInt),
            new FilterField("cardIk2Number", Properties.Resources.cardIk2Number, FilterValueType.valueString),
            new FilterField("cardIk2CrimNumber", Properties.Resources.cardCrimNumber, FilterValueType.valueString),
            new FilterField("cardIk2CodeAccount", Properties.Resources.cardCodeAccount, FilterValueType.valueString),
            new FilterField("cardIk2CodeMvd", Properties.Resources.cardCodeMvd, FilterValueType.valueList),
            new FilterField("cardIk2CodeLin", Properties.Resources.cardCodeLin, FilterValueType.valueList),
            new FilterField("cardIk2Year", Properties.Resources.cardYear, FilterValueType.valueString),
            new FilterField("cardIk2UkNumber", Properties.Resources.cardUkNumber, FilterValueType.valueList),
            new FilterField("cardIk2Address", Properties.Resources.cardAddress, FilterValueType.valueString),
            new FilterField("cardIk2CrimDate", Properties.Resources.cardCrimDate, FilterValueType.valueDate),
            new FilterField("cardIk2Victim", Properties.Resources.cardVictim, FilterValueType.valueString),
            new FilterField("cardIk2Organ", Properties.Resources.cardOrgan, FilterValueType.valueList),
            new FilterField("cardIk2Department", Properties.Resources.cardDepartment, FilterValueType.valueList),
            new FilterField("cardIk2ExamNumber", Properties.Resources.cardExamNumber, FilterValueType.valueString),
            new FilterField("cardIk2ExamDate", Properties.Resources.cardExamDate, FilterValueType.valueDate),
            new FilterField("cardIk2Expert", Properties.Resources.cardExpert, FilterValueType.valueInt),
            new FilterField("cardIk2Conclusion", Properties.Resources.cardConclusion, FilterValueType.valueString),
            new FilterField("cardIk2PersonInfo", Properties.Resources.cardPersonInfo, FilterValueType.valueInt),
            new FilterField("cardIk2CreateData", Properties.Resources.cardCreateData, FilterValueType.valueDate),
            new FilterField("cardIk2Ancillary", Properties.Resources.cardAncillary, FilterValueType.valueString),
            new FilterField("cardIk2Category", Properties.Resources.cardIk2Category, FilterValueType.valueList),
            new FilterField("cardIk2SortObject", Properties.Resources.cardIk2Sort, FilterValueType.valueList),
            new FilterField("cardIk2Object", Properties.Resources.cardObject, FilterValueType.valueString),
            new FilterField("cardIk2Size", Properties.Resources.cardSize, FilterValueType.valueString),
            new FilterField("cardIk2Concent", Properties.Resources.cardConcent, FilterValueType.valueString),
            new FilterField("cardIk2Amount", Properties.Resources.cardAmount, FilterValueType.valueString)
        };
        #endregion

    }
    /// <summary>
    /// Реализация фильрта для базы данных Oracle
    /// </summary>
    public class FilterItemOracle : FilterItem
    {
        // Constructors
        public FilterItemOracle(int id) : base()
        {
            base.Id = id;
        }
        
        // Public method
        public override int Save()
        {
            OracleParameter prmRes;
            OracleCommand oracleCommand;
            string sqlInsert = "begin :res := modern.prk_tab.filter_ins(:a_name, :a_text); end;";
            string sqlUpdate = "begin modern.prk_tab.filter_upd(:a_id, :a_name, :a_text); end;";


            WFOracle.DB.StartTransaction();
            try
            {
                base.Text = LoadFilterText();

                if (base.Id == 0)
                {
                    oracleCommand = new OracleCommand(sqlInsert, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                    WFOracle.AddInParameter("a_name", OracleType.NVarChar, base.Name, oracleCommand, false);
                    WFOracle.AddInParameter("a_text", OracleType.NClob, base.Text, oracleCommand, false);
                    prmRes = WFOracle.AddOutParameter("res", OracleType.Number, oracleCommand);
                }
                else
                {
                    oracleCommand = new OracleCommand(sqlUpdate, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                    WFOracle.AddInParameter("a_id", OracleType.Number, base.Id, oracleCommand, false);
                    WFOracle.AddInParameter("a_name", OracleType.NVarChar, base.Name, oracleCommand, false);
                    WFOracle.AddInParameter("a_text", OracleType.NClob, base.Text, oracleCommand, false);
                    prmRes = null;
                }

                oracleCommand.ExecuteNonQuery();
                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }

            if (base.Id == 0 && prmRes != null)
            {
                base.Id = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
                return base.Id;
            }
            else
                return base.Id;

        }
        public override void Load()
        {
            OracleCommand oracleCommnad = new OracleCommand("select id, name, text from modern.filters where id = :id", WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, base.Id, oracleCommnad, false);

            using (OracleDataReader oracleDateReader = oracleCommnad.ExecuteReader())
            {
                if (oracleDateReader.Read())
                {
                    base.Name = oracleDateReader["NAME"].ToString();
                    base.Text = oracleDateReader["TEXT"].ToString();
                }
            }
            // 0-ikl, 1-ik2, 2-areaFind
            string[] partCondition = base.Text.Split('\n');
            // Карточка ИКЛ
            string[] partIkl = partCondition[0].Split('\f');
            for (int i = 0; i < partIkl.Count() - 1; i++)
            {
                string[] partFilterIkl = partIkl[i].Split('\r');
                base.IklFilter[i].Name = partFilterIkl[0];
                base.IklFilter[i].Caption = partFilterIkl[1];
                base.IklFilter[i].FilterFieldType = FilterItem.CreateFilterTypeItem(partFilterIkl[2],
                    partFilterIkl[5], partFilterIkl[6]);
                base.IklFilter[i].IsPartValue = Convert.ToBoolean(partFilterIkl[3]);
                base.IklFilter[i].IsReportShow = Convert.ToBoolean(partFilterIkl[4]);
            }
            // Карточка ИК-2
            string[] partIk2 = partCondition[1].Split('\f');
            for (int i = 0; i < partIk2.Count() - 1; i++)
            {
                string[] partFilterIk2 = partIk2[i].Split('\r');
                base.Ik2Filter[i].Name = partFilterIk2[0];
                base.Ik2Filter[i].Caption = partFilterIk2[1];
                base.Ik2Filter[i].FilterFieldType = FilterItem.CreateFilterTypeItem(partFilterIk2[2],
                    partFilterIk2[5], partFilterIk2[6]);
                base.Ik2Filter[i].IsPartValue = Convert.ToBoolean(partFilterIk2[3]);
                base.Ik2Filter[i].IsReportShow = Convert.ToBoolean(partFilterIk2[4]);
            }

            // Облавсть поиска
            string[] areaFind = partCondition[2].Split('\r');
            base.InIkl = areaFind[0].ToUpper() == "TRUE";
            base.InIk2 = areaFind[1].ToUpper() == "TRUE";
            base.InArchive = areaFind[2].ToUpper() == "TRUE";

        }
        public override void Apply()
        {
            OracleCommand oracleCommand = new OracleCommand("", WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            StringBuilder sql = new StringBuilder("SELECT ");
            StringBuilder sqlSelectedFieldsIkl = new StringBuilder("");
            StringBuilder sqlSelectedFieldsIk2 = new StringBuilder("");
            List<SelectedField> selectedFields = new List<SelectedField>();

            if (this.InIkl)
            {
                sqlSelectedFieldsIkl.Append(ForSelectFields(FieldAccessory.ikl, ref selectedFields));
                sql.Append(sqlSelectedFieldsIkl);
                if (this.InArchive)
                    sql.Append(" FROM (SELECT * FROM MODERN.IKL UNION ALL SELECT * FROM MODERN.IKL_ARCH) I," +
                               " (SELECT * FROM MODERN.CARD UNION ALL SELECT * FROM MODERN.CARD_ARCH) C WHERE C.ID = I.ID ");
                else
                    sql.Append(" FROM MODERN.IKL I, MODERN.CARD C WHERE C.ID = I.ID ");
                sql.Append(
                    ForConditionWhere((
                    from FilterField innerfilterField in this.IklFilter
                    where !innerfilterField.IsDefaultValue
                    select innerfilterField).ToArray<FilterField>(), oracleCommand)
                );
            } 
            else
            {
                sqlSelectedFieldsIk2.Append(ForSelectFields(FieldAccessory.ik2, ref selectedFields));
                sql.Append(sqlSelectedFieldsIk2);
                if (this.InArchive)
                    sql.Append(" FROM (SELECT * FROM MODERN.IK2 UNION ALL SELECT * FROM MODERN.IK2_ARCH) I," +
                               " (SELECT * FROM MODERN.CARD UNION ALL SELECT * FROM MODERN.CARD_ARCH) C WHERE I.ID = C.ID");
                else
                    sql.Append(" FROM MODERN.IK2 I, MODERN.CARD C WHERE C.ID = I.ID");
                sql.Append(
                    ForConditionWhere((
                    from FilterField innerfilterField in this.Ik2Filter
                    where !innerfilterField.IsDefaultValue
                    select innerfilterField).ToArray<FilterField>(), oracleCommand)
                );
            } 

            if (String.IsNullOrEmpty((sqlSelectedFieldsIkl.ToString() + sqlSelectedFieldsIk2.ToString()).Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.NotSelectedReportFIeld);

            oracleCommand.CommandText = sql.ToString();

            OracleDataAdapter dataAdapter = new OracleDataAdapter(oracleCommand);
            DataTable dataTable = new DataTable("filterResult");

            if (base.DataGridView != null)
            {
                foreach (SelectedField selectedField in selectedFields.Where(n=>n != null))
                {
                    DataColumn dataColumn = new DataColumn(selectedField.DbNameShort, Type.GetType(ConvertFilterType(selectedField.FilterValueType)));
                    dataColumn.Caption = selectedField.Caption;
                    dataTable.Columns.Add(dataColumn);
                }

                base.DataGridView.DataSource = dataTable;
                dataAdapter.Fill(dataTable);

                foreach (DataColumn dc in dataTable.Columns)
                    base.DataGridView.Columns[dc.ColumnName].HeaderText = dc.Caption;
                base.DataGridView.Columns[0].HeaderText += String.Format(resDataNames.formFindCountOnHeader,
                    dataTable.Rows.Count);
            }
        }

        // Private methods
        private string LoadFilterText()
        {
            StringBuilder filterText = new StringBuilder();
            foreach (FilterField filterField in this.IklFilter)
                filterText.Append(
                    String.Format("{0}\r{1}\r{2}\r{3}\r{4}\r{5}\r{6}\f",
                                filterField.Name, filterField.Caption, filterField.FilterFieldType.GetType().Name,
                                filterField.IsPartValue.ToString(), filterField.IsReportShow.ToString(),
                                filterField.FilterFieldType.Condition.Name, filterField.FilterFieldType.Condition.Value));
            filterText.Append('\n');
            foreach (FilterField filterField in this.Ik2Filter)
                filterText.Append(
                    String.Format("{0}\r{1}\r{2}\r{3}\r{4}\r{5}\r{6}\f",
                                filterField.Name, filterField.Caption, filterField.FilterFieldType.GetType().Name,
                                filterField.IsPartValue.ToString(), filterField.IsReportShow.ToString(),
                                filterField.FilterFieldType.Condition.Name, filterField.FilterFieldType.Condition.Value));
            filterText.Append('\n');
            filterText.Append(String.Format("{0}\r{1}\r{2}", base.InIkl.ToString(), base.InIk2.ToString(), base.InArchive.ToString()));
            return filterText.ToString();
        }
        private string ForConditionWhere(FilterField[] filterFields, OracleCommand command)
        {
            StringBuilder stringCondition = new StringBuilder(" ");
            foreach (FilterField filterField in filterFields)
            {
                switch (filterField.FilterValueType)
                {
                    case FilterValueType.valueInt:
                        #region intValue
                        switch (filterField.FilterFieldType.GetType().Name)
                        {
                            case "FilterFieldRange":
                                FilterFieldRange filterFieldRange = filterField.FilterFieldType as FilterFieldRange;
                                if (filterFieldRange != null)
                                {
                                    if (!String.IsNullOrEmpty(filterField.FilterFieldType.Condition.Value))
                                    {

                                        int fromValue;
                                        int toValue;
                                        try
                                        {
                                            fromValue = Convert.ToInt32(filterFieldRange.ValueFrom, CultureInfo.InvariantCulture);
                                            toValue = Convert.ToInt32(filterFieldRange.ValueTo, CultureInfo.InvariantCulture);
                                        }
                                        catch
                                        {
                                            throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorIntBetween, filterField.Caption));
                                        }
                                        WFOracle.AddInParameter("P_FROM_" + compareFields[filterField.Name].DbName, OracleType.Number, fromValue, command, false);
                                        WFOracle.AddInParameter("P_TO_" + compareFields[filterField.Name].DbName, OracleType.Number, toValue, command, false);
                                        stringCondition.Append(String.Format(" AND {0} BETWEEN :{1} AND :{2}", FieldName(compareFields[filterField.Name].DbName),
                                            "P_FROM_" + compareFields[filterField.Name].DbName, "P_TO_" + compareFields[filterField.Name].DbName));
                                    }
                                }
                                break;
                            case "FilterFieldList":
                                try
                                {
                                    FilterFieldList filterFieldList = filterField.FilterFieldType as FilterFieldList;
                                    if (filterFieldList != null)
                                    {
                                        if (!String.IsNullOrEmpty(filterFieldList.Condition.Value))
                                        {
                                            foreach (FieldCondition conditionValue in filterFieldList.Value)
                                                Convert.ToInt32(conditionValue.Value, CultureInfo.InvariantCulture);

                                            stringCondition.Append(String.Format(" AND {0} IN ({1})",
                                                FieldName(compareFields[filterField.Name].DbName), filterField.FilterFieldType.Condition.Value));
                                        }
                                    }
                                }
                                catch
                                {
                                    throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorIntList, filterField.Caption));
                                }
                                break;
                            case "FilterFieldOne":
                                try
                                {
                                    if (!String.IsNullOrEmpty(filterField.FilterFieldType.Condition.Value))
                                    {
                                        int filterValue = Convert.ToInt32(filterField.FilterFieldType.Condition.Value, CultureInfo.InvariantCulture);
                                        WFOracle.AddInParameter("P_" + compareFields[filterField.Name].DbName, OracleType.Number, filterValue, command, false);
                                        stringCondition.Append(String.Format(" AND {0} = :{1}", FieldName(compareFields[filterField.Name].DbName), "P_" + compareFields[filterField.Name].DbName));
                                    }
                                }
                                catch (Exception)
                                {
                                    throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorIntValue, filterField.Caption));
                                }
                                break;
                            default:
                                continue;
                        }
                        #endregion
                        break;
                    case FilterValueType.valueString:
                        #region StringValue
                        switch (filterField.FilterFieldType.GetType().Name)
                        {
                            case "FilterFieldList":
                                stringCondition.Append(String.Format(" AND {0} IN ('{1}')", 
                                    FieldName(compareFields[filterField.Name].DbName), 
                                    filterField.FilterFieldType.Condition.Value.Split(',').
                                    Aggregate((curr, next)=>curr+"','"+next)                                    
                                    ));
                                break;
                            case "FilterFieldOne":
                                if (filterField.IsPartValue)
                                {
                                    stringCondition.Append(String.Format(" AND {0} LIKE '%{1}%'", 
                                        FieldName(compareFields[filterField.Name].DbName),
                                        filterField.FilterFieldType.Condition.Value));
                                }
                                else
                                {
                                    WFOracle.AddInParameter("P_" + compareFields[filterField.Name].DbName,
                                        OracleType.NVarChar, filterField.FilterFieldType.Condition.Value, command, false);
                                    stringCondition.Append(String.Format(" AND {0} = :{1}", 
                                        FieldName(compareFields[filterField.Name].DbName), 
                                        "P_" + compareFields[filterField.Name].DbName));
                                }
                                break;
                            default:
                                continue;
                        }
                        #endregion
                        break;
                    case FilterValueType.valueDate:
                        #region DateValue
                        switch (filterField.FilterFieldType.GetType().Name)
                        {
                            case "FilterFieldRange":
                                FilterFieldRange filterFieldRange = filterField.FilterFieldType as FilterFieldRange;
                                if (filterFieldRange != null)
                                {
                                    DateTime fromValue;
                                    DateTime toValue;
                                    try
                                    {
                                        fromValue = Convert.ToDateTime(filterFieldRange.ValueFrom);
                                        toValue = Convert.ToDateTime(filterFieldRange.ValueTo);
                                    }
                                    catch
                                    {
                                        throw new WFException(ErrType.Assert, ErrorsMsg.ErrorDateBetween);
                                    }
                                    WFOracle.AddInParameter("P_FROM_" + compareFields[filterField.Name].DbName, OracleType.DateTime, fromValue.ToShortDateString(), command, false);
                                    WFOracle.AddInParameter("P_TO_" + compareFields[filterField.Name].DbName, OracleType.DateTime, toValue.ToShortDateString(), command, false);
                                    stringCondition.Append(String.Format(" AND TRUNC({0}) BETWEEN :{1} AND :{2}",
                                        FieldName(compareFields[filterField.Name].DbName), "P_FROM_" + compareFields[filterField.Name].DbName, "P_TO_" +
                                        compareFields[filterField.Name].DbName));
                                }
                                break;
                            case "FilterFieldList":
                                try
                                {
                                    FilterFieldList filterFieldList = filterField.FilterFieldType as FilterFieldList;
                                    if (filterFieldList != null)
                                    {
                                        if (!String.IsNullOrEmpty(filterFieldList.Condition.Value))
                                        {
                                            var shortDate = (from FieldCondition conditionValue in filterFieldList.Value
                                                     select Convert.ToDateTime(conditionValue.Value).ToShortDateString()).
                                                     Aggregate((curr, next) => curr + "','" + next);

                                            stringCondition.Append(String.Format(" AND TRUNC({0}) IN ('{1}')",
                                                FieldName(compareFields[filterField.Name].DbName), shortDate));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorDateList, filterField.Caption));
                                }
                                break;
                            case "FilterFieldOne":
                                try
                                {
                                    if (!String.IsNullOrEmpty(filterField.FilterFieldType.Condition.Value))
                                    {
                                        DateTime filterValue = Convert.ToDateTime(filterField.FilterFieldType.Condition.Value);
                                        WFOracle.AddInParameter("P_" + compareFields[filterField.Name].DbName, OracleType.DateTime, filterValue.ToShortDateString(), command, false);
                                        stringCondition.Append(String.Format(" AND TRUNC({0}) = :{1}", FieldName(compareFields[filterField.Name].DbName), "P_" + compareFields[filterField.Name].DbName));
                                    }
                                }
                                catch (Exception)
                                {
                                    throw new WFException(ErrType.Assert, ErrorsMsg.ErrorDateUserValue);
                                }
                                break;
                            default:
                                continue;
                        }
                        #endregion
                        break;
                    case FilterValueType.valueList:
                        #region ListValue
                        switch (filterField.FilterFieldType.GetType().Name)
                        {
                            case "FilterFieldList":
                                try
                                {
                                    FilterFieldList filterFieldList = filterField.FilterFieldType as FilterFieldList;
                                    if (filterFieldList != null)
                                    {
                                        if (!String.IsNullOrEmpty(filterFieldList.Condition.Value))
                                        {
                                            foreach (FieldCondition conditionValue in filterFieldList.Value)
                                                Convert.ToInt32(conditionValue.Value, CultureInfo.InvariantCulture);

                                            switch (filterField.Name)
                                            {
                                                case "cardIklUkNumber":
                                                case "cardIk2UkNumber":
                                                    stringCondition.Append(String.Format(
                                                        " AND {0} IN (select card_id from modern.card_ukitem u where u.ukitem_id in " +
                                                        " (select id from modern.ukitem u connect by prior id = parent_id start with id in ({1})))",
                                                        FieldName(compareFields[filterField.Name].DbNameShortMutant), filterField.FilterFieldType.Condition.Value));
                                                    break;
                                                case "cardIklDepartment":
                                                case "cardIk2Department":
                                                    stringCondition.Append(String.Format(
                                                        " AND C.ID in (select id from modern.card where expert_id in (select id from modern.expert where division_id in ({0})))",
                                                        filterField.FilterFieldType.Condition.Value));
                                                    break;
                                                default:
                                                    stringCondition.Append(String.Format(" AND {0} IN ({1})",
                                                        FieldName(compareFields[filterField.Name].DbNameShort), filterField.FilterFieldType.Condition.Value));
                                                    break;
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorIntList, filterField.Caption));
                                }
                                break;
                            case "FilterFieldOne":
                                try
                                {
                                    if (!String.IsNullOrEmpty(filterField.FilterFieldType.Condition.Value))
                                    {
                                        int filterValue = Convert.ToInt32(filterField.FilterFieldType.Condition.Value, CultureInfo.InvariantCulture);
                                        switch (filterField.Name)
                                        {
                                            case "cardIklUkNumber":
                                            case "cardIk2UkNumber":
                                                WFOracle.AddInParameter("P_" + compareFields[filterField.Name].DbNameShortMutant, OracleType.Number, filterValue, command, false);
                                                stringCondition.Append(String.Format(
                                                    " AND {0} IN (select card_id from modern.card_ukitem u where u.ukitem_id in"+ 
                                                    " (select id from modern.ukitem u connect by prior id = parent_id start with id = :{1}))",
                                                    FieldName(compareFields[filterField.Name].DbNameShortMutant), "P_" + compareFields[filterField.Name].DbNameShortMutant));
                                                break;
                                            case "cardIklDepartment":
                                            case "cardIk2Department":
                                                WFOracle.AddInParameter("P_" + compareFields[filterField.Name].DbNameShortMutant, OracleType.Number, filterValue, command, false);
                                                stringCondition.Append(String.Format(
                                                    " AND C.ID in (select id from modern.card where expert_id in (select id from modern.expert where division_id = :{0}))",
                                                    "P_" + compareFields[filterField.Name].DbNameShortMutant));
                                                break;
                                            default:
                                                WFOracle.AddInParameter("P_" + compareFields[filterField.Name].DbNameShort, OracleType.Number, filterValue, command, false);
                                                stringCondition.Append(String.Format(" AND {0} = :{1}", FieldName(compareFields[filterField.Name].DbNameShort), "P_" + compareFields[filterField.Name].DbNameShort));
                                                break;
                                        }

                                    }
                                }
                                catch (Exception)
                                {
                                    throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorIntValue, filterField.Caption));
                                }
                                break;
                            default:
                                continue;
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
            }
            return stringCondition.ToString();
        }
        private string ForSelectFields(FieldAccessory fieldAccessory, ref List<SelectedField> selectedFields) 
        {
            List<string> cardsList = new List<string>();
            selectedFields.Clear();
            if (fieldAccessory == FieldAccessory.ikl)
                selectedFields.AddRange(from FilterField innerFilterField in this.IklFilter
                                         where !innerFilterField.IsDefaultValue && innerFilterField.IsReportShow
                                         select compareFields[innerFilterField.Name]);
            else
                selectedFields.AddRange(from FilterField innerFilterField in this.Ik2Filter
                                        where !innerFilterField.IsDefaultValue && innerFilterField.IsReportShow
                                        select compareFields[innerFilterField.Name]);

            foreach (SelectedField selectedField in selectedFields.Where(n=>n != null))
	        {
                if (fieldAccessory == FieldAccessory.ikl) 
                {
                    if (selectedField.FieldAccessory == FieldAccessory.ikl || selectedField.FieldAccessory == FieldAccessory.all)
                    {
                        cardsList.Add(FieldName(selectedField.DbName));
                    }
                    else
                        cardsList.Add("null");
                }

                if (fieldAccessory == FieldAccessory.ik2)
                {
                    if (selectedField.FieldAccessory == FieldAccessory.ik2 || selectedField.FieldAccessory == FieldAccessory.all)
                    {
                        cardsList.Add(FieldName(selectedField.DbName));
                    }
                    else
                        cardsList.Add("null");
                }
            }
            
            if (cardsList.Count > 0)
                return cardsList.Aggregate((curr, next) => curr + ", " + next);
            else
                return "";
        }
        private string FieldName(string fieldName)
        {
            if (fieldName == "ID")
                return "C.ID";
            else
                return fieldName;
        }
        private string ConvertFilterType(FilterValueType filterValueType)
        {
            switch (filterValueType)
            {
                case FilterValueType.valueInt:
                    return "System.Int32";
                case FilterValueType.valueDate:
                    return "System.DateTime";
                default:
                    return "System.String";
            }
        }

        // Fields
        private SelectedFields compareFields = new SelectedFields(new List<SelectedField> (){
         #region Fields
		 // Совпадающие поля карточек ИКЛ и ИК-2
            new SelectedField("cardIklId", "ID", FieldAccessory.all, FilterValueType.valueInt, Properties.Resources.cardId),
            new SelectedField("cardIk2Id", "ID", FieldAccessory.all, FilterValueType.valueInt, Properties.Resources.cardId),
            
            new SelectedField("cardIklCrimNumber", "CRIM_NUM", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardCrimNumber),
            new SelectedField("cardIk2CrimNumber", "CRIM_NUM", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardCrimNumber),

            new SelectedField("cardIklUkNumber", "(select modern.string_agg(to_char(u.state_name)) state from modern.card_ukitem cu, modern.ukitem u where card_id = c.id and u.id = cu.ukitem_id) ID_UK", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardUkNumber),
            new SelectedField("cardIk2UkNumber", "(select modern.string_agg(to_char(u.state_name)) state from modern.card_ukitem cu, modern.ukitem u where card_id = c.id and u.id = cu.ukitem_id) ID_UK", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardUkNumber),

            //new SelectedField("cardIklUkNumber", "ID", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardUkNumber),
            //new SelectedField("cardIk2UkNumber", "ID", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardUkNumber),

            new SelectedField("cardIklOrgan", "(select note from modern.organization where id = org_id) ORG_ID", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardOrgan),
            new SelectedField("cardIk2Organ", "(select note from modern.organization where id = org_id) ORG_ID", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardOrgan),

            new SelectedField("cardIklDepartment", "(select name from modern.division where id = (select division_id from modern.expert where id = expert_id)) DEPT_ID", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardDepartment),
            new SelectedField("cardIk2Department", "(select name from modern.division where id = (select division_id from modern.expert where id = expert_id)) DEPT_ID", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardDepartment),

            new SelectedField("cardIklExpert", "(select surname||' '||name name from modern.expert where id = expert_id) EXPERT_ID", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardExpert),
            new SelectedField("cardIk2Expert", "(select surname||' '||name name from modern.expert where id = expert_id) EXPERT_ID", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardExpert),

            new SelectedField("cardIklExamNumber", "EXAM_NUM", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardExamNumber),
            new SelectedField("cardIk2ExamNumber", "EXAM_NUM", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardExamNumber),

            new SelectedField("cardIklExamDate", "EXAM_DATE", FieldAccessory.all, FilterValueType.valueDate, Properties.Resources.cardExamDate),
            new SelectedField("cardIk2ExamDate", "EXAM_DATE", FieldAccessory.all, FilterValueType.valueDate, Properties.Resources.cardExamDate),

            new SelectedField("cardIklExpertConclusion", "EXAM_NOTE", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardExpertConclusion),
            new SelectedField("cardIk2Conclusion", "EXAM_NOTE", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardExpertConclusion),

            new SelectedField("cardIklCreateData", "DATE_INS", FieldAccessory.all, FilterValueType.valueDate, Properties.Resources.cardCreateData),
            new SelectedField("cardIk2CreateData", "DATE_INS", FieldAccessory.all, FilterValueType.valueDate, Properties.Resources.cardCreateData),

            new SelectedField("cardIklNumber", "CARD_NUM", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardNumber),
            new SelectedField("cardIk2Number", "CARD_NUM", FieldAccessory.all, FilterValueType.valueString, Properties.Resources.cardNumber),
            
            // Поля для ИКЛ
            new SelectedField("cardIklSuspect", "PERSON", FieldAccessory.ikl, FilterValueType.valueString, Properties.Resources.cardSuspect),
            new SelectedField("cardIklAncillary", "ANCILLARY", FieldAccessory.ikl, FilterValueType.valueString, Properties.Resources.cardAncillary),
            new SelectedField("cardIklCategory", "(select name from modern.class_ikl where id = class_id) CLASS_ID", FieldAccessory.ikl, FilterValueType.valueString, Properties.Resources.cardIk2Sort),
            
            // Поля для ИК-2
            new SelectedField("cardIk2CodeMvd", "(select short_name from modern.code_mvd where id = mvd_id) MVD_ID", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardCodeMvd),
            new SelectedField("cardIk2CodeLin", "(select organ from modern.code_lin where id = LIN_ID) LIN_ID", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardCodeLin),
            new SelectedField("cardIk2Year", "YEAR_STATE", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardYear),
            new SelectedField("cardIk2Address", "ADDRESS_CRIME", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardAddress),
            new SelectedField("cardIk2Victim", "VICTIM", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardVictim),
            new SelectedField("cardIk2Ancillary", "SN_DNA", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardAncillary),
            new SelectedField("cardIk2Object", "OBJ", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardObject),
            new SelectedField("cardIk2Size", "SPOTSIZE", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardSize),
            new SelectedField("cardIk2Concent", "CONCENT", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardConcent),
            new SelectedField("cardIk2Amount", "AMOUNT", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardAmount),
            new SelectedField("cardIk2CrimDate", "DATE_CRIME", FieldAccessory.ik2, FilterValueType.valueDate, Properties.Resources.cardCrimDate),
            new SelectedField("cardIk2Category", "(select short_name from modern.class_object where id = CLASS_ID) CLASS_ID", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardIk2Category),
            new SelectedField("cardIk2SortObject", "(select short_name from modern.sort_object where id = SORT_ID) SORT_ID", FieldAccessory.ik2, FilterValueType.valueString, Properties.Resources.cardIk2Sort)
	    #endregion
        });
    }
        
    /// <summary>
    /// Отдельное поле фильтра
    /// </summary>
    public class FilterField
    {
        public FilterField() { }
        public FilterField(string name, string caption, FilterValueType filterValueType) : this ()
        {
            this.Name = name;
            this.Caption = caption;
            this.FilterValueType = filterValueType;
            this.FilterFieldType = new FilterFieldAll();
        }

        // Properties
        public string Name { get; set; }
        public string Caption { get; set; }
        public FilterValueType FilterValueType { get; set; }
        public FilterFieldType FilterFieldType { get; set; }
        public virtual bool IsDefaultValue 
        {
            get
            {
                return (this.FilterFieldType.Condition.IsDefault && IsReportShow == false);
            }
        }
        public bool IsReportShow { get; set; }
        public bool IsPartValue { get; set; }

    }
    public abstract class FilterFieldType
    {
        public FilterFieldType()
        {
            this.Condition = new FieldCondition("", "");
        }
        public FieldCondition Condition { get; set;}
    }
    public class FilterFieldRange : FilterFieldType
    {
        public FilterFieldRange() : base() { }
        public FilterFieldRange(string name) : this()
        {
            string[] values = name.Split('-');
            if (values.Count() > 1)
            {
                this.ValueFrom = values[0];
                this.ValueTo = values[1];
            }
        }

        public string ValueFrom 
        {
            get
            {
                return this.valueFrom;
            }
            set
            {
                this.valueFrom = value;
                if (String.IsNullOrEmpty(this.valueFrom) && String.IsNullOrEmpty(this.valueTo))
                    base.Condition = new FieldCondition("", "");
                else
                    base.Condition = new FieldCondition(valueFrom + "-" + valueTo, valueFrom + "-" + valueTo);
            }
        }
        public string ValueTo
        {
            get
            {
                return this.valueTo;
            }
            set
            {
                this.valueTo = value;
                if (String.IsNullOrEmpty(this.valueFrom) && String.IsNullOrEmpty(this.valueTo))
                    base.Condition = new FieldCondition("", "");
                else
                    base.Condition = new FieldCondition(valueFrom + "-" + valueTo, valueFrom + "-" + valueTo);
            }
        }

        // Fields
        private string valueFrom;
        private string valueTo;
    }
    public class FilterFieldList : FilterFieldType
    {
        public FilterFieldList() : base() { } 
        public FilterFieldList(string name, string value) : this()
        {
            string[] names = name.Split(',');
            string[] values = value.Split(',');
            if (values.Count() > 0 && names.Count() > 0)
                for (int i = 0; i < names.Count(); i++)
                    this.AddValue(new FieldCondition(names[i], values[i]));   

        }
        
        // Methods
        public void AddValue(FieldCondition fieldCondition)
        {
            fieldConditions.Add(fieldCondition);
            base.Condition = new FieldCondition(
                fieldConditions.Select(curr => curr.Name).Aggregate((curr, next) => curr + ',' + next),
                fieldConditions.Select(curr => curr.Value).Aggregate((curr, next) => curr + ',' + next)
                );
        }
        public void DeleteByValue(string value)
        {
            this.findValue = value;
            FieldCondition findFieldCondition = fieldConditions.Find(FindByValue);
            if (findFieldCondition != null)
            {
                fieldConditions.Remove(findFieldCondition);
                base.Condition = new FieldCondition(
                    fieldConditions.Select(curr => curr.Name).Aggregate((curr, next) => curr + ',' + next),
                    fieldConditions.Select(curr => curr.Value).Aggregate((curr, next) => curr + ',' + next)
                    );
            }
        }
        public void DeleteByName(string name)
        {
            this.findName = name;
            FieldCondition findFieldCondition = fieldConditions.Find(FindByName);
            if (findFieldCondition != null)
            {
                fieldConditions.Remove(findFieldCondition);
                if (fieldConditions.Count > 0)
                    base.Condition = new FieldCondition(
                        fieldConditions.Select(curr => curr.Name).Aggregate((curr, next) => curr + ',' + next),
                        fieldConditions.Select(curr => curr.Value).Aggregate((curr, next) => curr + ',' + next)
                        );
                else
                    base.Condition = new FieldCondition("", "");
            }
        }
        public void AddRange(FieldCondition[] fieldConditions)
        {
            this.fieldConditions.Clear();
            this.fieldConditions.AddRange(fieldConditions);
            base.Condition = new FieldCondition(
                fieldConditions.Select(curr => curr.Name).Aggregate((curr, next) => curr + ',' + next),
                fieldConditions.Select(curr => curr.Value).Aggregate((curr, next) => curr + ',' + next)
                );
        }
        public void AddRangeForString(FieldCondition[] fieldConditions)
        {
            this.fieldConditions.Clear();
            this.fieldConditions.AddRange(fieldConditions);
            base.Condition = new FieldCondition(
                fieldConditions.Select(curr => curr.Name).Aggregate((curr, next) => "'"+curr + "','" + next + "'"),
                fieldConditions.Select(curr => curr.Value).Aggregate((curr, next) => "'" + curr + "','" + next + "'")
                );
        }

        // Properties
        public Collection<FieldCondition> Value 
        {
            get
            {
                return new Collection<FieldCondition>(this.fieldConditions);
            }
        }

        // Private methods
        private bool FindByName(FieldCondition fieldCondition)
        {
            return this.findName == fieldCondition.Name;
        }
        private bool FindByValue(FieldCondition fieldCondition)
        {
            return this.findValue == fieldCondition.Value;
        }

        // Fields
        private List<FieldCondition> fieldConditions = new List<FieldCondition>();
        string findValue;
        string findName;
    }
    public class FilterFieldOne : FilterFieldList
    {
        public FilterFieldOne() : base() { }
        public FilterFieldOne(string name, string value) : base(name, value) {}
    }
    public class FilterFieldAll : FilterFieldType
    { 
        public FilterFieldAll() : base()
        {
            base.Condition = new FieldCondition("", "");
        }
        
    }

    /// <summary>
    /// Условие фильтрации для поля
    /// </summary>
    public class FieldCondition
    {
        public FieldCondition() { }
        public FieldCondition(string name, string value)
            : this()
        {
            this.Name = name;
            this.Value = value;
        }

        // Properties
        public string Value {get; set; }
        public string Name { get; set; }
        public bool IsDefault { get { return String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(Value); } }
    }
    internal enum FieldAccessory { ikl, ik2, all}
    internal class SelectedField
    {
        public SelectedField(){}
        public SelectedField(string name, string fieldDbName, FieldAccessory fieldAccessory, FilterValueType filterValueType, string caption)
        {
            this.Name = name;
            this.DbName = fieldDbName;
            this.Caption = caption;
            this.FieldAccessory = fieldAccessory;
            this.FilterValueType = filterValueType;
        }

        public string Name { get; set; }
        public string DbName { get; set; }
        public string DbNameShort 
        {
            get 
            {
                if (DbName.ToUpper().StartsWith("(SELECT"))
                    return DbName.Split(' ').Last();
                else
                    return DbName;
            }
        }
        public string DbNameShortMutant
        { 
            get
            {
                return DbNameShort.Split('_').First();
            }
        }

        public string Caption { get; set; }
        public FilterValueType FilterValueType { get; set; }
        public FieldAccessory FieldAccessory { get; set; }

        internal class EqualityDbName : IEqualityComparer<SelectedField>
        {
            public bool Equals(SelectedField x, SelectedField y)
            {
                return x.DbName.Equals(y.DbName);
            }
            public int GetHashCode(SelectedField obj)
            {
                return obj.DbName.GetHashCode();
            }
        }
        internal class EqualityName : IEqualityComparer<SelectedField>
        {
            public bool Equals(SelectedField x, SelectedField y)
            {
                return x.Name.Equals(y.Name);
            }
            public int GetHashCode(SelectedField obj)
            {
                return obj.DbName.GetHashCode();
            }
        }
        
    }
    internal class SelectedFields
    {
        // Constructors
        public SelectedFields() 
        { 
            this.selectedFileds = new List<SelectedField>();
        }
        public SelectedFields(List<SelectedField> selectedFileds)
        {
            this.selectedFileds = selectedFileds;
        }

        // Interface
        public SelectedField this[string item]
        {
            get
            {
                this.findField = item;
                return selectedFileds.Find(FindFieldByName);
            }
        }

        // Private methods
        private bool FindFieldByName(SelectedField item)
        {
            return this.findField.ToUpper() == item.Name.ToUpper();
        }
    
        // Fields
        private List<SelectedField> selectedFileds;
        private string findField;
    }
    public enum FilterValueType { valueInt, valueString, valueDate, valueList }
}
