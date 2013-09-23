using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WFDatabase;
using System.Windows.Forms;
using WFExceptions;
using System.Data.OracleClient;

namespace GeneLibrary.Items
{
    public class History
    {
        public void GetFilteredCards(FilterHistoryField[] filterFields)
        {
            gate.Find(filterFields);
        }
        public void GetSearchedCards(FilterHistoryField[] filterFields, CardFields cardField)
        {
            if (cardField == CardFields.none)
                gate.FastFind(filterFields[0].Value[0].ToString());
            else
                gate.FindByField(filterFields);
        }
        public void GetCardHistory(int cardId)
        {
            gate.CardHistory(cardId);
        }

        public DataTable DT { get { return gate.DT; } }
        public DataTable HT { get { return gate.HT; } }

        // Private members
        private HistoryGate gate = GateFactory.HistoryGate();
    }

    public abstract class HistoryGate
    {
        public abstract void Find(FilterHistoryField[] filterFields);
        public abstract void FastFind(string condition);
        public abstract void FindByField(FilterHistoryField[] filterFields);
        public abstract void CardHistory(int cardId);

        public DataTable DT { get { return dt; } }
        public DataTable HT { get; set; }

        // Fields
        private DataTable dt = new DataTable("tableCards");
    }

    public class HistoryGateOracle: HistoryGate
    {
        public override void Find(FilterHistoryField[] filterFields)
        {
            OracleCommand command = new OracleCommand("", WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            StringBuilder sql = new StringBuilder(
                "select h.obj_id, nvl(c.card_num, 'Удалена') name " +
                "from modern.history h left join modern.card c on h.obj_id = c.id, modern.history_action ha " +
                "where ha.group_id = 1 and h.HIS_TYPE_ID = 1");

            prepareWhere(filterFields, sql, command);
            sql.Append("group by h.obj_id, c.card_num  order by h.obj_id");

            command.CommandText = sql.ToString();
            OracleDataAdapter dataAdapter = new OracleDataAdapter(command);
            base.DT.Clear();
            dataAdapter.Fill(base.DT);
        }
        public override void FastFind(string condition)
        {
            OracleCommand command = new OracleCommand("", WFOracle.DB.OracleConnection);

            command.CommandText =
                "select c.id, c.card_num, c.id||' {'||c.card_num||'}' name from modern.card c ";
            command.CommandText += " where c.hash like :filterString";
            WFOracle.AddInParameter("filterString", OracleType.NVarChar, String.Format("%{0}%", condition.ToUpper()), command, false);
            
            OracleDataAdapter dataAdapter = new OracleDataAdapter(command);
            base.DT.Clear();
            dataAdapter.Fill(base.DT);
        }
        public override void FindByField(FilterHistoryField[] filterFields)
        {
            OracleCommand command = new OracleCommand("", WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            StringBuilder sql = new StringBuilder(
                "select c.id, c.card_num, c.id||' {'||c.card_num||'}' name from modern.card c where 1=1 ");

            prepareWhere(filterFields, sql, command);
            sql.Append("order by id");

            command.CommandText = sql.ToString();
            OracleDataAdapter dataAdapter = new OracleDataAdapter(command);
            base.DT.Clear();
            dataAdapter.Fill(base.DT);
        }
        public override void CardHistory(int cardId)
        {
            OracleCommand command = new OracleCommand("", WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            String sql =
                " select id, date_insert, to_char(note) note," +
                " (select surname||' '||name||' '||patronic from modern.expert where id = h.expert_id) expert,"+
                " (select name from modern.history_action where id = h.action_id) action"+
                " from modern.history h connect by prior h.parent_id = h.id start with "+
                " h.id = (select max(id) from modern.history where obj_id = :p_id and HIS_TYPE_ID = 1) order by id";

            command.CommandText = sql;
            WFOracle.AddInParameter("p_id", OracleType.Number, cardId, command, false);
            OracleDataAdapter dataAdapter = new OracleDataAdapter(command);
            CreateDataTableColumns();
            dataAdapter.Fill(base.HT);
        }

        // Private methods
        private void prepareWhere(FilterHistoryField[] filterFields, StringBuilder sql, OracleCommand command)
        {
            foreach (FilterHistoryField filterField in filterFields)
            {
                switch (filterField.FilterType)
                {
                    case TypeHistoryFilter.single:
                        if (filterField.OracleType == OracleType.DateTime)
                            sql.Append(String.Format("and trunc({0}) =:{1} ", fieldName(filterField.Name, filterField.prefix), paramName(filterField.Name)));
                        else
                            sql.Append(String.Format("and {0} =:{1} ", fieldName(filterField.Name, filterField.prefix), paramName(filterField.Name)));
                        WFOracle.AddInParameter(paramName(filterField.Name), filterField.OracleType, filterField.Value[0], command, true);
                        break;
                    case TypeHistoryFilter.between:
                        if (filterField.OracleType == OracleType.DateTime)
                            sql.Append(String.Format("and trunc({0}) between :{1}_from and :{1}_to ", fieldName(filterField.Name, filterField.prefix), paramName(filterField.Name))); 
                        else
                            sql.Append(String.Format("and {0} between :{1}_from and :{1}_to ", fieldName(filterField.Name, filterField.prefix), paramName(filterField.Name))); 
                        
                        WFOracle.AddInParameter(paramName(filterField.Name)+"_from", filterField.OracleType, filterField.Value[0], command, true);
                        WFOracle.AddInParameter(paramName(filterField.Name)+"_to", filterField.OracleType, filterField.Value[1], command, true);
                        break;
                    case TypeHistoryFilter.list:
                        if (filterField.OracleType == OracleType.DateTime)
                            sql.Append(String.Format("and trunc({0}) in ({1}) ", fieldName(filterField.Name, filterField.prefix), 
                                filterField.Value.Aggregate((n, next)=>n + ", " + next)));
                        else
                            sql.Append(String.Format("and {0} in ({1}) ", fieldName(filterField.Name, filterField.prefix),
                                filterField.Value.Where(n=>!String.IsNullOrEmpty(n.ToString().Trim())).Aggregate((n, next) => n + ", " + next)));
                        break;
                    default:
                        break;
                }
            }
        }
        private string fieldName(HistoryFields field, string pref)
        {
            switch (field)
            {
                case HistoryFields.id:
                    if (pref.ToLower() == "h")
                        return "h.obj_id";
                    else
                        return pref + ".id";
                case HistoryFields.expert:
                    return pref + ".expert_id";
                case HistoryFields.action:
                    return pref + ".action_id";
                case HistoryFields.parent:
                    return pref + "parent_id";
                case HistoryFields.date_ins:
                    if (pref.ToLower() == "h")
                        return "h.date_insert";
                    else
                        return pref + ".date_ins";
                case HistoryFields.note:
                    return pref + ".note";
                case HistoryFields.kind_id:
                    return pref + ".kind_id";
                case HistoryFields.card_num:
                    return pref + ".card_num";
                case HistoryFields.crim_num:
                    return pref + ".crim_num";
                default:
                    throw new WFException(ErrType.Assert, String.Format(ErrorsMsg.NotValueInEnum, "HistoryFields", field.ToString()));
            }
        }
        private string paramName(HistoryFields field)
        {
            switch (field)
            {
                case HistoryFields.id:
                    return "p_id";
                case HistoryFields.expert:
                    return "p_expert_id";
                case HistoryFields.action:
                    return "p_action_id";
                case HistoryFields.parent:
                    return "p_parent_id";
                case HistoryFields.date_ins:
                    return "p_date_ins";
                case HistoryFields.note:
                    return "p_note";
                case HistoryFields.kind_id:
                    return "p_kind_id";
                case HistoryFields.card_num:
                    return "p_card_num";
                case HistoryFields.crim_num:
                    return "p_crim_num";
                default:
                    throw new WFException(ErrType.Assert, String.Format(ErrorsMsg.NotValueInEnum, "HistoryFields", field.ToString()));
            }
        }
        private void CreateDataTableColumns()
        {
            base.HT = new DataTable("tableHistory");
            DataColumn dateIns = new DataColumn("DATE_INSERT", Type.GetType("System.DateTime"));
            dateIns.Caption = resDataNames.historyTableDate;
            DataColumn expert = new DataColumn("EXPERT", Type.GetType("System.String"));
            expert.Caption = resDataNames.historyTableExpert;
            DataColumn action = new DataColumn("ACTION", Type.GetType("System.String"));
            action.Caption = resDataNames.historyTableAction;

            this.HT.Columns.Add(dateIns);
            this.HT.Columns.Add(expert);
            this.HT.Columns.Add(action);
        }
    }

    /// <summary>
    /// Условие фильтрации по полю истории
    /// </summary>
    public class FilterHistoryField
    {
        public FilterHistoryField() { }
        /// <summary>
        /// Фильтр состоящий из одного значения
        /// </summary>
        /// <param name="name">Имя фильтруемого поля</param>
        /// <param name="value">Значение фильтра</param>
        public FilterHistoryField(string pref, HistoryFields name, object value, OracleType oracleType)
        {
            this.prefix = pref;
            this.Name = name;
            this.Value.Add(value);
            this.filterType = TypeHistoryFilter.single;
            this.oracleType = oracleType;
        }
        /// <summary>
        /// Фильтр содержит множество значений
        /// </summary>
        /// <param name="name">Имя фильтруемого поля</param>
        /// <param name="value">Список значений</param>
        public FilterHistoryField(string pref, HistoryFields name, object[] value, OracleType oracleType)
        {
            this.prefix = pref;
            this.Name = name;
            this.Value.AddRange(value);
            this.filterType = TypeHistoryFilter.list;
            this.oracleType = oracleType;
        }
        /// <summary>
        /// Фильтр содержит диапазон значений
        /// </summary>
        /// <param name="name">Имя фильтруемого поля</param>
        /// <param name="valueFrom">Первой значение из диапазона</param>
        /// <param name="valueTo">Последнее значение диапазона</param>
        public FilterHistoryField(string pref, HistoryFields name, object valueFrom, object valueTo, OracleType oracleType)
        {
            this.prefix = pref;
            this.Name = name;
            this.Value.Add(valueFrom);
            this.Value.Add(valueTo);
            this.filterType = TypeHistoryFilter.between;
            this.oracleType = oracleType;
        }

        public HistoryFields Name;
        public string prefix;
        public List<object> Value = new List<object>();
        public TypeHistoryFilter FilterType 
        { 
            get { return this.filterType; }
            set { this.filterType = value; }
        }
        public OracleType OracleType 
        { 
            get { return this.oracleType; }
            set { this.oracleType = value; }
        }

        // Fields
        private TypeHistoryFilter filterType;
        private OracleType oracleType;
    }

    public enum HistoryFields { id, expert, action, parent, date_ins, note, kind_id, card_num, crim_num }
    public enum CardFields { none, id, number, victim, uk_number, date_ins}
    public enum TypeHistoryFilter { single, between, list}
}
