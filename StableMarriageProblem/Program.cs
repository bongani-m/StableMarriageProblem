using System;
using System.Collections.Generic;
using System.Linq;

namespace StableMarriageProblem
{
    class Program
    {
        static void Main(string[] args)
        {

            // Created people
            Matcher m1 = new Matcher { Name = "Joe", MaxChosen = 1 };
            Matcher m2 = new Matcher { Name = "Jacob", MaxChosen = 1 };
            Matcher m3 = new Matcher { Name = "John", MaxChosen = 1 };

            Matcher f1 = new Matcher { Name = "Kate", MaxChosen = 2 };
            Matcher f2 = new Matcher { Name = "Kathy", MaxChosen = 1 };
            Matcher f3 = new Matcher { Name = "Karen", MaxChosen = 1 };

            // Create list of men and women
            var men = new List<Matcher> { m1, m2, m3 };
            var women = new List<Matcher> { f1, f2, f3 };

            // people picked
            m1.SetChoices( new List<Matcher> { f1, f2, f3 });
            m2.SetChoices( new List<Matcher> { f2, f1, f3 });
            m3.SetChoices(new List<Matcher> { f1, f2, f3 }) ;

            f1.SetChoices ( new List<Matcher> { m1, m2, m3 });
            f2.SetChoices ( new List<Matcher> { m1, m2, m3 });
            f3.SetChoices ( new List<Matcher> { m3, m2, m1 });


            //Create a filter with connects
            Matcher.Match(men, women);
           
            foreach(var man in men)
            {
                var match = man.Choice[0];
                Console.WriteLine("{0} matched with {1}, {0} got their rank: {2}, {1} got their rank: {3}", man.Name, match.Name, match.Rank[man.Name], man.Rank[match.Name]);
                foreach (Matcher matchee in man.Choice)
                {
                    Console.WriteLine("{0} matched with {1}", man.Name, matchee);
                }
            }

            foreach (var man in women)
            {
                Matcher match;
               if(man.Choice.Count != 0)
                {
                    match = man.Choice[0];
                    Console.WriteLine("{0} matched with {1}, {0} got their rank: {2}, {1} got their rank: {3}", man.Name, match.Name, match.Rank[man.Name], man.Rank[match.Name]);
                    foreach (Matcher matchee in man.Choice)
                    {
                        Console.WriteLine("{0} matched with {1}", man.Name, matchee);
                    }
                }
                
               
            }


            Console.ReadLine();
        }

        public static void stableMarriage(List<Person> men, List<Person> women)
        {
            while (!witoutSpouse(men))
            {
                foreach (Person man in men)
                {
                    if (man.Spouse == null)
                    {
                        var woman = man.ChoicesRemaining.Dequeue();
                        if (woman.Spouse == null)
                        {
                            man.Married(woman);
                        }
                        else
                        {
                            if (woman.CheckRanking(man))
                            {
                                woman.Spouse.Spouse = null;
                                man.Married(woman);
                            }
                        }
                    }

                }
            }
        }


        public static bool witoutSpouse(List<Person> men)
        {
            foreach(Person man in men)
            {
                if(man.Spouse == null)
                {
                    return false;
                }
            }
            return true;
        }
    }

   
        class Person
    {
        public string Name { get; set; }
        public Person Spouse { get; set; }
        public Queue<Person> ChoicesRemaining { get; set; }
        private List<Person> Choices;
        public Dictionary<string, int> Rank;
        public Person()
        {
            ChoicesRemaining = new Queue<Person>();
            Rank = new Dictionary<string, int>();
        }
        public override string ToString()
        {
            return String.Format("{0} married {1}: Spouse Ranking: {2}, Spouses Ranking of Them {3}", Name, Spouse.Name, Spouse.Rank[this.Name], Rank[Spouse.Name] );
        }

        public void Married(Person spouse)
        {
            Spouse = spouse;
            spouse.Spouse = this;
        }

       public void SetChoices(List<Person> choices)
        {
            Choices = choices;
            for(var i = 0; i < choices.Count; i++)
            {
                var person = choices[i];
                person.Rank.Add(this.Name, i + 1);
                ChoicesRemaining.Enqueue(person);
            }
        }

        public bool CheckRanking(Person mate)
        {
            return (mate.Rank[this.Name] < Spouse.Rank[this.Name]);
        }
        
    }
}


/*
 Problems
 - Uneven
    -> Only do people who interviewed and keep a list instead of 1 spouse 
     
     */