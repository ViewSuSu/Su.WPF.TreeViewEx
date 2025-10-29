# WPF Tree Control Library Usage Guide

## Overview

**TreeView has been a pain point for developers!** The internet is filled with various fragmented solutions, from complex custom templates to cumbersome data bindings. Developers have gone to great lengths to implement a fully functional TreeView control in WPF. However, to this day, there is no unified, complete, and easy-to-use encapsulation solution.

**Now I present a viable solution!**

This project provides a complete, object-oriented approach to tree node operations, freeing developers from tedious template definitions, data bindings, and event handling, allowing them to focus on business logic implementation.

## 🎬 Demo Animation
<div align="left">
<img src="./HD.gif" alt="TreeViewEx 功能演示" width="30%">
</div>


## 🎯 Core Design Philosophy

### 🏗️ Robust Architecture Based on SOLID Principles

This project adopts **object-oriented design principles**, strictly following **SOLID principles**, especially the **Open/Closed Principle**:

- **✅ Open for Extension**: You can easily inherit from base classes like `TreeNodeEx`, `MenuBase`, etc., to add custom functionality
- **✅ Closed for Modification**: Core APIs are stable, version updates won't break existing code
- **✅ Single Responsibility**: Each class has clear responsibility boundaries, avoiding "God objects"
- **✅ Interface Segregation**: Provides fine-grained configuration options for selective use

### 🌟 Pure Implementation with Zero Dependencies

**Key Feature: No third-party dependencies!**

```csharp
// Pure native WPF implementation
// ✅ No dependency on MVVM frameworks (Prism, MVVMLight, etc.)
// ✅ No dependency on UI component libraries (MaterialDesign, MahApps, etc.)  
// ✅ No dependency on IOC containers (Autofac, Unity, etc.)
// ✅ No dependency on any other NuGet packages

// Ready to use out of the box, no complex dependency configuration or environment setup required
```

### 🔒 Secure API Design

```csharp
// The compiler prevents incorrect usage patterns
// ❌ These will cause compilation errors, avoiding runtime exceptions:
// node.MenuItems.Add(...);        // MenuItems is read-only
// provider.Controller = null;     // Controller is read-only  
// node.Children = new List<>();   // Children collection is protected

// ✅ Only allowed through designed public methods
node.AddChild("Safe Operation");
node.MenuItemModels.Add(menu);     // Correct collection operation
```

## 🚧 Current Version Status

**Optimization work continues!** The current version has reasonably implemented core requirements and provides stable basic functionality, but the project acknowledges there's room for improvement:

### ✅ Implemented Stable Features
- **Complete tree structure management** - Node creation, deletion, traversal, and other basic operations
- **Checkbox system** - Three-state management, automatic cascading, status querying
- **Context menu system** - Tree-level and node-level menus with shortcut key support
- **Style configuration system** - Icons, colors, fonts, and other visual customizations
- **Selection management** - Multi-select support, selection status tracking
- **Copy functionality** - Complete node structure copying
- **Data binding** - Association with custom data objects

### ⚠️ Known Areas for Optimization

#### 1. **Performance Optimization Needs**
- **Virtualized display**: The current version may have performance bottlenecks when handling large-scale node data (e.g., 1000+ nodes). Plans to introduce virtualization technology to only render visible area nodes, significantly improving performance.
- **Memory management**: Further optimization needed for node object lifecycle management to reduce memory usage.

#### 2. **Asynchronous Operation Support**
- **Data loading**: All current operations are synchronous, which may block the UI thread when processing large amounts of data or remote data.
- **Batch operations**: Need to implement asynchronous batch node operations for better user experience.

#### 3. **Advanced Feature Planning**
- **Lazy loading**: Support for lazy loading of child nodes, loading data only when needed.
- **Animation effects**: Smooth animation effects for node expand/collapse.
- **Drag and drop support**: Complete drag and drop operation support.
- **Filtering and search**: Real-time filtering and search functionality.

## Quick Start

### 1. Basic Usage - Say Goodbye to Complex Configuration!

```csharp
// Traditional approach: Requires defining templates, bindings, styles...
// This project's approach: Done in 3 lines of code!
var nodes = new List<TreeNodeEx>
{
    TreeNodeEx.CreateNode("Root Node 1"),
    TreeNodeEx.CreateNode("Root Node 2")
};
var provider = TreeViewExProvider.GetTreeViewPanelProvider(nodes);
// <ContentControl Content="{Binding Provider.TreeView, Mode=OneWay}" />
```

## 🎨 Complete Style Configuration System

### 1. **Icon Configuration (TreeNodeExIconOptions)**

```csharp
// Complete icon configuration system
var node = TreeNodeEx.CreateNode("Node with Icon");

// Set icon
node.TreeNodeExIconOptions.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/icon.png"));
// Or load from resources
node.TreeNodeExIconOptions.Icon = (ImageSource)FindResource("MyIcon");

// Custom icon size
node.TreeNodeExIconOptions.Width = 24;  // Default 16, adjustable

// Automatic display logic: Icon displays when Icon is not null and Width > 0
if (node.TreeNodeExIconOptions.IsShowImageSource)
{
    // Icon will display before node text
}
```

**Icon Configuration Property Description:**
- `Icon: ImageSource` - Icon image source, supports all WPF image formats
- `Width: double` - Icon display width, height automatically adjusts proportionally
- `IsShowImageSource: bool` - Read-only property, determines if display conditions are met

### 2. **Text Style Configuration (TreeNodeExTextOptions)**

```csharp
// Complete text style configuration
var node = TreeNodeEx.CreateNode("Styled Text");

// Font size configuration
node.TreeNodeExTextOptions.FontSize = 14;  // Default system font size

// Font weight configuration
node.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;      // Bold
node.TreeNodeExTextOptions.FontWeight = FontWeights.SemiBold;  // Semi-bold
node.TreeNodeExTextOptions.FontWeight = FontWeights.Normal;    // Normal (default)

// Practical application scenarios
var titleNode = TreeNodeEx.CreateNode("Chapter Title");
titleNode.TreeNodeExTextOptions.FontSize = 16;
titleNode.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;

var contentNode = TreeNodeEx.CreateNode("Content Text");  
contentNode.TreeNodeExTextOptions.FontSize = 12;
contentNode.TreeNodeExTextOptions.FontWeight = FontWeights.Normal;
```

**Text Style Property Description:**
- `FontSize: double` - Font size, defaults to system message font size
- `FontWeight: FontWeight` - Font weight, defaults to normal weight

### 3. **Highlight Color Configuration**

```csharp
// Node highlight color configuration
node.HighlightColor = Colors.Blue;                    // Use system colors
node.HighlightColor = Color.FromRgb(255, 0, 0);       // RGB colors
node.HighlightColor = Color.FromArgb(255, 0, 120, 215); // Theme colors

// Automatically generated brush property
Brush highlightBrush = node.HighlightColorBrush;  // Read-only, automatically created
```

### 2. Complete Tree Structure Building - As Simple as Operating Regular Objects!

```csharp
// Traditional approach: Requires defining complex data models and bindings
// This project's approach: Intuitive object operations
var project = TreeNodeEx.CreateNode("My Project");
project.TreeNodeExIconOptions.Icon = LoadIcon("project.png");
project.TreeNodeExIconOptions.Width = 20; // Custom icon size

var srcFolder = project.AddChild("Source Code");
srcFolder.TreeNodeExIconOptions.Icon = folderIcon;
srcFolder.AddChild("MainWindow.xaml.cs");
srcFolder.AddChild("MainViewModel.cs");

var configFolder = project.AddChild("Configuration");
configFolder.AddChild("app.config");

// Add context menus - Use MenuItemModels!
srcFolder.MenuItemModels.Add(new TreeNodeMenu("New File", node => {
    node.AddChild($"NewFile_{DateTime.Now:HHmmss}.cs");
}));
```

### 3. Checkbox Functionality - Automatic Cascading, Zero Configuration!

```csharp
// Traditional approach: Requires manual checkbox state synchronization
// This project's approach: Automatic cascading out of the box
var parent = TreeNodeEx.CreateNode("Parent Node");
parent.IsShowCheckBox = true;

var child1 = parent.AddChild("Child Node 1");
child1.IsShowCheckBox = true;
child1.IsChecked = true;

var child2 = parent.AddChild("Child Node 2"); 
child2.IsShowCheckBox = true;
child2.IsChecked = false;

// Automatic effects:
// - Check parent node → All child nodes automatically checked
// - Uncheck parent node → All child nodes automatically unchecked  
// - Some child nodes checked → Parent node shows as partially checked
// All of this without writing a single line of state synchronization code!
```

### 4. 🆕 Complete Menu System - Supports Shortcuts and Icons!

```csharp
// Tree-level menus (shared across entire tree control)
provider.Controller.Options.MenuItemModels.Add(new TreeViewMenu("Refresh", RefreshTree)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.R),
    Icon = refreshIcon
});

// Node-level menus (specific to individual nodes)
var fileNode = TreeNodeEx.CreateNode("ImportantFile.txt");
fileNode.MenuItemModels.Add(new TreeNodeMenu("Encrypt", node => EncryptFile(node)));
fileNode.MenuItemModels.Add(new TreeNodeMenu("Backup", node => BackupFile(node)));

// Menu items with shortcuts (automatically register global shortcuts)
var copyMenu = new TreeViewMenu("Copy", CopyAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.C)
};
// Automatically displays as: "Copy (Ctrl+C)" and responds to Ctrl+C shortcut

// Menu items with icons
var saveMenu = new TreeViewMenu("Save", SaveAction)
{
    Icon = saveIcon
};
```

### 5. 🆕 Shortcut System - Automatic Registration and Management!

```csharp
// Complete shortcut key support
var menu = new TreeViewMenu("Select All", SelectAllAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.A)
};

// Support for multiple modifier key combinations
var complexShortcut = new TreeViewMenu("Advanced Operation", AdvancedAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control | ModifierKeys.Shift, Key.S)
};
// Displays as: "Advanced Operation (Ctrl+Shift+S)"

// Automatic shortcut features:
// ✅ Automatically displays shortcut hints after menu text
// ✅ Automatically registers with WPF input binding system
// ✅ Globally effective, no additional handling required
// ✅ Supports all standard keys and modifier key combinations
```

### 6. 🆕 New Features

```csharp
// Node copying - Complete copying of node and all its children
var original = GetComplexNodeStructure();
var copied = original.Copy(); // One-click copy of entire subtree
var copiedTo = original.CopyTo(targetParent); // Copy to specified location

// Checkbox status querying - Powerful status analysis capabilities
var checkedNodes = parent.GetCheckedChildren(); // Get directly checked child nodes
var allChecked = parent.GetAllCheckedDescendants(); // Get all checked descendant nodes
var checkboxCount = parent.GetCheckBoxChildrenCount(); // Count child nodes displaying checkboxes
var checkedCount = parent.GetCheckedChildrenCount(); // Count checked child nodes
var hasChecked = parent.HasCheckedChildren(); // Check if any checked items exist
```

## 📝 Important Notes

### Menu System Usage Instructions

**Correct Menu Addition Methods:**
```csharp
// ✅ Correct: Tree-level menus use MenuItemModels
provider.Controller.Options.MenuItemModels.Add(new TreeViewMenu("Action Name", action));

// ✅ Correct: Node-level menus use MenuItemModels
node.MenuItemModels.Add(new TreeNodeMenu("Action Name", action));

// ❌ Incorrect: MenuItems is a read-only property, cannot add directly
// node.MenuItems.Add(...); // This will cause compilation error!
// provider.Controller.Options.MenuItems.Add(...); // This is also wrong!
```

**Property Description:**
- `TreeViewExPropertyOptions.MenuItemModels`: Tree-level **writable** menu model collection
- `TreeNodeEx.MenuItemModels`: Node-level **writable** menu model collection  
- `TreeNodeEx.MenuItems`: Read-only WPF menu item collection for **displaying** menus
- `TreeViewExPropertyOptions.MenuItems`: Read-only tree-level WPF menu item collection

### Shortcut Key Usage Points

```csharp
// Shortcut keys automatically handle the following:
var menu = new TreeViewMenu("Test", TestAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Alt, Key.T)
};

// Automatically completes:
// 1. Menu display text: "Test (Alt+T)"
// 2. Global shortcut registration: Alt+T triggers TestAction
// 3. No manual InputBindings handling required
```

### 7. Data Binding and Association

```csharp
// Associate custom data objects
var customData = new MyDataClass { Id = 1, Name = "Data Object" };
node.Data = customData;

// Access associated data in menus
node.MenuItemModels.Add(new TreeNodeMenu("View Data", (selectedNode) =>
{
    if (selectedNode.Data is MyDataClass data)
    {
        Console.WriteLine($"Data ID: {data.Id}, Name: {data.Name}");
    }
}));
```

### 8. Node Operations

```csharp
// Delete node
node.Delete(); // Delete single node (public method)

// Conditional deletion
node.DeleteChildrenNodesByPredicate(child => 
    child.Text.Contains("Temporary")); // Delete all children with text containing "Temporary"

node.DeleteFirstChildNodeByPredicate(child => 
    child.Text == "Specific Node"); // Delete first matching child node
```

### 9. Selection Management

```csharp
// Get currently selected nodes
var selectedNodes = controller.SelectedNodes;

// Manually set selection status
node.IsSelected = true;

// Iterate through selected nodes
foreach (var selectedNode in controller.SelectedNodes)
{
    Console.WriteLine($"Selected Node: {selectedNode.Text}");
}
```

## 🔧 API Usage Guide

### Controller Related
```csharp
// Get controller
var controller = provider.Controller;

// Access selected nodes
var selected = controller.SelectedNodes;

// Access source nodes
var sourceNodes = controller.SourceTreeNodes;

// Access tree-level menus (correct usage)
var treeMenus = controller.Options.MenuItemModels;
```

### Node Related
```csharp
// Create node
var node = TreeNodeEx.CreateNode("Node Name");

// Add child nodes
node.AddChild("Child Node");
node.AddChild(childNode);
node.AddRange(children);

// Copy nodes
var copied = node.Copy();
var copiedTo = node.CopyTo(parent);

// Checkbox queries
var checkedChildren = node.GetCheckedChildren();
var allChecked = node.GetAllCheckedDescendants();
var checkboxCount = node.GetCheckBoxChildrenCount();
var checkedCount = node.GetCheckedChildrenCount();
var hasChecked = node.HasCheckedChildren();
```

### Menu Related
```csharp
// Create menu items
var menu = new TreeViewMenu("Menu Item", action); // Tree-level menu
var nodeMenu = new TreeNodeMenu("Menu Item", action); // Node-level menu

// Add shortcuts
menu.Shortcut = new MenuShortcut(ModifierKeys.Control, Key.S);

// Add icons
menu.Icon = yourIcon;

// Add to appropriate collections
controller.Options.MenuItemModels.Add(menu); // Tree-level
node.MenuItemModels.Add(nodeMenu); // Node-level
```

## 🎯 Version Recommendations

### Current Version Suitable Scenarios
- ✅ Small to medium scale data (node count < 500)
- ✅ Local data operations
- ✅ Synchronous data processing
- ✅ Basic tree structure display
- ✅ Need complete context menu support
- ✅ Need checkbox functionality
- ✅ Need shortcut key support
- ✅ Need icon and style customization

### Scenarios for Optimized Version
- 🔄 Large scale data (node count > 1000)
- 🔄 Remote data loading
- 🔄 Asynchronous operation requirements
- 🔄 High-performance requirement scenarios
- 🔄 Complex animation effect requirements

## Summary

**While optimization continues, the current version is already powerful enough!**

This project provides a **stable, feature-complete, easy-to-use** TreeView solution that addresses 90% of daily development needs. For scenarios requiring extreme performance with ultra-large-scale data, the project is actively developing optimized versions.

**Remember Key Points:**
- **Tree-level menus**: Use `Controller.Options.MenuItemModels`
- **Node-level menus**: Use `node.MenuItemModels`
- **Node deletion**: Use `node.Delete()`
- **Shortcut keys**: Automatically registered via `Shortcut` property
- **Icon configuration**: Set node icons via `TreeNodeExIconOptions`
- **Text styling**: Set text appearance via `TreeNodeExTextOptions`

The original intention behind developing this encapsulation library was to free developers from tedious technical details and allow them to focus on creating better user experiences. Whether you're developing file managers, configuration interfaces, data browsers, or any application requiring tree structures, this library can provide the perfect solution.

---

## 💡 Design Philosophy

**"Simple should not be complex, complex should not be simple"**

- Common functionality should be **ready out of the box**
- Advanced functionality should be **extensible**
- Incorrect usage should result in **compile-time errors**
- Architecture design should be **future-oriented**

**Start enjoying a pleasant TreeView development experience!**