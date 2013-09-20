using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLibrary.Common;
using System.Data;
using WFExceptions;
using System.Collections.ObjectModel;

namespace GeneLibrary.Items.Contents.ResearchContent
{
    public class ResearchResult
    {
        // Interface
        public Collection<LocusContent> LocusContents { get { return new Collection<LocusContent>(locusContents); } }
        public Collection<Formula> Formuls { get { return new Collection<Formula>(formuls); } }
        public Collection<Result> Results { get { return new Collection<Result>(results); } }
        
        // Fields
        private List<Formula> formuls = new List<Formula>();
        private List<Result> results = new List<Result>();
        private List<LocusContent> locusContents = new List<LocusContent>();
    }
    public class LocusContent
    { 
        // Constructors
        public LocusContent() {}
        public LocusContent(string name, string title)
        {
            this.Name = name;
            this.Title = title;
        }

        // Interface
        public string Name { get; set; }
        public string Title { get; set; }
        public Collection<Card> Cards { get { return new Collection<Card>(cards); } }
        public Collection<Result> Result { get { return new Collection<Result>(results); } }
        public Collection<Formula> Formula { get { return new Collection<Formula>(formuls); } }
        public bool IsHomozigotic(int index)
        {
            return Cards.ElementAt(index).Allele.Count == 1;
        }

        // Fields
        private List<Card> cards = new List<Card>();
        private List<Result> results = new List<Result>();
        private List<Formula> formuls = new List<Formula>();
    }
    public class Card
    {
        // Constructors
        public Card() { }
        public Card(int id)
        {
            this.Id = id;
        }

        // Interface
        public int Id { get; set; }
        public void AddAllele(Allele[] allelies)
        {
            foreach (Allele loopAllele in allelies)
                this.allele.Add(loopAllele);
        }
        public Collection<Allele> Allele { get { return new Collection<Allele>(allele); } }

        // Fields
        private List<Allele> allele = new List<Allele>();
    }
    public class Result
    {
        // Constructors
        public Result() { }
        public Result(string name, string title)
        {
            this.Name = name;
            this.Title = title;
        }

        // Interface
        public string Name { get; set; }
        public string Title { get; set; }
        public double Value { get; set; }
    }
    public class Formula
    { 
        // Constructors
        public Formula() { }
        public Formula(string name, string title)
        {
            this.Name = name;
            this.Title = title;
        }

        // Interface
        public string Name { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}