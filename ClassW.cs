using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1_AsadullinaD.D._БПИ_23_01
{
    public class ClassW
    {
        public string Surname { get; }
        public decimal Salary { get; }
        public int YearOfHire { get; }

        public ClassW (string surname, decimal salary, int yearOfHire)
        {
            Surname = surname;
            Salary = salary;
            YearOfHire = yearOfHire;
        }

        // Метод 1: стаж в годах
        public int GetExperience()
        {
            DateTime today = DateTime.Today;
            DateTime hireDate = new DateTime(YearOfHire, 1, 1);

            int years = today.Year - hireDate.Year;
            if (today < hireDate.AddYears(years))
                years--;

            return years < 0 ? 0 : years;
        }

        // Метод 2: дни с момента поступления
        public int GetDaysSinceHire()
        {
            DateTime today = DateTime.Today;
            DateTime hireDate = new DateTime(YearOfHire, 1, 1);

            int days = (today - hireDate).Days;
            return days < 0 ? 0 : days;
        }
    }
}
