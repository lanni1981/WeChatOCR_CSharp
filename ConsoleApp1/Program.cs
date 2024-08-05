#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolveEventHandler;
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolveEventHandler;

            using var ocr = new ImageOcr();
            var files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"测试集"));
            files.ToList().ForEach(f => { ocr.Run(f, OcrResultCallback); });

            //files.AsParallel().ForAll(f => { ocr.Run(f, OcrResultCallback); });

            //var imagePath = files.LastOrDefault();
            //ocr.Run(imagePath, OcrResultCallback);

            Console.ReadLine();
        }
        static void OcrResultCallback(string imgPath, WeiOcrResult? result)
        {
            if (result == null || result.OcrResult == null) return;
            var r = result.OcrResult.SingleResult.Select(a => a.SingleStrUtf8);
            var str = string.Join("\n", r);
            Console.WriteLine($"识别【{imgPath}】成功");
            Console.WriteLine(str);
            File.WriteAllText(imgPath.Replace(".png", ".txt"), str);
        }
        private static Assembly? LoadAssembly(string assemblyPath)
        {
            try
            {
                if (!File.Exists(assemblyPath)) throw new Exception("未找到文件：" + assemblyPath);
                var ass = Assembly.Load(File.ReadAllBytes(assemblyPath));
                return ass is null ? throw new Exception("加载：" + assemblyPath + " 失败！") : ass;
            }
            catch { throw; }
        }

        private static Assembly? AssemblyResolveEventHandler(object s, ResolveEventArgs e)
        {
            try
            {
                var fields = e.Name.Split(',');
                if (!fields.Any()) return null;
                foreach (var field in fields)
                {
                    var name = field + ".dll";
                    IList<string> paths = [ImageOcr.GetWeChatDir(),];
                    var path = paths.FirstOrDefault(File.Exists);
                    if (path == null) continue;
                    return LoadAssembly(path);
                }
                return null;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }
    }
}
