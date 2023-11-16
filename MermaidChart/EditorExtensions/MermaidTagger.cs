using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Animation;
using MermaidChart.Utils;

namespace MermaidChart
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("code")]
    [TagType(typeof(IntraTextAdornmentTag))]
    internal class MermaidTagger: IViewTaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            return  new MermaidIntraAdornmentTag((IWpfTextView)textView) as ITagger<T>;
        }
    }

    internal class MermaidIntraAdornmentTag : IntraTextAdornmentTagger<MermaidLink, MermaidControlsAdornment>
    {

        public MermaidIntraAdornmentTag(IWpfTextView textView) : base(textView) { }


        protected override MermaidControlsAdornment CreateAdornment(MermaidLink data, SnapshotSpan span)
        {
            return new MermaidControlsAdornment(data);
        }

        protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, MermaidLink>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
        {
            var text = spans.First().Snapshot.GetText();
            var links = MermaidLinkUtils.FindLinks(text);
            foreach (var link in links.Cast<MermaidLink>())
            {
                var span = new SnapshotSpan(spans.First().Snapshot, new Span(link.Position + link.Length, 0));
                yield return Tuple.Create(span, (PositionAffinity?)PositionAffinity.Successor, link);
            }
        }

        protected override bool UpdateAdornment(MermaidControlsAdornment adornment, MermaidLink data)
        {
            return false;
        }
    }
}
