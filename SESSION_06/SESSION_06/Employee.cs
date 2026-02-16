using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_06
{
    public enum SecurityLevel
    {
        Guest,
        Developer,
        Secretary,
        DBA,
        SecurityOfficer
    }

    internal class Employee
    {
        private int _id;
        private string _name;
        private SecurityLevel _securityLevel;
        private decimal _salary;
        private HireDate _hireDate;
        private char _gender;

        // Properties
        public int Id 
        { 
            get { return _id; }
            set { _id = value; }
        }

        public string Name 
        { 
            get { return _name; }
            set { _name = value ?? string.Empty; }
        }

        public SecurityLevel SecurityLevel 
        { 
            get { return _securityLevel; }
            set { _securityLevel = value; }
        }

        public decimal Salary 
        { 
            get { return _salary; }
            set 
            { 
                if (value >= 0)
                    _salary = value;
                else
                    _salary = 0;
            }
        }

        public HireDate HireDate 
        { 
            get { return _hireDate; }
            set { _hireDate = value; }
        }

        public char Gender 
        { 
            get { return _gender; }
            set 
            { 
                // Restrict to M or F only
                char upperValue = char.ToUpper(value);
                if (upperValue == 'M' || upperValue == 'F')
                    _gender = upperValue;
                else
                    throw new ArgumentException("Gender must be 'M' (Male) or 'F' (Female)");
            }
        }

        // Default Constructor
        public Employee()
        {
            _id = 0;
            _name = string.Empty;
            _securityLevel = SecurityLevel.Guest;
            _salary = 0;
            _hireDate = new HireDate(1, 1, 2000);
            _gender = 'M';
        }

        // Parameterized Constructor
        public Employee(int id, string name, SecurityLevel securityLevel, decimal salary, HireDate hireDate, char gender)
        {
            this._id = id;
            this._name = name ?? string.Empty;
            this._securityLevel = securityLevel;
            this.Salary = salary; // Use property for validation
            this._hireDate = hireDate;
            this.Gender = gender; // Use property for validation
        }

        // Parameterized Constructor with validation
        public Employee(int id, string name, SecurityLevel securityLevel, decimal salary, int day, int month, int year, char gender)
        {
            this._id = id;
            this._name = name ?? string.Empty;
            this._securityLevel = securityLevel;
            this.Salary = salary; // Use property for validation
            this._hireDate = new HireDate(day, month, year);
            this.Gender = gender; // Use property for validation
        }

        // Override ToString to display employee data with currency format
        public override string ToString()
        {
            return String.Format("Employee ID: {0}\nName: {1}\nSecurity Level: {2}\nSalary: {3:C}\nHire Date: {4}\nGender: {5}",
                Id, 
                Name, 
                SecurityLevel, 
                Salary, 
                HireDate.GetHireDate(), 
                Gender == 'M' ? "Male" : "Female");
        }
    }
}
