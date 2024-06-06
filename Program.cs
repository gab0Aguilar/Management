using System;


public class ManagementSystem
{
    public class User
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public List<UserGrades> Grades { get; set; } = new List<UserGrades>();
        public List<UserNotes> Notes { get; set; } = new List<UserNotes>();
    }

    public class UserGrades
    {
        public string? Username { get; set; }
        public int Grade { get; set; }
        public string? Title { get; set; }
        public string Subject { get; set; } = "";
    }

    public class UserNotes
    {
        public string? Username { get; set; }
        public string? Content { get; set; }
        public string? Title { get; set; }
        public string Subject { get; set; } = "";
    }

    private static List<User> UserIndex = new List<User>
    {
        new User { Username = "Gabo", Password = "password1", Role = "Profesor" },
        new User { Username = "David", Password = "password2", Role = "Profesor" },
        new User { Username = "Coto", Password = "password3", Role = "Estudiante" },
        new User { Username = "Victoria", Password = "password4", Role = "Estudiante" },
        new User { Username = "Grettel", Password = "password5", Role = "Estudiante" }
    };

    private static int currentUser = -1;
    private static string currentUserRole = "";
    private static readonly List<string> predefinedSubjects = new List<string> { "Math", "Science", "Language" };

    static void Main(string[] args)
    {
        bool exitProgram = false;

        while (!exitProgram)
        {
            Banner("Login");

            bool loggedIn = false;
            int attempts = 0;
            while (!loggedIn && attempts < 3)
            {
                Console.Write("Enter your username: ");
                string username = Console.ReadLine() ?? "";
                Console.Write("Enter your password: ");
                string password = Console.ReadLine() ?? "";
                for (int i = 0; i < UserIndex.Count; i++)
                {
                    if (UserIndex[i].Username == username && UserIndex[i].Password == password)
                    {
                        currentUser = i;
                        currentUserRole = UserIndex[i].Role ?? "";
                        loggedIn = true;
                        break;
                    }
                }

                if (!loggedIn)
                {
                    Console.WriteLine("Incorrect username or password. Please try again.");
                    attempts++;
                }
            }

            if (loggedIn)
            {
                bool logout = false;
                while (!logout)
                {
                    Banner("Main Menu");

                    Console.WriteLine("Menu:");
                    Console.WriteLine("1. Manage Grades");
                    if (currentUserRole == "Profesor")
                    {
                        Console.WriteLine("2. Manage Users");
                    }
                    Console.WriteLine("3. Manage Notes");
                    Console.WriteLine("4. View Profile");
                    Console.WriteLine("5. Logout");

                    Console.Write("\nEnter your choice: ");
                    if (!int.TryParse(Console.ReadLine(), out int choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            ShowGradeOptions();
                            break;
                        case 2:
                            if (currentUserRole == "Profesor")
                            {
                                ShowUserOptions();
                            }
                            else
                            {
                                Console.WriteLine("Access denied. This section is restricted.");
                            }
                            break;
                        case 3:
                            ShowNotesOptions();
                            break;
                        case 4:
                            ViewProfile();
                            break;
                        case 5:
                            logout = true;
                            Console.WriteLine("Logged out successfully.");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("You have exceeded the maximum number of attempts. Login failed.");
            }

            Console.Write("\nDo you want to exit the program? (yes/no): ");
            string input = Console.ReadLine()?.ToLower() ?? "";

            if (input == "yes" || input == "y")
            {
                exitProgram = true;
            }
        }
    }

    static void ShowGradeOptions()
    {
        Banner("Manage Grades");

        bool goBack = false;
        while (!goBack)
        {
            Console.WriteLine("1. Add grades");
            Console.WriteLine("2. List grades");
            Console.WriteLine("3. Remove grades");
            Console.WriteLine("4. Edit grades");
            Console.WriteLine("5. Calculate average");
            Console.WriteLine("6. Back to main menu");

            Console.Write("\nEnter your choice: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddGrades();
                    break;
                case 2:
                    ListGrades();
                    break;
                case 3:
                    RemoveGrades();
                    break;
                case 4:
                    EditGrades();
                    break;
                case 5:
                    CalculateAverage();
                    break;
                case 6:
                    goBack = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddGrades()
    {
        Banner("Add Grades");

        string targetUser = currentUserRole == "Profesor" ? PromptForUsername() : UserIndex[currentUser].Username ?? "";
        Console.Write("Enter the subject (Math, Science, Language): ");
        string subject = Console.ReadLine() ?? "";
        if (!predefinedSubjects.Contains(subject))
        {
            Console.WriteLine("Invalid subject. Please enter one of the predefined subjects (Math, Science, Language).");
            return;
        }
        Console.Write("Enter the grades separated by commas (e.g., 80,70,90): ");
        string gradesInput = Console.ReadLine() ?? "";
        Console.Write("Enter the title for the grades: ");
        string title = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(gradesInput))
        {
            try
            {
                List<int> newGrades = new List<int>(Array.ConvertAll(gradesInput.Split(','), int.Parse));
                var user = UserIndex.FirstOrDefault(u => u.Username == targetUser);
                if (user != null)
                {
                    user.Grades.AddRange(newGrades.Select(g => new UserGrades { Username = targetUser, Grade = g, Subject = subject, Title = title }));
                    Console.WriteLine("Grades added successfully for " + targetUser + ".");
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }
            catch
            {
                Console.WriteLine("Invalid grade format. Please enter numbers separated by commas.");
            }
        }
    }

    static void ListGrades()
    {
        Banner("List Grades");

        string targetUser = currentUserRole == "Profesor" ? PromptForUsername() : UserIndex[currentUser].Username ?? "";
        var user = UserIndex.FirstOrDefault(u => u.Username == targetUser);
        if (user != null && user.Grades.Count > 0)
        {
            Console.WriteLine("\nGrades for " + targetUser + ":");
            foreach (var grade in user.Grades)
            {
                Console.WriteLine($"Title: {grade.Title}, Subject: {grade.Subject}, Grade: {grade.Grade}");
            }
        }
        else
        {
            Console.WriteLine("No grades available for " + targetUser + ".");
        }
    }

    static void RemoveGrades()
    {
        Banner("Remove Grades");

        string targetUser = currentUserRole == "Profesor" ? PromptForUsername() : UserIndex[currentUser].Username ?? "";
        var user = UserIndex.FirstOrDefault(u => u.Username == targetUser);
        if (user != null && user.Grades.Count > 0)
        {
            Console.WriteLine("Current grades for " + targetUser + ":");
            for (int i = 0; i < user.Grades.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Title: {user.Grades[i].Title}, Subject: {user.Grades[i].Subject}, Grade: {user.Grades[i].Grade}");
            }

            Console.Write("Enter the number of the grade you want to remove: ");
            if (int.TryParse(Console.ReadLine(), out int gradeIndex) && gradeIndex > 0 && gradeIndex <= user.Grades.Count)
            {
                user.Grades.RemoveAt(gradeIndex - 1);
                Console.WriteLine("Grade removed successfully.");
            }
            else
            {
                Console.WriteLine("Invalid grade selection. Please enter a valid number within the range.");
            }
        }
        else
        {
            Console.WriteLine("No grades available to remove for " + targetUser + ".");
        }
    }

    static void EditGrades()
    {
        Banner("Edit Grades");

        string targetUser = currentUserRole == "Profesor" ? PromptForUsername() : UserIndex[currentUser].Username ?? "";
        var user = UserIndex.FirstOrDefault(u => u.Username == targetUser);
        if (user != null && user.Grades.Count > 0)
        {
            Console.WriteLine("Current grades for " + targetUser + ":");
            for (int i = 0; i < user.Grades.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Title: {user.Grades[i].Title}, Subject: {user.Grades[i].Subject}, Grade: {user.Grades[i].Grade}");
            }

            Console.Write("Enter the number of the grade you want to edit: ");
            if (int.TryParse(Console.ReadLine(), out int gradeIndex) && gradeIndex > 0 && gradeIndex <= user.Grades.Count)
            {
                Console.Write("Enter the new grade: ");
                if (int.TryParse(Console.ReadLine(), out int newGrade))
                {
                    user.Grades[gradeIndex - 1].Grade = newGrade;
                    Console.WriteLine("Grade updated successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid grade input. Please enter a valid number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid grade selection. Please enter a valid number within the range.");
            }
        }
        else
        {
            Console.WriteLine("No grades available to edit for " + targetUser + ".");
        }
    }

    static void CalculateAverage()
    {
        Banner("Calculate Average");

        string targetUser = currentUserRole == "Profesor" ? PromptForUsername() : UserIndex[currentUser].Username ?? "";
        Console.Write("Enter the subject for which you want to calculate the average (Math, Science, Language): ");
        string subject = Console.ReadLine() ?? "";
        if (!predefinedSubjects.Contains(subject))
        {
            Console.WriteLine("Invalid subject. Please enter one of the predefined subjects (Math, Science, Language).");
            return;
        }

        var user = UserIndex.FirstOrDefault(u => u.Username == targetUser);
        if (user != null && user.Grades.Any(g => g.Subject == subject))
        {
            var gradesForSubject = user.Grades.Where(g => g.Subject == subject).ToList();
            double average = gradesForSubject.Average(g => g.Grade);
            Console.WriteLine($"The average grade for {targetUser} in {subject} is: {average:F2}");
        }
        else
        {
            Console.WriteLine($"No grades available for {targetUser} in {subject}.");
        }
    }

    static void ShowUserOptions()
    {
        Banner("Manage Users");

        bool goBack = false;
        while (!goBack)
        {
            Console.WriteLine("1. Add user");
            Console.WriteLine("2. Remove user");
            Console.WriteLine("3. View users");
            Console.WriteLine("4. Back to main menu");

            Console.Write("\nEnter your choice: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddUser();
                    break;
                case 2:
                    RemoveUser();
                    break;
                case 3:
                    ViewUsers();
                    break;
                case 4:
                    goBack = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddUser()
    {
        Banner("Add User");

        Console.Write("Enter the new username: ");
        string newUser = Console.ReadLine() ?? "";
        Console.Write("Enter the new password: ");
        string newPassword = Console.ReadLine() ?? "";
        Console.Write("Enter the role (Profesor/Estudiante): ");
        string newRole = Console.ReadLine() ?? "";

        if (!string.IsNullOrWhiteSpace(newUser) && !string.IsNullOrWhiteSpace(newPassword) && (newRole == "Profesor" || newRole == "Estudiante"))
        {
            UserIndex.Add(new User { Username = newUser, Password = newPassword, Role = newRole });
            Console.WriteLine("User added successfully.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please try again.");
        }
    }

    static void RemoveUser()
    {
        Banner("Remove User");

        Console.Write("Enter the username to remove: ");
        string removeUser = Console.ReadLine() ?? "";

        var user = UserIndex.FirstOrDefault(u => u.Username == removeUser);
        if (user != null)
        {
            UserIndex.Remove(user);
            Console.WriteLine("User removed successfully.");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    static void ViewUsers()
    {
        Banner("View Users");

        Console.WriteLine("List of users:");
        for (int i = 0; i < UserIndex.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {UserIndex[i].Username} ({UserIndex[i].Role})");
        }
    }

    static void ShowNotesOptions()
    {
        Banner("Manage Notes");

        bool goBack = false;
        while (!goBack)
        {
            Console.WriteLine("1. Add note");
            Console.WriteLine("2. Remove note");
            Console.WriteLine("3. Edit note");
            Console.WriteLine("4. View notes");
            Console.WriteLine("5. Back to main menu");

            Console.Write("\nEnter your choice: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddNote();
                    break;
                case 2:
                    RemoveNote();
                    break;
                case 3:
                    EditNote();
                    break;
                case 4:
                    ViewNotes();
                    break;
                case 5:
                    goBack = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddNote()
    {
        Banner("Add Note");

        string targetUser = currentUserRole == "Profesor" ? PromptForUsername() : UserIndex[currentUser].Username ?? "";
        Console.Write("Enter the subject (Math, Science, Language): ");
        string subject = Console.ReadLine() ?? "";
        if (!predefinedSubjects.Contains(subject))
        {
            Console.WriteLine("Invalid subject. Please enter one of the predefined subjects (Math, Science, Language).");
            return;
        }
        Console.Write("Enter the note title: ");
        string title = Console.ReadLine() ?? "";
        Console.Write("Enter the note content: ");
        string content = Console.ReadLine() ?? "";

        var user = UserIndex.FirstOrDefault(u => u.Username == targetUser);
        if (user != null)
        {
            user.Notes.Add(new UserNotes { Username = targetUser, Title = title, Content = content, Subject = subject });
            Console.WriteLine("Note added successfully.");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    static void RemoveNote()
    {
        Banner("Remove Note");

        string targetUser = currentUserRole == "Profesor" ? PromptForUsername() : UserIndex[currentUser].Username ?? "";
        var user = UserIndex.FirstOrDefault(u => u.Username == targetUser);
        if (user != null && user.Notes.Count > 0)
        {
            Console.WriteLine("Current notes for " + targetUser + ":");
            for (int i = 0; i < user.Notes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {user.Notes[i].Title}");
            }

            Console.Write("Enter the number of the note you want to remove: ");
            if (int.TryParse(Console.ReadLine(), out int noteIndex) && noteIndex > 0 && noteIndex <= user.Notes.Count)
            {
                user.Notes.RemoveAt(noteIndex - 1);
                Console.WriteLine("Note removed successfully.");
            }
            else
            {
                Console.WriteLine("Invalid note selection. Please enter a valid number within the range.");
            }
        }
        else
        {
            Console.WriteLine("No notes available to remove for " + targetUser + ".");
        }
    }

    static void EditNote()
    {
        Banner("Edit Note");

        string targetUser = currentUserRole == "Profesor" ? PromptForUsername() : UserIndex[currentUser].Username ?? "";
        var user = UserIndex.FirstOrDefault(u => u.Username == targetUser);
        if (user != null && user.Notes.Count > 0)
        {
            Console.WriteLine("Current notes for " + targetUser + ":");
            for (int i = 0; i < user.Notes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {user.Notes[i].Title}");
            }

            Console.Write("Enter the number of the note you want to edit: ");
            if (int.TryParse(Console.ReadLine(), out int noteIndex) && noteIndex > 0 && noteIndex <= user.Notes.Count)
            {
                Console.Write("Enter the new title: ");
                string newTitle = Console.ReadLine() ?? "";
                Console.Write("Enter the new content: ");
                string newContent = Console.ReadLine() ?? "";

                user.Notes[noteIndex - 1].Title = newTitle;
                user.Notes[noteIndex - 1].Content = newContent;
                Console.WriteLine("Note updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid note selection. Please enter a valid number within the range.");
            }
        }
        else
        {
            Console.WriteLine("No notes available to edit for " + targetUser + ".");
        }
    }

    static void ViewNotes()
    {
        Banner("View Notes");

        string targetUser = currentUserRole == "Profesor" ? PromptForUsername() : UserIndex[currentUser].Username ?? "";
        var user = UserIndex.FirstOrDefault(u => u.Username == targetUser);
        if (user != null && user.Notes.Count > 0)
        {
            Console.WriteLine("\nNotes for " + targetUser + ":");
            foreach (var note in user.Notes)
            {
                Console.WriteLine($"Title: {note.Title}");
                Console.WriteLine($"Content: {note.Content}\n");
            }
        }
        else
        {
            Console.WriteLine("No notes available for " + targetUser + ".");
        }
    }

    static void ViewProfile()
    {
        Banner("Profile");

        var user = UserIndex[currentUser];
        Console.WriteLine($"Username: {user.Username}");
        Console.WriteLine($"Role: {user.Role}");
    }

    static string PromptForUsername()
    {
        Console.Write("Enter the username: ");
        return Console.ReadLine() ?? "";
    }

    static void Banner(string input)
    {
        string text = input;

        int letterSize = 7;
        string[] result = new string[letterSize];

        for (int i = 0; i < letterSize; i++)
        {
            result[i] = "";
        }

        foreach (char character in text)
        {
            int index = GetLetterIndex(character);

            if (index >= 0)
            {
                for (int i = 0; i < letterSize; i++)
                {
                    result[i] += letters[index, i];
                }
            }
            else
            {
                for (int i = 0; i < letterSize; i++)
                {
                    result[i] += "    ";
                }
            }
        }

        foreach (string line in result)
        {
            Console.WriteLine(line);
        }
    }

    static string[,] letters = {
        {
            "  ###   ",
            " #   #  ",
            " #####  ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            "       "
        },
        {
            " ####   ",
            " #   #  ",
            " ####   ",
            " #   #  ",
            " #   #  ",
            " ####   ",
            "       "
        },
        {
            "  ###   ",
            " #   #  ",
            " #      ",
            " #      ",
            " #   #  ",
            "  ###   ",
            "       "
        },
        {
            " ####   ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            " ####   ",
            "       "
        },
        {
            " #####  ",
            " #      ",
            " #####  ",
            " #      ",
            " #      ",
            " #####  ",
            "       "
        },
        {
            " #####  ",
            " #      ",
            " #####  ",
            " #      ",
            " #      ",
            " #      ",
            "       "
        },
        {
            "  ###   ",
            " #      ",
            " #  ##  ",
            " #   #  ",
            " #   #  ",
            "  ###   ",
            "       "
        },
        {
            " #   #  ",
            " #   #  ",
            " #####  ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            "       "
        },
        {
            "  ###   ",
            "   #    ",
            "   #    ",
            "   #    ",
            "   #    ",
            "  ###   ",
            "       "
        },
        {
            " #####  ",
            "    #   ",
            "    #   ",
            "    #   ",
            " #  #   ",
            "  ##    ",
            "       "
        },
        {
            " #   #  ",
            " #  #   ",
            " ###    ",
            " #  #   ",
            " #   #  ",
            " #    # ",
            "       "
        },
        {
            " #      ",
            " #      ",
            " #      ",
            " #      ",
            " #      ",
            " #####  ",
            "       "
        },
        {
            " #   #  ",
            " ## ##  ",
            " # # #  ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            "       "
        },
        {
            " #   #  ",
            " #   #  ",
            " ##  #  ",
            " # # #  ",
            " #  ##  ",
            " #   #  ",
            "       "
        },
        {
            "  ###   ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            "  ###   ",
            "       "
        },
        {
            " ####   ",
            " #   #  ",
            " ####   ",
            " #      ",
            " #      ",
            " #      ",
            "       "
        },
        {
            "  ###   ",
            " #   #  ",
            " #   #  ",
            " # # #  ",
            " #  #   ",
            "  ## #  ",
            "       "
        },
        {
            " ####   ",
            " #   #  ",
            " ####   ",
            " #  #   ",
            " #   #  ",
            " #    # ",
            "       "
        },
        {
            "  ###   ",
            " #      ",
            "  ###   ",
            "     #  ",
            " #   #  ",
            "  ###   ",
            "       "
        },
        {
            " #####  ",
            "   #    ",
            "   #    ",
            "   #    ",
            "   #    ",
            "   #    ",
            "       "
        },
        {
            " #   #  ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            "  ###   ",
            "       "
        },
        {
            " #   #  ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            " #   #  ",
            "  # #   ",
            "       "
        },
        {
            " #   #  ",
            " #   #  ",
            " #   #  ",
            " # # #  ",
            " # # #  ",
            "  # #   ",
            "       "
        },
        {
            " #   #  ",
            " #   #  ",
            "  # #   ",
            "   #    ",
            "  # #   ",
            " #   #  ",
            "       "
        },
        {
            " #   #  ",
            " #   #  ",
            "  # #   ",
            "   #    ",
            "   #    ",
            "   #    ",
            "       "
        },
        {
            " #####  ",
            "    #   ",
            "   #    ",
            "  #     ",
            " #      ",
            " #####  ",
            "       "
        }
    };

    static int GetLetterIndex(char character)
    {
        if (character >= 'A' && character <= 'Z')
        {
            return character - 'A';
        }
        else if (character >= 'a' && character <= 'z')
        {
            return character - 'a';
        }
        else
        {
            return -1;
        }
    }
}
