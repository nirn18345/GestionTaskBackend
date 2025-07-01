using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Utils
{
    public static class ResponseMessage
    {
        public const string LoginSuccess = "Login successful";
        public const string InvalidCredentials = "Invalid email or password";


        public const string EmailNull = "Email field is required";
        public const string PasswordNull = "Password field is required";

        public const string AccountExist = "The entered account already exists";
        public const string AccountCreated = "Account successfully created";
        public const string EmailAccountNull = "Email field is required";
        public const string PassworAccountdNull = "Password field is required";

        // User
        public const string UserIdRequired = "The UserId field is required.";
        public const string FirstNameRequired = "The First Name field is required.";
        public const string LastNameRequired = "The Last Name field is required.";
        public const string DniRequired = "The DNI field is required.";
        public const string DniInvalid = "The DNI must contain only numbers and follow the correct format.";
        public const string PhoneInvalid = "The entered phone number is not valid.";
        public const string AccountIdRequired = "The account identifier is required.";
        public const string RoleIdRequired = "The user role is required.";


        public const string UserList = "User list retrieved successfully";
        public const string UserFound = "User found successfully";

        public const string UserExist = "A user with the provided information already exists.";
        public const string UserNotFound = "User not found";
        public const string UserCreated = "User successfully created.";
        public const string UserDelete = "User successfully deleted.";
        public const string UserUpdate = "User successfully updated.";
        public const string DniAlreadyExists = "The entered DNI is already associated with another user";

        // Role
        public const string RolExist = "A role with the provided information already exists.";
        public const string RolCreated = "Role successfully created.";
        public const string RolRequired = "The RoleName field is required.";

        // Task

        public const string TaskList = "Task list retrieved successfully";
        public const string TaskFound = "Task found successfully";
        public const string TaskNotFound = "Task not found";

        public const string TaskExist = "A task with the same information already exists.";
        public const string TaskCreated = "Task successfully created.";
        public const string TaskUpdate = "Task successfully updated.";
        public const string TaskDelete = "Task successfully deleted.";
        public const string TasktitleRequired = "The Title field is required.";



    }
}
