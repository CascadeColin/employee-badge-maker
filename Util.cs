using SkiaSharp;
using System.Net.Http;
using System.Threading.Tasks;

// Creates a class of utility methods
namespace CatWorx.BadgeMaker
{
    class Util
    {
        public static void PrintEmployees(List<Employee> employees)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                string template = "ID: {0} | Name: {1} | Photo: {2}";
                Console.WriteLine(String.Format(template, employees[i].GetId(), employees[i].GetFullName(), employees[i].GetPhotoURL()));
            }
        }
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
        public static void MakeCSV(List<Employee> employees)
        {
            if (!Directory.Exists("data"))
            {
                Directory.CreateDirectory("data");
            }
            using (StreamWriter file = new StreamWriter("data/employees.csv"))
            {
                file.WriteLine("ID,Name,PhotoURL");
                for (int i = 0; i < employees.Count; i++)
                {
                    string template = "{0},{1},{2}";
                    file.WriteLine(String.Format(template, employees[i].GetId(), employees[i].GetFullName(), employees[i].GetPhotoURL()));
                }
            }
        }
        async public static Task MakeBadges(List<Employee> employees)
        {
            // dimensions of the badge template in pixels (669x1044)
            int BADGE_WIDTH = 669;
            int BADGE_HEIGHT = 1044;
            using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    // TODO: wrap in try/catch block
                    // place employee picture onto the badge template
                    SKImage employeePhoto = SKImage.FromEncodedData(await client.GetStreamAsync(employees[i].GetPhotoURL()));
                    SKImage badgeTemplate = SKImage.FromEncodedData(File.OpenRead("badge.png"));
                    SKBitmap badge = new SKBitmap(BADGE_WIDTH, BADGE_HEIGHT);

                    // SKData data = badgeTemplate.Encode();
                    // data.SaveTo(File.OpenWrite("data/employeeBadge.png"));

                    // write company name in the specified location
                    // write employee name in specified location
                    // write employee ID#
                    // write new png file to data/
                }
            }
        }
    }
}