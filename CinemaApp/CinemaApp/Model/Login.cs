﻿using CinemaApp.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace CinemaApp.Model
{
    public class Login
    {
        // Metoda statyczna do logowania na konto Admina
        public static bool SignIn(string login, string password) 
        {
            // Algorytm haszujący SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Konwertowanie podanego loginu i hasła na tablicę bajtów
                byte[] enteredLoginBytes = Encoding.UTF8.GetBytes(login);
                byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(password);

                // Obliczenie hashu podanego loginu oraz hasła
                byte[] enteredHashBytesLogin = sha256.ComputeHash(enteredLoginBytes);
                byte[] enteredHashBytesPassword = sha256.ComputeHash(enteredPasswordBytes);

                // Konwertowanie hashy na stringi
                string enteredHashStringLogin = BitConverter.ToString(enteredHashBytesLogin).Replace("-", "").ToLower();
                string enteredHashStringPassword = BitConverter.ToString(enteredHashBytesPassword).Replace("-", "").ToLower();

                string directoryPath = "Passwd";
                string fileName = "password.txt";
                string filePath = Path.Combine(directoryPath, fileName);

                StreamReader sr;
                try
                {
                    sr = new StreamReader(filePath);
                }
                catch (Exception ex)
                {
                    throw new CannotReadFileException("Sprawdź czy plik z hasłami: password.txt istnieje w katalogu. Jeśli nie istnieje wprowadź nowy login i hasło:");
                }

                // Oczytanie z pliku loginu i hasła
                string hashedLogin  = sr.ReadLine();
                string hashedPassword = sr.ReadLine();
                sr.Close();

                // Porównanie hashy wprowadzonego loginu i hasła z hashami zapisanymi w pliku
                if (enteredHashStringLogin ==  hashedLogin && enteredHashStringPassword == hashedPassword)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static void HashPassword(string login, string password) 
        {
            // Algorytm haszujący SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Konwertowanie podanego loginu i hasła na tablicę bajtów
                byte[] loginBytes = Encoding.UTF8.GetBytes(login);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Obliczenie hashu podanego loginu oraz hasła
                byte[] hashBytesLogin = sha256.ComputeHash(loginBytes);
                byte[] hashBytesPassword = sha256.ComputeHash(passwordBytes);

                // Konwertowanie haszy na stringi
                string hashStringLogin = BitConverter.ToString(hashBytesLogin).Replace("-", "").ToLower();
                string hashStringPassword = BitConverter.ToString(hashBytesPassword).Replace("-", "").ToLower();


                // Zapisywanie haszy do pliku
                string directoryPath = "Passwd";

                string fileName = "password.txt";
                string filePath = Path.Combine(directoryPath, fileName);

                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine(hashStringLogin);
                sw.WriteLine(hashStringPassword);
                sw.Close();
            }
        }

    }
}