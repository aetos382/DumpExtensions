namespace DumpExtensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

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

        private object _smallIconDataTemplate;

        public object SmallIconDataTemplate
        {
            get
            {
                if (_smallIconDataTemplate == null)
                {
                    _smallIconDataTemplate = GetTemplate("SmallIcon");
                }

                return _smallIconDataTemplate;
            }
        }

        private static object _mediumIconDataTemplate;

        public object MediumIconDataTemplate
        {
            get
            {
                if (_mediumIconDataTemplate == null)
                {
                    _mediumIconDataTemplate = GetTemplate("MediumIcon");
                }

                return _mediumIconDataTemplate;
            }
        }

        private static object _largeIconDataTemplate;

        public object LargeIconDataTemplate
        {
            get
            {
                if (_mediumIconDataTemplate == null)
                {
                    _mediumIconDataTemplate = GetTemplate("LargeIcon");
                }

                return _mediumIconDataTemplate;
            }
        }

        private static object _detailViewDataTemplate;

        public object DetailViewDataTemplate
        {
            get
            {
                if (_mediumIconDataTemplate == null)
                {
                    _mediumIconDataTemplate = GetTemplate("DetailView");
                }

                return _mediumIconDataTemplate;
            }
        }

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

        private static object GetTemplate(string key)
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string resourcePath = string.Format("{0};component/Templates/Template.xaml", assemblyName);

            var templateDictionary = new ResourceDictionary();
            templateDictionary.Source = new Uri(resourcePath, UriKind.Relative);

            if (!templateDictionary.Contains(key))
            {
                return null;
            }

            object item = templateDictionary[key];
            return item;
        }
    }
}
