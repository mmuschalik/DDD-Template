using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class Beneficiary
    {
        public readonly Person Person;
        public readonly decimal Allocation;

        public Beneficiary(Person person, decimal allocation)
        {
            Ensure.IsPercentage(allocation, "allocation");

            Person = person;
            Allocation = allocation;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Beneficiary))
                return false;

            var b = obj as Beneficiary;

            return Person.FirstName == b.Person.FirstName && Person.SurName == b.Person.SurName;
        }

        public override int GetHashCode()
        {
            return (Person.FirstName + Person.SurName).GetHashCode();
        }
    }
}
