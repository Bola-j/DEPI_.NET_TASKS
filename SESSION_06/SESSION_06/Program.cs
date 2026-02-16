namespace SESSION_06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create an array of 3 employees
            Employee[] EmpArr = new Employee[3];

            try
            {
                // Employee 1: DBA
                EmpArr[0] = new Employee(
                    id: 101,
                    name: "Ahmed Mohamed",
                    securityLevel: SecurityLevel.DBA,
                    salary: 15000m,
                    day: 15,
                    month: 3,
                    year: 2020,
                    gender: 'M'
                );

                // Employee 2: Guest
                EmpArr[1] = new Employee(
                    id: 102,
                    name: "Sara Ali",
                    securityLevel: SecurityLevel.Guest,
                    salary: 5000m,
                    day: 20,
                    month: 7,
                    year: 2023,
                    gender: 'F'
                );

                // Employee 3: Security Officer with full permissions
                EmpArr[2] = new Employee(
                    id: 103,
                    name: "Omar Hassan",
                    securityLevel: SecurityLevel.SecurityOfficer,
                    salary: 18000m,
                    day: 10,
                    month: 1,
                    year: 2019,
                    gender: 'M'
                );

                // Display all employees
                Console.WriteLine("========== EMPLOYEE INFORMATION ==========\n");
                
                for (int i = 0; i < EmpArr.Length; i++)
                {
                    Console.WriteLine($"Employee #{i + 1}");
                    Console.WriteLine(EmpArr[i].ToString());
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine();
                }

                // Example of runtime error prevention
                Console.WriteLine("\n========== TESTING GENDER VALIDATION ==========");
                try
                {
                    Employee invalidEmployee = new Employee(
                        id: 104,
                        name: "Test User",
                        securityLevel: SecurityLevel.Developer,
                        salary: 10000m,
                        day: 1,
                        month: 1,
                        year: 2024,
                        gender: 'X' // Invalid gender
                    );
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error prevented: {ex.Message}");
                }

                // Example of negative salary prevention
                Console.WriteLine("\n========== TESTING SALARY VALIDATION ==========");
                Employee emp = new Employee();
                emp.Id = 105;
                emp.Name = "Test Employee";
                emp.Salary = -5000; // Will be set to 0
                Console.WriteLine($"Employee with negative salary attempt: Salary set to {emp.Salary:C}");

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
