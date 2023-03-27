namespace CatWorx.BadgeMaker
{
    class Employee
    {
        private string FirstName;
        private string LastName;
        private int Id;
        private string PhotoURL;
        public Employee(string firstName, string lastName, int id, string photoURL)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = id;
            PhotoURL = photoURL;
        }
        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
        public int GetId()
        {
            return Id;
        }
        public string GetPhotoURL()
        {
            return PhotoURL;
        }
        public string GetCompanyName()
        {
            return "CatWorx";
        }
    }
}