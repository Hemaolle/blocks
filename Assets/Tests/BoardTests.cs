using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BoardTests {

    [Test]
    public void BoardToStringAndFromStringEqual() {        
        var b = new Board(FiveBySix());
        var s = b.ToString();
        b.FromString(s);
        Assert.That(s == b.ToString());        
    }

    private static Configuration FiveBySix()
    {
        return new Configuration(5, 6, 3);        
    }
}
