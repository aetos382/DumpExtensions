namespace DumpExtensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.ExtensionManager;
    using Microsoft.VisualStudio.ExtensionsExplorer;
    using Microsoft.VisualStudio.Shell;

    [Export(typeof(IVsExtensionManagerDialogProvider))]
    public class ExtensionAnalyzerProvider :
        IVsExtensionManagerDialogProvider
    {
        public ExtensionAnalyzerProvider()
        {
        }

        public IVsExtensionsTreeNode Search(string searchTerms)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get
            {
                return "Extension Analyzer";
            }
        }

        public float SortOrder
        {
            get
            {
                return 100;
            }
        }

        public object SmallIconDataTemplate { get; }

        public object MediumIconDataTemplate { get; }

        public object LargeIconDataTemplate { get; }

        public object DetailViewDataTemplate { get; }

        public object HeaderContent { get; }

        public object View { get; }

        public object ItemContainerStyle { get; }

        private IVsExtensionsTreeNode _rootNode = null;

        public IVsExtensionsTreeNode ExtensionsTree
        {
            get
            {
                if (this._rootNode == null)
                {
                    this._rootNode = new PackageAnalyzerTreeNode
                        {
                            Nodes = new IVsExtensionsTreeNode[]
                                {
                                    new PackageAnalyzerTreeNode
                                        {
                                            Name = "ほげ",
                                            Extensions = this.InstalledExtensions
                                        }
                                }
                        };
                }

                return this._rootNode;
            }
        }

        public bool ListVisibility
        {
            get
            {
                return true;
            }
        }

        public bool ListMultiSelect
        {
            get
            {
                return false;
            }
        }

        private List<IVsExtension> _installExtensions;

        private List<IVsExtension> InstalledExtensions
        {
            get
            {
                if (this._installExtensions == null)
                {
                    this._installExtensions =
                        this.ExtensionManager.GetInstalledExtensions().Select(x => new InstalledExtension(x)).Cast<IVsExtension>().ToList();
                }

                return this._installExtensions;
            }
        }

        private IVsExtensionManager _extensionManager;

        private IVsExtensionManager ExtensionManager
        {
            get
            {
                if (this._extensionManager == null)
                {
                    this._extensionManager = (IVsExtensionManager)Package.GetGlobalService(typeof(SVsExtensionManager));
                }

                return this._extensionManager;
            }
        }
    }
}
