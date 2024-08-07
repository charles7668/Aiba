using Aiba.Enums;
using Aiba.Model.Extensions;

namespace Aiba.Tests.ExtensionTests
{
    [TestClass]
    public class MediaTypeExtensionTest
    {
        [TestMethod]
        public void TestMediaTypeFlagToStringExtension()
        {
            MediaTypeFlag flag = MediaTypeFlag.MANGA;
            string result = flag.GetMediaTypeString();
            Assert.AreEqual("manga", result);
            flag = MediaTypeFlag.VIDEO;
            result = flag.GetMediaTypeString();
            Assert.AreEqual("video", result);
            flag = MediaTypeFlag.ALL;
            result = flag.GetMediaTypeString();
            string[] splitResult = result.Split('|');
            string[] expect = ["manga", "video"];
            foreach (string s in splitResult)
            {
                Assert.IsTrue(expect.Any(x => x == s.Trim()));
            }
        }

        [TestMethod]
        public void TestStringToMediaTypeFlag()
        {
            string input = "manga";
            MediaTypeFlag result = input.GetFlag();
            Assert.AreEqual(MediaTypeFlag.MANGA, result);
            input = "video";
            result = input.GetFlag();
            Assert.AreEqual(MediaTypeFlag.VIDEO, result);
            input = "manga|video";
            result = input.GetFlag();
            Assert.AreEqual(MediaTypeFlag.ALL, result);
            input = "";
            result = input.GetFlag();
            Assert.IsFalse(result.HasFlag(MediaTypeFlag.MANGA) || result.HasFlag(MediaTypeFlag.VIDEO));
        }
    }
}