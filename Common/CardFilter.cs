using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using WFDatabase;

namespace GeneLibrary.Common
{
    /// <summary>
    /// В конструктор поступает выражение из файла, содержащее данные структурированные следующим образом:
    ///
    /// Разделитель ";" - разделяет набор столбцов по которым производится фильтрация, 
    /// "^" - Имя, условие, тип значения, параметр, значение
    ///    
    /// Name!ID^
    /// Condition! > ^
    /// Type!System.Int32^
    /// Param!:pID^
    /// Value!1;
    ///
    /// Name!CARD^
    /// Condition! = ^
    /// Type!System.String^
    /// Param!:pCARD^
    /// Value!fff;
    /// 
    /// Класс преобразует эти данные в следущие сущности:
    ///     1. Строка условия WHERE к базе данных, содержащая параметры, а не значения!!! Это необходимо для
    ///     оптимизации, поскольку запрос с конкретными значениями - постоянно парсить Oraclom, а параметрический 
    ///     запрос парситься однажды, потом сохраняется в кэше.
    ///     Метод выполняющий этот функционал - FilterString()
    ///     2. Создается список параметров запроса с их значениями - метод SetParameters(OracleCommand oracleCommand)
    /// </summary>
    class CardsFilterOracle
    {
        // Constructors
        public CardsFilterOracle(string filter)
        {
            var Columns = from s in filter.Split(';')
                          where !String.IsNullOrEmpty(s)
                          select s;

            foreach (string s in Columns)
            {
                string[] ss = s.Split('^');
                filterFieldsList.Add(new FilterField()
                {
                    Name = ss[0].Split('!')[1],
                    Condition = ss[1].Split('!')[1],
                    Type = ss[2].Split('!')[1],
                    Param = ss[3].Split('!')[1],
                    FieldValue = ss[4].Split('!')[1]
                });
            }
        }

        // Interface methods
        public string FilterString()
        {
            if (filterFieldsList.Count == 0)
                return "";
            
            StringBuilder sb = new StringBuilder();
            foreach (FilterField f in filterFieldsList)
            {
                switch (f.Condition)
                {
                    case "like":
                    case "like$start":
                    case "like$end":
                        sb.AppendFormat(" and ({0} {1} {2})", f.Name, "like", f.Param);
                        break;
                    case "between":
                        sb.AppendFormat(" and ({0} between {1}_FROM and {2}_TO)", f.Name, f.Param, f.Param);
                        break;
                    default:
                        sb.AppendFormat(" and ({0} {1} {2})", f.Name, f.Condition, f.Param);
                        break;
                }
            }

            return sb.ToString();
        }
        public void SetParameters(OracleCommand oracleCommand)
        {
            if (filterFieldsList.Count == 0)
                return;
            
            oracleCommand.Parameters.Clear();
            foreach (FilterField f in filterFieldsList)
            {
                switch (f.Condition)
                { 
                    case "between":
                        BetweenValue betweenValue = new BetweenValue(f.FieldValue);
                        WFOracle.AddInParameter("p" + f.Name + "_FROM", GetOracleType(f), betweenValue.First, oracleCommand, false);
                        WFOracle.AddInParameter("p" + f.Name + "_TO", GetOracleType(f), betweenValue.Last, oracleCommand, false);
                        break;
                    case "like":
                        WFOracle.AddInParameter("p" + f.Name, GetOracleType(f), "%" + f.FieldValue + "%", oracleCommand, false);
                        break;
                    case "like$start":
                        WFOracle.AddInParameter("p" + f.Name, GetOracleType(f), f.FieldValue + "%", oracleCommand, false);
                        break;
                    case "like$end":
                        WFOracle.AddInParameter("p" + f.Name, GetOracleType(f), "%" + f.FieldValue, oracleCommand, false);
                        break;
                    default:
                        WFOracle.AddInParameter("p" + f.Name, GetOracleType(f), f.FieldValue, oracleCommand, false);
                        break;
                }
            }
        }

        // Private method
        private static OracleType GetOracleType(FilterField filterField)
        { 
            switch(filterField.Type)
            {
                case "System.Int32":
                    return OracleType.Number;
                case "System.DateTime":
                    return OracleType.DateTime;
                case "System.String":
                default:
                    return OracleType.NVarChar;
            }
        }

        // Fields
        private List<FilterField> filterFieldsList = new List<FilterField>();

        // Private type
        private struct FilterField
        {
            public string Name { get; set; }
            public string Condition { get; set; }
            public string Type { get; set; }
            public string Param { get; set; }
            public string FieldValue 
            {
                get { return fieldValue; }
                set
                {
                    if (String.IsNullOrEmpty(value) && String.IsNullOrEmpty(Condition))
                    {
                        fieldValue = "";
                        return;
                    }
                    switch (Type)
                    {
                        case "System.Int32":
                            if (String.IsNullOrEmpty(value))
                                fieldValue = "0";
                            else
                                fieldValue = value;
                            break;
                        case "System.DateTime":
                            if (String.IsNullOrEmpty(value))
                                fieldValue = DateTime.Today.ToString();
                            else
                                fieldValue = value;
                            break;
                        case "System.String":
                            if (String.IsNullOrEmpty(value))
                                fieldValue = "";
                            else
                                fieldValue = value;
                            break;
                    }
                }
            }

            // Private
            private string fieldValue;
        }
        private class BetweenValue
        {
            //Constructors
            public BetweenValue(string betweenValue)
            {
                if (betweenValue == null)
                    this.betweenValue = "";
                else
                    this.betweenValue = betweenValue;

                this.testValue = this.betweenValue.Split('-');
            }

            // Properties
            public string First 
            {
                get
                {
                    if (IsValid(testValue))
                        return testValue[0];
                    else
                        return betweenValue;
                }
            }
            public string Last 
            {
                get
                {
                    if (IsValid(testValue))
                        return testValue[1];
                    else
                        return betweenValue;
                }
            
            }

            // Private methods
            private static bool IsValid(string[] testValue)
            {
                return (testValue.Count() == 2 && !String.IsNullOrEmpty(testValue[0]) && !String.IsNullOrEmpty(testValue[1]));
            }
           
            // Fields
            private string betweenValue;
            private string[] testValue;
        }

    }
}
