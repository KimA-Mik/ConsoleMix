Console.WriteLine("Hello, World!");


using (var sr = new StreamReader(@"D:\For Work\3\browserLink.js"))
{
    using (var sw = new StreamWriter(@"D:\For Work\3\browserLinkRes.js", false))
    {
        while (!sr.EndOfStream)
        {
            char cur = (char)sr.Read();
            if (IsEOSNeeded(cur))
            {
                sw.Write("{0}\n", cur);
            }
            else
            {
                sw.Write(cur);
            }
        }
    }
}

bool IsEOSNeeded(char check)
{
    switch (check)
    {
        case '{':
        case '}':
        case ';':
            return true;
        default:
            return false;
    }
}