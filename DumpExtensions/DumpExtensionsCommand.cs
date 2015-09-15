//------------------------------------------------------------------------------
// <copyright file="DumpExtensionsCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DumpExtensions
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.VisualStudio.ExtensionManager;

    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class DumpExtensionsCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("b11ebf6c-db99-4dd9-87dc-43461ed55c78");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpExtensionsCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private DumpExtensionsCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static DumpExtensionsCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new DumpExtensionsCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var extensionManager = (IVsExtensionManager)this.ServiceProvider.GetService(typeof(SVsExtensionManager));
            var extensions = extensionManager.GetInstalledExtensions().ToArray().OrderBy(x => x.Header.Name);

            int maxNameLength = extensions.Max(x => x.Header.Name.Length);
            int maxPathLength = extensions.Max(x => x.InstallPath.Length);

            string format = string.Format("{{0,-{0}}} {{1}}{1}", maxNameLength, Environment.NewLine);

            var infos = extensions
                .Select(x => string.Format(CultureInfo.InvariantCulture, format, x.Header.Name, x.InstallPath));

            var outputPane = (IVsOutputWindowPane)this.ServiceProvider.GetService(typeof(SVsGeneralOutputWindowPane));

            string header1 = string.Format(CultureInfo.InvariantCulture, format, "Name", "InstallPath");
            string header2 = string.Format(CultureInfo.InvariantCulture, format, new string('-', maxNameLength), new string('-', maxPathLength));

            outputPane.OutputStringThreadSafe(header1);
            outputPane.OutputStringThreadSafe(header2);

            foreach (var info in infos)
            {
                outputPane.OutputStringThreadSafe(info);
            }
        }
    }
}
