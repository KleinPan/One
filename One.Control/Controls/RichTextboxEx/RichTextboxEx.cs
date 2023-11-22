using One.Control.Controls.Panels;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace One.Control.Controls.RichTextboxEx;

[TemplatePart(Name = "PART_StackPanel", Type = typeof(OverlapPanel))]
public class RichTextboxEx : RichTextBox
{
    public bool ShowLineNumber
    {
        get { return (bool)GetValue(ShowLineNumberProperty); }
        set { SetValue(ShowLineNumberProperty, value); }
    }

    /// <summary> 行号 </summary>
    public static readonly DependencyProperty ShowLineNumberProperty =
        DependencyProperty.Register("ShowLineNumber", typeof(bool), typeof(RichTextboxEx), new PropertyMetadata(false));

    static RichTextboxEx()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RichTextboxEx), new FrameworkPropertyMetadata(typeof(RichTextboxEx)));
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);
        LineNumbers();
    }

    #region SearchRectangle

    public ObservableCollection<Rect> SearchRectangles
    {
        get { return (ObservableCollection<Rect>)GetValue(SearchRectanglesProperty); }

        set { SetValue(SearchRectanglesProperty, value); }
    }

    public static readonly DependencyProperty SearchRectanglesProperty = DependencyProperty.Register("SearchRectangles", typeof(ObservableCollection<Rect>), typeof(RichTextboxEx), new PropertyMetadata(null));

    private void ClearSearch()
    {
        this.SearchRectangles.Clear();
    }

    #endregion SearchRectangle

    private void LineNumbers()
    {
        if (!ShowLineNumber)
        {
            return;
        }

        try
        {
            if (stackPanel == null)
            {
                return;
            }
            stackPanel.Children.Clear();
            int lineNumber = 1;
            var lineStartPos = Document.ContentStart.GetLineStartPosition(0);

            while (true)
            {
                //每一行的Top位置是准的
                var endRect = lineStartPos.GetCharacterRect(LogicalDirection.Forward);

                //var startRect = lineStartPos.GetCharacterRect(LogicalDirection.Backward);

                Label label = new();

                label.Margin = new Thickness(0, endRect.Top + 3, 0, 0);

                var test = $"{endRect.Top},{endRect.Height}";
                //label.ToolTip = test;

                label.Content = lineNumber;
                stackPanel.Children.Add(label);

                lineStartPos = lineStartPos.GetLineStartPosition(1, out int result);
                if (result == 0)
                {
                    break;
                }

                ++lineNumber;
            }
        }
        catch (Exception)
        {
        }
    }

    public OverlapPanel stackPanel;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        stackPanel = GetTemplateChild("PART_StackPanel") as OverlapPanel;
    }

    public void HighLightSearch(string searchString)
    {
        this.ClearSearch();

        foreach (string word in SplitWords(searchString.Trim()))
        {
            // Filters out double spaces: we don't want to search for spaces in the text.
            if (!word.Trim().Equals(""))
            {
                // Then, search for occurrences of "word"
                TextPointer position = Document.ContentStart;

                while (position != null)
                {
                    if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                    {
                        string textInRun = position.GetTextInRun(LogicalDirection.Forward).ToLower();

                        while (textInRun.ToLower().Contains(word.ToLower()))
                        {
                            int indexInRun = textInRun.IndexOf(word.ToLower());

                            if (indexInRun >= 0)
                            {
                                position = position.GetPositionAtOffset(indexInRun);

                                TextRange tr = new TextRange(position, position.GetPositionAtOffset(word.Length));

                                DoSearch(tr);

                                position = tr.End;
                                textInRun = position.GetTextInRun(LogicalDirection.Forward).ToLower();
                            }
                        }
                    }

                    position = position.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
        }
    }

    private void DoSearch(TextRange range)
    {
        try
        {
            Rect leftRectangle = range.Start.GetCharacterRect(LogicalDirection.Forward);
            Rect rightRectangle = range.End.GetCharacterRect(LogicalDirection.Backward);

            Rect rect = new Rect(leftRectangle.TopLeft, rightRectangle.BottomRight);

            Point translatedPoint = this.TranslatePoint(new Point(0, 0), null);
            Point endPoint = this.TranslatePoint(new Point(this.ActualWidth, this.ActualHeight), null);
            rect.Offset(translatedPoint.X - 1, translatedPoint.Y - 1);

            if (rect.X >= translatedPoint.X - 1 & rect.X <= endPoint.X & rect.Y >= translatedPoint.Y - 1 & rect.Y <= (endPoint.Y - 10))
            {
                this.SearchRectangles.Add(rect);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while doing search. Exception: {0}", ex.Message);
        }
    }

    /// <summary> Splits strings on spaces, but preserves string which are between double quotes </summary>
    /// <param name="inputString"> </param>
    /// <returns> </returns>
    public static string[] SplitWords(string inputString)
    {
        // return inputString.Split(new char[] { ' ' });

        return inputString.Split('"')
                 .Select((element, index) => index % 2 == 0  // If even index
                                       ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                                       : new string[] { element })  // Keep the entire item
                 .SelectMany(element => element).ToArray();
    }
}