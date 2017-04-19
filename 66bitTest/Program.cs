using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _66bitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var pathToDirectory = args.Length == 0 ? "." : args[0];
            foreach (var kvp in GetMethodsByClass(pathToDirectory))
            {
                Console.WriteLine(kvp.Key.Name);
                foreach (var method in kvp.Value)
                {
                    Console.WriteLine("\t" + method);
                }
            }
        }

        public static Dictionary<Type, List<string>> GetMethodsByClass(string pathToDirectory)
        {
            var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | 
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            return Directory.GetFiles(pathToDirectory, "*.dll")
                .SelectMany(file => Assembly.LoadFrom(file).GetTypes())
                .Where(type => type.IsClass)
                .ToDictionary(type => type, type => type
                    .GetMethods(flags)
                    .Where(method => method.IsPublic || method.IsFamily)
                    .Select(method => method.Name)
                    .ToList());
        }
    }
}
