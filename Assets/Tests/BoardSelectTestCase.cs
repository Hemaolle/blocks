//-----------------------------------------------------------------------
// <copyright file="BoardSelectTestCase.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
public class BoardSelectTestCase
{
    public BoardSelectTestCase(
        string name,
        string layout,
        string selected,
        string expectedRemovals,
        string expectedMovesFrom,
        string expectedMovesTo,
        string expectedAdds)
    {
        Name = name;
        Layout = layout;
        Selected = selected;
        ExpectedRemovals = expectedRemovals;
        ExpectedMovesFrom = expectedMovesFrom;
        ExpectedMovesTo = expectedMovesTo;
        ExpectedAdds = expectedAdds;
    }

    public string Layout { get; }

    public string Selected { get; }

    public string ExpectedRemovals { get; }

    public string ExpectedMovesFrom { get; }

    public string ExpectedMovesTo { get; }

    public string ExpectedAdds { get; }

    public string Name { get; }
}