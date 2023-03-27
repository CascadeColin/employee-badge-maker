using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CatWorx.BadgeMaker
{
    class PeopleFetcher
    {
        // using a HttpClient... contains the memory of the async stream to that code block
        // Call API to generate a random user
        // Store fetched data using a constructor (converting JSON to C# datatypes)
        // create methods for manipulating constructed data, if necessary
        // create a badge for each fetched user
        public static List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            while (true)
            {
                Console.WriteLine("Enter first name: (leave empty to exit)");
                string firstName = Console.ReadLine() ?? "";
                if (firstName == "")
                {
                    break;
                }
                Console.Write("Enter last name: ");
                string lastName = Console.ReadLine() ?? "";
                Console.Write("Enter employee ID: ");
                int id = Int32.Parse(Console.ReadLine() ?? "");
                Console.Write("Enter Photo URL: ");
                string photoURL = Console.ReadLine() ?? "";
                Employee currentEmployee = new Employee(firstName, lastName, id, photoURL);
                employees.Add(currentEmployee);
            }
            return employees;
        }
        async public static Task<List<Employee>> GetFromApi()
        {
            List<Employee> employees = new List<Employee>();
            using (HttpClient api = new HttpClient())
            {
                string response = await api.GetStringAsync("https://randomuser.me/api/?results=10&nat=us&inc=name,id,picture");
                JObject json = JObject.Parse(response);
                var employeeList = json["results"];
                if (employeeList is not null)
                {
                    foreach (JToken token in employeeList!)
                    {
                        Employee emp = new Employee
                        (
                            token.SelectToken("name.first")!.ToString(),
                            token.SelectToken("name.last")!.ToString(),
                            Int32.Parse(token.SelectToken("id.value")!.ToString().Replace("-", "")),
                            token.SelectToken("picture.large")!.ToString()
                        );
                        employees.Add(emp);
                    }
                }
                else
                {
                    Console.WriteLine("Data not found - API may be down");
                }
            }
            return employees;
        }
    }
}