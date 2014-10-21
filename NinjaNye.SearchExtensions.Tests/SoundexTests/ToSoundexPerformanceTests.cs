using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Soundex;

namespace NinjaNye.SearchExtensions.Tests.SoundexTests
{
    [TestFixture]
    public class ToSoundexPerformanceTests
    {
        private List<string> words;

        [SetUp]
        public void Setup()
        {
            this.BuildWords(1000000);
        }

        private void BuildWords(int wordCount)
        {
            Console.WriteLine("Building {0} words...", wordCount);
            this.words = new List<string>();
            for (int i = 0; i < wordCount; i++)
            {
                string randomWord = this.BuildRandomWord();
                this.words.Add(randomWord);
            }
        }

        private const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private string BuildRandomWord()
        {
            var letterCount = RandomInt(2, 10);
            var sb = new StringBuilder(letterCount);
            for (int i = 0; i < letterCount; i++)
            {
                var letterIndex = RandomInt(0, 51);
                sb.Append(letters[letterIndex]);
            }
            return sb.ToString();
        }

        private static int RandomInt(int min, int max)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[4];

            rng.GetBytes(buffer);
            int result = BitConverter.ToInt32(buffer, 0);

            return new Random(result).Next(min, max);
        }

        [Test]
        public void ToSoundex_OneMillionRecords_UnderOneSecond()
        {
            //Arrange
            Console.WriteLine("Processing {0} words", words.Count);
            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin soundex...");
            stopwatch.Start();
             
            //Act
            var result = words.Select(SoundexProcessor.ToSoundex).ToList();
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Results retrieved: {0}", result.Count()); 
            //Assert
            Assert.True(stopwatch.Elapsed.TotalMilliseconds < 700);
        }

        [Test]
        public void SearchSoundex_OneMillionWordsComparedToOneWord_UnderOneSecond()
        {
            //Arrange
            Console.WriteLine("Processing {0} words", words.Count);
            
            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin soundex search...");
            stopwatch.Start();
             
            //Act
            var result = words.Search(x => x).Soundex("test").ToList();
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Results retrieved: {0}", result.Count); 
            //Assert
            Assert.True(stopwatch.Elapsed.Milliseconds < 1000);
        }

        [Test]
        public void SearchSoundex_OneMillionWordsComparedToTwoWords_UnderOneSecond()
        {
            //Arrange
            Console.WriteLine("Processing {0} words", words.Count);

            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin soundex search...");
            stopwatch.Start();

            //Act
            var result = words.Search(x => x).Soundex("test", "bacon").ToList();
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Results retrieved: {0}", result.Count);
            //Assert
            Assert.True(stopwatch.Elapsed.Milliseconds < 1000);
        }

        [Test]
        public void SearchSoundex_OneMillionWordsComparedToTenWords_UnderOneSecond()
        {
            //Arrange
            Console.WriteLine("Processing {0} words", words.Count);

            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin soundex search...");
            stopwatch.Start();

            //Act
            var result = words.Search(x => x).Soundex("historians", "often", "articulate", "great", "battles",
                                                      "elegantly", "without", "pause", "for", "thought").ToList();
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Results retrieved: {0}", result.Count);
            //Assert
            Assert.True(stopwatch.Elapsed.Milliseconds < 1000);
        }

    }
}