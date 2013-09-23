using System;
using System.Data;
using System.Data.OracleClient;
using WFExceptions;
using System.Collections.Generic;
using GeneLibrary.Items;
using GeneLibrary.Common;
using GeneLibrary.Properties;
using System.Text;
using System.Globalization;
using System.Collections.ObjectModel;
using GeneLibrary.Items.Find;


namespace WFDatabase
{
    /// <summary>
    /// Интерфейс работы с коннекцией к базе данных
    /// </summary>
    abstract public class WFDatabaseBase
    {
        /// <summary>
        /// Функции для работы с коннекцией
        /// </summary>
        /// <param name="aDataSource">Имя источника данных</param>
        /// <param name="aLogin">Имя пользователя</param>
        /// <param name="aPassword">Пароль</param>
        abstract public void Connect(string dataSource, string connectedUser, string password);
        /// <summary>
        /// Закрывает соединение с базой данных.
        /// </summary>
        abstract public void Close();
        /// <summary>
        /// Проверяет, является ли текущее соединение активным
        /// </summary>
        /// <returns>Если соединение активно, возвращается true иначе false</returns>
        abstract public bool IsActive();
        /// <summary>
        /// Открывает для соединения новую транзакцию
        /// </summary>
        abstract public void StartTransaction();
        /// <summary>
        /// Проверяет, находится ли соединение в активной транзакции
        /// </summary>
        /// <returns>Если существует активная транзакция возвращается значение true, иначе false</returns>
        abstract public bool InTransaction();
        /// <summary>
        /// Подтверждение изменений в базе, завершение выполнения транзакции, уничтожение объекта транзакции
        /// </summary>
        abstract public void Commit();
        /// <summary>
        /// Отмена изменений в базе, завершение выполнения транзакции, уничтожение объекта транзакции
        /// </summary>
        abstract public void Rollback();
    }

    /// <summary>
    /// Класс предоставляющий доступ к базе данных Jracle. Синглетон. 
    /// Перед использованием необходимо инициализировать соедининене с базой: WfOracle.Db.Connect(Data Source, User ID, Password);
    /// </summary>
    public sealed class WFOracle : WFDatabaseBase, IDisposable
    {
        public static WFOracle DB
        {
            get
            {
                if (_db == null)
                {
                    lock (_syncRoot)
                    {
                        if (_db == null)
                            _db = new WFOracle();
                    }
                }
                return _db;
            }
        }
        public override void Connect(string dataSource, string connectedUser, string password)
        {
            OracleConnectionStringBuilder builder = new OracleConnectionStringBuilder();
            builder.DataSource = dataSource;
            builder.UserID = connectedUser;
            builder.Password = password;
            if (this.IsActive())
                this.Close();
            else
            {
                _connect = new OracleConnection(builder.ConnectionString);
                _connect.Open();
            }
        }
        public override void Close()
        {
            if (_connect != null)
            _connect.Close();
        }
        public override bool IsActive()
        {
            return (_connect != null)&&(_connect.State != System.Data.ConnectionState.Closed);
        }
        public override void StartTransaction()
        {
            if (_connect == null)
                throw new WFException(ErrType.Assert, "WfOracle.Db._connection is null");

            if (_trans != null)
                throw new WFException(ErrType.Assert, ErrMsg.EmConnectInTransaction);
            _trans = _connect.BeginTransaction();
        }
        public override bool InTransaction()
        {
            if (_trans == null)
                throw new WFException(ErrType.Assert, "WfOracle.Db._trans is null");

            return _trans != null;
        }
        public override void Commit()
        {
            if (_trans == null)
                throw new WFException(ErrType.Assert, "WfOracle.Db._trans is null");
            
            _trans.Commit();
            _trans.Dispose();
            _trans = null;
        }
        public override void Rollback()
        {
            if (_trans == null)
                throw new WFException(ErrType.Assert, "WfOracle.Db._trans is null");

            _trans.Rollback();
            _trans.Dispose();
            _trans = null;
        }
        public OracleConnection OracleConnection
        {
            get { return _connect; }
        }
        public OracleTransaction OracleTransaction
        {
            get { return _trans; }
        }
        public static void AddInParameter(string name, OracleType oracleType, object value, OracleCommand command, bool IsNull)
        {
            OracleParameter prm = new OracleParameter(name, oracleType);
            prm.IsNullable = IsNull;
            if (!IsNull && value == null)
                throw new WFException(ErrType.Assert, ErrMsg.EmIsNullParam);
            if (IsNull && value == null)
                prm.Value = DBNull.Value;
            else
                prm.Value = value;
            command.Parameters.Add(prm);
        }
        public static OracleParameter AddOutParameter(string name, OracleType oracleType, OracleCommand command)
        {
            OracleParameter prm = new OracleParameter(name, oracleType);
            prm.Direction = ParameterDirection.Output;
            command.Parameters.Add(prm);
            return prm;
        }

        // Private
        private static volatile WFOracle _db;
        private static object _syncRoot = new Object();
        private OracleTransaction _trans;
        private OracleConnection _connect;

        // Constructors
        private WFOracle(){ }

        //Dispose
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connect.Dispose();
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Простейшие методы обращения к базе данных
    /// </summary>
    public sealed class OracleWork
    {
        public static void OracleSqlSimple(string sql, DataTable dataTable, string id)
        {
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            dataTable.Clear();
            da.Fill(dataTable);
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[id] };
        }
        public static void OracleSqlById(string sql, DataTable dataTable, string id, int value)
        {
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter(id, OracleType.Number, value, cmd, false);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            dataTable.Clear();
            da.Fill(dataTable);
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[id] };
        }
        public static void OracleDeleteSimple(string sql, int[] ids, string id)
        {
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (int i in ids)
            {
                cmd.Parameters.Clear();
                WFOracle.AddInParameter(id, OracleType.Number, i, cmd, false);
                cmd.ExecuteNonQuery();
            }
        }
        // Constructors
        private OracleWork() { }
    }

    /// <summary>
    /// 
    /// Тип объекта зависит от переменных в файлах настройки
    /// </summary>
    public sealed class GateFactory
    {
        static public LogOnBase LogOnBase()
        {
            if (DbValue == DBVal.Oracle)
                return LogOnOracle.LogOntoSystem();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        static public TreeBuilderBase TreeBuilderBase()
        {
            if (DbValue == DBVal.Oracle)
                return new TreeBuilderOra();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        
        }
        static public FilterItem FilterItem(int id)
        {
            if (DbValue == DBVal.Oracle)
                return new FilterItemOracle(id);
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        static public FilterVocabularyGate FilterVocabularyGate()
        {
            if (DbValue == DBVal.Oracle)
                return new FilterVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        static public GLDefault GLDefault()
        {
            if (DbValue == DBVal.Oracle)
                return new GeneLibraryDefaultOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }

        /// <summary>
        /// Создание шлюза для списка локусов с их аллелями
        /// </summary>
        /// <returns>Шлюз списка локусов с их аллелями</returns>
        static public LocuseVocabularyGate LocuseVocabularyGate()
        {
            if (DbValue == DBVal.Oracle)
                return new LocuseVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза для списка подразделений экспертов
        /// </summary>
        /// <returns>Шлюз списка подразделений экспертов</returns>
        static public DivisionVocabularyGate DivisionDictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new DivisionVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка кодов МВД
        /// </summary>
        /// <returns>Шлюз списка кодов МВД</returns>
        static public MvdVocabularyGate MvdDictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new MvdVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка райлинорганов
        /// </summary>
        /// <returns>Шлюз списка райлинорганов</returns>
        static public LinVocabularyGate LinDictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new LinVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создает шлюз списка должностей
        /// </summary>
        /// <returns>Шлюз списка должностей</returns>
        static public PostVocabularyGate PostDictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new PostVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка экспертов
        /// </summary>
        /// <returns>Шлюз списка экспертов</returns>
        static public ExpertVocabularyGate ExpertDictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new ExpertVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза методов расчета
        /// </summary>
        /// <returns>Шлюз списка методов расчета</returns>
        static public MethodVocabularyGate MethodDictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new MethodVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка методов
        /// </summary>
        /// <returns>Шлюз списка методов</returns>
        static public UKVocabularyGate UKDictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new UKVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка ролей
        /// </summary>
        /// <returns>Шлюз списка ролей</returns>
        static public RoleVocabularyGate RoleDictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new RoleVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка карточек ИКЛ
        /// </summary>
        /// <returns>Шлюз списка карточек ИКЛ</returns>
        static public IklVocabularyGate IklVocabularyGate()
        {
            if (DbValue == DBVal.Oracle)
                return new IklVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка организаций
        /// </summary>
        /// <returns>Шлюз списка организаций</returns>
        static public OrganizationVocabularyGate OrganizationDictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new OrganizationVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка карточек ИК-2
        /// </summary>
        /// <returns>Шлюз списка списка карточек ИК-2</returns>
        static public IK2VocabularyGate IK2DictionaryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new IK2VocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза профиля 
        /// </summary>
        /// <returns>Шлюз профиля </returns>
        static public ProfileGate ProfileGate()
        {
            if (DbValue == DBVal.Oracle)
                return new ProfileGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза поиска карточек
        /// </summary>
        /// <returns>Шлюз поиска</returns>
        static public FindCoincidenceGate FindCoincidenceGate() 
        {
            if (DbValue == DBVal.Oracle)
                return new FindCoincidenceGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка видов объекта
        /// </summary>
        /// <returns>Шлюз списка видов объекта</returns>
        static public SortObjectVocabularyGate SortObjectVocabularyGate()
        {
            if (DbValue == DBVal.Oracle)
                return new SortObjectVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка категорий объекта
        /// </summary>
        /// <returns>Шлюз списка категорий объекта</returns>
        static public ClassObjectVocabularyGate ClassObjectVocabularyGate()
        {
            if (DbValue == DBVal.Oracle)
                return new ClassObjectVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка категорий карточки ИКЛ
        /// </summary>
        /// <returns>Шлюз списка категорий карточки ИКЛ</returns>
        static public ClassIklVocabularyGate ClassIklVocabularyGate()
        {
            if (DbValue == DBVal.Oracle)
                return new ClassIklVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза списка архивных карточек
        /// </summary>
        /// <returns>Шлюз списка архивных карточек</returns>
        static public ArchiveVocabularyGate ArchiveVocabularyGate()
        {
            if (DbValue == DBVal.Oracle)
                return new ArchiveVocabularyGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза выверки справочников
        /// </summary>
        /// <returns>Шлюз выверки справочников</returns>
        static public CompareVocabulare CompareVocabulare()
        {
            if (DbValue == DBVal.Oracle)
                return new CompareVocabulareOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }

        /// <summary>
        /// Создание шлюза элемента списка локусов
        /// </summary>
        /// <returns>Элемент списка локусов</returns>
        static public LocuseItemGate LocuseItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new LocuseItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза элемента списка подразделений
        /// </summary>
        /// <returns>Элемент списка подраздлений</returns>
        static public DivisionItemGate DivisionItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new DivisionItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создает шлюз для элемента списка кодов МВД
        /// </summary>
        /// <returns>Объект шлюза элемента списка кодов МВД</returns>
        static public MvdItemGate MvdItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new MvdItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создает шлюз для элемента списка кодов райлинорганов
        /// </summary>
        /// <returns>Шлюз элемента райлиноргана</returns>
        static public LinItemGate LinItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new LinItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создание шлюза для элемента списка должностей
        /// </summary>
        /// <returns>Шлюз элемента списка должностей</returns>
        static public PostItemGate PostItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new PostItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создается шлюз элемента справочника экспертов
        /// </summary>
        /// <returns>Шлюз элемента справочника экспертов</returns>
        static public ExpertItemGate ExpertItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new ExpertItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создается шлюз элемента справочника методов расчета
        /// </summary>
        /// <returns>Шлюз элемента справочника методов расчета</returns>
        static public MethodItemGate MethodItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new MethodItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создается шлюз элемента справочника статей УК
        /// </summary>
        /// <returns>Шлюз элемента справочника статей УК</returns>
        static public UKItemGate UKItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new UKItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создается шлюз элемента справочника ролей
        /// </summary>
        /// <returns>Шлюз элемента справочника ролей</returns>
        static public RoleItemGate RoleItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new RoleItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создается шлюз элемента справочника икл
        /// </summary>
        /// <returns>Шлюз элемента справочника икл</returns>
        static public IklItemGate IklItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new IklItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создается шлюз элемента справочника организаций
        /// </summary>
        /// <returns>Шлюз элемента справочника организаций</returns>
        static public OrganizationItemGate OrganizationItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new OrganizationItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создается шлюз элемента справочника карточек ИК-2
        /// </summary>
        /// <returns>Шлюз элемента справочника карточек ИК-2</returns>
        static public IK2ItemGate IK2ItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new IK2ItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создает шлюз для элемента списка видов объекта
        /// </summary>
        /// <returns>Объект шлюза элемента списка видов объекта</returns>
        static public SortObjectItemGate SortObjectItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new SortObjectItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создает шлюз для элемента списка категорий объекта
        /// </summary>
        /// <returns>Объект шлюза элемента списка категорий объекта</returns>
        static public ClassObjectItemGate ClassObjectItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new ClassObjectItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создает шлюз для элемента списка категорий карточки ИКЛ
        /// </summary>
        /// <returns>Объект шлюза элемента списка категорий карточки ИКЛ</returns>
        static public ClassIklItemGate ClassIklItemGate()
        {
            if (DbValue == DBVal.Oracle)
                return new ClassIklItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создает шлюз для родительского класса карточек ИКЛ и ИК-2
        /// </summary>
        /// <returns>Шлюз родительского класса карточек ИКЛ и ИК-2</returns>
        static public CardItemGateOracle CardItemGateOracle()
        {
            if (DbValue == DBVal.Oracle)
                return new CardItemGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }
        /// <summary>
        /// Создает шлюз для истории карточек
        /// </summary>
        /// <returns>Шлюз истории карточек</returns>
        static public HistoryGate HistoryGate()
        {
            if (DbValue == DBVal.Oracle)
                return new HistoryGateOracle();
            else
                throw new WFException(ErrType.Critical, Resources.UnknownDB);
        }

        /// <summary>
        /// Метод возвращает объект базы данных
        /// </summary>
        /// <returns>Объект коннекции к базе</returns>
        static public WFDatabase.WFDatabaseBase DB()
        {
            if (DbValue == DBVal.Oracle)
                return WFDatabase.WFOracle.DB;
            else
                throw new WFException(ErrType.Critical, GeneLibrary.Properties.Resources.UnknownDB);

        }

        /// <summary>
        /// Метод возвращает значение константы из перечисления хранящего типы баз данных
        /// </summary>
        /// <returns>Возвращает тип используемой базы данных </returns>
        static public DBVal DbValue { get { return DBVal.Oracle; } }

        // Constructors
        private GateFactory() { }
    }

    /// <summary>
    /// Перечисление содержит набор возможных источников данных
    /// </summary>
    public enum DBVal { None, Oracle };

    /// <summary>
    /// Сообщения об ошибках
    /// </summary>
    internal struct ErrMsg
    {
        public const string EmNotValidDir= " Not valid direction value of param";
        public const string EmNotValidType = " Not valid type value of param";
        public const string EmConnectInTransaction = " Connect in transaction";
        public const string EmIsNullParam = " Param value is null";
    }
}
