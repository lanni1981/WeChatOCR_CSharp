﻿#nullable enable

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class WeiOcrResult
    {
        public int TaskId { get; set; }
        public OcrResult? OcrResult { get; set; }
    }
    public class OcrResult
    {
        public List<SingleResult>? SingleResult { get; set; }
    }

    public class SingleResult
    {
        public SinglePos? SinglePos { get; set; }
        public string? SingleStrUtf8 { get; set; }
        public float SingleRate { get; set; }
        public List<OneResult>? OneResult { get; set; }
    }

    public class SinglePos
    {
        public List<Pos>? Pos { get; set; }
    }

    public class Pos
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class OneResult
    {
        public OnePos? OnePos { get; set; }
        public string? OneStrUtf8 { get; set; }

    }

    public class OnePos
    {
        public List<Pos>? Pos { get; set; }
    }

    public class ParseOcrResult
    {
        public static WeiOcrResult? ParseJson(string jsonResponseStr)
        {
            var rt = JsonConvert.DeserializeObject<WeiOcrResult>(jsonResponseStr);
            if (rt != null && rt.OcrResult != null && rt.OcrResult.SingleResult != null)
            {
                foreach (var item in rt.OcrResult.SingleResult)
                {
                    if (item.SingleStrUtf8 != null) item.SingleStrUtf8 = Encoding.UTF8.GetString(Convert.FromBase64String(item.SingleStrUtf8));
                }
            }
            return rt;
        }
    }
}
