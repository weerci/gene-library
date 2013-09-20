using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using GeneLibrary.Common;

namespace GeneLibrary.Items
{
    abstract class Adjusting
    {
        public abstract void Load(int userId);

        /// <summary>
        /// Свойство содержит значение полей ИКЛ, выбранных для копирования в новую карточку
        /// </summary>
        public Collection<SelectedField> IklSelectedFields { get { return new Collection<SelectedField>(iklSelectedFields); } }
        public Collection<SelectedField> Ik2SelectedFields { get { return new Collection<SelectedField>(ik2SelectedFields); } }

        // Privte
        private List<SelectedField> iklSelectedFields = new List<SelectedField>();
        private List<SelectedField> ik2SelectedFields = new List<SelectedField>();
    }

    class AdjustingOracle : Adjusting
    {
        public override void Load(int userId)
        { 
                        
        }
    }

    public class SelectedField
    { 
        // Constructor
        public SelectedField(string name, string value, string nameTypes, bool isActive)
        {
            this.IsActive = isActive;
            this.Name = name;
            this.Value = value;
            this.NamesOfType = nameTypes;
        }

        // Properties
        public bool IsActive { get; set;  }
        public string Name { get; set; }
        public string Value { get; set; }
        public string NamesOfType { get; set; }
    }
}
