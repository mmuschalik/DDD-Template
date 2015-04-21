using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class Person
    {
        public readonly string FirstName;
        public readonly string SurName;

        public Person(string firstName, string surName)
        {
            Ensure.NotNullOrEmpty(firstName, "firstName");
            Ensure.NotNullOrEmpty(surName, "surName");

            FirstName = firstName;
            SurName = surName;
        }

        public string GetFullName()
        {
            return FirstName + " " + SurName;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Person))
                return false;

            var p = obj as Person;

            return p.FirstName == FirstName && p.SurName == SurName;
        }

        public override int GetHashCode()
        {
            return (FirstName + SurName).GetHashCode();
        }
    }
}
