using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MiniTappsk.Models;

namespace MiniTappsk.Services
{
    public static class TaskStorage
    {
        private static readonly string AppFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MiniTappsk");

        private static readonly string FilePath = Path.Combine(AppFolder, "tasks.json");

        public static List<TodoTask> Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new List<TodoTask>();

                var json = File.ReadAllText(FilePath);
                var list = JsonSerializer.Deserialize<List<TodoTask>>(json);
                return list ?? new List<TodoTask>();
            }
            catch
            {
                
                return new List<TodoTask>();
            }
        }

        public static void Save(IEnumerable<TodoTask> tasks)
        {
            try
            {
                Directory.CreateDirectory(AppFolder);

                var json = JsonSerializer.Serialize(tasks,
                    new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(FilePath, json);
            }
            catch
            {
                
            }
        }
    }
}
