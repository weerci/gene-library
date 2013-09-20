using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Common;
using System.Globalization;
using GeneLibrary.Items.Contents.ResearchContent;
using WFExceptions;

namespace GeneLibrary.Items.Research
{
    class ResearchGridDecorator : ResearchDecorator
    {
        public ResearchGridDecorator(Research research, DataGridView dataGridViewProbably,
            DataGridView dataGridViewRatio)
            : base(research)
        {
            this.ResearchDataViewProbably = dataGridViewProbably;
            this.ResearchDataViewRatio = dataGridViewRatio;
        }
        public override void DirectIdent(string cardId, string fildName)
        {
            this.DirectIdent(Tools.GetIntFromText(cardId, fildName));
        }
        public override void DirectIdent(int cardId)
        {
            base.DirectIdent(cardId);
            List<string[]> rowsProbablyTable = new List<string[]>();
            List<string[]> rowsRatioTable = new List<string[]>();

            #region foreach on LocusContents
            foreach (LocusContent locusContent in base.ProbablyResult.LocusContents)
            {
                string alleleFrequency =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("P{0}={1}", allele.Name, allele.Frequency)).
                    Aggregate((curr, next) => curr + "; " + next);
                string alleleFormula;
                if (locusContent.IsHomozigotic(0))
                    alleleFormula = String.Format(ResourceStudy.formulaHomozigoticProbably,
                        locusContent.Cards.ElementAt(0).Allele.ElementAt(0).Name);
                else
                    alleleFormula = String.Format(ResourceStudy.formulaHeteroZigoticProbably,
                        locusContent.Cards.ElementAt(0).Allele.OrderBy(allele => Tools.AlleleConvert(allele.Name)).ElementAt(0).Name,
                        locusContent.Cards.ElementAt(0).Allele.OrderBy(allele => Tools.AlleleConvert(allele.Name)).ElementAt(1).Name);

                rowsProbablyTable.Add(new string[] 
                    {   locusContent.Title, 
                        alleleFrequency, 
                        alleleFormula, 
                        locusContent.Result.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture)
                    });

                rowsRatioTable.Add(
                    new string[]
                    {   locusContent.Title, 
                        alleleFrequency,
                        "",
                        "1",
                        alleleFormula, 
                        locusContent.Result.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture)
                    });
            }
            #endregion

            #region Заполнение таблиц прямых результатов и результатов отношения вероятностей
            double probablyResultDouble = base.ProbablyResult.Results.ElementAt(0).Value;
            string probablyResultString = base.ProbablyResult.Results.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);

            rowsProbablyTable.Add(new string[] { ResourceStudy.resultSummary, "", "", probablyResultString });

            rowsRatioTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "1", "", probablyResultString });
            rowsRatioTable.Add(new string[] 
                {   ResourceStudy.LR, 
                    (1/base.ProbablyResult.Results.ElementAt(0).Value).ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture), 
                    "", "", "", ""
                });

            ResearchDataViewProbably.DataSource = Tools.FillTable(
                new string[4] { ResourceStudy.locus, ResourceStudy.frequency, 
                String.Format(ResourceStudy.formula, ""), String.Format(ResourceStudy.result, "") },
                rowsProbablyTable);

            ResearchDataViewRatio.DataSource = Tools.FillTable(
                new string[6] 
                {   
                    ResourceStudy.locus, 
                    ResourceStudy.frequency, 
                    String.Format(ResourceStudy.formula, "1"), 
                    String.Format(ResourceStudy.result, "1"), 
                    String.Format(ResourceStudy.formula, "2"), 
                    String.Format(ResourceStudy.result, "2")
                },
                rowsRatioTable);
            #endregion
        }
        public override void OneChildAndParent(string cardChildId, string cardParentId, string fildChildName, string fildParentName)
        {
            this.OneChildAndParent(
                Tools.GetIntFromText(cardChildId, fildChildName),
                Tools.GetIntFromText(cardParentId, fildParentName));
        }
        public override void OneChildAndParent(int cardChildId, int cardParentId)
        {
            base.OneChildAndParent(cardChildId, cardParentId);
            List<string[]> rowsProbablyTable = new List<string[]>();
            List<string[]> rowsRatioTable = new List<string[]>();

            #region foreach on LocusContents
            // цикл идет по base.ProbablyResult.LocusContents, но результаты находятся и для ratioResult
            // поскольку в этом случае последовательности одинаковы
            foreach (LocusContent locusContent in base.ProbablyResult.LocusContents)
            {

                string alleleChildCardName =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("{0}", allele.Name)).
                    Aggregate((curr, next) => curr + "; " + next);

                string alleleParentCardName;
                if (locusContent.Cards.Count() > 1)
                    alleleParentCardName =
                        locusContent.Cards.ElementAt(1).Allele.
                        OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                        Select(allele => String.Format("{0}", allele.Name)).
                        Aggregate((curr, next) => curr + "; " + next);
                else
                    alleleParentCardName = ResourceStudy.alleleNotFound;

                string alleleFrequencyChildCard =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("P{0}={1}", allele.Name, allele.Frequency)).
                    Aggregate((curr, next) => curr + "; " + next);

                string alleleFormula1;
                string alleleFormula2;
                if (locusContent.IsHomozigotic(0))
                {
                    alleleFormula1 = String.Format(ResourceStudy.formulaOneParentHomozigotic,
                        locusContent.Cards.ElementAt(0).Allele.ElementAt(0).Name);
                    alleleFormula2 = String.Format(ResourceStudy.formula2OneParentHomozigotic,
                        locusContent.Cards.ElementAt(0).Allele.ElementAt(0).Name);
                }
                else
                {
                    alleleFormula1 = String.Format(ResourceStudy.formulaOneParentHeterozigotic,
                        locusContent.Cards.ElementAt(0).Allele.OrderBy(allele => Tools.AlleleConvert(allele.Name)).ElementAt(0).Name,
                        locusContent.Cards.ElementAt(0).Allele.OrderBy(allele => Tools.AlleleConvert(allele.Name)).ElementAt(1).Name);
                    alleleFormula2 = String.Format(ResourceStudy.formula2OneParentHeterozigotic,
                        locusContent.Cards.ElementAt(0).Allele.OrderBy(allele => Tools.AlleleConvert(allele.Name)).ElementAt(0).Name,
                        locusContent.Cards.ElementAt(0).Allele.OrderBy(allele => Tools.AlleleConvert(allele.Name)).ElementAt(1).Name);
                }

                rowsProbablyTable.Add(new string[] 
                    {   locusContent.Title, 
                        alleleChildCardName,
                        alleleParentCardName,
                        alleleFrequencyChildCard,
                        alleleFormula1, 
                        locusContent.Result.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture)
                    });

                rowsRatioTable.Add(
                    new string[]
                    {   locusContent.Title, 
                        alleleChildCardName,
                        alleleParentCardName,
                        alleleFrequencyChildCard,
                        alleleFormula1,
                        base.RatioResult.LocusContents.Where(n=>n.Name==locusContent.Title).First().
                            Result.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture),
                        alleleFormula2,
                        base.RatioResult.LocusContents.Where(n=>n.Name==locusContent.Title).First().
                            Result.ElementAt(1).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture),
                    });
            }
            #endregion

            #region Заполнение таблиц прямых результатов и результатов отношения вероятностей

            string probablyResultString = base.ProbablyResult.Results.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);

            double ratio1Result = Convert.ToDouble(base.RatioResult.Results.ElementAt(0).Value);
            double ratio2Result = Convert.ToDouble(base.RatioResult.Results.ElementAt(1).Value);
            string ratio1ResultString = ratio1Result.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);
            string ratio2ResultString = ratio2Result.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);

            rowsProbablyTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "", "", probablyResultString });
            rowsRatioTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "", "", ratio1ResultString, "", ratio2ResultString });
            rowsRatioTable.Add(new string[] { ResourceStudy.LR, 
                (ratio1Result/ratio2Result).ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture), 
                    "", "", "", "", "", ""
                });

            ResearchDataViewProbably.DataSource = Tools.FillTable(
                new string[6] { 
                    ResourceStudy.locus, ResourceStudy.childAllele, ResourceStudy.parentAllele, ResourceStudy.frequencyChild,
                String.Format(ResourceStudy.formula, ""), String.Format(ResourceStudy.result, "") },
                rowsProbablyTable);

            ResearchDataViewRatio.DataSource = Tools.FillTable(
                new string[8] 
                {   
                    ResourceStudy.locus, ResourceStudy.childAllele, ResourceStudy.parentAllele, ResourceStudy.frequencyChild,
                    String.Format(ResourceStudy.formula, "1"), String.Format(ResourceStudy.result, "1"), 
                    String.Format(ResourceStudy.formula, "2"), String.Format(ResourceStudy.result, "2") 
                }, rowsRatioTable);
            #endregion
        }
        public override void TwoParent(string cardChildId, string cardKnownParentId, string cardUnknownParentId,
            string fieldChildName, string fieldKnownParentName, string fieldUnknownParentName)
        {
            this.TwoParent(
                Tools.GetIntFromText(cardChildId, fieldChildName),
                Tools.GetIntFromText(cardKnownParentId, fieldKnownParentName),
                Tools.GetIntFromText(cardUnknownParentId, fieldUnknownParentName)
                );
        }
        public override void TwoParent(int cardChildId, int cardKnownParentId, int cardUnknownParentId)
        {
            base.TwoParent(cardChildId, cardKnownParentId, cardUnknownParentId);
            List<string[]> rowsProbablyTable = new List<string[]>();
            List<string[]> rowsRatioTable = new List<string[]>();

            #region foreach on LocusContents
            // цикл идет по base.ProbablyResult.LocusContents, но результаты находятся и для ratioResult
            // поскольку в этом случае последовательности одинаковы
            foreach (LocusContent locusContent in base.ProbablyResult.LocusContents)
            {

                string alleleChildCardName =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("{0}", allele.Name)).
                    Aggregate((curr, next) => curr + "; " + next);

                string alleleKnownParentCardName;
                if (locusContent.Cards.Count() > 1)
                    alleleKnownParentCardName =
                        locusContent.Cards.ElementAt(1).Allele.
                        OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                        Select(allele => String.Format("{0}", allele.Name)).
                        Aggregate((curr, next) => curr + "; " + next);
                else
                    alleleKnownParentCardName = ResourceStudy.alleleNotFound;

                string alleleUnknownParentCardName;
                if (locusContent.Cards.Count() > 2)
                    alleleUnknownParentCardName =
                        locusContent.Cards.ElementAt(2).Allele.
                        OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                        Select(allele => String.Format("{0}", allele.Name)).
                        Aggregate((curr, next) => curr + "; " + next);
                else
                    alleleUnknownParentCardName = ResourceStudy.alleleNotFound;

                string alleleFrequencyChildCard =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("P{0}={1}", allele.Name, allele.Frequency)).
                    Aggregate((curr, next) => curr + "; " + next);

                rowsProbablyTable.Add(new string[] 
                    {   locusContent.Title, 
                        alleleChildCardName,
                        alleleKnownParentCardName,
                        alleleUnknownParentCardName,
                        alleleFrequencyChildCard,
                        locusContent.Formula.ElementAt(0).Value, 
                        locusContent.Result.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture)
                    });

                rowsRatioTable.Add(
                    new string[]
                    {   locusContent.Title, 
                        alleleChildCardName,
                        alleleKnownParentCardName,
                        alleleUnknownParentCardName,
                        alleleFrequencyChildCard,
                        "",
                        "1",
                        locusContent.Formula.ElementAt(0).Value,
                        base.RatioResult.LocusContents.Where(n=>n.Name==locusContent.Title).First().
                            Result.ElementAt(1).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture)
                    });
            }
            #endregion

            #region Заполнение таблиц прямых результатов и результатов отношения вероятностей

            string probablyResultString = base.ProbablyResult.Results.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);
            rowsProbablyTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "", "", "", probablyResultString });

            string ratioResultString = base.RatioResult.Results.ElementAt(1).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);
            rowsRatioTable.Add(new string[] { ResourceStudy.LR, 
                (1/base.RatioResult.Results.ElementAt(1).Value).ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture), 
                    "", "", "", "", "", "", "" });
            rowsRatioTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "", "", "", "1", "", ratioResultString });

            ResearchDataViewProbably.DataSource = Tools.FillTable(
                new string[7] { 
                    ResourceStudy.locus, 
                    ResourceStudy.childAllele, 
                    ResourceStudy.knownParentAllele, 
                    ResourceStudy.assumedParentAllele,
                    ResourceStudy.frequencyChild,
                    String.Format(ResourceStudy.formula, ""), 
                    String.Format(ResourceStudy.result, "") 
                },
                rowsProbablyTable
            );

            ResearchDataViewRatio.DataSource = Tools.FillTable(
                new string[9] 
                {   
                    ResourceStudy.locus, 
                    ResourceStudy.childAllele, 
                    ResourceStudy.knownParentAllele, 
                    ResourceStudy.assumedParentAllele, 
                    ResourceStudy.frequencyChild,
                    String.Format(ResourceStudy.formula, "1"), 
                    String.Format(ResourceStudy.result, "1"), 
                    String.Format(ResourceStudy.formula, "2"), 
                    String.Format(ResourceStudy.result, "2") 
                },
                rowsRatioTable
            );
            #endregion

        }
        public override void SuppParent(string cardChaildId, string cardFirstParentId, string cardSecondId, string cardChaildName, string cardFirstParentName, string cardSecondName)
        {
            this.SuppParent(
                Tools.GetIntFromText(cardChaildId, cardChaildName),
                Tools.GetIntFromText(cardFirstParentId, cardFirstParentName),
                Tools.GetIntFromText(cardSecondId, cardSecondName));
        }
        public override void SuppParent(int cardChaildId, int cardFirstParentId, int cardSecondParentId)
        {
            base.SuppParent(cardChaildId, cardFirstParentId, cardSecondParentId);
            List<string[]> rowsProbablyTable = new List<string[]>();
            List<string[]> rowsRatioTable = new List<string[]>();

            #region foreach on LocusContents
            // цикл идет по base.ProbablyResult.LocusContents, но результаты находятся и для ratioResult
            // поскольку в этом случае последовательности одинаковы
            foreach (LocusContent locusContent in base.ProbablyResult.LocusContents)
            {
                string alleleChildCardName =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("{0}", allele.Name)).
                    Aggregate((curr, next) => curr + "; " + next);

                string alleleOneParentCardName;
                if (locusContent.Cards.Count() > 1)
                    alleleOneParentCardName =
                        locusContent.Cards.ElementAt(1).Allele.
                        OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                        Select(allele => String.Format("{0}", allele.Name)).
                        Aggregate((curr, next) => curr + "; " + next);
                else
                    alleleOneParentCardName = ResourceStudy.alleleNotFound;

                string alleleSecondParentCardName;
                if (locusContent.Cards.Count() > 2)
                    alleleSecondParentCardName =
                        locusContent.Cards.ElementAt(2).Allele.
                        OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                        Select(allele => String.Format("{0}", allele.Name)).
                        Aggregate((curr, next) => curr + "; " + next);
                else
                    alleleSecondParentCardName = ResourceStudy.alleleNotFound;

                string alleleFrequencyChildCard =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("P{0}={1}", allele.Name, allele.Frequency)).
                    Aggregate((curr, next) => curr + "; " + next);

                string alleleFormula;
                if (locusContent.IsHomozigotic(0))
                    alleleFormula = String.Format(ResourceStudy.formulaSuppHomozigotic,
                        locusContent.Cards.ElementAt(0).Allele.ElementAt(0).Name);
                else
                    alleleFormula = String.Format(ResourceStudy.formulaSuppHeterozigotic,
                        locusContent.Cards.ElementAt(0).Allele.OrderBy(allele => Tools.AlleleConvert(allele.Name)).ElementAt(0).Name,
                        locusContent.Cards.ElementAt(0).Allele.OrderBy(allele => Tools.AlleleConvert(allele.Name)).ElementAt(1).Name);

                rowsProbablyTable.Add(new string[] 
                    {   locusContent.Title, 
                        alleleChildCardName,
                        alleleOneParentCardName,
                        alleleSecondParentCardName,
                        alleleFrequencyChildCard,
                        alleleFormula, 
                        locusContent.Result.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture)
                    });

                rowsRatioTable.Add(
                    new string[]
                    {   locusContent.Title, 
                        alleleChildCardName,
                        alleleOneParentCardName,
                        alleleSecondParentCardName,
                        alleleFrequencyChildCard,
                        "",
                        "1",
                        alleleFormula,
                        base.RatioResult.LocusContents.Where(n=>n.Name==locusContent.Title).First().
                            Result.ElementAt(1).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture),
                    });
            }
            #endregion

            #region Заполнение таблиц прямых результатов и результатов отношения вероятностей

            string probablyResultString = base.ProbablyResult.Results.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);

            double ratio1Result = Convert.ToDouble(base.RatioResult.Results.ElementAt(0).Value);
            double ratio2Result = Convert.ToDouble(base.RatioResult.Results.ElementAt(1).Value);
            string ratio1ResultString = ratio1Result.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);
            string ratio2ResultString = ratio2Result.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);

            rowsProbablyTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "", "", "", probablyResultString });
            rowsRatioTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "", "", "", "1", "", ratio2ResultString });
            rowsRatioTable.Add(new string[] { ResourceStudy.LR, 
                (1/ratio2Result).ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture), 
                    "", "", "", "", "", "", ""
                });

            ResearchDataViewProbably.DataSource = Tools.FillTable(
                new string[7] { 
                    ResourceStudy.locus, 
                    ResourceStudy.childAllele, 
                    ResourceStudy.alleleFristParent, 
                    ResourceStudy.alleleSecondParent, 
                    ResourceStudy.frequencyChild,
                    String.Format(ResourceStudy.formula, ""), 
                    String.Format(ResourceStudy.result, ""), 
                    },
                rowsProbablyTable);

            ResearchDataViewRatio.DataSource = Tools.FillTable(
                new string[9] 
                {   
                    ResourceStudy.locus, 
                    ResourceStudy.childAllele, 
                    ResourceStudy.alleleFristParent, 
                    ResourceStudy.alleleSecondParent, 
                    ResourceStudy.frequencyChild,
                    String.Format(ResourceStudy.formula, "1"), 
                    String.Format(ResourceStudy.result, "1"), 
                    String.Format(ResourceStudy.formula, "2"), 
                    String.Format(ResourceStudy.result, "2") 
                    },
                rowsRatioTable);
            #endregion
        }
        public override void Blend(string blendId, string personId, string blendName, string personName)
        {
            this.Blend(
                Tools.GetIntFromText(blendId, blendName),
                Tools.GetIntFromText(personId, personName)
                );
        }
        public override void Blend(int blendId, int personId)
        {
            base.Blend(blendId, personId);
            List<string[]> rowsProbablyTable = new List<string[]>();
            List<string[]> rowsRatioTable = new List<string[]>();

            #region foreach on LocusContents
            // цикл идет по base.ProbablyResult.LocusContents, но результаты находятся и для ratioResult
            // поскольку в этом случае последовательности одинаковы
            foreach (LocusContent locusContent in base.ProbablyResult.LocusContents)
            {
                string alleleBlend =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("{0}", allele.Name)).
                    Aggregate((curr, next) => curr + "; " + next);

                string allelePerson;
                if (locusContent.Cards.Count() > 1)
                    allelePerson =
                        locusContent.Cards.ElementAt(1).Allele.
                        OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                        Select(allele => String.Format("{0}", allele.Name)).
                        Aggregate((curr, next) => curr + "; " + next);
                else
                    allelePerson = ResourceStudy.alleleNotFound;

                string alleleFrequencyBlend =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("P{0}={1}", allele.Name, allele.Frequency)).
                    Aggregate((curr, next) => curr + "; " + next);

                string alleleFormula =
                    String.Format("({0})^2", locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("P{0}", allele.Name)).
                    Aggregate((curr, next) => curr + " + " + next));
                ;

                rowsProbablyTable.Add(new string[] 
                    {   locusContent.Title, 
                        alleleBlend,
                        allelePerson,
                        alleleFrequencyBlend,
                        alleleFormula,
                        locusContent.Result.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture)
                    });

                rowsRatioTable.Add(
                    new string[]
                    {   locusContent.Title, 
                        alleleBlend,
                        allelePerson,
                        alleleFrequencyBlend,
                        "",
                        "1",
                        alleleFormula,
                        base.RatioResult.LocusContents.Where(n=>n.Name==locusContent.Title).First().
                            Result.ElementAt(1).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture),
                    });
            }
            #endregion

            #region Заполнение таблиц прямых результатов и результатов отношения вероятностей

            string probablyResultString = base.ProbablyResult.Results.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);

            double ratio1Result = Convert.ToDouble(base.RatioResult.Results.ElementAt(0).Value);
            double ratio2Result = Convert.ToDouble(base.RatioResult.Results.ElementAt(1).Value);
            string ratio1ResultString = ratio1Result.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);
            string ratio2ResultString = ratio2Result.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);

            rowsProbablyTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "", "", probablyResultString });
            rowsRatioTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "", "", "1", "", ratio2ResultString });
            rowsRatioTable.Add(new string[] { ResourceStudy.LR, 
                (1/ratio2Result).ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture), 
                    "", "", "", "", "", ""
                });

            ResearchDataViewProbably.DataSource = Tools.FillTable(
                new string[6] { 
                    ResourceStudy.locus, 
                    ResourceStudy.alleleBlend, 
                    ResourceStudy.allelePerson, 
                    ResourceStudy.alleleBlendFrequency, 
                    String.Format(ResourceStudy.formula, ""), 
                    String.Format(ResourceStudy.result, ""), 
                    },
                rowsProbablyTable);

            ResearchDataViewRatio.DataSource = Tools.FillTable(
                new string[8] 
                {   
                    ResourceStudy.locus, 
                    ResourceStudy.alleleBlend, 
                    ResourceStudy.allelePerson, 
                    ResourceStudy.alleleBlendFrequency, 
                    String.Format(ResourceStudy.formula, "1"), 
                    String.Format(ResourceStudy.result, "1"), 
                    String.Format(ResourceStudy.formula, "2"), 
                    String.Format(ResourceStudy.result, "2") 
                    },
                rowsRatioTable);
            #endregion
        }
        public override void TwoBlend(string blendId, string personOneId, string personSecondId, string blendName,
            string personOneName, string personSecondName)
        {
            this.TwoBlend(
                           Tools.GetIntFromText(blendId, blendName),
                           Tools.GetIntFromText(personOneId, personOneName),
                           Tools.GetIntFromText(personSecondId, personSecondName)
                           );

        }
        public override void TwoBlend(int blendId, int personOneId, int personSecondId)
        {
            base.TwoBlend(blendId, personOneId, personSecondId);
            List<string[]> rowsProbablyTable = new List<string[]>();
            List<string[]> rowsRatioTable = new List<string[]>();

            #region foreach on LocusContents

            foreach (LocusContent locusContent in base.ProbablyResult.LocusContents)
            {

                string alleleBlendName =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("{0}", allele.Name)).
                    Aggregate((curr, next) => curr + "; " + next);

                string allelePersonOneName;
                if (locusContent.Cards.Count() > 1)
                    allelePersonOneName =
                        locusContent.Cards.ElementAt(1).Allele.
                        OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                        Select(allele => String.Format("{0}", allele.Name)).
                        Aggregate((curr, next) => curr + "; " + next);
                else
                    allelePersonOneName = ResourceStudy.alleleNotFound;

                string allelePersonSecondName;
                if (locusContent.Cards.Count() > 2)
                    allelePersonSecondName =
                        locusContent.Cards.ElementAt(2).Allele.
                        OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                        Select(allele => String.Format("{0}", allele.Name)).
                        Aggregate((curr, next) => curr + "; " + next);
                else
                    allelePersonSecondName = ResourceStudy.alleleNotFound;

                string alleleFrequencyBlendName =
                    locusContent.Cards.ElementAt(0).Allele.
                    OrderBy(allele => Tools.AlleleConvert(allele.Name)).
                    Select(allele => String.Format("P{0}={1}", allele.Name, allele.Frequency)).
                    Aggregate((curr, next) => curr + "; " + next);

                rowsProbablyTable.Add(new string[] 
                    {   
                        locusContent.Title, 
                        alleleBlendName,
                        allelePersonOneName,
                        allelePersonSecondName,
                        alleleFrequencyBlendName,
                        locusContent.Formula.ElementAt(0).Value, 
                        locusContent.Result.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture)
                    });
            }
            #endregion

            #region Заполнение таблиц прямых результатов и результатов отношения вероятностей

            string probablyResultString = base.ProbablyResult.Results.ElementAt(0).Value.ToString(GeneLibraryConst.DefaultAccuracyStudy, CultureInfo.InvariantCulture);
            rowsProbablyTable.Add(new string[] { ResourceStudy.resultSummary, "", "", "", "", "", probablyResultString });

            ResearchDataViewProbably.DataSource = Tools.FillTable(
                new string[7] { 
                    ResourceStudy.locus, 
                    ResourceStudy.alleleBlend, 
                    ResourceStudy.alleleTwoBlend2, 
                    ResourceStudy.alleleTwoBlend1,
                    ResourceStudy.frequencyBlend,
                    String.Format(ResourceStudy.formula, ""), 
                    String.Format(ResourceStudy.result, "") 
                },
                rowsProbablyTable
            );

            #endregion
        }

        public DataGridView ResearchDataViewProbably { get; set; }
        public DataGridView ResearchDataViewRatio { get; set; }
    }
}
