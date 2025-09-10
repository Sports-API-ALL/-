// Program.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;

class Program
{
    // !!! EXTREMELY IMPORTANT SECURITY WARNING !!!
    // 1. RUN THIS CODE ON A COMPUTER THAT IS COMPLETELY OFFLINE.
    // 2. NEVER SHARE YOUR 12 WORDS WITH ANYONE.
    // 3. SECURELY DELETE THE SOURCE CODE AND ANY OUTPUT FILES AFTER USE.
    // 4. USE AT YOUR OWN RISK.

    private static ulong _permutationCount = 0;
    private static ulong _validCount = 0;
    private static readonly string OutputFileName = "valid_phrases.txt";
    private const ulong ReportingInterval = 100_000; // 每处理10万次组合就报告一次进度

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
        // ▼▼▼▼▼   请在这里填入你的12个助记词，顺序可以随意   ▼▼▼▼▼
        // ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
        List<string> words = new List<string>
        {
            "word1", "word2", "word3", "word4", "word5", "word6",
            "word7", "word8", "word9", "word10", "word11", "word12"
        };
        // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
        // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

        if (words.Contains("word1"))
        {
            Console.WriteLine("错误：请将代码中的 'word1', 'word2', ... 替换为你自己的12个助记词。");
            return;
        }

        if (words.Count != 12 || words.Distinct().Count() != 12)
        {
            Console.WriteLine("错误：必须提供12个不重复的单词。");
            return;
        }

        // 清空旧的输出文件
        if (File.Exists(OutputFileName))
        {
            File.Delete(OutputFileName);
        }

        Console.WriteLine("开始遍历12个助记词的所有组合...");
        Console.WriteLine($"总共需要检查 {Factorial(12):N0} 种可能性。");
        Console.WriteLine("这将会花费非常长的时间，请耐心等待。");
        Console.WriteLine($"找到的有效助记词短语将被保存在 '{OutputFileName}' 文件中。");
        Console.WriteLine("----------------------------------------------------------");

        DateTime startTime = DateTime.Now;

        // 开始生成排列组合并校验
        await GeneratePermutationsAndValidate(words.ToArray(), 0);

        TimeSpan duration = DateTime.Now - startTime;
        Console.WriteLine("----------------------------------------------------------");
        Console.WriteLine("所有组合遍历完成！");
        Console.WriteLine($"总共检查了 {_permutationCount:N0} 个组合。");
        Console.WriteLine($"找到了 {_validCount:N0} 个有效的助记词短语。");
        Console.WriteLine($"总耗时: {duration.TotalHours:F2} 小时 ({duration.TotalMinutes:F2} 分钟)。");
        Console.WriteLine($"请检查项目文件夹下的 '{OutputFileName}' 文件。");
    }

    private static async Task GeneratePermutationsAndValidate(string[] currentWords, int k)
    {
        if (k == currentWords.Length)
        {
            _permutationCount++;

            string phrase = string.Join(" ", currentWords);
            try
            {
                // 使用 NBitcoin 库来验证助记词是否符合BIP39标准（包含校验和检查）
                // 如果助记词无效，Mnemonic 的构造函数会抛出 FormatException
                var mnemonic = new Mnemonic(phrase, Wordlist.English);

                // 如果代码能执行到这里，说明这是一个有效的助记词
                _validCount++;
                Console.WriteLine($"[有效] (总数: {_validCount}) -> {phrase}");
                await File.AppendAllTextAsync(OutputFileName, phrase + Environment.NewLine);
            }
            catch (FormatException)
            {
                // 校验和不匹配，这是预料之中的，忽略即可
            }

            if (_permutationCount % ReportingInterval == 0)
            {
                Console.WriteLine($"...已检查 {_permutationCount:N0} 个组合，找到 {_validCount} 个有效组合...");
            }
            return;
        }

        for (int i = k; i < currentWords.Length; i++)
        {
            Swap(ref currentWords[k], ref currentWords[i]);
            await GeneratePermutationsAndValidate(currentWords, k + 1);
            Swap(ref currentWords[k], ref currentWords[i]); // 回溯
        }
    }

    private static void Swap(ref string a, ref string b)
    {
        (a, b) = (b, a);
    }

    private static ulong Factorial(int n)
    {
        ulong result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= (ulong)i;
        }
        return result;
    }
}