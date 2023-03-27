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
            int PHOTO_LEFT_X = 184;
            int PHOTO_TOP_Y = 215;
            int PHOTO_RIGHT_X = 486;
            int PHOTO_BOTTOM_Y = 517;
            int COMPANY_NAME_Y = 150;
            int EMPLOYEE_NAME_Y = 600;
            int EMPLOYEE_ID_Y = 730;
            SKPaint paint = new SKPaint();
            paint.TextSize = 42.0f;
            paint.IsAntialias = true;
            paint.Color = SKColors.White;
            paint.IsStroke = false;
            paint.TextAlign = SKTextAlign.Center;
            paint.Typeface = SKTypeface.FromFamilyName("Arial");
            using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    // TODO: wrap in try/catch block
                    // awaits the photo from http request, creates a new SKImage out of that data
                    SKImage employeePhoto = SKImage.FromEncodedData(await client.GetStreamAsync(employees[i].GetPhotoURL()));

                    // creates a badge template out of badge.png
                    SKImage badgeTemplate = SKImage.FromEncodedData(File.OpenRead("badge.png"));

                    // creates a badge bitmap using the dimensions of the badge.png
                    SKBitmap badge = new SKBitmap(BADGE_WIDTH, BADGE_HEIGHT);

                    // creates a new canvas wrapper around the bitmap so we can draw to it
                    SKCanvas canvas = new SKCanvas(badge);

                    // draws the template image taken from http request for the badge, using SKRect to match the entire dimensions of the image
                    canvas.DrawImage(badgeTemplate, new SKRect(0, 0, BADGE_WIDTH, BADGE_HEIGHT));

                    // draws the employee photo at the designated coordinates on the canvas, using SKRect to specify the size of the photo
                    canvas.DrawImage(employeePhoto, new SKRect(PHOTO_LEFT_X, PHOTO_TOP_Y, PHOTO_RIGHT_X, PHOTO_BOTTOM_Y));

                    // draws company name on canvas
                    canvas.DrawText(employees[i].GetCompanyName(), BADGE_WIDTH / 2f, COMPANY_NAME_Y, paint);

                    // updates text color to be black for any following code
                    paint.Color = SKColors.Black;

                    // draws employee name on canvas
                    canvas.DrawText(employees[i].GetFullName(), BADGE_WIDTH / 2f, EMPLOYEE_NAME_Y, paint);

                    // updates to courier for all following code
                    paint.Typeface = SKTypeface.FromFamilyName("Courier New");

                    // draws the text for employee's ID to the canvas
                    canvas.DrawText(employees[i].GetId().ToString(), BADGE_WIDTH / 2f, EMPLOYEE_ID_Y, paint);

                    // creates a new SKImage out of the formatted bitmap
                    SKImage finalImg = SKImage.FromBitmap(badge);

                    // encodes the SKImage as a PNG into data (PNG is default if no params passed to Encode())
                    SKData data = finalImg.Encode();

                    // writes the PNG to fs at designated location
                    data.SaveTo(File.OpenWrite($"data/{employees[i].GetId()}_badge.png"));
                }
            }
        }
    }
}