using System;
using Malukah_songs;

class Program
{
    static int failures;
    static void Check(string name, Action test)
    {
        try { test(); Console.WriteLine("PASS: " + name); }
        catch (Exception error) { failures++; Console.WriteLine("FAIL: " + name + ": " + error.Message); }
    }
    static void Assert(bool condition) { if (!condition) throw new Exception("Assertion failed"); }
    static int Main()
    {
        Check("clear populated download", () => {
            var item = new ItemViewModel();
            item.Download_url = "https://example.invalid/download";
            item.Download_url = null;
            Assert(item.Download_url == null);
        });
        Check("clear populated stream", () => {
            var item = new ItemViewModel();
            item.Stream_url = "https://example.invalid/stream";
            item.Stream_url = null;
            Assert(item.Stream_url == null);
        });
        Check("repeated raw stream URL does not notify", () => {
            var item = new ItemViewModel();
            item.Stream_url = "https://example.invalid/stream";
            var first = item.Stream_url;
            int notifications = 0;
            item.PropertyChanged += (s, e) => notifications++;
            item.Stream_url = "https://example.invalid/stream";
            Assert(item.Stream_url == first && notifications == 0);
        });
        Check("repeated raw download URL does not notify", () => {
            var item = new ItemViewModel();
            item.Download_url = "https://example.invalid/download";
            var first = item.Download_url;
            int notifications = 0;
            item.PropertyChanged += (s, e) => notifications++;
            item.Download_url = "https://example.invalid/download";
            Assert(item.Download_url == first && notifications == 0);
        });
        Check("ordinary URLs retain existing client parameter", () => {
            var item = new ItemViewModel();
            item.Stream_url = "https://example.invalid/stream";
            item.Download_url = "https://example.invalid/download";
            Assert(item.Stream_url.StartsWith("https://example.invalid/stream?client_id="));
            Assert(item.Download_url.Substring(item.Download_url.IndexOf('?')) ==
                   item.Stream_url.Substring(item.Stream_url.IndexOf('?')));
        });
        Check("empty URLs stay empty", () => {
            var item = new ItemViewModel();
            item.Stream_url = "";
            item.Download_url = "";
            Assert(item.Stream_url == "" && item.Download_url == "");
        });
        return failures == 0 ? 0 : 1;
    }
}
