namespace GatewayWithTokenAuth
{
    using System.Collections.Generic;

    public class UserCollection
    {
        public static ISet<LoginSpecification> Users { get; }

        static UserCollection()
        {
            Users = new HashSet<LoginSpecification>();
            Users.Add(new LoginSpecification() { Login = "maciek", Password = "qwe123" });
            Users.Add(new LoginSpecification() { Login = "kulfon", Password = "asd" });
        }
    }

    public class LoginSpecification
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            var specification = obj as LoginSpecification;
            return specification != null && Equals(specification);
        }

        protected bool Equals(LoginSpecification other)
        {
            return string.Equals(Login, other.Login) && string.Equals(Password, other.Password);
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Login != null ? Login.GetHashCode() : 0) * 397) ^ (Password != null ? Password.GetHashCode() : 0);
            }
        }
    }
}
