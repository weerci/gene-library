using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Security;
using System.Globalization;
using System.Drawing;
using WFExcel;
using GeneLibrary.Items;
using WFExceptions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data.OracleClient;
using WFDatabase;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace GeneLibrary.Common
{
    public class CardForm : Form
    {
        // Properties
        public string FormId { get { return this.formId; } }

        // Methods
        protected void OnDataLoad(int cardId)
        {
            if (onDataLoad != null) 
                onDataLoad(cardId);
        }
        protected void OnCloseForm(ComboBoxItem comboBoxItem)
        {
            if (onCloseForm != null)
                onCloseForm(comboBoxItem);
        }
        protected void OnUpdateFormId(ComboBoxItem comboBoxItem)
        {
            if (onUpdateFormId != null)
                onUpdateFormId(comboBoxItem);
        }

        // Fields
        private string formId = Guid.NewGuid().ToString();

        // Events
        internal static event UpdateId onDataLoad;
        internal event FormInTree onCloseForm;
        internal event FormInTree onUpdateFormId;
    }

    internal delegate void UpdateId(int id);
    internal delegate void VaidateInput(bool IsValid);
    internal delegate void UpdateIdsText(List<int> ids, string text);
    internal delegate void CheckTextInput(Type type, int currentCondition, Control control, out bool result);
    internal delegate void ActiveIklCard(GeneLibrary.Items.Vocabulary vocabulary, int id);
    internal delegate void ActiveIk2Card(GeneLibrary.Items.Vocabulary vocabulary, int id);
    internal delegate void ActiveStudyForm(int id);
    internal delegate void FormInTree(ComboBoxItem comboBoxItem);
    internal delegate void UpdateEnabledControl();

    /// <summary>
    /// Класс для использования в элементах ListBox Combobox и пр.
    /// </summary>
    public class ComboBoxItem
    {
        public ComboBoxItem(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public ComboBoxItem(int id, string name, string shortName) : this(id, name)
        {
            this.Short = shortName;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Short { get; set; }
    }

    /// <summary>
    /// Класс помогающий сохранять и извлекать данные сериализации 
    /// </summary>
    public sealed class WFSerialize
    {
        static private Stream NewFileStream(string fileName)
        {
            return File.Create(fileName);
        }
        static private Stream GetFileStream(string fileName)
        {
            if (File.Exists(fileName))
                return File.OpenRead(fileName);
            else
                return null;
        }
        static public void Serialize(string path, object value)
        {
            if (value == null)
                return;
            BinaryFormatter f = new BinaryFormatter();
            using (Stream nf = NewFileStream(path))
            {
                f.Serialize(nf, value);
            }
        }
        static public object Deserialize(string path)
        {
            BinaryFormatter f = new BinaryFormatter();
            using (Stream strm = GetFileStream(path))
            {
                if ((strm != null) && (strm.Length != 0))
                {
                    return f.Deserialize(strm);
                }
                else
                    return null;
            }
        }

        private WFSerialize() { }
    }

    /// <summary>
    /// Сохраняемый динамический список. 
    /// Предназначен для хранения списка элементов в файловой системе. Программы могут читать и изменять список через 
    /// механизм сериализации.
    /// Каждому элементу в списке соответствует некоторое число, что-то вроде "индекса цитируемости", его значение
    /// зависит от того, сколько раз элемент сохранялся в списке. При добавлении элемента в список, 
    /// проверяется его наличие в этом списке, и если элемент найден, то изменяется связанный с ним 
    /// "индекс цитируемости", если элемент не найден, то создается новый элемент, и с ним
    /// связывается "индекс цитируемости" минимального значения.                     
    /// При создании списка, можно указать его глубину - количество сохраняемых в списке элементов. 
    /// До тех пор пока количество размещенных в списке элементов меньше его глубины, новые элементы продолжают добавляються. 
    /// Когда же достигнута максимальная глубина, то новый элемент - добавляется, а один из старых - удаляется. 
    /// Удаляться будет тот, чей "индекс цитируемости" минимален.
    /// </summary>
    [Serializable]
    public class SaveList
    {
        // Constructors
        public SaveList() { }
        /// <summary>
        /// Конструктор задающий колличество поддерживаемых списком значений
        /// </summary>
        /// <param name="startItemCount">Кличество элементов поддерживаемых списком. По умолчанию 10</param>
        public SaveList(int startItemCount) : this()
        {
            this.startItemCount = startItemCount;
        }
        /// <summary>
        /// Конструтор задающий количество элементов поддерживаемых списком и максимальный индекс, при достижении
        /// которого, будет произведена реорганизация индекса.
        /// </summary>
        /// <param name="startItemCount">Число элементов списка</param>
        /// <param name="reorganizeSize">Максимальный размер индекса, по достижении которого будет произведена 
        /// реогранизация индекса</param>
        public SaveList(int startItemCount, int reorganizeSize) : this(startItemCount)
        {
            this.reorganizeSize = reorganizeSize;
        }

        // Interfaces 
        public void Add(SaveItem item)
        {
            findItem = item;
            SaveItem currItem = sl.Find(FindItem);
            if (currItem == null)
            {
                if (sl.Count == startItemCount)
                {
                    SaveItem min = sl.Min();
                    sl.Remove(min);
                }
                item.Count = 1;
                sl.Add(item);
            }
            else
            {
                currItem.Count++;

                // Реорганизация списка
                if (currItem.Count == reorganizeSize)
                    ReorganizeList();
            }
        }
        
        // Properties
        public ReadOnlyCollection<SaveItem> ListItems
        {
            get
            {
                return new ReadOnlyCollection<SaveItem>(sl);
            }
        }
        public int StartItemCount
        {
            get { return startItemCount; }
            set { startItemCount = value; }
        }
        public int ReorganizeSize
        {
            get { return reorganizeSize; }
            set { reorganizeSize = value; }
        }

        // Fields
        private List<SaveItem> sl = new List<SaveItem>();
        private SaveItem findItem;
        private int startItemCount = 10;
        private int reorganizeSize = 10000;

        // Private methods
        private bool FindItem(SaveItem si)
        {
            return si.Id == findItem.Id;
        }
        private void ReorganizeList()
        {
//            var orderedList = sl.Select(o => o).OrderBy(o => o.Count).Select(o => o.Count++);
            var orderedList = from f in sl
                              orderby (f.Count)
                              select f;
            int i = 0;
            foreach (SaveItem saveItem in orderedList)
                saveItem.Count = i++;
        }

    }
    [Serializable]
    public class SaveItem : IComparable
    {
        //Constructors
        public SaveItem() {}
        public SaveItem(int id, string name) : this()
        {
            this.Id = id;
            this.Name = name;
        }
        public SaveItem(int id, string name, string altName)
            : this(id, name)
        {
            this.AltName = altName;        
        }

        // Methods
        public int CompareTo(object obj)
        {
            SaveItem saveItem = obj as SaveItem;
            if (saveItem == null)
                throw new InvalidCastException();
            else
            {
                if (saveItem.Count > this.Count)
                    return -1;
                else if (saveItem.Count == this.Count)
                    return 0;
                else
                    return 1;
            }
        }
        public override bool Equals(object obj)
        {
            if (!(obj is SaveItem))
                return false;
            return (this.CompareTo(obj) == 0);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(SaveItem saveItem1, SaveItem saveItem2)
        {   
            if (object.ReferenceEquals(saveItem1, saveItem2))
                return true;
            if (object.ReferenceEquals(saveItem1, null))
                return false;
            return saveItem1.Equals(saveItem2);
        }
        public static bool operator !=(SaveItem saveItem1, SaveItem saveItem2)
        {
            return !(saveItem1 == saveItem2);
        }
        public static bool operator <(SaveItem saveItem1, SaveItem saveItem2)
        {
            return (saveItem1.CompareTo(saveItem2) < 0);
        }
        public static bool operator >(SaveItem saveItem1, SaveItem saveItem2)
        {
            return (saveItem1.CompareTo(saveItem2) > 0);
        }

        // Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string AltName { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Вывод сообщения с поддержкой языков, где чтение происходит справо-налево
    /// </summary>
    public static class AwareMessageBox
    {
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            if (IsRightToLeft(owner))
            {
                options |= MessageBoxOptions.RtlReading |
                MessageBoxOptions.RightAlign;
            }
            return MessageBox.Show(owner, text, caption,
            buttons, icon, defaultButton, options);
        }
        private static bool IsRightToLeft(IWin32Window owner)
        {
            Control control = owner as Control;

            if (control != null)
            {
                return control.RightToLeft == RightToLeft.Yes;
            }

            // If no parent control is available, ask the CurrentUICulture
            // if we are running under right-to-left.
            return CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
        }
    }

    public sealed class Tools
    {
        // Constructor
        private Tools() { }

        /// <summary>
        /// Переданным контролам назначает обработчик события, для стандартных событий
        /// </summary>
        /// <param name="enumerable">Списoк контролов</param>
        /// <param name="eventHandler">Обработчик события</param>
        public static void SetControlChangedHandler(IEnumerable<Control> enumerable, EventHandler eventHandler)
        {
            if (eventHandler != null)
                foreach (var e in enumerable)
                    switch (e.GetType().Name)
                    {
                        case "TextBox":
                            ((TextBox)e).TextChanged += new EventHandler(eventHandler);
                            break;
                        case "ComboBox":
                            ((ComboBox)e).TextChanged += new EventHandler(eventHandler);
                            break;
                        case "DateTimePicker":
                            ((DateTimePicker)e).TextChanged += new EventHandler(eventHandler);
                            break;
                        case "TextBoxId":
                            ((TextBoxId)e).TextChanged += new EventHandler(eventHandler);
                            break;
                    }
        }
        /// <summary>
        /// Отображает для контрола всплывающую подсказку. Подсказка указывает на середину контрола.
        /// </summary>
        /// <param name="control">Контрол, для которого выводится подсказка</param>
        /// <param name="title">Название подсказки</param>
        /// <param name="text">Текст подсказки</param>
        /// <param name="toolTypeIcon">Иконка подсказки</param>
        /// <param name="duration">Длительность отображения в миллисекундах, если задано значение 0 - подсказка отображается до тех пор, пока пользователь на нее не кликнет</param>
        public static void ShowTip(Control control, string title, string text, ToolTipIcon toolTypeIcon, int duration)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.ToolTipIcon = toolTypeIcon;
            toolTip.BackColor = SystemColors.Window;
            toolTip.ToolTipTitle = title;
            //toolTip.IsBalloon = true;
            toolTip.Active = true;
            toolTip.Show(text, control, control.Width / 2, control.Height / 2, 800);
        }
        /// <summary>
        /// Удаляет все элементы списка из комбобокса, добавляет значение "Нет данных" (id = -1), и позиционируется на него
        /// </summary>
        public static void ClearComboBox(ComboBox comboBox)
        { 
            if (comboBox != null)
            {
                comboBox.Items.Clear();
                comboBox.Items.Add(new ComboBoxItem(-1, resDataNames.emptyComboBox));
                if (comboBox.Items.Count > 0)
                    comboBox.SelectedIndex = 0;
            }
            
        }
        /// <summary>
        /// Формирование названия статьи. 
        /// </summary>
        /// <param name="ukItem">Статья в формате "Статья;Часть;Пункт".</param>
        /// <returns>Статья в формате "ст. XXX ч. XX п. X</returns>
        public static string UkName(string ukItem, string ukArtcl)
        {
            if (ukArtcl == "0") // не статья, а текст
                return ukItem.Trim(); 
            
            string[] ukArray = ukItem.Split(';');
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < ukArray.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        result.Append(resDataNames.shortState + ukArray[i] + " ");
                        break;
                    case 1:
                        result.Append(resDataNames.shortPart + ukArray[i] + " ");
                        break;
                    case 2:
                        result.Append(resDataNames.shortPoint + ukArray[i] + " ");
                        break;
                    default:
                        break;
                }
            }
            return result.ToString().Trim();
        }
        /// <summary>
        /// Преобразует массив названий статей в список разделенный запятыями
        /// </summary>
        /// <param name="ukName">Массив названий</param>
        /// <returns>Строка содержащая элементы списка, разделенные запятыями</returns>
        public static string UkList(string[] ukName)
        {
            if (ukName.Length > 0)
            {
                char[] charsToTrim = { ',', ' ' };
                return ukName.Aggregate((acc, elem) => acc + ", " + elem).TrimEnd(charsToTrim);
            }
            else
                return "";
        }
        /// <summary>
        /// Возвращается основная форма приложения
        /// </summary>
        /// <returns>Основная форма приложения</returns>
        public static Main MainForm()
        {
            return Application.OpenForms[0] as Main;
        }
        /// <summary>
        /// Определяет, открыта ли форма с выбранным именем
        /// </summary>
        /// <param name="formName">Имя формы</param>
        /// <returns>Возаращается true, если форма открыта, false - если не открыта</returns>
        public static bool IsMdiFormOpen(string formName)
        {
            return (from Form applicationForm in Application.OpenForms
                    where applicationForm.Name == formName
                    select applicationForm).Count() > 0;
        }
        /// <summary>
        /// Для локуса amelogenin алели имеют значение X или Y. Для всех остальных - числовые значения.
        /// Функция реализует возможность сортировки для алелей
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double AlleleConvert(string value)
        {
            if (value.ToUpper().Trim() == "X")
                return 0;
            else if (value.ToUpper().Trim() == "Y")
                return 1;
            else
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Заполнение коллекции полей для получения отчетности
        /// </summary>
        /// <param name="reportField">Коллекция для заполнения</param>
        /// <param name="locuses">Выборка локусов из профиля</param>
        public static void FillReportFields(Collection<ReportField> reportField, Locus[] locuses)
        {
            string[] alleles;
            // Заполнение профиля
            foreach (Locus locus in locuses)
            {
                alleles = (from Allele allele in locus.Allele where allele.Checked orderby Tools.AlleleConvert(allele.Name) select allele.Name).ToArray<string>();
                switch (locus.Name)
                {
                    case "Amelogenin":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#AMELOGENIN_" + i.ToString(), alleles[i]));
                        break;
                    case "CSF1PO":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#CSF1PO_" + i.ToString(), alleles[i]));
                        break;
                    case "D13S317":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D13S317_" + i.ToString(), alleles[i]));
                        break;
                    case "D16S539":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D16S539_" + i.ToString(), alleles[i]));
                        break;
                    case "D18S51":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D18S51_" + i.ToString(), alleles[i]));
                        break;
                    case "D19S433":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D19S433_" + i.ToString(), alleles[i]));
                        break;
                    case "D21S11":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D21S11_" + i.ToString(), alleles[i]));
                        break;
                    case "D2S1338":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D2S1338_" + i.ToString(), alleles[i]));
                        break;
                    case "D3S1358":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D3S1358_" + i.ToString(), alleles[i]));
                        break;
                    case "D5S818":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D5S818_" + i.ToString(), alleles[i]));
                        break;
                    case "D7S820":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D7S820_" + i.ToString(), alleles[i]));
                        break;
                    case "D8S1179":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#D8S1179_" + i.ToString(), alleles[i]));
                        break;
                    case "F13A01":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#F13A01_" + i.ToString(), alleles[i]));
                        break;
                    case "F13B":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#F13B_" + i.ToString(), alleles[i]));
                        break;
                    case "FESFPS":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#FESFPS_" + i.ToString(), alleles[i]));
                        break;
                    case "FGA":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#FGA_" + i.ToString(), alleles[i]));
                        break;
                    case "HPRTB":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#HPRTB_" + i.ToString(), alleles[i]));
                        break;
                    case "LPL":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#LPL_" + i.ToString(), alleles[i]));
                        break;
                    case "Penta D":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#PentaD_" + i.ToString(), alleles[i]));
                        break;
                    case "Penta E":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#PentaE_" + i.ToString(), alleles[i]));
                        break;
                    case "SE33":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#SE33_" + i.ToString(), alleles[i]));
                        break;
                    case "TH01":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#TH01_" + i.ToString(), alleles[i]));
                        break;
                    case "TPOX":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#TPOX_" + i.ToString(), alleles[i]));
                        break;
                    case "vWA":
                        for (int i = 0; i < alleles.Count(); i++)
                            reportField.Add(new ReportField("#VWA_" + i.ToString(), alleles[i]));
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Вывод сообщения с учетом возможности использования языков с написанием справо-налево
        /// </summary>
        /// <param name="a_text">Текст сообщения</param>
        /// <returns>Выбранная пользователем кнопка закрытия диалоговой формы</returns>
        public static DialogResult ShowMessage(string a_text)
        {
            return AwareMessageBox.Show(null, a_text, Application.ProductName, MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }
        /// <summary>
        /// Очистка значений списка, без удаления элементов 
        /// </summary>
        /// <param name="listView">Очищаемый список</param>
        public static void ClearListViewText(ListView listView)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                listView.Items[i].Text = "";
                for (int j = 0; j < listView.Items[i].SubItems.Count; j++)
                {
                    listView.Items[i].SubItems[j].Text = "";
                    listView.Items[i].SubItems[j].BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }
            
        }
        /// <summary>
        /// Возвращает имя карточки по имени в перечислении
        /// </summary>
        /// <param name="cardKind">Имя в пееречислении</param>
        /// <returns>Имя карточки</returns>
        public static string CardKindName(CardKind cardKind)
        {
            switch (cardKind)
            {
                case CardKind.ikl:
                    return resDataNames.ikl;
                case CardKind.ik2:
                    return resDataNames.ik2;
                case CardKind.archive:
                    return resDataNames.archive;
                case CardKind.ident:
                    return resDataNames.ident;
                default:
                    return "";
            }
        }
        /// <summary>
        /// Сопоставление номера с именем карточки в перечислении
        /// </summary>
        /// <param name="cardKind">0 - ИКЛ, 1 - ИК2</param>
        /// <returns>Имя карточки в перечислении</returns>
        public static CardKind CardKindType(int cardKind)
        {
            switch (cardKind)
            {
                case 0:
                    return CardKind.ikl;
                case 1:
                    return CardKind.ik2;
                default:
                    throw new WFException(ErrType.Critical, ErrorsMsg.NotValidCardKind);
            }
        }
        /// <summary>
        /// Преобразует входящий объект в строку, если объект null - возвращается пустая строка
        /// </summary>
        /// <param name="obj">Объект преобразования</param>
        /// <returns>Строка преобразования</returns>
        public static string AsNullString(object obj)
        {
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }
        /// <summary>
        /// Выгрузка отчетности в Excel
        /// </summary>
        /// <param name="header">Заголовок отчета</param>
        /// <param name="startCell">Ячейка Excel с которой начинается вывод значений</param>
        /// <param name="dataGridView">Таблица со значениями, которую нужно выгрузить в Excel</param>
        public static void ToExcel(string header, Point startCell, DataGridView dataGridView, ExcelCellOrientation orientation)
        {
            // Отображаются только видимые столбцы
            List<DataGridViewColumn> dgrCollection = new List<DataGridViewColumn>();
            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                if (col.Visible)
                    dgrCollection.Add(col);
            }

            try
            {
                Excel.Application app = Tools.ExcelApplication();
                Excel.Workbook wb = app.Workbooks.Add(Missing.Value);
                Excel.Worksheet sh = (Excel.Worksheet)wb.ActiveSheet;

                string[,] arrExcel = new string[dataGridView.RowCount, dgrCollection.Count];
                for (int i = 0; i < dataGridView.RowCount; ++i)
                    for (int j = 0; j < dgrCollection.Count; ++j)
                        if (dataGridView.Rows[i].Cells[j].Value == null)
                            arrExcel[i, j] = "";
                        else
                            arrExcel[i, j] = dataGridView.Rows[i].Cells[j].Value.ToString();

                // Заголовок
                sh.Cells[1, 1] = header;
                sh.Cells.WrapText = false;
                sh.get_Range(sh.Cells[1, 1], sh.Cells[1, 1]).Font.Bold = true;
                sh.get_Range(sh.Cells[1, 1], sh.Cells[1, 1]).Font.Size = 14;

                // Названия столбцов
                for (int j = 0; j < dgrCollection.Count; ++j)
                    sh.Cells[startCell.X - 1, j + 1] = dgrCollection[j].HeaderText;
                sh.get_Range(sh.Cells[startCell.X - 1, 1], sh.Cells[startCell.X - 1, dgrCollection.Count]).Font.Bold = true;
                if (orientation == ExcelCellOrientation.Vertical)
                    sh.get_Range(sh.Cells[startCell.X - 1, 1], sh.Cells[startCell.X - 1, dgrCollection.Count]).Orientation = 90;


                // Данные
                sh.get_Range(sh.Cells[startCell.X, startCell.Y], sh.Cells[startCell.X + dataGridView.RowCount - 1, startCell.Y + dgrCollection.Count - 1]).Value2 = arrExcel;
                sh.get_Range(sh.Cells[startCell.X - 1, startCell.Y], sh.Cells[startCell.X - 1 + dataGridView.RowCount - 1, startCell.Y + dgrCollection.Count - 1]).AutoFilter(
                        1,
                        Missing.Value,
                        Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd,
                        Missing.Value,
                        true);
                sh.get_Range(sh.Cells[startCell.X, startCell.Y], sh.Cells[startCell.X, dgrCollection.Count]).Select();
                app.ActiveWindow.FreezePanes = true;

                app.Visible = true;
                app.UserControl = true;
            }
            catch (Exception e)
            {
                throw new WFExceptions.WFException(WFExceptions.ErrType.Message, e.Message, e);
            }
        }
        /// <summary>
        /// Возвращает экземпляр приложения Excel
        /// </summary>
        /// <returns></returns>
        public static Excel.Application ExcelApplication()
        {
            try
            {
                return new Excel.Application();
            }
            catch 
            {
                throw new WFException(ErrType.Message, ErrorsMsg.NotInstallExcel);
            }
        }
        /// <summary>
        /// Переданное значение проверяется на соответствие заявленному типу
        /// </summary>
        /// <param name="fieldName">Поле содержащее значение, для отображения в отчете</param>
        /// <param name="value">Значение</param>
        /// <param name="typeString">Тип, к которму планируется приведение значения</param>
        /// <returns>Строковое представление значения</returns>
        public static string CheckType(string fieldName, object value, FilterValueType filterValueType)
        {
            switch (filterValueType)
            {
                case FilterValueType.valueInt:
                    try
                    {
                        Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorIntValue, fieldName));
                    }
                    break;
                case FilterValueType.valueDate:
                    try
                    {
                        Convert.ToDateTime(value);
                    }
                    catch
                    {
                        throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorDateValue, fieldName));
                    }
                    break;
                case FilterValueType.valueString:
                case FilterValueType.valueList:
                default:
                    break;
            }
            return value.ToString();
        }
        /// <summary>
        /// Возвращается профиль карточки по переданному пользователем идентификатору
        /// </summary>
        /// <param name="profileId">Идентификатор профиля</param>
        /// <returns>Профиль с указанным пользователем идентификатором</returns>
        public static Profiles GetProfileById(int profileId)
        {
            Profiles profile = new Profiles();
            profile.Load(profileId, GeneLibraryConst.DefaultMethod);
            return profile;
        }
        /// <summary>
        /// Создается строковый объект DataTable по переданному заголовку и списку данных.
        /// Нужно помнить что количество столбцов в параметре header и
        /// количество элементов в массиве string[] параметра tableRows - должно совпадать
        /// </summary>
        /// <param name="headers">Массив строк из которых будет построен заголовок таблиц</param>
        /// <param name="tableRows">Спиосок массива строк, которые будут использованы для заполнения таблицы</param>
        /// <returns>Заполненный строками объект DataTable</returns>
        public static DataTable FillTable(string[] headers, List<string[]> tableRows)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < headers.Count(); i++)
                dataTable.Columns.Add(new DataColumn(headers[i], System.Type.GetType("System.String")));
            foreach (string[] tableCells in tableRows)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < tableCells.Count(); i++)
                    dataRow[i] = tableCells[i];
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
        /// <summary>
        /// Получение идентификатора карточки из текстового поля
        /// </summary>
        /// <param name="cardIdText">Строковое значение номера поля</param>
        /// <param name="fieldName">Поле, имя которого будет упомянуто в исключении, если таковое произойдет</param>
        /// <returns>Идентификатор каротчки</returns>
        public static int GetIntFromText(string text, string fieldName)
        {
            try
            {
                return Convert.ToInt32(text, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new WFException(ErrType.Message, String.Format(ErrorsMsg.NotInteger, fieldName));
            }
        }
        /// <summary>
        /// ListView заполняется данными сравниваемых профилей
        /// </summary>
        /// <param name="profilesOne">Профиль с которым происходит сравнение</param>
        /// <param name="profieleSecond">Сравниваемый профиль</param>
        /// <param name="listViewProfiles">ListView в который будут выведены данные по профилям</param>
        public static void FillListViewProfilesData(Profiles profileOne, Profiles profileSecond, ListView listViewProfiles)
        {
            try
            {
                listViewProfiles.BeginUpdate();
                listViewProfiles.Items.Clear();
                
                if (profileOne == null)
                    return;
                if (profileSecond == null)
                    return;

                #region Fill ListBox
                foreach (string s in (from Locus locus in profileOne.Locus where locus.CheckedAlleleCount > 0 select locus.Name).
                    Union(from Locus locus in profileSecond.Locus where locus.CheckedAlleleCount > 0 select locus.Name).OrderBy(locusName => locusName))
                {
                    Locus cardLocus = profileOne[s];
                    Locus locus = profileSecond[s];
                    ListViewItem listViewItem = listViewProfiles.Items.Add("");
                    ListViewItem.ListViewSubItem subItem;
                    listViewItem.UseItemStyleForSubItems = false;


                    for (int i = 0; i < 9 - cardLocus.CheckedAlleleCount; i++)
                        listViewItem.SubItems.Add("");

                    foreach (Allele allele in (from Allele allele in cardLocus.Allele
                                               where allele.Checked == true
                                               orderby allele.Name
                                               select allele))
                    {
                        subItem = listViewItem.SubItems.Add(allele.Name);
                        if (locus.CheckedAllele(allele.Id))
                            subItem.BackColor = Color.FromArgb(165, 218, 180);
                    }


                    subItem = listViewItem.SubItems.Add(s);
                    subItem.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);

                    foreach (Allele allele in (from Allele allele in locus.Allele
                                               where allele.Checked == true
                                               orderby allele.Name
                                               select allele))
                    {
                        subItem = listViewItem.SubItems.Add(allele.Name);
                        if (cardLocus.CheckedAllele(allele.Id))
                            subItem.BackColor = Color.FromArgb(165, 218, 180);
                    }
                    for (int i = 18 - locus.CheckedAlleleCount; i < 19; i++)
                        listViewItem.SubItems.Add("");

                }
                #endregion

            }
            finally
            {
                listViewProfiles.EndUpdate();
            }
        }
        /// <summary>
        /// Заполняется listView данными по трем профилям пользователей
        /// </summary>
        /// <param name="profileOne">Превый профиль</param>
        /// <param name="profileSecond">Второй профиль</param>
        /// <param name="profileThrid">Третий профиль</param>
        /// <param name="listView">lisvView в который будут выведены данные</param>
        /// <param name="shiftSecond">Смещение от левого края listView, для заголовка второго профиля</param>
        /// <param name="shiftThrid">Смещение от левого края listView, для заголовка третьего профиля</param>
        public static void FillListViewTreeProfiles(Profiles profileOne, Profiles profileSecond,
            Profiles profileThrid, ListView listView, out int shiftSecond, out int shiftThrid)
        {
            shiftSecond = 0;
            shiftThrid = 0;
            try
            {
                listView.BeginUpdate();
                
                #region Очистка и проверка входных данных
                listView.Clear();

                if (profileOne == null)
                    return;
                if (profileSecond == null)
                    return;
                if (profileThrid == null)
                    return;
                
                #endregion
                
                #region Fill ListBox
               
                int countAlleleOne = profileOne.Locus.Select(n => n.CheckedAlleleCount).Max()+1;
                int countAlleleSecond = profileSecond.Locus.Select(n => n.CheckedAlleleCount).Max()+1;
                int countAlleleThrid = profileThrid.Locus.Select(n => n.CheckedAlleleCount).Max();
                int widthColumn = 35;
                int widthHeader = 95;
                int widthCorrect = 10;
                shiftSecond = (countAlleleOne) * widthColumn + widthHeader - widthCorrect;
                shiftThrid = (countAlleleOne + countAlleleSecond) * widthColumn + widthHeader;
                
                for (int i = 0; i < countAlleleOne+countAlleleSecond+countAlleleThrid+1; i++)
                {
                    if (i == 0)
                        listView.Columns.Add("", widthHeader);
                    else
                        listView.Columns.Add("", widthColumn);
                }

                foreach (string s in (
                    from Locus locus in profileOne.Locus where locus.CheckedAlleleCount > 0 select locus.Name).
                    Union(from Locus locus in profileSecond.Locus where locus.CheckedAlleleCount > 0 select locus.Name).
                    Union(from Locus locus in profileThrid.Locus where locus.CheckedAlleleCount > 0 select locus.Name).
                    OrderBy(locusName => locusName))
                {
                    ListViewItem listViewItem = listView.Items.Add(s);
                    listViewItem.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    listViewItem.UseItemStyleForSubItems = false;

                    listViewItem.SubItems.AddRange(Tools.TampStringArrayEmptyString(
                            profileOne[s].Allele.Where(n => n.Checked).OrderBy(n => Tools.AlleleConvert(n.Name)).Select(n => n.Name).ToArray(),
                            countAlleleOne));
                    listViewItem.SubItems.AddRange(Tools.TampStringArrayEmptyString(
                            profileSecond[s].Allele.Where(n => n.Checked).OrderBy(n => Tools.AlleleConvert(n.Name)).Select(n => n.Name).ToArray(),
                            countAlleleSecond));
                    listViewItem.SubItems.AddRange(Tools.TampStringArrayEmptyString(
                            profileThrid[s].Allele.Where(n => n.Checked).OrderBy(n => Tools.AlleleConvert(n.Name)).Select(n => n.Name).ToArray(),
                            countAlleleThrid));
                    foreach (ListViewItem.ListViewSubItem  subItem in listViewItem.SubItems)
                    {
                        if (profileOne[s].Allele.Where(n=>n.Checked).Select(n=>n.Name).Contains(subItem.Text))  
                            subItem.BackColor = Color.FromArgb(165, 218, 180);
                    }
                }
                #endregion

            }
            finally
            {
                listView.EndUpdate();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="fixedSize"></param>
        /// <returns></returns>
        private static string[] TampStringArrayEmptyString(string[] strings, int fixedSize)
        {
            string[] resultString = new string[fixedSize];
            if (strings.Count() >= fixedSize)
            {
                for (int i = 0; i < fixedSize; i++)
                {
                    resultString[i] = strings[i];
                }
            }
            else
            {
                for (int i = 0; i < strings.Count(); i++)
                {
                    resultString[i] = strings[i];
                }
                for (int i = strings.Count(); i < fixedSize; i++)
                {
                    resultString[i] = "";
                }
            }
            return resultString;
        }
        /// <summary>
        /// Проверяет соответствие номеру карточки в тексте контрола, заданной маске
        /// </summary>
        /// <param name="controls">Указатель на контрол или массив контролов</param>
        /// <returns>Если текст, хотя бы одного контрола не соответствует маске - возвращает false</returns>
        public static bool CorrectCardId(Control[] controls)
        {
            foreach (var control in controls)
                if (Regex.IsMatch(control.Text, @"[^1234567890]") || String.IsNullOrEmpty(control.Text.Trim()))
                    return false;
            return true;
        }
        /// <summary>
        /// Метод вызова справки
        /// </summary>
        /// <param name="topic">Связанныая тема</param>
        public static void GetHelp(string topic, HelpNavigator helpNavigator)
        {
            try
            {
                Help.ShowHelp(null, Path.ChangeExtension(Application.ExecutablePath, ".chm"), helpNavigator, topic + ".htm");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

    }

    /// <summary>
    /// Список словарей
    /// </summary>
    public enum DictionaryKind { Mvd, Lin, Division, Post, Exp, Method, UK, Role, Organization, Sort, ClassObject, ClassIkl, Locus };
    public enum CardKind { none, ikl, ik2, archive, ident };

    public static class GeneLibraryConst
    {
        public static int LocusViewCount { get { return 24; } }
        public static int AlleleViewCount { get { return 19; } }
        public static int columnFormFindHeaderWidth1 { get { return 35; } }
        public static int columnFormFindHeaderWidth2 { get { return 120; } }
        public static int DefaultMethod { get { return 1; } }
        public static string DefaultAccuracyStudy { get { return "E03"; } }
    }
    public abstract class CardItem 
    {
        public static CardKind CardKind(int cardId)
        {
            return cardItemGate.GetCardKind(cardId);  
        }
        public abstract CardItem CardCopy();

        // Private
        private static CardItemGate cardItemGate = GateFactory.CardItemGateOracle();
    }
    public abstract class CardItemGate
    {
        abstract public CardKind GetCardKind(int cardId);
    }
    public class CardItemGateOracle : CardItemGate
    {
        public override CardKind GetCardKind(int cardId)
        {
            string sql = " begin :res := modern.pkg_card.card_kind(:a_id); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_id", OracleType.Number, cardId, cmd, false);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);

                cmd.ExecuteNonQuery();
                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            int getCardId = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
            if (getCardId == -1)
                throw new WFException(ErrType.Message, String.Format(ErrorsMsg.NotCardFound, cardId));
            return Tools.CardKindType(getCardId);
        }
    }
    public enum ExcelCellOrientation { Vertical, Horizontal };
    public sealed class GeneTableRow
    {
        // Constructors
        public GeneTableRow (DataGridViewSelectedRowCollection rows, CardKind cardKind) 
        {
            this.CardKind = cardKind;
            this.Rows = rows;
        }
        
        // Properties
        public DataGridViewSelectedRowCollection Rows { get; set; }
        public CardKind CardKind { get; set; }
    }
    public sealed class CompareTableRow
    { 
        // Constructors
        public CompareTableRow(DataGridViewSelectedRowCollection rows, Vocabulary vocabulary)
        {
            this.Rows = rows;
            this.vocabulary = vocabulary;
        }

        // Properties
        public DataGridViewSelectedRowCollection Rows { get; set; }
        public string[] ColumnCaption { get { return this.vocabulary.IdAndCaption(); } }

        // Private
        private Vocabulary vocabulary;
    }
    public sealed class NativeMethods    
    {
        public const int WM_SCROLL = 276; // Horizontal scroll
        public const int WM_VSCROLL = 277; // Vertical scroll
        public const int SB_LINEUP = 0; // Scrolls one line up
        public const int SB_LINELEFT = 0;// Scrolls one cell left
        public const int SB_LINEDOWN = 1; // Scrolls one line down
        public const int SB_LINERIGHT = 1;// Scrolls one cell right
        public const int SB_PAGEUP = 2; // Scrolls one page up
        public const int SB_PAGELEFT = 2;// Scrolls one page left
        public const int SB_PAGEDOWN = 3; // Scrolls one page down
        public const int SB_PAGERIGTH = 3; // Scrolls one page right
        public const int SB_PAGETOP = 6; // Scrolls to the upper left
        public const int SB_LEFT = 6; // Scrolls to the left
        public const int SB_PAGEBOTTOM = 7; // Scrolls to the upper right
        public const int SB_RIGHT = 7; // Scrolls to the right
        public const int SB_ENDSCROLL = 8; // Ends scroll     
        public const int SB_THUMBPOSITION = 4; // Scroll to absolute position
        public const int SB_THUMBTRACK = 5; 
        
        [DllImport("user32")]
        internal static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32")]
        internal static extern int SetParent(int hWndChild, int hWndNewParent);
    }
}
