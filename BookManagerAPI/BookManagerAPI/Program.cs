using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class Program
{
    static readonly HttpClient client = new HttpClient(); //create new http client in class so it instantiates upon creation of the class
    static readonly string apiURL = "https://caee87266c32804c749e.free.beeceptor.com/books/"; //api link, can be changed to whatever supports JSON CRUD

    public static async Task AddBook() //Add a book to the database/list 
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Enter Title:");
        var titleInput = Console.ReadLine();
        Console.WriteLine("Enter Author:");
        var authorInput = Console.ReadLine();
        Console.WriteLine("Enter Year:");
        int yearInput = int.TryParse(Console.ReadLine(), out int year)? year : 0; //parse string to int

        var book = new { titleInput, authorInput, yearInput }; //create object out of values
        var jsonConverted = JsonSerializer.Serialize(book); //serialize it into json
        var contentConvertedForHttp = new StringContent(jsonConverted, Encoding.UTF8, "application/json");
        //convert it into content that can be sent through http

        try
        {
            var response = await client.PostAsync(apiURL, contentConvertedForHttp); //PostAsync = Create, uses the http-ready value
            Console.WriteLine($"Book Added: {response.StatusCode}"); //status code to notify of progress

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); //error message

        }
        Console.ForegroundColor = ConsoleColor.White;
    }
    public static async Task UpdateBook() //change values to an already added book
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Enter Book ID to Update:");
        var editbookId = Console.ReadLine();
        Console.WriteLine("Enter New Title:");
        var editbookTitle = Console.ReadLine();
        Console.WriteLine("Enter New Author");
        var editbookAuthor = Console.ReadLine();
        Console.WriteLine("Enter New Year:");
        int editbookYear = int.TryParse(Console.ReadLine(), out int year)? year : 0; //parse for string to int

        var editedbook = new { editbookTitle, editbookAuthor, editbookYear }; //create new object using user-submitted values
        var jsonConvertedEdited = JsonSerializer.Serialize(editedbook); //serialize into json
        var httpformattedEdited = new StringContent(jsonConvertedEdited, Encoding.UTF8, "application/json"); //format it into http-sendable content

        try
        {
            var response = await client.PutAsync($"{apiURL}/{editbookId}", httpformattedEdited); //PutAsync = Update
            Console.WriteLine($"Book Edited: {response.StatusCode}"); //status code

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); //error message

        }
        Console.ForegroundColor = ConsoleColor.White;

    }
    public static async Task DeleteBook() //delete book from api database
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Enter Book ID to delete:");
        var deleteBookId = Console.ReadLine(); //get id

        try
        {
            var response = await client.DeleteAsync($"{apiURL}/{deleteBookId}"); //DeleteAsync = Delete, uses api link and id to find book
            Console.WriteLine($"Book Deleted: {response.StatusCode}"); //status code

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); //error message

        }
        Console.ForegroundColor = ConsoleColor.White;
    }
    public static async Task ListAllBooks() //lists all values in the REST API 
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        try
        {
            var response = await client.GetStringAsync(apiURL); //GetStringAsync = Read
            Console.WriteLine("Books:");
            Console.WriteLine(response); //displays all books

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); //error message
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static async Task Main(string[] args)
    {
        bool running = true;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Welcome to Book Manager, select an option:");

        while (running)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Add Book");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("2. Update Book");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("3. Delete Book");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("4. List All Books");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("5. Exit");
            Console.ForegroundColor = ConsoleColor.White;

            var userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    await AddBook();
                    break;
                case "2":
                    await UpdateBook();
                    break;
                case "3":
                    await DeleteBook();
                    break;
                case "4":
                    await ListAllBooks();
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid Entry");
                    break;
            }
        }
    }
}
