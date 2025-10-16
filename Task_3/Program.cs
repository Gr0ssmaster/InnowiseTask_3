﻿using Task_3.Connections;
using Task_3.Interfaces;
using Task_3.Models;
using Task_3.Repositories;

class Program
{
    static void Main(string[] args)
    {
        IDbConnectionFactory connectionFactory = new SqlConnectionFactory();
        if (!CheckConnection(connectionFactory))
        {
            return;
        }
        ITaskRepository taskRepository = new TaskRepository(connectionFactory);
        Manager(taskRepository);
    }
    static void ShowAllTasks(ITaskRepository repo)
    {
        var tasks = repo.GetAllTasks();
        Console.WriteLine("====== Список задач ======\n");

        foreach (var t in tasks)
        {
            Console.WriteLine($"Id: {t.Id}. \nНазвание: {t.Title} \nСтатус: {(t.IsCompleted ? "Выполнена" : "Не выполненна")}" +
                $"\nДата создания: {t.CreatedAt:g} \nОписание: {t.Description}\n");
        }
    }
    static void AddTask(ITaskRepository repo)
    {
        string title = StringInput("Введите название задачи: ");

        string description = StringInput("Введите описание задачи: ");

        var task = new Tasks
        {
            Title = title,
            Description = description,
            IsCompleted = false,
            CreatedAt = DateTime.Now,
        };

        int id = repo.AddTask(task);
        Console.WriteLine("Задача добавлена.");
    }

    static void DeleteTask(ITaskRepository repo)
    {
        int id = NumInput("Введите Id задачи для удаления: ");
        bool deleted = repo.DeleteTask(id);
        Console.WriteLine(deleted ? "Задача успешно удалена." : "Ошибка! Задача не найдена.");
    }

    static void UpdateTask(ITaskRepository repo)
    {
        int id = NumInput("Введите Id задачи: ");
        var task = repo.GetTaskById(id);
        if (task == null)
        {
            Console.WriteLine("Задача не найдена!");
            return;
        }
        Console.WriteLine($"Текущий статус задачи: {(task.IsCompleted ? "Выполнено" : "Не выполненно")}");
        Console.Write("Отметить задачу как: 1 -> выполненную, 0-> не выполненную: ");
        bool newStatus = false;
        while (true)
        {
            int choice = NumInput("Введите ваш выбор: ");
            if (choice == 1)
            {
                newStatus = true;
                break;
            }
            else if (choice == 0)
            {
                newStatus = false;
                break;
            }
            else
            {
                Console.WriteLine("Не коректный ввод. Попробуйте ещё раз.");
            }
        }


        bool updated = repo.UpdateTask(id, newStatus);
        Console.WriteLine(updated ? "Статус Обновлён!" : "Ошибка при обновлении!");
    }

    static void Manager(ITaskRepository taskRep)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню программы:");
            Console.WriteLine("1. Просмотреть список задач.");
            Console.WriteLine("2. Добавить задачу.");
            Console.WriteLine("3. Обновить состояние задачи.");
            Console.WriteLine("4. Удалить задачу.");
            Console.WriteLine("5. Выход.");
            string choice = StringInput("Введите действие: ");
            Console.Clear();
            if (DoActionManager(choice, taskRep))
            {
                break;
            }
            Console.WriteLine("Нажмите клавишу чтобы продолжить.");
            Console.ReadKey();
        }
    }

    static bool DoActionManager(string choice, ITaskRepository taskRep)
    {
        switch (choice)
        {
            case "1":
                ShowAllTasks(taskRep);
                return false;
            case "2":
                AddTask(taskRep);
                return false;
            case "3":
                UpdateTask(taskRep);
                return false;
            case "4":
                DeleteTask(taskRep);
                return false;
            case "5":
                return true;
            default:
                Console.WriteLine("Неверный выбор!");
                return false;
        }
    }
    static int NumInput(string message)
    {
        int num;
        while (true)
        {
            Console.Write(message);
            if (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Введено не число! Повторите ввод!");
                continue;
            }
            return num;
        }
    }
    static string StringInput(string message)
    {
        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            else
            {
                Console.WriteLine("Поле не должно быть пустым!");
            }
        }
    }
    static bool CheckConnection(IDbConnectionFactory connectionFactory)
    {
        if (connectionFactory is SqlConnectionFactory sqlFactory)
        {
            if (!sqlFactory.TestConnection())
            {
                Console.WriteLine("\nНажмите любую клавишу, чтобы выйти.");
                Console.ReadKey();
                return false;
            }
        }
        else
        {
            Console.WriteLine("Неизвестное подключение.");
            return false;
        }
        return true;
    }
}