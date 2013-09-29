using System;
using System.Collections.Generic;
using System.Linq;
using GeneLibrary.Common;
using GeneLibrary.Items.Contents.ResearchContent;
using WFExceptions;
using System.Windows.Forms;


namespace GeneLibrary.Items.Research
{
    class Research
    {
        public Research(){         }
        public Research(int methodId)
        {
            _methodId = methodId;
        }

        // Прямая идентификация
        public virtual void DirectIdent(string cardId, string fildName)
        {
            DirectIdent(Tools.GetIntFromText(cardId, fildName));
        }
        public virtual void DirectIdent(int cardId)
        {
            this.probablyResult = new ResearchResult();
            this.ratioResult = new ResearchResult();

            Profiles profileChild = Tools.GetProfileById(cardId, MethodId);
            var locuses = profileChild.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            if (locuses.Count() == 0)
                throw new WFException(ErrType.Message, ErrorsMsg.LocusesListIsEmpty);

            foreach (Locus loopLocus in locuses)
            {
                LocusContent locusContentProbably = new LocusContent(loopLocus.Name, loopLocus.Name);
                
                Card card = new Card(cardId);
                Result result = new Result("result", String.Format(ResourceStudy.result, ""));
                Formula formula = new Formula("formula", String.Format(ResourceStudy.formula, ""));
                if (loopLocus.IsHomozygotic)
                {
                    Allele allele = loopLocus.Allele.Where(n => n.Checked).ElementAt(0);
                    card.Allele.Add(allele);
                    result.Value = Math.Pow(allele.Frequency, 2);
                }
                else
                {
                    var alleles = loopLocus.Allele.Where(n => n.Checked).Where((n, index) => index < 2);
                    foreach (var loop in alleles)
                        card.Allele.Add(loop);
                    result.Value = 2 * (alleles.ElementAt(0).Frequency * alleles.ElementAt(1).Frequency);
                }
                // Add probably contents
                locusContentProbably.Result.Add(result);
                locusContentProbably.Cards.Add(card);
                locusContentProbably.Formula.Add(formula);
                this.probablyResult.LocusContents.Add(locusContentProbably);

            }
            string resultName = String.Format(ResourceStudy.result, "");
            double resultValue = this.ProbablyResult.LocusContents.
                Select(n => n.Result.Where(r=>r.Name == "result").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);

            this.ProbablyResult.Results.Add(new Result(resultName, resultName) { Value = resultValue });
        }
        
        // Исследование родства ребенка и одного предполагаемого родителя
        public virtual void OneChildAndParent(string cardChildId,   string cardParentId, string fildChildName, string fildParentName)
        {
            OneChildAndParent(Tools.GetIntFromText(cardChildId, fildChildName), Tools.GetIntFromText(cardParentId, fildParentName));
        }
        public virtual void OneChildAndParent(int cardChildId, int cardParentId)
        {
            this.probablyResult = new ResearchResult();
            this.ratioResult = new ResearchResult();

            Profiles profileChild = Tools.GetProfileById(cardChildId, MethodId);
            Profiles profileParent = Tools.GetProfileById(cardParentId, MethodId);

            var child = profileChild.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            var parent = profileParent.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");

            var commonLocuses = profileChild.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin").
                Intersect(profileParent.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin"), new EqualityLocusById()).
                Where(n => n.Allele.Where(l=>l.Checked).Intersect(profileParent[n.Name].Allele.Where(l=>l.Checked), new EqualityAlleleById()).Count() > 0);

            foreach (Locus loopLocus in commonLocuses)
            {
                LocusContent locusContentProbably = new LocusContent(loopLocus.Name, loopLocus.Name);
                LocusContent locusContentRatio = new LocusContent(loopLocus.Name, loopLocus.Name);

                Card cardChild = new Card(cardChildId);
                Card cardParent = new Card(cardParentId);
                Result resultProbably = new Result("result", String.Format(ResourceStudy.result, ""));
                Result resultRatio1 = new Result("resultRatio1", String.Format(ResourceStudy.result, "resultRatio1"));
                Result resultRatio2 = new Result("resultRatio2", String.Format(ResourceStudy.result, "resultRatio2"));
                Formula formulaProbably = new Formula("formula", String.Format(ResourceStudy.formula, ""));
                Formula formulaRatio1 = new Formula("formula1", String.Format(ResourceStudy.formula, ""));
                Formula formulaRatio2 = new Formula("formula2", String.Format(ResourceStudy.formula, ""));

                var alleles = loopLocus.Allele.Where(n => n.Checked).Where((n, index) => index < 2);
                cardChild.AddAllele(alleles.ToArray<Allele>());
                cardParent.AddAllele(profileParent[loopLocus.Name].Allele.Where(n=>n.Checked).ToArray<Allele>());

                if (loopLocus.IsHomozygotic)
                {
                    resultProbably.Value = alleles.ElementAt(0).Frequency * (2 - alleles.ElementAt(0).Frequency);
                    resultRatio1.Value = alleles.ElementAt(0).Frequency * (2 - alleles.ElementAt(0).Frequency);
                    resultRatio2.Value = Math.Pow(resultRatio1.Value, 2);
                }
                else
                {
                    if (alleles.Count() > 1)
                    {
                        double firstFrequency = alleles.ElementAt(0).Frequency;
                        double secondFrequency = alleles.ElementAt(1).Frequency;
                        var parentAllelies = profileParent[loopLocus.Name].Allele.Where(n => n.Checked);

                        // Прямая гипотеза заключается в том, что родителями ребенка являются представленный родитель
                        // и неизвестное лицо, поэтому для расчета вероятности из профиля ребена выбирается
                        // та аллель, которая отсутствует у представленного родителя
                        var alleleUnknownParent = alleles.Except(parentAllelies, new EqualityAlleleById());
                        double freqRatio1;
                        if (alleleUnknownParent.Count() == 0)
                            // Если аллели ребенка и родителя совпадают, в расчет выбирается аллель с меньшей  вероятностью
                            freqRatio1 = alleles.Min(all => all.Frequency);
                        else
                            freqRatio1 = alleleUnknownParent.Last().Frequency;

                        resultProbably.Value = (firstFrequency + secondFrequency) * (2 - (firstFrequency + secondFrequency));
                        resultRatio1.Value = freqRatio1 * (2 - freqRatio1);
                        resultRatio2.Value = 2 * firstFrequency * (2 - firstFrequency) * secondFrequency * (2 - secondFrequency) -
                            Math.Pow(2 * firstFrequency * secondFrequency, 2);
                    }

                }
                // Add probably contents
                locusContentProbably.Result.Add(resultProbably);
                locusContentProbably.Cards.Add(cardChild);
                locusContentProbably.Cards.Add(cardParent);
                locusContentProbably.Formula.Add(formulaProbably);
                this.probablyResult.LocusContents.Add(locusContentProbably);

                // Add ratio contents
                locusContentRatio.Result.Add(resultRatio1);
                locusContentRatio.Result.Add(resultRatio2);
                locusContentRatio.Cards.Add(cardChild);
                locusContentRatio.Cards.Add(cardParent);
                locusContentRatio.Formula.Add(formulaRatio1);
                locusContentRatio.Formula.Add(formulaRatio2);
                this.ratioResult.LocusContents.Add(locusContentRatio);

            }
            string resultName = String.Format(ResourceStudy.result, "");
            if (this.ProbablyResult.LocusContents.Count() == 0)
            {
                throw new WFException(ErrType.Message,  
                    String.Format(ResourceStudy.locusesNotFound, cardChildId, cardParentId));
            }
            double resultProbablyValue = this.ProbablyResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "result").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);
            double resultRatio1Value = this.RatioResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "resultRatio1").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);
            double resultRatio2Value = this.RatioResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "resultRatio2").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);

            this.ProbablyResult.Results.Add(new Result(resultName, resultName) { Value = resultProbablyValue });
            this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio1Value });
            this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio2Value });
        }
        
        // Исследование родства ребенка одного известного и одного предполагаемого родителя
        public virtual void TwoParent(
            string cardChildId,    string cardKnownParentId,    string cardUnknownParentId,
            string fieldChildName, string fieldKnownParentName, string fieldUnknownParentName
            )
        {
            TwoParent(
                Tools.GetIntFromText(cardChildId, fieldChildName),
                Tools.GetIntFromText(cardKnownParentId, fieldKnownParentName),
                Tools.GetIntFromText(cardUnknownParentId, fieldUnknownParentName));
        }
        public virtual void TwoParent(int cardChildId, int cardKnownParentId, int cardUnknownParentId)
        {
            this.probablyResult = new ResearchResult();
            this.ratioResult = new ResearchResult();

            Profiles profileChild = Tools.GetProfileById(cardChildId, _methodId);
            Profiles profileKnownParent = Tools.GetProfileById(cardKnownParentId, _methodId);
            Profiles profileUnknownParent = Tools.GetProfileById(cardUnknownParentId, _methodId);

            var child = profileChild.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            var parentKnown = profileKnownParent.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            var parentUnknown = profileUnknownParent.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");

            var commonLocuses = child.Intersect(parentKnown, new EqualityLocusById()).Intersect(parentUnknown, new EqualityLocusById());
            if (commonLocuses.Count() == 0)
                throw new WFException(ErrType.Message,
                    String.Format(ResourceStudy.locusesNotFound, cardChildId, cardUnknownParentId));
 
            foreach (Locus loopLocus in commonLocuses)
            {
                LocusContent locusContentProbably = new LocusContent(loopLocus.Name, loopLocus.Name);
                LocusContent locusContentRatio = new LocusContent(loopLocus.Name, loopLocus.Name);

                Card cardChild = new Card(cardChildId);
                Card cardKnownParent = new Card(cardKnownParentId);
                Card cardUnknownParent = new Card(cardUnknownParentId);

                Result resultProbably = new Result("result", String.Format(ResourceStudy.result, ""));
                Result resultRatio1 = new Result("resultRatio1", String.Format(ResourceStudy.result, "resultRatio1"));
                Result resultRatio2 = new Result("resultRatio2", String.Format(ResourceStudy.result, "resultRatio2"));
                Formula formulaProbably = new Formula("formula", String.Format(ResourceStudy.formula, "formulaProbably"));
                Formula formulaRatio1 = new Formula("formula1", String.Format(ResourceStudy.formula, "formulaRatio1"));
                Formula formulaRatio2 = new Formula("formula2", String.Format(ResourceStudy.formula, "formulaRatio2"));

                // Находим алелли текущего локуса для ребенка
                var allelesChild = loopLocus.Allele.Where(n => n.Checked).Where((n, index) => index < 2);
                cardChild.AddAllele(allelesChild.ToArray<Allele>());

                // Находим аллели текущего локуса для известного родителя
                var locusesKnownParent = parentKnown.Where(n => n.Name == loopLocus.Name);
                Allele[] alleliesKnownParent;
                if (locusesKnownParent.Count() > 0)
                {
                    alleliesKnownParent = locusesKnownParent.First().Allele.Where(n => n.Checked).Where((n, index) => index < 2).ToArray();
                    cardKnownParent.AddAllele(alleliesKnownParent);
                }
                else
                    alleliesKnownParent = null;

                // Находим аллели текущего локуса для предполагаемого родителя
                var locusesUnknownParent = parentUnknown.Where(n => n.Name == loopLocus.Name);
                Allele[] alleliesUnknownParent;
                if (locusesUnknownParent.Count() > 0)
                {
                    alleliesUnknownParent = locusesUnknownParent.First().Allele.Where(n => n.Checked).Where((n, index) => index < 2).ToArray();
                    cardUnknownParent.AddAllele(alleliesUnknownParent);

                    var parentIntersect = alleliesUnknownParent.Intersect(allelesChild, new EqualityAlleleById());
                    // Гетерозиготные локусы известного родителя и ребенка совпадают полностью
                    if (loopLocus.IsHomozygotic)
                    {
                        resultProbably.Value = allelesChild.ElementAt(0).Frequency *(2 - allelesChild.ElementAt(0).Frequency);
                        formulaProbably.Value = String.Format(ResourceStudy.formulaTwoParent1, allelesChild.ElementAt(0).Name);
                    }
                    else if (allelesChild.Intersect(alleliesKnownParent, new EqualityAlleleById()).Count() == 2)
                    {
                        double firstFrequency = allelesChild.ElementAt(0).Frequency;
                        double secondFrequency = allelesChild.ElementAt(1).Frequency;
                        resultProbably.Value = (firstFrequency + secondFrequency) * (2 - (firstFrequency + secondFrequency));
                        formulaProbably.Value = String.Format(ResourceStudy.formulaTwoParent2, allelesChild.ElementAt(0).Name,
                            allelesChild.ElementAt(1).Name);
                    }
                    else
                    {
                        double frequency = alleliesUnknownParent.Intersect(allelesChild.Except(alleliesKnownParent, new EqualityAlleleById()), new EqualityAlleleById()).ElementAt(0).Frequency;
                        resultProbably.Value = frequency * (2 - frequency);
                        formulaProbably.Value = String.Format(ResourceStudy.formulaTwoParent1,
                            alleliesUnknownParent.Intersect(allelesChild.Except(alleliesKnownParent, new EqualityAlleleById()), new EqualityAlleleById()).ElementAt(0).Name);
                    }
                    ;
                    
                    // Add probably contents
                    locusContentProbably.Result.Add(resultProbably);
                    locusContentProbably.Cards.Add(cardChild);
                    locusContentProbably.Cards.Add(cardKnownParent);
                    locusContentProbably.Cards.Add(cardUnknownParent);
                    locusContentProbably.Formula.Add(formulaProbably);
                    this.probablyResult.LocusContents.Add(locusContentProbably);

                    // Add ratio contents
                    resultRatio1.Value = 1;
                    resultRatio2.Value = resultProbably.Value;
                    locusContentRatio.Result.Add(resultRatio1);
                    locusContentRatio.Result.Add(resultRatio2);
                    locusContentRatio.Cards.Add(cardChild);
                    locusContentRatio.Cards.Add(cardKnownParent);
                    locusContentRatio.Cards.Add(cardUnknownParent);
                    locusContentRatio.Formula.Add(formulaRatio1);
                    locusContentRatio.Formula.Add(formulaRatio2);
                    this.ratioResult.LocusContents.Add(locusContentRatio);
                }
            }

            string resultName = String.Format(ResourceStudy.result, "");
            if (this.ProbablyResult.LocusContents.Count() == 0)
            {
                throw new WFException(ErrType.Message,
                    String.Format(ResourceStudy.locusesNotFound, cardChildId, cardUnknownParentId));
            }
            double resultProbablyValue = this.ProbablyResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "result").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);
            double resultRatio1Value = this.RatioResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "resultRatio1").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);
            double resultRatio2Value = this.RatioResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "resultRatio2").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);

            this.ProbablyResult.Results.Add(new Result(resultName, resultName) { Value = resultProbablyValue });
            this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio1Value });
            this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio2Value });
        }

        // Исследование родства ребенка и пары предполагаемых родителей
        public virtual void SuppParent(
            string cardChaildId,   string cardFirstParentId,   string cardSecondId,
            string cardChaildName, string cardFirstParentName, string cardSecondName)
        { 
            this.SuppParent(
                Tools.GetIntFromText(cardChaildId, cardChaildName),
                Tools.GetIntFromText(cardFirstParentId, cardFirstParentName),
                Tools.GetIntFromText(cardSecondId, cardSecondName)
            );
        }
        public virtual void SuppParent(int cardChaildId, int cardFirstParentId, int cardSecondParentId)
        {
            this.probablyResult = new ResearchResult();
            this.ratioResult = new ResearchResult();

            Profiles profileChild = Tools.GetProfileById(cardChaildId, _methodId);
            Profiles profileFirstParent = Tools.GetProfileById(cardFirstParentId, _methodId);
            Profiles profileSecondParent = Tools.GetProfileById(cardSecondParentId, _methodId);

            var child = profileChild.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            var parentFirst = profileFirstParent.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            var parentSecond = profileSecondParent.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");

            var commonLocuses = child.Intersect(parentFirst, new EqualityLocusById()).Intersect(parentSecond, new EqualityLocusById());
            if (commonLocuses.Count() == 0)
                throw new WFException(ErrType.Message,
                    String.Format(ResourceStudy.locusForThreeNotFound, cardChaildId, cardFirstParentId, cardSecondParentId));

            foreach (Locus loopLocus in commonLocuses)
            {
                LocusContent locusContentProbably = new LocusContent(loopLocus.Name, loopLocus.Name);
                LocusContent locusContentRatio = new LocusContent(loopLocus.Name, loopLocus.Name);

                Card cardChild = new Card(cardChaildId);
                Card cardFirstParent = new Card(cardFirstParentId);
                Card cardSecondParent = new Card(cardSecondParentId);

                Result resultProbably = new Result("result", String.Format(ResourceStudy.result, ""));
                Result resultRatio1 = new Result("resultRatio1", String.Format(ResourceStudy.result, "resultRatio1"));
                Result resultRatio2 = new Result("resultRatio2", String.Format(ResourceStudy.result, "resultRatio2"));
                Formula formulaProbably = new Formula("formula", String.Format(ResourceStudy.formula, "formulaProbably"));
                Formula formulaRatio1 = new Formula("formula1", String.Format(ResourceStudy.formula, "formulaRatio1"));
                Formula formulaRatio2 = new Formula("formula2", String.Format(ResourceStudy.formula, "formulaRatio2"));

                // Находим алелли текущего локуса для ребенка
                var alleliesChild = loopLocus.Allele.Where(n=>n.Checked).Where((n, index) => index < 2);
                var alleliesFirstParent = profileFirstParent[loopLocus.Name].Allele.Where(n=>n.Checked).Where((n, index) => index < 2);
                var alleliesSecondParent = profileSecondParent[loopLocus.Name].Allele.Where(n =>n.Checked).Where((n, index) => index < 2);

                cardChild.AddAllele(alleliesChild.ToArray<Allele>());
                cardFirstParent.AddAllele(alleliesFirstParent.ToArray<Allele>());
                cardSecondParent.AddAllele(alleliesSecondParent.ToArray<Allele>());

                if (loopLocus.IsHomozygotic)
                {
                    resultProbably.Value = Math.Pow(alleliesChild.ElementAt(0).Frequency * (2 - alleliesChild.ElementAt(0).Frequency), 2);
                    resultRatio1.Value = 1;
                    //[P11 * (2 - P11)]^2
                    resultRatio2.Value = Math.Pow(alleliesChild.ElementAt(0).Frequency * (2 - alleliesChild.ElementAt(0).Frequency), 2); 
                }
                else
                {
                    double firstFrequency = alleliesChild.ElementAt(0).Frequency;
                    double secondFrequency = alleliesChild.ElementAt(1).Frequency;
                    resultProbably.Value = 2 * firstFrequency * (2 - firstFrequency) * secondFrequency * (2 - secondFrequency) -
                            Math.Pow(2 * firstFrequency * secondFrequency, 2);
                    resultRatio1.Value = 1;
                    //2*Ра * (2 – Ра) * Рв * (2 – Рв) – (2*Ра*Рв)2  
                    resultRatio2.Value =
                        2 * firstFrequency * (2 - firstFrequency) * secondFrequency * (2 - secondFrequency) -
                        Math.Pow(2 * firstFrequency * secondFrequency, 2);
                }
                // Add probably contents
                locusContentProbably.Result.Add(resultProbably);
                locusContentProbably.Cards.Add(cardChild);
                locusContentProbably.Cards.Add(cardFirstParent);
                locusContentProbably.Cards.Add(cardSecondParent);
                locusContentProbably.Formula.Add(formulaProbably);
                this.probablyResult.LocusContents.Add(locusContentProbably);

                // Add ratio contents
                resultRatio1.Value = 1;
                resultRatio2.Value = resultProbably.Value;
                locusContentRatio.Result.Add(resultRatio1);
                locusContentRatio.Result.Add(resultRatio2);
                locusContentRatio.Cards.Add(cardChild);
                locusContentRatio.Cards.Add(cardFirstParent);
                locusContentRatio.Cards.Add(cardSecondParent);
                locusContentRatio.Formula.Add(formulaRatio1);
                locusContentRatio.Formula.Add(formulaRatio2);
                this.ratioResult.LocusContents.Add(locusContentRatio);
            }

            string resultName = String.Format(ResourceStudy.result, "");
            double resultProbablyValue = this.ProbablyResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "result").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);
            double resultRatio1Value = this.RatioResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "resultRatio1").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);
            double resultRatio2Value = this.RatioResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "resultRatio2").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);

            this.ProbablyResult.Results.Add(new Result(resultName, resultName) { Value = resultProbablyValue });
            this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio1Value });
            this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio2Value });
        
        }

        // Исследование участия в смеси предполагаемого лица
        public virtual void Blend(
            string blendId, string personId,
            string blendName, string personName
            )
        {
            this.Blend(
                Tools.GetIntFromText(blendId, blendName),
                Tools.GetIntFromText(personId, personName)
                );    
        }
        public virtual void Blend(int blendId, int personId)
        {
            this.probablyResult = new ResearchResult();
            this.ratioResult = new ResearchResult();

            Profiles profileBlend = Tools.GetProfileById(blendId, _methodId);
            Profiles profilePerson = Tools.GetProfileById(personId, _methodId);

            var blend = profileBlend.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            var person = profilePerson.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");

            var commonLocuses = blend.Intersect(person, new EqualityLocusById());
            if (commonLocuses.Count() == 0)
                throw new WFException(ErrType.Message,
                    String.Format(ResourceStudy.locusForThreeNotFound, blendId, personId));

            foreach (Locus loopLocus in commonLocuses)
            {
                LocusContent locusContentProbably = new LocusContent(loopLocus.Name, loopLocus.Name);
                LocusContent locusContentRatio = new LocusContent(loopLocus.Name, loopLocus.Name);

                Card cardBlend = new Card(blendId);
                Card cardPerson = new Card(personId);

                Result resultProbably = new Result("result", String.Format(ResourceStudy.result, ""));
                Result resultRatio1 = new Result("resultRatio1", String.Format(ResourceStudy.result, "resultRatio1"));
                Result resultRatio2 = new Result("resultRatio2", String.Format(ResourceStudy.result, "resultRatio2"));
                Formula formulaProbably = new Formula("formula", String.Format(ResourceStudy.formula, "formulaProbably"));
                Formula formulaRatio1 = new Formula("formula1", String.Format(ResourceStudy.formula, "formulaRatio1"));
                Formula formulaRatio2 = new Formula("formula2", String.Format(ResourceStudy.formula, "formulaRatio2"));

                // Находим аллели текущего локуса
                var alleliesBlend = loopLocus.Allele.Where(n => n.Checked);
                var alleliesPerson = profilePerson[loopLocus.Name].Allele.Where(n => n.Checked).Where((n, index) => index < 2);

                cardBlend.AddAllele(alleliesBlend.ToArray<Allele>());
                cardPerson.AddAllele(alleliesPerson.ToArray<Allele>());

                resultProbably.Value = 
                    Math.Pow(alleliesBlend.Select(n=>n.Frequency).Aggregate((curr, next) => curr + next), 2);

                // Add probably contents
                locusContentProbably.Result.Add(resultProbably);
                locusContentProbably.Cards.Add(cardBlend);
                locusContentProbably.Cards.Add(cardPerson);
                locusContentProbably.Formula.Add(formulaProbably);
                this.probablyResult.LocusContents.Add(locusContentProbably);

                // Add ratio contents
                resultRatio1.Value = 1;
                resultRatio2.Value = resultProbably.Value;
                locusContentRatio.Result.Add(resultRatio1);
                locusContentRatio.Result.Add(resultRatio2);
                locusContentRatio.Cards.Add(cardBlend);
                locusContentRatio.Cards.Add(cardPerson);
                locusContentRatio.Formula.Add(formulaRatio1);
                locusContentRatio.Formula.Add(formulaRatio2);
                this.ratioResult.LocusContents.Add(locusContentRatio);
            }

            string resultName = String.Format(ResourceStudy.result, "");
            double resultProbablyValue = this.ProbablyResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "result").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);
            double resultRatio1Value = this.RatioResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "resultRatio1").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);
            double resultRatio2Value = this.RatioResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "resultRatio2").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);

            this.ProbablyResult.Results.Add(new Result(resultName, resultName) { Value = resultProbablyValue });
            this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio1Value });
            this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio2Value });
        }

        // Участие в смеси одного известного и одного предполагаемого лица
        public virtual void TwoBlend(string blendId, string personOneId, string personSecondId, string blendName,
            string personOneName, string personSecondName)
        {
            this.TwoBlend(
                Tools.GetIntFromText(blendId, blendName),
                Tools.GetIntFromText(personOneId, personOneName),
                Tools.GetIntFromText(personSecondId, personSecondName)
                );
        }
        public virtual void TwoBlend(int blendId, int personOneId, int personSecondId)
        {
            this.probablyResult = new ResearchResult();
//            this.ratioResult = new ResearchResult();

            Profiles profileBlend = Tools.GetProfileById(blendId, _methodId);
            Profiles profilePersonOne = Tools.GetProfileById(personOneId, _methodId);
            Profiles profilePersonSecond = Tools.GetProfileById(personSecondId, _methodId);

            var blend = profileBlend.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            var personOne = profilePersonOne.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            var personSecond = profilePersonSecond.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");

            // Проверка смеси на происхождение от 2 лиц (не более 4 аллелей в любом из локусов)
            if (blend.Where(n=> n.CheckedAlleleCount > 4).Count() > 0)
                throw new WFException(ErrType.Message, String.Format(ResourceStudy.Ik2Gh2, 
                    blend.Where(n=> n.CheckedAlleleCount > 4).ElementAt(0).Name));

            var commonLocuses = blend.Intersect(personOne, new EqualityLocusById());
            if (commonLocuses.Count() == 0)
                throw new WFException(ErrType.Message,
                    String.Format(ResourceStudy.locusForThreeNotFound, blendId, personOneId, personSecondId));

            foreach (Locus loopLocus in commonLocuses)
            {
                LocusContent locusContentProbably = new LocusContent(loopLocus.Name, loopLocus.Name);
//                LocusContent locusContentRatio = new LocusContent(loopLocus.Name, loopLocus.Name);

                Card cardBlend = new Card(blendId);
                Card cardPersonOne = new Card(personOneId);
                Card cardPersonSecond = new Card(personSecondId);

                Result resultProbably = new Result("result", String.Format(ResourceStudy.result, ""));
//                Result resultRatio1 = new Result("resultRatio1", String.Format(ResourceStudy.result, "resultRatio1"));
//                Result resultRatio2 = new Result("resultRatio2", String.Format(ResourceStudy.result, "resultRatio2"));
                Formula formulaProbably = new Formula("formula", String.Format(ResourceStudy.formula, "formulaProbably"));
//                Formula formulaRatio1 = new Formula("formula1", String.Format(ResourceStudy.formula, "formulaRatio1"));
//                Formula formulaRatio2 = new Formula("formula2", String.Format(ResourceStudy.formula, "formulaRatio2"));

                // Находим аллели текущего локуса
                var alleliesBlend = loopLocus.Allele.Where(n => n.Checked);
                var alleliesPersonOne = profilePersonOne[loopLocus.Name].Allele.Where(n => n.Checked).Where((n, index) => index < 2);
                var alleliesPersonSecond = profilePersonSecond[loopLocus.Name].Allele.Where(n => n.Checked).Where((n, index) => index < 2);

                cardBlend.AddAllele(alleliesBlend.ToArray<Allele>());
                cardPersonOne.AddAllele(alleliesPersonOne.ToArray<Allele>());
                cardPersonSecond.AddAllele(alleliesPersonSecond.ToArray<Allele>());

                var restAllele = alleliesBlend.Except(alleliesPersonOne, new EqualityAlleleById());
                double value1;
                double value2;
                double value3;

                switch (restAllele.Count())
                {
                    case 0:
                        value1 = alleliesBlend.ElementAt(0).Frequency;
                        value2 = alleliesBlend.ElementAt(1).Frequency;
                        resultProbably.Value = Math.Pow(value1, 2) + Math.Pow(value2, 2) + 2 * value1 * value2;
                        formulaProbably.Value = String.Format(ResourceStudy.formulaTwoBleand1, alleliesBlend.ElementAt(0).Value, alleliesBlend.ElementAt(1).Value);
                        break;
                    case 1:
                        switch (alleliesBlend.Count())
                        {
                            case 2:
                                value1 = restAllele.ElementAt(0).Frequency;
                                value2 = alleliesBlend.Except(restAllele, new EqualityAlleleById()).ElementAt(0).Frequency;
                                resultProbably.Value = Math.Pow(value1, 2) + 2 * value1 * value2;
                                formulaProbably.Value = String.Format(ResourceStudy.formulaTwoBleand2, restAllele.ElementAt(0).Value,
                                    alleliesBlend.Except(restAllele, new EqualityAlleleById()).ElementAt(0).Value);
                                break;
                            case 3:
                                var v = alleliesBlend.Except(restAllele, new EqualityAlleleById());
                                value1 = restAllele.ElementAt(0).Frequency;
                                value2 = v.ElementAt(0).Frequency;
                                value3 = v.ElementAt(1).Frequency;
                                resultProbably.Value = Math.Pow(value1, 2) + 2 * value1 * value2 + 2 * value1 * value3;
                                formulaProbably.Value = String.Format(ResourceStudy.formulaTwoBleand3, restAllele.ElementAt(0).Value,
                                    v.ElementAt(0).Value, v.ElementAt(1).Value);
                                break;
                            default:
                                throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorTwoBlend1, 
                                    1, 2, 3));
                        }
                        break;
                    case 2:
                        switch (alleliesBlend.Count())
                        {
                            case 3:
                            case 4:
                                value1 = restAllele.ElementAt(0).Frequency;
                                value2 = restAllele.ElementAt(1).Frequency;
                                resultProbably.Value = 2 * value1 * value2;
                                formulaProbably.Value = String.Format(ResourceStudy.formulaTwoBleand4, restAllele.ElementAt(0).Value,
                                    restAllele.ElementAt(1).Value);
                                break;
                            default:
                                throw new WFException(ErrType.Message, String.Format(ErrorsMsg.ErrorTwoBlend1,
                                    2, 3, 4));
                        }
                        break;
                    default:
                        break;
                }

                // Add probably contents
                locusContentProbably.Result.Add(resultProbably);
                locusContentProbably.Cards.Add(cardBlend);
                locusContentProbably.Cards.Add(cardPersonOne);
                locusContentProbably.Cards.Add(cardPersonSecond);
                locusContentProbably.Formula.Add(formulaProbably);
                this.probablyResult.LocusContents.Add(locusContentProbably);

                //// Add ratio contents
                //resultRatio1.Value = 1;
                //resultRatio2.Value = resultProbably.Value;
                //locusContentRatio.Result.Add(resultRatio1);
                //locusContentRatio.Result.Add(resultRatio2);
                //locusContentRatio.Cards.Add(cardBlend);
                //locusContentRatio.Cards.Add(cardPerson);
                //locusContentRatio.Formula.Add(formulaRatio1);
                //locusContentRatio.Formula.Add(formulaRatio2);
                //this.ratioResult.LocusContents.Add(locusContentRatio);
            }

            string resultName = String.Format(ResourceStudy.result, "");
            double resultProbablyValue = this.ProbablyResult.LocusContents.
                Select(n => n.Result.Where(r => r.Name == "result").ElementAt(0).Value).
                Aggregate((curr, next) => curr * next);
            //double resultRatio1Value = this.RatioResult.LocusContents.
            //    Select(n => n.Result.Where(r => r.Name == "resultRatio1").ElementAt(0).Value).
            //    Aggregate((curr, next) => curr * next);
            //double resultRatio2Value = this.RatioResult.LocusContents.
            //    Select(n => n.Result.Where(r => r.Name == "resultRatio2").ElementAt(0).Value).
            //    Aggregate((curr, next) => curr * next);

            this.ProbablyResult.Results.Add(new Result(resultName, resultName) { Value = resultProbablyValue });
            //this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio1Value });
            //this.RatioResult.Results.Add(new Result(resultName, resultName) { Value = resultRatio2Value });
        }
        
        // Properties
        public virtual ResearchResult ProbablyResult { get { return probablyResult; } }
        public virtual ResearchResult RatioResult { get { return ratioResult; } }
        public int MethodId { get { return _methodId; } }

        // Fields
        private ResearchResult probablyResult;
        private ResearchResult ratioResult;
        private int _methodId = GeneLibraryConst.DefaultMethod; 
    }

    class ResearchDecorator : Research
    {
        // Constructors
        public ResearchDecorator(Research research, int methodId) : base(methodId)
        {
            this.research = research;   
        }

        public ResearchDecorator(Research research)
        {
            // TODO: Complete member initialization
            this.research = research;
        }

        // Interface
        public override void DirectIdent(int cardId)
        {
            research.DirectIdent(cardId);
        }
        public override void DirectIdent(string cardId, string fildName)
        {
            research.DirectIdent(cardId, fildName);
        }
        public override void OneChildAndParent(int cardChildId, int cardParentId)
        {
            research.OneChildAndParent(cardChildId, cardParentId);
        }
        public override void OneChildAndParent(string cardChildId, string cardParentId, string fildChildName, string fildParentName)
        {
            research.OneChildAndParent(cardChildId, cardParentId, fildChildName, fildParentName);
        }
        public override void TwoParent(int cardChildId, int cardKnownParentId, int cardUnknownParentId)
        {
            research.TwoParent(cardChildId, cardKnownParentId, cardUnknownParentId);
        }
        public override void TwoParent(
            string cardChildId, string cardKnownParentId, string cardUnknownParentId,
            string fieldChildName, string fieldKnownParentName, string fieldUnknownParentName
            )
        {
            research.TwoParent(cardChildId, cardKnownParentId, cardUnknownParentId, fieldChildName, fieldKnownParentName, fieldUnknownParentName); 
        }
        public override void SuppParent(int cardChaildId, int cardFirstParentId, int cardSecondParentId)
        {
            research.SuppParent(cardChaildId, cardFirstParentId, cardSecondParentId);
        }
        public override void SuppParent(string cardChaildId, string cardFirstParentId, string cardSecondId, string cardChaildName, string cardFirstParentName, string cardSecondName)
        {
            research.SuppParent(cardChaildId, cardFirstParentId, cardSecondId, cardChaildName, cardFirstParentName, cardSecondName);
        }
        public override void Blend(int blendId, int personId)
        {
            research.Blend(blendId, personId);
        }
        public override void Blend(string blendId, string personId, string blendName, string personName)
        {
            research.Blend(blendId, personId, blendName, personName);
        }
        public override void TwoBlend(string blendId, string personOneId, string personSecondId, string blendName,
            string personOneName, string personSecondName)
        {
            research.TwoBlend(blendId, personOneId, personSecondId, blendName, personOneName, personSecondName);
        }
        public override void TwoBlend(int blendId, int personOneId, int personSecondId)
        {
            research.TwoBlend(blendId, personOneId, personSecondId);
        }

        // Properties
        public override ResearchResult ProbablyResult
        {
            get
            {
                return this.research.ProbablyResult;
            }
        }
        public override ResearchResult RatioResult
        {
            get
            {
                return this.research.RatioResult;
            }
        }
        
        // Fields
        private Research research;
    }

    class EqualityAlleleById : IEqualityComparer<Allele>
    {
        public bool Equals(Allele allele, Allele withAllele)
        {
            return allele.Id.Equals(withAllele.Id);
        }
        public int GetHashCode(Allele obj)
        {
            return obj.Id.GetHashCode();
        }
    }
    class EqualityLocusById : IEqualityComparer<Locus>
    {
        public bool Equals(Locus locus, Locus withLocus)
        {
            return locus.Id.Equals(withLocus.Id);
        }
        public int GetHashCode(Locus obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
