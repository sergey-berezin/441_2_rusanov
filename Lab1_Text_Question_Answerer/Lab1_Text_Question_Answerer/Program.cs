using BertModelLibrary;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lab1_Text_Question_Answerer
{
    class Program
    {
        static Semaphore consoleSemaphore = new Semaphore(1, 1);
        static async Task Main(string[] args)
        {

            try
            {
                if (args.Length > 0)
                {
                    string path = @args[0];
                    //string path = "..\\..\\..\\..\\hobbit.txt";
                    string text = GetTextFromFile(path);
                    Console.WriteLine(text);

                    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                    CancellationToken token = cancelTokenSource.Token;

                    var bertModel = new BertModel();
                    string? question = "start";
                    consoleSemaphore.WaitOne();
                    while ((question = Console.ReadLine()) != "")
                    {
                        consoleSemaphore.Release();
                        if (question == "cancel") { cancelTokenSource.Cancel(); }
                        var answer = Task.Run(() => ProcessQuestionAsync(bertModel, text, question, token));
                        consoleSemaphore.WaitOne();
                    }
                }
                else
                {
                    throw new ArgumentException("No file path in command line arguments!");
                }
            }
            catch (Exception ex)
            {
                consoleSemaphore.WaitOne();
                Console.WriteLine(ex.Message);
                consoleSemaphore.Release();
            }

        }
        static async Task ProcessQuestionAsync(BertModel bertModel, string text, string question, CancellationToken token)
        {
            try
            {
                var answer = await Task.Run(() => bertModel.AnswerQuestionAsync(text, question, token));
                consoleSemaphore.WaitOne();
                Console.WriteLine(question + " : " + answer);
                consoleSemaphore.Release();
            }
            catch (Exception ex)
            {
                consoleSemaphore.WaitOne();
                Console.WriteLine(question + " : " + ex.Message);
                consoleSemaphore.Release();
            }
        }

        static string GetTextFromFile(string path)
        {
            StreamReader? reader = null;
            try
            {
                reader = new StreamReader(path);
                string text = reader.ReadToEnd();
                return text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception loading from file: {ex.Message}");
                return "";
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }
    }
}