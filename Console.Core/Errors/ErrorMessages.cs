using System.Collections.Generic;
public static class ErrorMessages
{
    public static dictionary<string, string> ErrorList = new Dictionary<string, string>
    {
        { 1001, "Name must not be null." },
        { 1002, "Name must not be empty." },
        { 1003, "Name must not be empty." },
        { 1004, "Age must be between 18 and 65." },
        { 1005, "Age must be numeric" },
        { 1006, "Salary must not be null." },
        { 1007, "Salary must be a valid decimal." },
        { 1008, "File path must not be null." },
        { 1009, "File path must not be empty." },
        { 1010, "File path must be a valid path." }
    };
}