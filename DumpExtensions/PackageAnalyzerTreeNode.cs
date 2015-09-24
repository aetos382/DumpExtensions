namespace DumpExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.ExtensionsExplorer;

    class PackageAnalyzerTreeNode :
        IVsExtensionsTreeNode
    {
        public string Name { get; internal set; }

        public IList<IVsExtensionsTreeNode> Nodes { get; internal set; }

        public IVsExtensionsTreeNode Parent { get; internal set; }

        public IList Extensions { get; internal set; }

        public bool IsSearchResultsNode { get; internal set; }

        public bool IsSelected { get; set; }

        public bool IsExpanded { get; set; }
    }
}
