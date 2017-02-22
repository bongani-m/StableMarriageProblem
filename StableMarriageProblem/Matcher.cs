using System;
using System.Collections.Generic;
using System.Text;

namespace StableMarriageProblem
{
    class Matcher
    {
        public string Name { get; set; }
        public int MaxChosen;
        public List<Matcher> Choice;
        public Queue<Matcher> ChoicesRemaining { get; set; }
        private List<Matcher> Choices;
        public Dictionary<string, int> Rank;


        public Matcher()
        {
            Choice = new List<Matcher>();
            ChoicesRemaining = new Queue<Matcher>();
            Choices = new List<Matcher>();
            Rank = new Dictionary<string, int>();
        }

        //public static void Match(List<Matcher> s1, List<Matcher> s2)
        //{
        //    while (WithoutMatches(s1))
        //    {
        //        foreach (Matcher applicant in s1)
        //        {
        //            if (applicant.Choice.Count < applicant.MaxChosen)
        //            {
        //                var choice = applicant.ChoicesRemaining.Dequeue();
        //                if (choice.Choice.Count == 0)
        //                {
        //                    applicant.Matched(choice);
        //                }
        //                else
        //                {
        //                    if (choice.CheckRanking(applicant))
        //                    {
        //                        Console.WriteLine(applicant);
        //                        applicant.Matched(choice);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //}


        public static void Match(List<Matcher> s1, List<Matcher> s2)
        {
            //2.While some man(m) is free
            while (WithoutMatches(s1))
            {
                foreach (Matcher applicant in s1)
                {
                    if(applicant.Choice.Count < applicant.MaxChosen)
                    {
                        // 2.1 Let w be the first woman on list whom m has not proposed.
                        var choice = applicant.ChoicesRemaining.Dequeue();
                        // 2.2 if w is free
                        if (choice.Choice.Count == 0)
                        {
                            // Assign w to m, they are now a pair
                            applicant.Matched(choice);
                        }
                        // 2.3 else if w is engaged, and w prefers m over her current partner or has room left,
                        else if (choice.CheckRanking(applicant) || choice.Choice.Count < choice.MaxChosen)
                        {
                            // Assign w to m(m, w) are pair now, w's current least rank partner is free
                            applicant.Matched(choice);
                        }
                    }
                }



            }


            
           
        }




        public static bool WithoutMatches(List<Matcher> matchers)
        {
            foreach (Matcher matcher in matchers)
            {
                if (matcher.Choice.Count < matcher.MaxChosen)
                {
                    return true;
                }
            }

            return false;
        }

        public void Matched(Matcher match)
        {
            // add to match
            // if empty add
            if(match.Choice.Count == 0)
            {
                match.Choice.Add(this);
            }
            // if better than current choice place before
            else if (match.CheckRanking(this))
            {
                var count = match.Choice.Count;
                for(var i = 0; i < count; i++)
                {
                    if(this.Rank[match.Name] < match.Choice[i].Rank[match.Name])
                    {
                        match.Choice.Insert(i, this);
                        break;
                    }
                }
            }

            // else add to end
            else
            {
                match.Choice.Add(this);
            }

            // add to self
            // if empty add
            if (this.Choice.Count == 0)
            {
                this.Choice.Add(match);
            }
            // if better than current choice place before
            else if (this.CheckRanking(match))
            {
                var count = this.Choice.Count;
                for (var i = 0; i < count; i++)
                {
                    if (match.Rank[this.Name] < this.Choice[i].Rank[match.Name])
                    {
                        this.Choice.Insert(i, match);
                        break;
                    }
                }
            }
            // else add to end
            else
            {
                this.Choice.Add(match);
            }





        }

        public void SetChoices(List<Matcher> choices)
        {
            Choices = choices;
            for (var i = 0; i < choices.Count; i++)
            {
                var person = choices[i];
                person.Rank.Add(this.Name, i + 1);
                ChoicesRemaining.Enqueue(person);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public bool CheckRanking(Matcher mate)
        {
            var lowest = Choice[Choice.Count - 1];
            return (mate.Rank[this.Name] < lowest.Rank[this.Name]);
        }
    }
}
