using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class Translator
{
    private Dictionary<string, Dictionary<string, List<string>>> dictionaries;

    public Translator()
    {
        dictionaries = new Dictionary<string, Dictionary<string, List<string>>>();
        LoadDictionaries();
    }

    private void LoadDictionaries()
    {
        try
        {
            string json = File.ReadAllText("dictionaries.json");
            dictionaries = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(json);
            Console.WriteLine("Словари успешно загружены из файла.");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл словарей не найден. Создан новый файл.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке словарей: {ex.Message}");
        }
    }

    private void SaveDictionaries()
    {
        try
        {
            string json = JsonConvert.SerializeObject(dictionaries, Formatting.Indented);
            File.WriteAllText("dictionaries.json", json);
            Console.WriteLine("Словари успешно сохранены в файл.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении словарей: {ex.Message}");
        }
    }
    public void ExportToFile(string dictionaryName, string fileName)
    {
        if (dictionaries.ContainsKey(dictionaryName))
        {
            string json = JsonConvert.SerializeObject(dictionaries[dictionaryName], Formatting.Indented);
            File.WriteAllText(fileName, json);
            Console.WriteLine($"Словарь '{dictionaryName}' успешно экспортирован в файл '{fileName}'.");
        }
        else
        {
            Console.WriteLine($"Словарь '{dictionaryName}' не найден.");
        }
    }
    public void CreateDictionary(string name)
    {
        dictionaries[name] = new Dictionary<string, List<string>>();
        SaveDictionaries();
        Console.WriteLine($"Словарь '{name}' создан.");
    }

    public void ReplaceWord(string dictionaryName, string word, List<string> translations)
    {
        if (dictionaries.ContainsKey(dictionaryName) && dictionaries[dictionaryName].ContainsKey(word))
        {
            dictionaries[dictionaryName][word] = translations;
        }
        else
        {
            Console.WriteLine($"Словарь '{dictionaryName}' не содержит слово '{word}'.");
        }
    }

    public void DeleteWord(string dictionaryName, string word)
    {
        if (dictionaries.ContainsKey(dictionaryName) && dictionaries[dictionaryName].ContainsKey(word))
        {
            dictionaries[dictionaryName].Remove(word);
        }
        else
        {
            Console.WriteLine($"Словарь '{dictionaryName}' не содержит слово '{word}'.");
        }
    }

    public void SearchTranslation(string dictionaryName, string word)
    {
        if (dictionaries.ContainsKey(dictionaryName) && dictionaries[dictionaryName].ContainsKey(word))
        {
            Console.WriteLine($"Перевод слова '{word}':");
            foreach (var translation in dictionaries[dictionaryName][word])
            {
                Console.WriteLine(translation);
            }
        }
        else
        {
            Console.WriteLine($"Словарь '{dictionaryName}' не содержит слово '{word}'.");
        }
    }

    public void PrintDictionaries()
    {
        Console.WriteLine("Существующие словари:");
        foreach (var dictionary in dictionaries.Keys)
        {
            Console.WriteLine(dictionary);
        }
    }

    public void PrintAllWordsAndTranslations(string dictionaryName)
    {
        if (dictionaries.ContainsKey(dictionaryName))
        {
            Console.WriteLine($"Слова и их переводы в словаре '{dictionaryName}':");
            foreach (var entry in dictionaries[dictionaryName])
            {
                Console.WriteLine($"Слово: {entry.Key}");
                Console.WriteLine("Переводы:");
                foreach (var translation in entry.Value)
                {
                    Console.WriteLine($"- {translation}");
                }
            }
        }
        else
        {
            Console.WriteLine($"Словарь '{dictionaryName}' не найден.");
        }
    }
    public void AddWord(string dictionaryName, string word, List<string> translations)
    {
        if (dictionaries.ContainsKey(dictionaryName))
        {
            dictionaries[dictionaryName][word] = translations;
            SaveDictionaries();
            Console.WriteLine($"Слово '{word}' и его переводы добавлены в словарь '{dictionaryName}'.");
        }
        else
        {
            Console.WriteLine($"Словарь '{dictionaryName}' не найден.");
        }
    }
}

class Program
{
    static void Main()
    {
        Translator translator = new Translator();

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Создать словарь");
            Console.WriteLine("2. Добавить слово и перевод");
            Console.WriteLine("3. Заменить слово или перевод");
            Console.WriteLine("4. Удалить слово или перевод");
            Console.WriteLine("5. Найти перевод слова");
            Console.WriteLine("6. Экспортировать словарь в файл");
            Console.WriteLine("7. Вывести существующие словари");
            Console.WriteLine("8. Вывести все слова и их переводы в словаре");
            Console.WriteLine("9. Выйти");

            Console.Write("Выберите действие (1-9): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите название словаря: ");
                    string dictionaryName = Console.ReadLine();
                    translator.CreateDictionary(dictionaryName);
                    Console.WriteLine($"Словарь '{dictionaryName}' создан.");
                    break;

                case "2":
                    Console.Write("Введите название словаря: ");
                    string dictNameAdd = Console.ReadLine();
                    Console.Write("Введите слово: ");
                    string wordAdd = Console.ReadLine();
                    Console.Write("Введите перевод(ы) через запятую: ");
                    string translationsAddStr = Console.ReadLine();
                    List<string> translationsAdd = translationsAddStr.Split(',').Select(t => t.Trim()).ToList();
                    translator.AddWord(dictNameAdd, wordAdd, translationsAdd);
                    break;

                case "3":
                    Console.Write("Введите название словаря: ");
                    string dictNameReplace = Console.ReadLine();
                    Console.Write("Введите слово: ");
                    string wordReplace = Console.ReadLine();
                    Console.Write("Введите новый перевод(ы) через запятую: ");
                    string translationsReplaceStr = Console.ReadLine();
                    List<string> translationsReplace = translationsReplaceStr.Split(',').Select(t => t.Trim()).ToList();
                    translator.ReplaceWord(dictNameReplace, wordReplace, translationsReplace);
                    break;

                case "4":
                    Console.Write("Введите название словаря: ");
                    string dictNameDelete = Console.ReadLine();
                    Console.Write("Введите слово: ");
                    string wordDelete = Console.ReadLine();
                    translator.DeleteWord(dictNameDelete, wordDelete);
                    break;

                case "5":
                    Console.Write("Введите название словаря: ");
                    string dictNameSearch = Console.ReadLine();
                    Console.Write("Введите слово: ");
                    string wordSearch = Console.ReadLine();
                    translator.SearchTranslation(dictNameSearch, wordSearch);
                    break;

                case "6":
                    Console.Write("Введите название словаря: ");
                    string dictNameExport = Console.ReadLine();
                    Console.Write("Введите имя файла для экспорта: ");
                    string fileNameExport = Console.ReadLine();
                    translator.ExportToFile(dictNameExport, fileNameExport);
                    break;

                case "7":
                    translator.PrintDictionaries();
                    break;

                case "8":
                    Console.Write("Введите название словаря: ");
                    string dictNamePrintAll = Console.ReadLine();
                    translator.PrintAllWordsAndTranslations(dictNamePrintAll);
                    break;

                case "9":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    break;
            }
        }
    }
}

